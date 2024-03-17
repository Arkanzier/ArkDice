using ArkDice;
using Character;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text.Json;
using System.Xml.Linq;

namespace Simple_Dice_Roller
{

    public partial class Form1 : Form
    {
        //Attributes:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //A log of recent actions, displayed to the user.
        internal List<string> LogMessages;

        //Hold the character that's loaded.
        internal Character.Character LoadedCharacter;

        //Holds the collection of dice that are currently selected via the dice roller tab.
        internal DiceCollection CurrentDice;

        //Hold the most recently rolled collection of dice from the dice roller tab.
        internal DiceCollection LastDice;

        //These hold information about which rows in AbilitiesArea and SpellsArea to display extra information for.
        //The key is the row's ID value.
        //The other one is the panel that's used to display the stuff.
        internal Dictionary<string, Panel> AbilitiesAreaDetails;
        internal Dictionary<string, Panel> SpellsAreaDetails;

        //These hold some stuff about how much space should be given to detail views.
        internal const int MinAbilityDetailHeight = 75;
        internal const int MinSpellDetailHeight = 75;

        //Stores lists of spells and abilities to make them easy to load later on.
        public Dictionary<string, Character.Spell> SpellsLibrary;
        public Dictionary<string, Character.Ability> AbilitiesLibrary;

        //Various popup forms.
        internal EditCharacter? EditCharacterForm;
        internal EditAbilities? EditAbilitiesForm;
        internal EditSpells? EditSpellsForm;
        internal Panel? CharacterSelectPopup;

        //A class for holding the configurable settings.
        internal Settings.Settings Settings;


        public string Folderpath { get; private set; }

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public Form1()
        {
            InitializeComponent();

            //We'll start by zeroing everything out, so that we can guarantee everything is initialized.
            LogMessages = new List<string>();
            LoadedCharacter = new Character.Character();
            CurrentDice = new DiceCollection();
            LastDice = new DiceCollection();
            AbilitiesAreaDetails = new Dictionary<string, Panel>();
            SpellsAreaDetails = new Dictionary<string, Panel>();
            SpellsLibrary = new Dictionary<string, Spell>();
            AbilitiesLibrary = new Dictionary<string, Character.Ability>();
            EditCharacterForm = null;
            EditAbilitiesForm = null;
            EditSpellsForm = null;

            //Load the settings next, to make sure they're available everywhere.
            Settings = new Settings.Settings();

            DisplaySettings();
            DrawListOfCharacters(Dropdown_CharactersList);

            //Potentially prompt the user to load an existing character.
            Dictionary<string, string> characters = GetListOfCharacters();
            if (characters.Count > 0)
            {
                CreateCharacterSelectPopup();
            }
            //Else: do nothing, because the only option will be to load a blank character.

            //Directory that this stuff runs out of:
            //C:\Users\david\source\repos\ArkDice\Simple Dice Roller\bin\Debug\net7.0-windows
            //Will probably switch to net8.0-windows soon, since I'm updating this to .net 8.0.
            Folderpath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\";
        }


        //Misc functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Looks up the index for the AbilitiesArea or SpellsArea row with a particular ID.
        //Returns the row's index if found, or -1 if not.
        private int GetIndexForRow(string gridName, string id)
        {
            DataGridView grid;
            string idColIndex;
            if (gridName == "Abilities")
            {
                grid = AbilitiesArea;
                idColIndex = "Abilities_IDCol";
            }
            else if (gridName == "Spells")
            {
                grid = SpellsArea;
                idColIndex = "Spells_IDCol";
            }
            else
            {
                //Complain to a log file.
                return -1;
            }

            //Look up the index of the id column
            int colIndex = grid.Columns[idColIndex].Index;
            if (colIndex < 0)
            {
                //Complain to a log file.
                return -1;
            }

            for (int a = 0; a < grid.Rows.Count; a++)
            {
                if (grid.Rows[a].Cells[colIndex].Value.ToString() == id)
                {
                    return a;
                }
            }

            return -1;
        }

        //Loads a specified character.
        private void LoadCharacter(string charID)
        {
            //Load the libraries every time in case something has changed while we were doing stuff.
            SpellsLibrary = new Dictionary<string, Spell>();
            AbilitiesLibrary = new Dictionary<string, Ability>();

            LoadSpellsLibrary(Folderpath);
            LoadAbilitiesLibrary(Folderpath);

            //Detail views for the lists of abilities and spells.
            AbilitiesAreaDetails = new Dictionary<string, Panel>();
            SpellsAreaDetails = new Dictionary<string, Panel>();

            //Currently selected dice for the dice roller.
            CurrentDice = new DiceCollection();

            //Stores the last-rolled collection of dice from the dice roller.
            LastDice = new DiceCollection();

            //Stores the currently loaded character.
            Character.Character currentCharacter;
            if (charID == "")
            {
                //We're loading a blank character as part of the process of the user creating a new character.
                currentCharacter = new Character.Character();
                LogMessage("Could not find character " + charID);
            } else
            {
                //We're loading an existing character from a file.
                currentCharacter = new Character.Character(charID, Folderpath, ref AbilitiesLibrary, ref SpellsLibrary);
                LogMessage("Loaded character " + charID);
            }
            
            LoadedCharacter = currentCharacter;
            DisplayCharacter(currentCharacter);

            DrawRecharges();
        }

        //Sets the size and location of the detail panels in the Abilities and Spells lists.
        private void SetAndPositionDetailPanel(string gridName, string id)
        {
            Panel? panel;
            int expandAmount;
            DataGridView grid;
            int minHeight;
            if (gridName == "Abilities")
            {
                if (AbilitiesAreaDetails.ContainsKey(id))
                {
                    panel = AbilitiesAreaDetails[id];
                }
                else
                {
                    panel = null;
                }
                expandAmount = MinAbilityDetailHeight;
                grid = AbilitiesArea;
                minHeight = MinAbilityDetailHeight;
            }
            else if (gridName == "Spells")
            {
                if (SpellsAreaDetails.ContainsKey(id))
                {
                    panel = SpellsAreaDetails[id];
                }
                else
                {
                    panel = null;
                }
                expandAmount = MinSpellDetailHeight;
                grid = SpellsArea;
                minHeight = MinSpellDetailHeight;
            }
            else
            {
                //Whatever this is it's not supported.
                return;
            }

            int rowIndex = GetIndexForRow(gridName, id);

            int baseRowHeight = grid.Rows[rowIndex].GetPreferredHeight(rowIndex, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
            //MessageBox.Show("This row reports that it would like to have a height of " + baseRowHeight);
            //to do: get this working
            baseRowHeight = 25;

            if (panel == null)
            {
                //Set the row's height to standard.
                grid.Rows[rowIndex].Height = baseRowHeight;
                //MessageBox.Show("Just set row height to " + baseRowHeight);

                //Remove the padding from the button columns.
                if (gridName == "Abilities")
                {
                    grid.Rows[rowIndex].Cells["Abilities_UseButtonCol"].Style.Padding = new Padding(0);
                    grid.Rows[rowIndex].Cells["Abilities_Plus1Col"].Style.Padding = new Padding(0);
                    grid.Rows[rowIndex].Cells["Abilities_Minus1Col"].Style.Padding = new Padding(0);
                }
                else if (gridName == "Spells")
                {
                    grid.Rows[rowIndex].Cells["Spells_CastCol"].Style.Padding = new Padding(0);
                    grid.Rows[rowIndex].Cells["Spells_UpcastCol"].Style.Padding = new Padding(0);
                }

                return;
            }

            //We want to figure out how tall stuff wants to be.
            //Note: labels seem to default to being 15px tall.
            int panelHeight = panel.GetPreferredSize(new Size(100, expandAmount)).Height;
            if (panelHeight < minHeight)
            {
                panelHeight = minHeight;
            }

            int rowHeight = baseRowHeight + panelHeight;

            //Now we set the row height so we can get proper location information for the panel.
            grid.Rows[rowIndex].Height = rowHeight;

            //Set some padding to keep the buttons looking normal.
            if (gridName == "Abilities")
            {
                grid.Rows[rowIndex].Cells["Abilities_UseButtonCol"].Style.Padding = new Padding(0, 0, 0, panelHeight);
                grid.Rows[rowIndex].Cells["Abilities_Plus1Col"].Style.Padding = new Padding(0, 0, 0, panelHeight);
                grid.Rows[rowIndex].Cells["Abilities_Minus1Col"].Style.Padding = new Padding(0, 0, 0, panelHeight);
            }
            else if (gridName == "Spells")
            {
                grid.Rows[rowIndex].Cells["Spells_CastCol"].Style.Padding = new Padding(0, 0, 0, panelHeight);
                grid.Rows[rowIndex].Cells["Spells_UpcastCol"].Style.Padding = new Padding(0, 0, 0, panelHeight);
            }

            //Get the location and dimensions to set this.
            int top = grid.Rows[rowIndex].AccessibilityObject.Bounds.Top;
            int left = grid.Rows[rowIndex].AccessibilityObject.Bounds.Left;
            int bottom = grid.Rows[rowIndex].AccessibilityObject.Bounds.Bottom;
            int right = grid.Rows[rowIndex].AccessibilityObject.Bounds.Right;

            //We need to position things in relation to the grid, not the program.
            int gridTop = grid.AccessibilityObject.Bounds.Top;
            int gridLeft = grid.AccessibilityObject.Bounds.Left;

            //Don't show panels if there's nothing to actually show.
            //We'll be told top/bottom and left/right coordinates that exactly match the top left of the visible part of the grid.
            if (gridTop == top && gridTop == bottom && gridLeft == left && gridLeft == right)
            {
                panel.Visible = false;
                return;
            }
            else
            {
                panel.Visible = true;
            }

            top -= gridTop;
            left -= gridLeft;

            //We want to leave 1 row worth of space above the panel.
            top += baseRowHeight;

            if (top > bottom)
            {
                //This row is too short for this.
                //complain to a log?
                return;
            }

            int width = right - left;

            //MessageBox.Show("Going to set this panel to " + left + ", " + top + " and " + width + " x " + panelHeight);
            Rectangle rect = new Rectangle(left, top, width, panelHeight);

            //Now set the panel's size and location.
            panel.Bounds = rect;
            panel.BringToFront();
        }

        //The business end of a button used for testing things.
        private void TestButton_Click(object sender, EventArgs e)
        {
            while (LogMessages.Count > 0)
            {
                LogMessages.RemoveAt(LogMessages.Count - 1);
            }

            DisplayMessages();



            //Test populating a ListView and compare it to a DataGridView

            TestListView.Items.Clear();

            for (int a = 0; a < 50; a++)
            {
                string[] temp = { "item" + a, "something" };
                TestListView.Items.Add(new ListViewItem(temp));
            }



            //testing listviews
            /*
            ListViewTest.Columns.Add("Col1", 100, HorizontalAlignment.Left);
            ListViewTest.Columns.Add("Col2", 50, HorizontalAlignment.Left);
            ListViewTest.Columns.Add("Col3ccccc", 200, HorizontalAlignment.Left);

            string[] temp = { "row 1", "second box", "aaa" };
            ListViewTest.Items.Add(new ListViewItem(temp));

            ListViewItem templvi = new ListViewItem("row 2");
            templvi.SubItems.Add("#2");
            templvi.SubItems.Add("Programmatically added");
            ListViewTest.Items.Add(templvi);

            temp = new[] { "row 3", "333", "trace" };
            ListViewTest.Items.Add(new ListViewItem(temp));

            temp = new[] { "row 4", "this row has only 2 columns" };
            ListViewItem lvi = new ListViewItem(temp);
            ListViewTest.Items.Add(lvi);
            ListViewTest.Items[3].SubItems[1].Text = "#1";
            //this is, in fact, the proper way to access a cell and change it's text.

            //ListViewTest.Items[3].ListView.Columns[2].Dispose();
            //This gets rid of the column for all rows.

            //ListViewTest.Items.Add(new ListViewItem(new ListView()));

            //ListViewTest.Groups.Add(new ListViewGroup("group goes here"));
            //ListViewTest.Groups.Add(new ListViewGroup("this is a second group"));
            */
        }

        //Displays or hides the detail view for any given row in AbilitiesArea.
        private void ToggleGridExpand(string gridName, int rowNum)
        {
            DataGridView grid;
            Dictionary<string, Panel> details;
            string idCol;
            if (gridName == "Abilities")
            {
                grid = AbilitiesArea;
                details = AbilitiesAreaDetails;
                idCol = "Abilities_IDCol";
            }
            else if (gridName == "Spells")
            {
                grid = SpellsArea;
                details = SpellsAreaDetails;
                idCol = "Spells_IDCol";
            }
            else
            {
                //Complain to a log file?
                return;
            }

            string? id = grid.Rows[rowNum].Cells[idCol].Value.ToString();
            if (id == null)
            {
                //Complain to a log file.
                MessageBox.Show("Error: could not find id for row.");
                return;
            }

            if (details.ContainsKey(id))
            {
                //Remove this panel from the list.
                details[id].Dispose();
                details.Remove(id);
            }
            else
            {
                //Expand the row and display the details view.

                if (id == null)
                {
                    //Complain to a log file.
                    MessageBox.Show("Error: could not find id for row");
                    return;
                }

                //Create an appropriate panel and add it to the list.
                Panel newPanel = new Panel();
                if (gridName == "Abilities")
                {
                    CreateAbilityDetailPanel(newPanel, grid.Rows[rowNum]);
                }
                else if (gridName == "Spells")
                {
                    CreateSpellDetailPanel(newPanel, grid.Rows[rowNum]);
                }
                grid.Controls.Add(newPanel);
                details[id] = newPanel;
            }

            //Make sure everything is sized and positioned properly.
            SetAndPositionDetailPanel(gridName, id);

            if (gridName == "Abilities")
            {
                UpdateAbilitiesAreaDetails();
            }
            else if (gridName == "Spells")
            {
                UpdateSpellsAreaDetails();
            }
        }


        //-------- -------- -------- -------- -------- -------- -------- -------- 
        //Functions relating to the character select popup that appears on program start
        #region Character Select Popup

        //Create a popup prompting the user to load an existing character, or a blank one.
        private void CreateCharacterSelectPopup()
        {
            //A few things we might want to make customizable later:
            int horizontalMargin = 25;
            int verticalMargin = 10;    //25?
            int spaceBetweenRows = 10;
            int spaceBetweenElements = spaceBetweenRows;
            int minPopupWidth = 300;
            //int minPopupHeight = 200;


            //Get rid of an existing copy of the popup, if there is one, then create a popup.
            if (CharacterSelectPopup != null)
            {
                CharacterSelectPopup.Dispose();
                CharacterSelectPopup = null;
            }
            Panel popup = new Panel();

            //Store the popup for later, so we can delete it if this gets called again before it's gone.
            CharacterSelectPopup = popup;

            //Get the size of the main window.
            Rectangle windowRect = new Rectangle(new Point(0, 0), this.Size);

            //Restrict this popup to only 80% of the height/width of the main window.
            int maxWidth = (int)(windowRect.Width * 0.8);
            int maxHeight = (int)(windowRect.Height * 0.8);

            //--------

            //First row: some text to tell the user what's going on.
            Label newLabel = new Label();
            newLabel.Text = "Would you like to load an existing character?";
            popup.Controls.Add(newLabel);
            Size labelSize = newLabel.GetPreferredSize(new Size(maxWidth, maxHeight));

            //Do some sanity checking on what we got back.
            //Restrict this text to no more than 80% of the width of the main window.
            if (labelSize.Width > maxWidth)
            {
                labelSize.Width = maxWidth;

                //I don't know of a way to recalculate the appropriate dimensions with an enforced max width,
                //if GetPreferredSize failed, so we're just going to blindly add 15px (one row) to the height.
                labelSize.Height += 15;
            }
            //Do the same with the height.
            labelSize.Height = (labelSize.Height > maxHeight) ? maxHeight : labelSize.Height;

            int textRowHeight = labelSize.Height;
            int textRowWidth = labelSize.Width;

            //--------

            //Next row: character select dropdown and "load" button.
            //Create the dropdown with the list of characters.
            ComboBox dropdown = new ComboBox();
            DrawListOfCharacters(dropdown);
            popup.Controls.Add(dropdown);
            Size dropdownSize = dropdown.GetPreferredSize(new Size(0,0));

            //Put in a button to trigger the load, rather than having it automatically happen when something is selected.
            Button loadButton = new Button();
            loadButton.Text = "Load";
            loadButton.Click += new System.EventHandler(LoadCharacterFromPopup);
            popup.Controls.Add(loadButton);
            Size loadButtonSize = new Size(75, 23);

            int characterRowHeight = (dropdownSize.Height > loadButtonSize.Height) ? dropdownSize.Height : loadButtonSize.Height;
            int characterRowWidth = dropdownSize.Width + spaceBetweenElements + loadButtonSize.Width;

            //--------

            //Next row: a button to indicate that the user wants to load a blank character.
            Button newCharButton = new Button();
            newCharButton.Text = "New";
            newCharButton.Click += new System.EventHandler(NewCharacterFromPopup);
            popup.Controls.Add(newCharButton);
            Size newCharButtonSize = new Size(75, 23);

            int newCharRowHeight = newCharButtonSize.Height;
            int newCharRowWidth = newCharButtonSize.Width;

            //--------

            /*
            +---------------------------------------------------+
            |   Would you like to load an existing character?   |
            |            [dropdown here]     [load]             |
            |                     [   no  ]                     |
            +---------------------------------------------------+
            */
            //Now we position everything.

            //Figure out how wide to make the popup.
            int widestRowWidth = Math.Max (textRowWidth, Math.Max(newCharRowWidth, characterRowWidth));
            if (widestRowWidth + (horizontalMargin * 2) < minPopupWidth)
            {
                widestRowWidth = minPopupWidth;
            }

            //We'll use this variable to track vertical positioning.
            int currentHeight = verticalMargin;

            //Now we can set the width of the popup and position everything else around it.
            int popupWidth = widestRowWidth + (horizontalMargin * 2);
            popupWidth = (popupWidth < minPopupWidth) ? minPopupWidth : popupWidth;

            //First row
            int temp = (popupWidth - labelSize.Width) / 2;
            Rectangle textRowRect = new Rectangle(temp, currentHeight, labelSize.Width, labelSize.Height);
            newLabel.Bounds = textRowRect;

            currentHeight += labelSize.Height + spaceBetweenRows;

            //Second row
            //We're going to assume that this row will never be too wide, and skip checking for that.
            temp = (popupWidth - characterRowWidth) / 2;
            Rectangle dropdownRect = new Rectangle(temp, currentHeight, dropdownSize.Width, dropdownSize.Height);
            dropdown.Bounds = dropdownRect;
            temp += dropdownSize.Width + spaceBetweenElements;
            Rectangle loadButtonRect = new Rectangle (temp, currentHeight, loadButtonSize.Width, loadButtonSize.Height);
            loadButton.Bounds = loadButtonRect;

            currentHeight += characterRowHeight + spaceBetweenRows;

            //Third row
            temp = (popupWidth - newCharButtonSize.Width) / 2;
            Rectangle newCharButtonRect = new Rectangle(temp, currentHeight, newCharButtonSize.Width, newCharButtonSize.Height);
            newCharButton.Bounds = newCharButtonRect;

            currentHeight += newCharButtonSize.Height + spaceBetweenRows;

            //Now the entire popup.
            this.Controls.Add(popup);
            //Width is already in popupWidth
            int popupHeight = currentHeight + verticalMargin;

            int popupLeft = (windowRect.Width - popupWidth) / 2;
            int popupTop = (windowRect.Height - popupHeight) / 2;

            Rectangle popupRect = new Rectangle(popupLeft, popupTop, popupWidth, popupHeight);
            popup.Bounds = popupRect;

            popup.Show();
            popup.BringToFront();
        }

        //Fetches the character currently selected from the Character Select Popup and loads it, then closes the popup.
        //If no character is selected, it does nothing.
        internal void LoadCharacterFromPopup (object? sender, EventArgs e)
        {
            //Fetch the currently selected character, if there is one.
            if (CharacterSelectPopup == null)
            {
                //The panel isn't open, how did this get called?
                return;
            }

            string? charname = CharacterSelectPopup.Controls.OfType<ComboBox>().FirstOrDefault()?.Text;
            if (charname == null || charname == "")
            {
                //Nothing was selected in it.
                return;
            }

            LoadCharacter(charname);

            //We're done with this popup, so get rid of it.
            CharacterSelectPopup.Dispose();
            CharacterSelectPopup = null;
        }

        //Set things up so that a blank character will be properly loaded, then close the Character Select Popup.
        internal void NewCharacterFromPopup (object? sender, EventArgs e)
        {
            //start by trying out just closing the popup and seeing if anything complains
            if (CharacterSelectPopup != null)
            {
                CharacterSelectPopup.Dispose();
                CharacterSelectPopup = null;
            }

            LogMessage("Loaded blank character");
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions relating to the message log.
        #region Log Messages

        //Display the log messages in the log messages area.
        private void DisplayMessages()
        {
            string text = "";

            foreach (string message in LogMessages)
            {
                if (text != "")
                {
                    text += "\r\n";
                }
                text += message;
            }

            outputDescription.Text = text;
        }

        //Add a message to the log area, plus some related logic.
        private void LogMessage(string message)
        {
            LogMessages.Insert(0, message);

            //We'll limit the log to the most recent 20 rows for readability.
            //This will also prevent ballooning memory usage if the program is used very heavily.
            while (LogMessages.Count > 20)
            {
                LogMessages.RemoveAt(LogMessages.Count - 1);
            }

            DisplayMessages();
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to loading and saving the abilities and spells libraries.
        #region Library Functions

        //Loads a collection of abilities into a library that can be referenced later to quickly load abilities without having to type them or whatever.
        private bool LoadAbilitiesLibrary(string folderpath)
        {
            if (folderpath.Last() != '\\')
            {
                folderpath += "\\";
            }
            string filepath = folderpath + "dat\\Abilities.dat";

            if (File.Exists(filepath))
            {
                string fileContents = File.ReadAllText(filepath);
                try
                {
                    Dictionary<string, Ability>? temp = JsonSerializer.Deserialize<Dictionary<string, Ability>>(fileContents);

                    if (temp == null)
                    {
                        //Complain to a log file?
                        return false;
                    }
                    else
                    {
                        //Copy temp's contents into AbilitiesLibrary.
                        AbilitiesLibrary = temp;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: could not read abilities library from file.");
                    AbilitiesLibrary = new Dictionary<string, Ability>();
                    return false;
                }
            }

            return false;
        }

        //Loads a collection of spells into a library that can be referenced later to quickly load spells without having to type them or whatever.
        private bool LoadSpellsLibrary(string folderpath)
        {
            if (folderpath.Last() != '\\')
            {
                folderpath += "\\";
            }
            string filepath = folderpath + "dat\\Spells.dat";

            if (File.Exists(filepath))
            {
                string fileContents = File.ReadAllText(filepath);
                try
                {
                    Dictionary<string, Spell>? temp = JsonSerializer.Deserialize<Dictionary<string, Spell>>(fileContents);

                    if (temp == null)
                    {
                        //Complain to a log file?
                        return false;
                    }
                    else
                    {
                        //Copy temp's contents into SpellsLibrary.
                        SpellsLibrary = temp;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: could not read spells library from file.");
                    SpellsLibrary = new Dictionary<string, Spell>();
                    return false;
                }
            }

            return false;
        }

        //Writes the contents of the abilities library to disk.
        private bool SaveAbilitiesLibrary()
        {
            if (Folderpath.Last() != '\\')
            {
                Folderpath += "\\";
            }
            string filepath = Folderpath + "dat\\Abilities.dat";

            if (File.Exists(filepath))
            {
                //The library file already exists, let's make a backup before we overwrite it.
                string backupFolderpath = Folderpath + "dat\\Backups\\";
                string backupFilepath = backupFolderpath + "Abilities_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".dat";
                if (!File.Exists(backupFolderpath))
                {
                    Directory.CreateDirectory(backupFolderpath);
                }

                //Now we can move the old one into the backup folder.
                File.Move(filepath, backupFilepath);
            }

            //Save the new info to the file.
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string data = JsonSerializer.Serialize(AbilitiesLibrary, serializerOptions);
            File.WriteAllText(filepath, data);
            if (File.Exists(filepath))
            {
                LogMessage("Abilities list saved successfully.");
                return true;
            }
            else
            {
                LogMessage("Unable to save abilities list.");
                return false;
            }
        }

        //Writes the contents of the spells library to disk.
        private bool SaveSpellsLibrary()
        {
            if (Folderpath.Last() != '\\')
            {
                Folderpath += "\\";
            }
            string filepath = Folderpath + "dat\\Spells.dat";

            if (File.Exists(filepath))
            {
                //The library file already exists, let's make a backup before we overwrite it.
                string backupFolderpath = Folderpath + "dat\\Backups\\";
                string backupFilepath = backupFolderpath + "Spells_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".dat";
                if (!File.Exists(backupFolderpath))
                {
                    Directory.CreateDirectory(backupFolderpath);
                }

                //Now we can move the old one into the backup folder.
                File.Move(filepath, backupFilepath);
            }

            //Save the new info to the file.
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string data = JsonSerializer.Serialize(SpellsLibrary, serializerOptions);
            File.WriteAllText(filepath, data);
            if (File.Exists(filepath))
            {
                LogMessage("Spells list saved successfully.");
                return true;
            }
            else
            {
                LogMessage("Unable to save spells list.");
                return false;
            }
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the forms for editing abilities/characters/spells.
        #region Editing Forms

        //Opens a new window for editing abilities.
        private void BeginEditingAbilities()
        {
            EditAbilities editing = new EditAbilities(this);
            editing.Show();

            EditAbilitiesForm = editing;

        }

        //Opens a new window for editing the current character.
        private void BeginEditingCharacter()
        {
            EditCharacter editing = new EditCharacter(LoadedCharacter);
            editing.ParentForm = this;
            editing.Show();

            //Store a reference to the child so we can access it later.
            EditCharacterForm = editing;
        }

        //Opens a new window for editing spells.
        private void BeginEditingSpells()
        {
            
            EditSpells editing = new EditSpells(this);
            editing.Show();

            EditSpellsForm = editing;
        }

        //The onclick event for the edit abilities button. Pops up a new window where spells can be edited and assigned.
        private void Button_EditAbilities_Click(object sender, EventArgs e)
        {
            //Close any other popups, so the user can only have one open at a time.
            CloseEditingAbilities();
            CloseEditingCharacter();
            CloseEditingSpells();

            //Now open the thing.
            BeginEditingAbilities();
        }

        //The onclick event for the edit character button. Pops up a new window where various character attributes can be changed.
        private void Button_EditChar_Click(object sender, EventArgs e)
        {
            //Close any other popups, so the user can only have one open at a time.
            CloseEditingAbilities();
            CloseEditingCharacter();
            CloseEditingSpells();

            //Now open the thing.
            BeginEditingCharacter();
        }

        //The onclick event for the edit spells button. Pops up a new window where spells can be edited and assigned.
        private void Button_EditSpells_Click(object sender, EventArgs e)
        {
            //Close any other popups, so the user can only have one open at a time.
            CloseEditingAbilities();
            CloseEditingCharacter();
            CloseEditingSpells();

            //Now open the thing.
            BeginEditingSpells();
        }

        //Closes the new form used for editing abilities.
        public void CloseEditingAbilities()
        {
            if (EditAbilitiesForm != null)
            {
                EditAbilitiesForm.Close();
                EditAbilitiesForm = null;
            }
        }

        //Closes the new form used for editing characters.
        private void CloseEditingCharacter()
        {
            if (EditCharacterForm != null)
            {
                EditCharacterForm.Close();
                EditCharacterForm = null;
            }
        }

        //Closes the new form used for editing spells.
        private void CloseEditingSpells()
        {
            if (EditSpellsForm != null)
            {
                EditSpellsForm.Close();
                EditSpellsForm = null;
            }
        }

        //Called when something is closing the form for editing abilities.
        public void ClosingEditingAbilities()
        {
            EditAbilitiesForm = null;
            DisplayCharacter(LoadedCharacter);
        }

        //Called when something is closing the form for editing characters.
        public void ClosingEditingCharacter()
        {
            EditCharacterForm = null;
            DisplayCharacter(LoadedCharacter);
        }

        //Called when something is closing the form for editing spells.
        public void ClosingEditingSpells()
        {
            EditSpellsForm = null;
            DisplayCharacter(LoadedCharacter);
        }

        //Removes the specified ability from the library.
        public void RemoveAbility(string id)
        {
            //Make sure the ability is in the abilities library.
            if (AbilitiesLibrary.ContainsKey(id))
            {
                AbilitiesLibrary.Remove(id);
            }

            LoadedCharacter.RemoveAbility(id);

            //Save the changes to the abilities library to disk.
            SaveAbilitiesLibrary();

            //Update the list of abilities, in case something relevant changed.
            DisplayAbilities(LoadedCharacter);
        }

        //Removes the specified spell from the library.
        public void RemoveSpell(string id)
        {
            //Make sure the ability is in the abilities library.
            if (SpellsLibrary.ContainsKey(id))
            {
                SpellsLibrary.Remove(id);
            }

            LoadedCharacter.RemoveSpell(id);

            //Save the changes to the abilities library to disk.
            SaveSpellsLibrary();

            //Update the list of abilities, in case something relevant changed.
            DisplaySpells(LoadedCharacter);
        }

        //Takes an updated version of an ability and puts it into the abilities library and the loaded character (as appropriate).
        public void SaveUpdatedAbility(Ability ability)
        {
            string id = ability.ID;

            //Make sure the ability is in the abilities library.
            if (AbilitiesLibrary.ContainsKey(id))
            {
                AbilitiesLibrary[id] = ability;
            }
            else
            {
                AbilitiesLibrary.Add(id, ability);
            }

            //Make sure the ability is in the character, as appropriate.
            LoadedCharacter.AddOrUpdateAbility(ability, false, true);

            //Save the changes to the abilities library to disk.
            SaveAbilitiesLibrary();

            //Update the list of abilities, in case something relevant changed.
            DisplayAbilities(LoadedCharacter);
        }

        //Takes an updated version of a spell and puts it into the spells library and the loaded character (as appropriate).
        public void SaveUpdatedSpell(Spell spell)
        {
            string id = spell.ID;

            //Make sure the spell is in the abilities library.
            if (SpellsLibrary.ContainsKey(id))
            {
                SpellsLibrary[id] = spell;
            }
            else
            {
                SpellsLibrary.Add(id, spell);
            }

            //Make sure the spell is in the character, as appropriate.
            LoadedCharacter.AddOrUpdateSpell(spell, false, true);

            //Save the changes to the spells library to disk.
            SaveSpellsLibrary();

            //Update the list of spells, in case something relevant changed.
            DisplaySpells(LoadedCharacter);
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the dice roller tab.
        #region Dice Tab

        //The function that adds or removes dice
        private void AddDice(object sender, EventArgs e)
        {
            //Format:
            //Starts with basic command - add / remove / set?
            //Then optional adv / dis modifier.
            //Ends with dice string.



            var btn = sender as Button;
            if (btn == null)
            {
                //Complain to a log file?
                return;
            }
            var tag = btn.Tag as string;
            if (tag == null)
            {
                //Complain to a log file?
                return;
            }
            tag = tag.Trim();

            //Parse the relevant bits out of the string.
            string regex = "^([a-zA-Z]+)\\s*(adv|dis|)\\s*(d?)(\\d+)$";
            //Match matches = Regex.Match(tag, regex, RegexOptions.IgnoreCase);
            Match matches = Regex.Match(tag, regex, RegexOptions.IgnoreCase);
            //Matches should be:
            //0: full match
            //1: command
            //2: adv or dis or empty string
            //3: d or not
            //4: die size or flat number

            if (matches.Success)
            {
                string command = matches.Groups[1].Value;
                string advdis = matches.Groups[2].Value;
                string d = matches.Groups[3].Value;
                string number = matches.Groups[4].Value;

                int advdisint = 0;
                if (advdis == "adv")
                {
                    advdisint = 1;
                }
                else if (advdis == "dis")
                {
                    advdisint = -1;
                }

                bool subtract = false;
                if (command == "add")
                {
                    //Nothing to do here.
                }
                else if (command == "subtract")
                {
                    subtract = true;
                }

                int dieSize;
                if (!int.TryParse(number, out dieSize))
                {
                    //Complain to a log file?
                    return;
                }

                bool flatBonus = (d == "d") ? false : true;

                bool temp = CurrentDice.AddOneDie(dieSize, subtract, advdisint, flatBonus);

                //to do: add some error checking here

                if (!temp)
                {
                    MessageBox.Show("Adding die failed.");
                }
            }

            UpdateDiceArrayDisplay();
        }

        //Removes all dice from the dice roller's current selection.
        private void DiceRoller_Clear_Click(object sender, EventArgs e)
        {
            CurrentDice = new DiceCollection();

            UpdateDiceArrayDisplay();
        }

        //Someone typed something into the dice string textbox.
        //We want to be able to do stuff when the enter key is pressed.
        private void diceStringBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string diceString = diceStringBox.Text;
                ProcessTextInput(diceString);
                diceStringBox.Text = "";
            }
        }

        //Translate a dice string into some rolls.
        private void ProcessTextInput(string diceString)
        {
            DiceCollection dice = new DiceCollection(diceString);
            outputDescription.Text = dice.GetDescription();
            DiceResponse resp = dice.Roll();
            outputTotal.Text = "Rolled " + resp.Total;
            LogMessage(resp.Description);

            UpdateLastDice(diceString);
        }

        //Redo the most recent roll.
        private void RerollButton_Click(object sender, EventArgs e)
        {
            DiceResponse resp = LastDice.Roll();
            outputTotal.Text = "Rolled " + resp.Total;
            LogMessage(resp.Description);
        }

        //The 'roll dice' button got clicked.
        private void RollDice_Click(object sender, EventArgs e)
        {
            string diceString = diceStringBox.Text;
            if (diceString.Length > 0)
            {
                //We prioritize text typed into the textbox.
                ProcessTextInput(diceString);
                diceStringBox.Text = "";

                UpdateLastDice(diceString);
            }
            else
            {
                //Roll dice added via the buttons.
                DiceResponse resp = CurrentDice.Roll();
                outputTotal.Text = "Rolled " + resp.Total;
                LogMessage(resp.Description);

                UpdateLastDice(CurrentDice.GetDiceString());

                CurrentDice = new DiceCollection();
                UpdateDiceArrayDisplay();
            }
        }

        //Displays the collection of dice currently selected in the dice roller.
        private void UpdateDiceArrayDisplay()
        {
            DiceArrayDisplay.Text = CurrentDice.GetDiceString();
        }

        //Updates the display for the most recent collection of dice.
        private void UpdateLastDice(string diceString)
        {
            LastDice = new DiceCollection(diceString);
            LastRollDiceString.Text = LastDice.GetDiceString();
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the character tab.

        //The main body of the character tab.
        #region Character Tab: General

        //The onclick even for the save button. Saves the character.
        private void Button_SaveCharacter_Click(object sender, EventArgs e)
        {
            bool resp = LoadedCharacter.Save(Settings);

            //Display a message indicating success / failure.
            if (resp)
            {
                LogMessage("Character saved successfully.");
            }
            else
            {
                LogMessage("Could not save character.");
            }

            //This may be a new character, so we'll redraw the list.
            DrawListOfCharacters(Dropdown_CharactersList);
        }

        //Deal damage to the active character.
        private void Damage(object sender, EventArgs e)
        {
            //Collect the amount to deal.
            var btn = sender as Button;
            if (btn == null)
            {
                //Complain to a log file?
                return;
            }
            var tag = btn.Tag as string;
            if (tag == null)
            {
                //Complain to a log file?
                return;
            }
            tag = tag.Trim();

            int amount;
            Int32.TryParse(tag, out amount);

            LoadedCharacter.Damage(amount);
            UpdateHealthDisplay();
        }

        //Display a character's info in the character tab.
        private void DisplayCharacter(Character.Character character)
        {
            //Basics
            Char_ID.Text = character.ID;
            Char_Name.Text = character.Name;
            Char_Race.Text = character.Race;

            //Health
            UpdateHealthDisplay();

            //Stats
            Char_Str.Text = character.GetStr().ToString();
            Char_Dex.Text = character.GetDex().ToString();
            Char_Con.Text = character.GetCon().ToString();
            Char_Int.Text = character.GetInt().ToString();
            Char_Wis.Text = character.GetWis().ToString();
            Char_Cha.Text = character.GetCha().ToString();
            Char_StrMod.Text = character.GetStrMod().ToString();
            Char_DexMod.Text = character.GetDexMod().ToString();
            Char_ConMod.Text = character.GetConMod().ToString();
            Char_IntMod.Text = character.GetIntMod().ToString();
            Char_WisMod.Text = character.GetWisMod().ToString();
            Char_ChaMod.Text = character.GetChaMod().ToString();
            UpdateStatButtonLabels();

            Char_Prof.Text = character.Prof.ToString();

            DisplayClassList(character);

            DisplayAbilities(character);

            DisplaySpells(character);

            DisplaySpellSlots();

            //Generic abilities
            BasicAbilitiesArea.Rows.Clear();
            List<Ability> basicAbilities = character.GetBasicAbilities();
            for (int a = 0; a < basicAbilities.Count(); a++)
            {
                Ability thisAbility = basicAbilities[a];

                string id = thisAbility.ID;
                string name = thisAbility.Name;
                string text = thisAbility.Text;
                string recharge = thisAbility.RechargeCondition;
                string dice = thisAbility.getDiceString();
                //usesString?

                //number id name desc button
                string[] allOfRow = { (a + 1).ToString(), id, name, text, "Use" };
                BasicAbilitiesArea.Rows.Add(allOfRow);
            }
        }

        //Heal the active character.
        private void Heal(object sender, EventArgs e)
        {
            //Collect the amount to deal.
            var btn = sender as Button;
            if (btn == null)
            {
                //Complain to a log file?
                return;
            }
            var tag = btn.Tag as string;
            if (tag == null)
            {
                //Complain to a log file?
                return;
            }
            tag = tag.Trim();

            int amount;
            Int32.TryParse(tag, out amount);

            LoadedCharacter.Heal(amount);
            UpdateHealthDisplay();
        }

        //Onclick event used for 'click button to roll stat/skill/whatever' buttons.
        private void RollDice(object sender, EventArgs e)
        {
            //Collect the dice to roll
            var btn = sender as Button;
            if (btn == null)
            {
                //Complain to a log file?
                return;
            }
            var tag = btn.Tag as string;
            if (tag == null)
            {
                //Complain to a log file?
                return;
            }

            tag = tag.Trim();

            //Parse the relevant bits out of the string.
            //What we're looking for:
            //Starts with a command, probably roll, but I might add other stuff later.
            //Then there will be some form of dice string.
            string regex = "^([a-zA-Z]+)\\s+(.+)$";
            //Match matches = Regex.Match(tag, regex, RegexOptions.IgnoreCase);
            Match matches = Regex.Match(tag, regex, RegexOptions.IgnoreCase);
            //Matches should be:
            //0: full match
            //1: command
            //2: dice string

            if (matches.Success)
            {
                string command = matches.Groups[1].Value;
                string diceString = matches.Groups[2].Value;

                command = command.Trim();
                command = command.ToLower();

                if (command == "roll")
                {
                    DiceCollection d = new DiceCollection(diceString);
                    DiceResponse resp = LoadedCharacter.RollForCharacter(d);

                    outputTotal.Text = "Rolled " + resp.Total;
                    LogMessage(resp.Description);

                    UpdateDiceArrayDisplay();
                }
                else
                {
                    //Complain to a log file?
                    MessageBox.Show("Error: unsupported command");
                }
            }
            else
            {
                //Complain to a log file?
                MessageBox.Show("Could not parse command.");
            }
        }

        //Gives or sets temp HP
        //Give: sets the character's temp HP to the number specified, unless it's already higher.
        //Set: sets the character's temp HP to the number specified, even if it's already higher.
        private void TempHP(object sender, EventArgs e)
        {
            //First figure out whether to add or set temp HP.
            //give is true, set is false.
            var btn = sender as Button;
            if (btn == null)
            {
                //Complain to a log file?
                return;
            }
            var tag = btn.Tag as string;
            if (tag == null)
            {
                //Complain to a log file?
                return;
            }
            tag = tag.Trim();
            bool onlyIncrease = (tag == "give") ? true : false;

            //Then get the number from the text box.
            String temp = Textbox_TempHP.Text;
            if (temp.Trim().Length == 0)
            {
                return;
            }
            int number;
            Int32.TryParse(temp, out number);

            //Then call the function.
            LoadedCharacter.SetTempHP(number, onlyIncrease);
            UpdateHealthDisplay();
        }

        //Displays the character's current + temp HP out of their max HP.
        private void UpdateHealthDisplay()
        {
            string healthString;
            if (LoadedCharacter.TempHP > 0)
            {
                healthString = LoadedCharacter.CurrentHP + " + " + LoadedCharacter.TempHP + " (" + (LoadedCharacter.CurrentHP + LoadedCharacter.TempHP) + ") / " + LoadedCharacter.MaxHP;
            }
            else
            {
                healthString = LoadedCharacter.CurrentHP + " / " + LoadedCharacter.MaxHP;
            }

            Char_Health.Text = healthString;
        }

        //Sets the appropriate bonuses into the labels for the various stat, save, and skill buttons.
        private void UpdateStatButtonLabels()
        {
            //Stats:
            Button_Str.Text = "Strength " + LoadedCharacter.GetBonusForRollAsString("str");
            Button_Dex.Text = "Dexterity " + LoadedCharacter.GetBonusForRollAsString("dex");
            Button_Con.Text = "Constitution " + LoadedCharacter.GetBonusForRollAsString("con");
            Button_Int.Text = "Intelligence " + LoadedCharacter.GetBonusForRollAsString("int");
            Button_Wis.Text = "Wisdom " + LoadedCharacter.GetBonusForRollAsString("wis");
            Button_Cha.Text = "Charisma " + LoadedCharacter.GetBonusForRollAsString("cha");

            //Saves:
            Button_StrSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("strsave");
            Button_DexSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("dexsave");
            Button_ConSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("consave");
            Button_IntSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("intsave");
            Button_WisSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("wissave");
            Button_ChaSave.Text = "Save " + LoadedCharacter.GetBonusForRollAsString("chasave");

            //Skills:
            Button_Athletics.Text = "Athletics " + LoadedCharacter.GetBonusForRollAsString("Athletics");
            Button_Acrobatics.Text = "Acrobatics " + LoadedCharacter.GetBonusForRollAsString("Acrobatics");
            Button_SleightOfHand.Text = "Sleight of Hand " + LoadedCharacter.GetBonusForRollAsString("Sleight of hand");
            Button_Stealth.Text = "Stealth " + LoadedCharacter.GetBonusForRollAsString("Stealth");
            Button_Arcana.Text = "Arcana " + LoadedCharacter.GetBonusForRollAsString("Arcana");
            Button_History.Text = "History " + LoadedCharacter.GetBonusForRollAsString("History");
            Button_Investigation.Text = "Investigation " + LoadedCharacter.GetBonusForRollAsString("Investigation");
            Button_Nature.Text = "Nature " + LoadedCharacter.GetBonusForRollAsString("Nature");
            Button_Religion.Text = "Religion " + LoadedCharacter.GetBonusForRollAsString("Religion");
            //Button_AnimalHandling.Text = "Animal Handling " + LoadedCharacter.GetBonusForRollAsString("Animal Handling");
            Button_AnimalHandling.Text = "Animal Handl. " + LoadedCharacter.GetBonusForRollAsString("Animal Handling");
            Button_Insight.Text = "Insight " + LoadedCharacter.GetBonusForRollAsString("Insight");
            Button_Medicine.Text = "Medicine " + LoadedCharacter.GetBonusForRollAsString("Medicine");
            Button_Perception.Text = "Perception " + LoadedCharacter.GetBonusForRollAsString("Perception");
            Button_Survival.Text = "Survival " + LoadedCharacter.GetBonusForRollAsString("Survival");
            Button_Deception.Text = "Deception " + LoadedCharacter.GetBonusForRollAsString("Deception");
            Button_Intimidation.Text = "Intimidation " + LoadedCharacter.GetBonusForRollAsString("Intimidation");
            Button_Performance.Text = "Performance " + LoadedCharacter.GetBonusForRollAsString("Performance");
            Button_Persuasion.Text = "Persuasion " + LoadedCharacter.GetBonusForRollAsString("Persuasion");


        }

        #endregion

        //Functions for the list of classes on the character tab.
        #region Character Tab: Classes Area

        //Handles the onclick code for the classes list DataGridView.
        private void ClassesArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //First we figure out which button was clicked.
            //Column:
            int colIndex = e.ColumnIndex;
            string colName = ClassesArea.Columns[colIndex].Name;

            //Row:
            int rowNum = e.RowIndex;
            string className;
            string subclassName;
            try
            {
                className = ClassesArea.Rows[rowNum].Cells[0].Value.ToString();
                subclassName = ClassesArea.Rows[rowNum].Cells[1].Value.ToString();
            }
            catch
            {
                className = "";
                subclassName = "";
            }
            className ??= "";

            if (colName == "SpendHDButton")
            {
                DiceResponse resp = LoadedCharacter.SpendHDByClass(className, 1, false);

                LogMessage(resp.Description);

                //We also want to update the HP display.
                UpdateHealthDisplay();
            }
            else if (colName == "AddHDButton")
            {
                LoadedCharacter.AddOrSubtractHDForClass(className, 1);
            }
            else if (colName == "SubtractHDButton")
            {
                LoadedCharacter.AddOrSubtractHDForClass(className, -1);
            }

            //Update the display.
            DisplayClassList(LoadedCharacter);
        }

        //Displays the currently loaded character's classes in the form.
        private void DisplayClassList(Character.Character character)
        {
            ClassesArea.Rows.Clear();

            for (int a = 0; a < character.Classes.Count; a++)
            {
                ClassLevel thisClass = character.Classes[a];

                string name = thisClass.Name;
                string subclass = thisClass.Subclass;
                int level = thisClass.Level;
                string levelString = level.ToString();
                string hd = thisClass.CurrentHD.ToString() + " / " + thisClass.Level.ToString() + "d" + thisClass.HDSize.ToString();
                string spendButton = "Spend";
                string plusButton = "+1";
                string minusButton = "-1";

                string[] row = { name, subclass, levelString, hd, spendButton, plusButton, minusButton };
                ClassesArea.Rows.Add(row);
            }
        }

        #endregion

        //Functions relating to the section of the character tab with various conditions for recharging abilities.
        #region Character Tab: Recharge Area

        //Draws the list of options.
        //This will always have "Long Rest" and "Short Rest"
        //This may have more, depending on which abilities are present.
        private void DrawRecharges()
        {
            RechargesArea.Rows.Clear();

            List<string> recharges = LoadedCharacter.GetAbilityRechargeConditions();

            for (int a = 0; a < recharges.Count; a++)
            {
                RechargesArea.Rows.Add(recharges[a], "Trigger");
            }
        }

        //Handles the onclick code for the recharge list DataGridView.
        private void RechargesArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //First we figure out which button was clicked.
            //Column:
            int colIndex = e.ColumnIndex;
            string colName = RechargesArea.Columns[colIndex].Name;

            //Row:
            int rowNum = e.RowIndex;
            string? rechargeCondition = RechargesArea.Rows[rowNum].Cells[0].Value.ToString();

            if (rechargeCondition == null)
            {
                //Complain to a log file?
                return;
            }

            if (colName == "Recharge_Button")
            {
                LoadedCharacter.RechargeAbilities(rechargeCondition);
                LoadedCharacter.RechargeSpellSlots(rechargeCondition);
            }

            DisplayClassList(LoadedCharacter);
            DisplayAbilities(LoadedCharacter);
            DisplaySpellSlots();
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the abilities tab.
        #region Abilities Tab

        //Handlers for the buttons in the abilities list.
        private void AbilitiesArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //Complain to a log file?
                return;
            }
            string colName = AbilitiesArea.Columns[colIndex].Name;

            int rowNum = e.RowIndex;
            if (rowNum < 0)
            {
                //This is mainly triggered by someone clicking on one of the column headers, which registers as row -1.
                return;
            }
            string? abilityID = AbilitiesArea.Rows[rowNum].Cells[1].Value.ToString();
            if (abilityID == null)
            {
                //Complain to a log file?
                return;
            }

            if (colName == "Abilities_UseButtonCol")
            {
                DiceResponse resp = LoadedCharacter.UseAbility(abilityID);
                LogMessage(resp.Description);
            }
            else if (colName == "Abilities_Plus1Col")
            {
                bool resp = LoadedCharacter.ChangeAbilityUses(abilityID, 1);
            }
            else if (colName == "Abilities_Minus1Col")
            {
                bool resp = LoadedCharacter.ChangeAbilityUses(abilityID, -1);
            }
            else
            {
                //We don't want to redraw everything.
                return;
            }

            //Get the current scroll location.
            int scrollloc = AbilitiesArea.FirstDisplayedScrollingRowIndex;

            //Update everything in case stuff changed.
            DisplayCharacter(LoadedCharacter);
            DisplayClassList(LoadedCharacter);
            UpdateHealthDisplay();

            //Now scroll back to that same spot.
            AbilitiesArea.FirstDisplayedScrollingRowIndex = scrollloc;
        }

        //Used to trigger the expansion / contraction of rows for the details views.
        private void AbilitiesArea_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Check if this row is currently selected.
            int rowNum = e.RowIndex;
            if (rowNum < 0)
            {
                return;
            }

            //Exclude the 3 button columns.
            int colIndex = e.ColumnIndex;
            string colName = AbilitiesArea.Columns[colIndex].Name;
            if (colName == "Abilities_UseButtonCol" || colName == "Abilities_Plus1Col" || colName == "Abilities_Minus1Col")
            {
                return;
            }

            if (AbilitiesArea.SelectedRows.Count == 0)
            {
                //The row just got unselected somehow.
                //I don't think this is possible without multiselect.
            }
            else if (AbilitiesArea.SelectedRows[0].Index != rowNum)
            {
                //A different row just got selected.
                //Can this happen?
                ToggleGridExpand("Abilities", rowNum);
            }
            else
            {
                //This row just got selected.
                ToggleGridExpand("Abilities", rowNum);
            }
        }

        //to do: remove?
        private void AbilitiesArea_Click(object sender, EventArgs e)
        {
            //Not currently used, EventArgs doesn't include the info we need from DataGridViewCellEventArgs.
        }

        //Triggered when the abilities list scrolls. Updates the positions of any ability detail panels.
        private void AbilitiesArea_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateAbilitiesAreaDetails();
        }

        //Called when the abilities list is sorted. Makes sure the details panels get moved appropriately.
        private void AbilitiesArea_Sorted(object sender, EventArgs e)
        {
            UpdateAbilitiesAreaDetails();
        }

        //Triggers the appropriate sorting function when the user tells the program to sort the abilities list.
        private void AbilitiesArea_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int colIndex = e.Column.Index;
            string colName = AbilitiesArea.Columns[colIndex].Name;

            //Sorting abilities generally revolves around more than just the column in question, so we need the abilities in question.
            int index1 = e.RowIndex1;
            int index2 = e.RowIndex2;

            //Get the two ids here
            string? id1 = AbilitiesArea.Rows[index1].Cells["Abilities_IDCol"].Value.ToString();
            string? id2 = AbilitiesArea.Rows[index2].Cells["Abilities_IDCol"].Value.ToString();
            if (id1 == null || id2 == null)
            {
                //Something has gone wrong, let the auto sort handle it because maybe it'll be right.
                return;
            }

            Ability ability1 = LoadedCharacter.GetAbilityByID(id1);
            Ability ability2 = LoadedCharacter.GetAbilityByID(id2);

            string sortcol = AbilitiesArea.SortedColumn.Name;
            string sortdir = AbilitiesArea.SortOrder.ToString();
            //Will be "Ascending" or "Descending"

            bool sortReverse = (sortdir == "Ascending") ? false : true;

            if (colName == "Abilities_NameCol")
            {
                e.Handled = true;
                e.SortResult = ability1.Compare(ability2, "name", sortReverse);
                return;
            }
            else if (colName == "Abilities_RechargeCol")
            {
                e.Handled = true;
                e.SortResult = ability1.Compare(ability2, "rechargecondition", sortReverse);
                return;
            }
            else if (colName == "Abilities_DiceCol")
            {
                e.Handled = true;
                e.SortResult = ability1.Compare(ability2, "dice", sortReverse);
                return;
            }
            else if (colName == "Abilities_UsesCol")
            {
                e.Handled = true;
                e.SortResult = ability1.Compare(ability2, "usescol", sortReverse);
                return;
            }
            else
            {
                //Unsupported column, let it be sorted automatically.
            }
        }

        //Populate the details panel for the abilities list when it appears.
        private void CreateAbilityDetailPanel(Panel p, DataGridViewRow row)//and row info?
        {
            Label newLabel = new Label();
            newLabel.Text = row.Cells["Abilities_TextCol"].Value.ToString();
            newLabel.Left = 6;
            newLabel.Top = 6;

            //Set height and width to the panel's height and width -12 each
            newLabel.Width = AbilitiesArea.Rows[0].AccessibilityObject.Bounds.Width - 12;
            //newLabel.Height = 88; //hardcoded 100px height - 12px for margins = 88px
            //Figure out how tall the text area wants to be, then add 12px for a margin.
            int temp = newLabel.GetPreferredSize(new Size(newLabel.Width, 100)).Height + 12;
            newLabel.Height = temp;
            p.Controls.Add(newLabel);
        }

        //Display a character's abilities in the abilities tab.
        private void DisplayAbilities(Character.Character character)
        {
            AbilitiesArea.Rows.Clear();

            for (int a = 0; a < character.GetAbilities().Count(); a++)
            {
                Ability thisAbility = character.GetAbilities()[a];
                string id = thisAbility.ID;
                string name = thisAbility.Name;
                string text = thisAbility.Text;
                string recharge = thisAbility.RechargeCondition;
                string dice = thisAbility.getDiceString();
                string usesString = thisAbility.getUsesString();
                string usesChange = thisAbility.UsesChange.ToString();

                AbilitiesArea.Rows.Insert(a, a + 1, id, name, text, recharge, dice, usesString, usesChange, "Use", "+1", "-1");

                //Clicks handled by AbilitiesArea_CellContentClick()
            }
            UpdateAbilitiesAreaDetails();
        }

        //Handles the positioning of abilities area detail thingies, including for scrolling.
        private void UpdateAbilitiesAreaDetails()
        {
            if (AbilitiesAreaDetails == null || AbilitiesAreaDetails.Count == 0)
            {
                //We're already done.
                return;
            }

            foreach (var thingy in AbilitiesAreaDetails)
            {
                string id = thingy.Key;
                SetAndPositionDetailPanel("Abilities", id);
            }
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the spells tab.

        //The spell slots display.
        #region Magic Tab: Spell Slots

        //Displays the list of the character's spell slots in the magic tab.
        private void DisplaySpellSlots()
        {
            SpellSlotsList.Rows.Clear();

            for (int a = 1; a <= 9; a++)
            {
                //Standard slots first.
                if (LoadedCharacter.SpellSlotsMax[a] > 0)
                {
                    string usesString = LoadedCharacter.SpellSlotsCurrent[a].ToString() + " / " + LoadedCharacter.SpellSlotsMax[a].ToString();
                    SpellSlotsList.Rows.Add(a, "Standard", usesString, "+1", "-1");
                }

                //Warlock slots second.
                if (a <= 5 && LoadedCharacter.SpellSlotsWarlockMax[a] > 0)
                {
                    string usesString = LoadedCharacter.SpellSlotsWarlockCurrent[a].ToString() + " / " + LoadedCharacter.SpellSlotsWarlockMax[a].ToString();
                    SpellSlotsList.Rows.Add(a, "Warlock", usesString, "+1", "-1");
                }
            }

            return;
        }

        //Handles the onclick for the two button columns in the spell slots list.
        private void SpellSlotsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //Complain to a log file?
                return;
            }
            string colName = SpellSlotsList.Columns[colIndex].Name;

            int rowNum = e.RowIndex;

            //Figure out which type of spell slot we're looking at.\
            string levelString;
            string type;
            try
            {
                levelString = SpellSlotsList.Rows[rowNum].Cells[0].Value.ToString();
                type = SpellSlotsList.Rows[rowNum].Cells[1].Value.ToString();
            }
            catch
            {
                //Complain to a log?
                MessageBox.Show("Error: unable to identify spell slot level or type.");
                return;
            }

            //Convert the level we got from a string to an int.
            int tempint;
            int level;
            if (Int32.TryParse(levelString, out tempint))
            {
                level = tempint;
            }
            else
            {
                //Complain to a log?
                return;
            }

            if (colName == "SpellSlots_PlusOne")
            {
                ReplenishSpellSlot(level, type);
            }
            else if (colName == "SpellSlots_MinusOne")
            {
                SpendSpellSlot(level, type);
            }
            else
            {
                //This is a different column, we don't need to do anything.
            }
        }

        //Replenishes 1 spell slot for the specified level and type.
        //Type can be "standard" or "warlock"
        private void ReplenishSpellSlot(int level, string type)
        {
            type = type.ToLower();

            bool resp = LoadedCharacter.ChangeSpellSlots(level, type, 1);
            if (!resp)
            {
                //We were unable to add this spell slot. Are there any to replenish here?
                //We'll consider the matter resolved and not do anything.
            }

            //Update the list.
            DisplaySpellSlots();
        }

        //Spends 1 spell slot for the specified level and type.
        private void SpendSpellSlot(int level, string type)
        {
            type = type.ToLower();

            bool resp = LoadedCharacter.ChangeSpellSlots(level, type, -1);
            if (!resp)
            {
                //We were unable to add this spell slot. Are there any to replenish here?
                //We'll consider the matter resolved and not do anything.
            }

            //Update the list.
            DisplaySpellSlots();
        }

        #endregion

        //The list of available spells.
        #region Magic Tab: Spells List

        //Populate the details panel for the spells list when it appears.
        private void CreateSpellDetailPanel(Panel p, DataGridViewRow row)
        {
            //spell school goes here
            //more in the same row?
            //duration
            //book and page
            //expensive material component?

            Label descriptionLabel = new Label();
            descriptionLabel.Text = row.Cells["Spells_DescriptionCol"].Value.ToString();
            descriptionLabel.Left = 6;
            descriptionLabel.Top = 21;  //place it 1 row down

            //Set height and width to the panel's height and width -12 each
            descriptionLabel.Width = SpellsArea.Rows[0].AccessibilityObject.Bounds.Width - 12;
            descriptionLabel.Height = 88; //hardcoded 100px height - 12px for margins = 88px

            //upcasting benefit goes here

            p.Controls.Add(descriptionLabel);
        }

        //Display a character's spells in the spells tab.
        private void DisplaySpells(Character.Character character)
        {
            SpellsArea.Rows.Clear();

            for (int spellNum = 0; spellNum < character.Spells.Count(); spellNum++)
            {
                Spell thisSpell = character.Spells[spellNum];
                string id = thisSpell.ID;
                string name = thisSpell.Name;
                int level = thisSpell.Level;
                string school = thisSpell.School;
                bool vocal = thisSpell.Vocal;
                string vocalString = (vocal == true) ? "X" : "";
                bool somatic = thisSpell.Somatic;
                string somaticString = (somatic == true) ? "X" : "";
                bool material = thisSpell.Material;
                string materialString = (material == true) ? "X" : "";
                //material + expensivematerial as single entry?
                //write a function for this?
                string action = thisSpell.Action;
                string description = thisSpell.Description;
                string upcastingBenefit = thisSpell.UpcastingBenefit;
                string range = thisSpell.Range;
                int duration = thisSpell.Duration;
                string durationString = (duration > 0) ? duration.ToString() : "";
                //extra logic
                //convert to 1 minute / 1 hour / etc as appropriate?
                bool concentration = thisSpell.Concentration;
                string concentrationString = (concentration == true) ? "X" : "";
                string book = thisSpell.Book;
                int page = thisSpell.Page;


                //to do:
                //add hidden columns for description and upcasting benefit
                //and more?
                //how to display book and page? Details view only?

                SpellsArea.Rows.Insert(spellNum, spellNum + 1, id, name, level, school, range, durationString, concentrationString, vocalString, somaticString, materialString, action, description, upcastingBenefit, "Cast", "Upcast", book, page);

                //Now we potentially blank out the upcast button.
                //This is done for cantrips.
                //Eventually this will be done when there aren't any spell slots available of a high enough level.

                if (level == 0)
                {
                    //Cantrips can never be upcast.
                    //SpellsArea.Rows[a].Cells["Spells_UpcastCol"].Style.
                }
                else
                {
                    int numSlots = LoadedCharacter.GetSpellSlotsForLevel(level);
                    if (numSlots == 0)
                    {
                        //There aren't enough slots of this level.
                        //Disable the cast button
                    }

                    numSlots = 0;
                    for (int slvl = level + 1; slvl <= 9; slvl++)
                    {
                        numSlots += LoadedCharacter.GetSpellSlotsForLevel(slvl);
                    }
                    if (numSlots == 0)
                    {
                        //There are no higher-level spell slots available.
                        //Disable the upcast button.
                    }
                    //idea: set the padding-top on the cells to a large enough number to drop the button out of sight.
                    //complication: when restoring rows to normal height, we'll need to:
                    //set the padding to 0 on these cells
                    //set the height to the auto calculated value
                    //re-set the padding to hide the button(s).
                    //store padding values before zeroing, or just re-do this calculation?
                    //Actually, if this is being redrawn as a result of the change (which it should be), this calculation will be redone anyway.
                }
            }

            UpdateSpellsAreaDetails();
        }

        //Controls the addition and removal of details areas.
        private void SpellsArea_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Check if this row is currently selected.
            int rowNum = e.RowIndex;
            if (rowNum < 0)
            {
                return;
            }

            //Exclude the 3 button columns.
            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //This should just be because the user clicked on the header row, so we can ignore it.
                return;
            }
            string colName = SpellsArea.Columns[colIndex].Name;
            if (colName == "Spells_CastCol" || colName == "Spells_UpcastCol")
            {
                return;
            }

            if (SpellsArea.SelectedRows.Count == 0)
            {
                //The row just got unselected somehow.
                //I don't think this is possible without multiselect.
            }
            else if (SpellsArea.SelectedRows[0].Index != rowNum)
            {
                //A different row just got selected.
                //Can this happen?
                ToggleGridExpand("Spells", rowNum);
            }
            else
            {
                //This row just got selected.
                ToggleGridExpand("Spells", rowNum);
            }
        }

        //Handles things when someone clicks on one of the buttons.
        private void SpellsArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //Complain to a log file?
                return;
            }
            string colName = SpellsArea.Columns[colIndex].Name;

            int rowNum = e.RowIndex;
            if (rowNum < 0)
            {
                //This is mainly triggered by someone clicking on one of the column headers, which registers as row -1.
                return;
            }
            string? spellID = SpellsArea.Rows[rowNum].Cells["Spells_IDCol"].Value.ToString();
            if (spellID == null)
            {
                //Complain to a log file?
                return;
            }

            //We'll want the name for when we put a message in the log.
            string? spellName = SpellsArea.Rows[rowNum].Cells["Spells_NameCol"].Value.ToString();

            if (colName == "Spells_CastCol")
            {
                //Attempt to cast the spell.
                bool resp = LoadedCharacter.CastSpell(spellID);
                if (resp)
                {
                    LogMessage("Cast spell " + spellName);
                }
                else
                {
                    LogMessage("Failed to cast spell " + spellName);
                }
            }
            else if (colName == "Spells_UpcastCol")
            {
                //Trigger a popup with buttons for each possible upcast level.
                //Also the description of what's gained by upcasting.
                CreateUpcastPanel(spellID);
            }
            else
            {
                //We don't want to redraw everything.
                return;
            }

            //Get the current scroll location.
            int scrollloc = SpellsArea.FirstDisplayedScrollingRowIndex;

            DisplayAbilities(LoadedCharacter);
            DisplaySpellSlots();

            //Now scroll back to that same spot.
            SpellsArea.FirstDisplayedScrollingRowIndex = scrollloc;
        }

        //to do: remove?
        private void SpellsArea_Click(object sender, EventArgs e)
        {
            //Not currently used, EventArgs doesn't include the info we need from DataGridViewCellEventArgs.
        }

        //Manages updating things as appropriate when the user scrolls the list.
        private void SpellsArea_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateSpellsAreaDetails();
        }

        //Function to compare two rows within the spells list, for use in sorting by the various columns.
        private void SpellsArea_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int colIndex = e.Column.Index;
            string colName = SpellsArea.Columns[colIndex].Name;

            //Sorting abilities generally revolves around more than just the column in question, so we need the abilities in question.
            int index1 = e.RowIndex1;
            int index2 = e.RowIndex2;

            //Get the two ids here
            string? id1 = SpellsArea.Rows[index1].Cells["Spells_IDCol"].Value.ToString();
            string? id2 = SpellsArea.Rows[index2].Cells["Spells_IDCol"].Value.ToString();
            if (id1 == null || id2 == null)
            {
                //Something has gone wrong, let the auto sort handle it because maybe it'll be right.
                return;
            }

            Spell? spell1 = LoadedCharacter.GetSpellByID(id1);
            Spell? spell2 = LoadedCharacter.GetSpellByID(id2);
            if (spell1 == null || spell2 == null)
            {
                //Complain to a log file
                return;
            }

            if (colName == "Spells_NameCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "name");
                return;
            }
            else if (colName == "Spells_LevelCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "level");
                return;
            }
            else if (colName == "Spells_SchoolCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "school");
                return;
            }
            else if (colName == "Spells_RangeCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "range");
                return;
            }
            else if (colName == "Spells_DurationCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "duration");
                return;
            }
            else if (colName == "Spells_ConcentrationCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "concentration");
                return;
            }
            else if (colName == "Spells_VocalCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "vocal");
                return;
            }
            else if (colName == "Spells_SomaticCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "somatic");
                return;
            }
            else if (colName == "Spells_MaterialCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "material");
                return;
            }
            else if (colName == "Spells_ActionCol")
            {
                e.Handled = true;
                e.SortResult = spell1.Compare(spell2, "action");
                return;
            }
            else
            {
                //Unsupported column, let it be sorted automatically.
            }
        }

        //Called when the program finishes sorting the spells list. Triggers everything necessary at that time.
        private void SpellsArea_Sorted(object sender, EventArgs e)
        {
            UpdateSpellsAreaDetails();
        }

        //Handles the positioning of spells area detail thingies, including for scrolling.
        private void UpdateSpellsAreaDetails()
        {
            if (SpellsAreaDetails == null || SpellsAreaDetails.Count == 0)
            {
                //We're already done.
                return;
            }

            foreach (var thingy in SpellsAreaDetails)
            {
                string id = thingy.Key;

                SetAndPositionDetailPanel("Spells", id);

            }
        }

        #endregion

        //Functions for creating and managing the little panel that pops up when someone clicks the upcast button on one of the spells.
        //coming soon
        #region Magic Tab: Upcast Panel

        private void CloseUpcastPanel()
        {

        }

        //Create the popup panel for upcasting a spell.
        private void CreateUpcastPanel(string spellID)
        {
            //to do: consider passing in the spell object
            //we can access the character via this form

            //to do: I'll want to store a reference to this somewhere, so I can delete it later.
            //clicking elsewhere should close it.
            //just declare a Panel? variable somewhere?

            //get the spell object by it's ID.

            Panel newPanel = new Panel();
            this.Controls.Add(newPanel);


            //display some spell info
            //name, description etc

            //display upcasting benefits
            //button for casting at the regular level
            //handle this like the upcast buttons, but with a 0 shoved somewhere for number of additional spell levels?
            //store things based on spell slot level, not spell slot level offset?
            //buttons for casting at every higher level
            //see if i can visibly disable buttons for spell levels that the character is out of slots for.
            //maybe just leave gaps for nonfunctional buttons
            //button to close the popup

            //position the panel
            int left = 100;
            int top = 100;
            int width = 100;
            int height = 100;
            Rectangle rect = new Rectangle(left, top, width, height);
            newPanel.Bounds = rect;
            newPanel.Visible = true;
            newPanel.BringToFront();
        }

        private void UpcastSpell(string spellID)
        {

        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Functions that relate to the Settings tab.
        #region Tab: Settings

        //Called when the Create New Character button is clicked.
        //Loads a blank character ready for editing.
        private void Button_CreateNewCharacter_Click(object sender, EventArgs e)
        {
            LoadCharacter("");
        }

        //Called when the Load Character button is clicked.
        //Triggers a load of the character currently selected in Dropdown_CharactersList.
        private void Button_LoadCharacter_Click(object sender, EventArgs e)
        {
            //Get the character to load.
            string charname = Dropdown_CharactersList.Text;
            if (charname == "" || charname == null)
            {
                //There is no character selected, just quit.
                return;
            }

            //Check if we can find the appropriate file.
            Dictionary<string, string> charactersList = GetListOfCharacters();

            if (charactersList.ContainsKey(charname))
            {
                string filename = charactersList[charname];

                LoadCharacter(filename);
            }
        }

        //Displays the various settings in the settings tab.
        private void DisplaySettings()
        {
            Settings_SaveLocation.Text = Settings.GetBaseFolderPath();
        }

        //Populates the character select dropdown on the settings tab.
        private void DrawListOfCharacters(ComboBox? cb)
        {
            //Clear the list so we can just redraw everything.
            Dropdown_CharactersList.Items.Clear();

            //Get a list of characters to include, and refer to them by character name and filename.
            Dictionary<string, string> characters = GetListOfCharacters();

            foreach (KeyValuePair<string, string> character in characters)
            {
                string charID = character.Key;
                if (cb == null)
                {
                    Dropdown_CharactersList.Items.Add(charID);
                } else
                {
                    cb.Items.Add(charID);
                }
            }
        }

        private Dictionary<string, string> GetListOfCharacters()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string charactersFolder = Settings.GetCharacterFolderPath();

            //Make sure the characters folder exists before we try to access it.
            if (!Directory.Exists(charactersFolder))
            {
                //complain to a log file?
                return ret;
            }

            //Compile a list of characters and store the associated name + filename.
            string[] filepaths = Directory.GetFiles(charactersFolder, "*.char");
            for (int a = 0; a < filepaths.Length; a++)
            {
                //Pull out the relevant information.
                //The filename is based on the character's ID, but may have some characters removed.
                //We want the name and/or ID
                //We'll want the filename as well

                string filepath = filepaths[a];
                string filename = Path.GetFileName(filepath);
                string charID = filename.Substring(0, filename.Length - 5);
                //now trim off the last / or \ and everything before.

                ret[charID] = filepath;
            }

            //to do: store the most recent character name - filename associations so we don't need to do this loop all over again.
            //just make a new class variable and stick a copy of ret into it.

            return ret;
        }

        //Called when the save button in the settings tab is clicked.
        //Saves the updated settings to disk.
        private void Settings_SaveButton_Click(object sender, EventArgs e)
        {
            //Fetch the current values to save.
            string saveLocation = Settings_SaveLocation.Text;
            //to do: check this over to make sure it's not terrible.

            Settings.Update(saveLocation);

            bool resp = Settings.SaveToDisk();
            if (resp)
            {
                LogMessage("Settings saved successfully.");
            }
            else
            {
                //complain to a log file?
                LogMessage("Error: unable to save settings.");
            }
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 






        //Things to move elsewhere or delete
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        private void Char_Name_Click(object sender, EventArgs e)
        {

        }

        private void Char_Race_Click(object sender, EventArgs e)
        {

        }

        private void CharacterTab_Click(object sender, EventArgs e)
        {

        }

        private void MainTabArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}