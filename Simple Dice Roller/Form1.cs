using ArkDice;
using Character;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text.Json;

namespace Simple_Dice_Roller
{
    //to do:
    //put in ui stuff for clicking buttons representing standard die sizes to add them to a pool / handful / whatever, then click a button to roll them.
    //for convenience, have some way to easily select recently-rolled pools.
    //behind the scenes: just add stuff to one of these.
    //eventually put in some sort of die/dice class so i can define custom dice (ie: Edge of the Empire / Age of Rebellion / Force and Destiny dice).
    //or make it an attribute of the DicePile class.


    public partial class Form1 : Form
    {
        //Attributes:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        internal List<string> LogMessages;
        //sooner or later, swap this to displaying on a DataGridView
        //Columns: name -> total -> description
        //maybe timestamp too?

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

        internal EditCharacter? EditCharacterForm;
        internal EditAbilities? EditAbilitiesForm;
        internal EditSpells? EditSpellsForm;

        public string Folderpath { get; private set; }

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public Form1()
        {
            //Used to display messages to the user near the bottom of the page.
            LogMessages = new List<string>();

            InitializeComponent();

            EditCharacterForm = null;

            //Directory that this stuff runs out of:
            //C:\Users\david\source\repos\ArkDice\Simple Dice Roller\bin\Debug\net6.0-windows

            //to do:
            //load this from a file or something.
            //make this a global variable?
            Folderpath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\";

            //A collection of spell information so we can load them by ID.
            SpellsLibrary = new Dictionary<string, Spell>();
            AbilitiesLibrary = new Dictionary<string, Ability>();

            LoadSpellsLibrary(Folderpath);
            LoadAbilitiesLibrary(Folderpath);

            //function to parse the json and load the spells.
            //idea: change this setup to there being one file per spell, and then load that file on demand.
            //is that going to be worth the performance hit for loading a bunch of files at once?

            AbilitiesAreaDetails = new Dictionary<string, Panel>();
            SpellsAreaDetails = new Dictionary<string, Panel>();

            //Currently selected dice for the dice roller.
            CurrentDice = new DiceCollection();

            //Stores the last-rolled collection of dice from the dice roller.
            LastDice = new DiceCollection();

            //Stores the currently loaded character.
            //Character.Character currentCharacter = new Character.Character(contents);
            Character.Character currentCharacter = new Character.Character("Tiriel", SpellsLibrary, Folderpath);
            LoadedCharacter = currentCharacter;
            //MessageBox.Show(currentCharacter.ToString());
            DisplayCharacter(currentCharacter);

            DrawRecharges();
        }

        //Misc functions
        //-------- -------- -------- -------- -------- -------- -------- -------- 
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

        //Looks up the index for the AbilitiesArea row with a particular ID.
        //Returns the row's index if found, or -1 if not.
        //to do: make it take in some indicator of which grid it should reference.
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
            //MessageBox.Show("Column index is " + colIndex);
            if (colIndex < 0)
            {
                //complain
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
                   Dictionary<string,Ability>? temp = JsonSerializer.Deserialize<Dictionary<string,Ability>>(fileContents);

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
                string spellContents = File.ReadAllText(filepath);
                try
                {
                    List<Spell>? temp = JsonSerializer.Deserialize<List<Spell>>(spellContents);

                    if (temp == null)
                    {
                        //Complain to a log file?
                        return false;
                    }
                    else
                    {
                        //Copy temp's contents into SpellsLibrary.
                        for (int a = 0; a < temp.Count; a++)
                        {
                            string id = temp[a].ID.ToString();
                            SpellsLibrary[id] = temp[a];
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    SpellsLibrary = new Dictionary<string, Spell>();
                    return false;
                }
            }

            return false;
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
                File.Move (filepath, backupFilepath);
            }

            //Save the new info to the file.
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string data = JsonSerializer.Serialize (AbilitiesLibrary, serializerOptions);
            File.WriteAllText(filepath, data);
            if (File.Exists(filepath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //
        //rename?
        //to do: set up auto sizing for abilities.
        //make sure the auto sizing for spells works properly.
        //go over the minimum sizes and make sure I like them.
        //change to 50? 75?
        //to do: properly calculate width.
        //check the boundaries of the leftmost and rightmost visible cells?
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
            //MessageBox.Show("This row reports that it would like to have a height of " + rowHeight);
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

            top -= gridTop;
            left -= gridLeft;

            //We want to leave 1 row worth of space above the panel.
            top += baseRowHeight;

            if (top > bottom)
            {
                //This row is too short for this.
                //complain?
                return;
                //to do: change to just hiding this one and moving on?
            }

            int width = right - left;

            //MessageBox.Show("Going to set this panel to " + left + ", " + top + " and " + width + " x " + panelHeight);
            Rectangle rect = new Rectangle(left, top, width, panelHeight);

            //Now set the panel's size and location.
            panel.Bounds = rect;
            panel.Visible = true;
            panel.BringToFront();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
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
                //complain
                MessageBox.Show("Error: could not find id for row.");
                return;
            }

            //Expand the row.
            if (details.ContainsKey(id))
            //if (grid.Rows[rowNum].Height > expandAmount)
            {
                //Remove this panel from the list.
                details[id].Dispose();
                details.Remove(id);

                //Shrink the row back to normal.
                //grid.Rows[rowNum].Height = grid.Rows[rowNum].GetPreferredHeight(rowNum, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);
            }
            else
            {
                //Expand the row and display the details view.

                if (id == null /*|| text == null*/)
                {
                    //Log something?
                    MessageBox.Show("Error: could not find id for row");
                    return;
                }

                //MessageBox.Show("Expanding row " + id);

                //Expand the row.
                //grid.Rows[rowNum].Height = grid.Rows[rowNum].Height + expandAmount;

                //Create an appropriate panel and add it to the list.
                //to do: set up a function for when the panel is clicked: treat that like clicking the row again.
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

        #region Editing Forms

        //Opens a new window for editing abilities.
        private void BeginEditingAbilities()
        {
            string id = LoadedCharacter.ID;
            if (id == "")
            {
                MessageBox.Show("Error: could not identify loaded character.");
            }
            else
            {
                EditAbilities editing = new EditAbilities(this);
                editing.Show();
                //editing.ParentForm = this;

                EditAbilitiesForm = editing;
            }
        }
        //Opens a new window for editing the current character.
        private void BeginEditingCharacter()
        {
            string id = LoadedCharacter.ID;
            if (id == "")
            {
                //what to do here?
                //just pop up a blank one?
                MessageBox.Show("Error: could not identify loaded character.");
            }
            else
            {
                //EditCharacter editing = new EditCharacter(id);
                EditCharacter editing = new EditCharacter(LoadedCharacter);
                editing.ParentForm = this;
                editing.Show();

                //Store a reference to the child so we can access it later.
                EditCharacterForm = editing;
            }
        }

        //Opens a new window for editing spells.
        private void BeginEditingSpells()
        {
            string id = LoadedCharacter.ID;
            if (id == "")
            {
                MessageBox.Show("Error: could not identify loaded character.");
            }
            else
            {
                EditSpells editing = new EditSpells();
                //editing.Parent = this;
                editing.Show();

                EditSpellsForm = editing;
            }
        }

        //The onclick event for the edit abilities button. Pops up a new window where spells can be edited and assigned.
        private void Button_EditAbilities_Click(object sender, EventArgs e)
        {
            if (EditAbilitiesForm == null)
            {
                BeginEditingAbilities();
                //Close the other popups?
            }
            else
            {
                BeginEditingAbilities();
            }
        }

        //The onclick event for the edit character button. Pops up a new window where various character attributes can be changed.
        private void Button_EditChar_Click(object sender, EventArgs e)
        {
            if (EditSpellsForm == null)
            {
                BeginEditingCharacter();
                //Close the other popups?
            }
            else
            {
                BeginEditingCharacter();
            }
        }

        //The onclick event for the edit spells button. Pops up a new window where spells can be edited and assigned.
        private void Button_EditSpells_Click(object sender, EventArgs e)
        {
            if (EditSpellsForm == null)
            {
                BeginEditingSpells();
                //Close the other popups?
            }
            else
            {
                BeginEditingSpells();
            }
        }

        //Closes the new form used for editing abilities.
        public void CloseEditingAbilities()
        {
            if (EditAbilitiesForm != null)
            {
                EditAbilitiesForm.Close();
                EditAbilitiesForm = null;
                //bring this form to the top?
            }
        }

        //Closes the new form used for editing characters.
        private void CloseEditingCharacter()
        {
            if (EditCharacterForm != null)
            {
                EditCharacterForm.Close();
                EditCharacterForm = null;
                //bring this form to the top?
            }
        }

        //Closes the new form used for editing spells.
        private void CloseEditingSpells()
        {
            if (EditSpellsForm != null)
            {
                EditSpellsForm.Close();
                //bring this form to the top?
                EditSpellsForm = null;
            }
        }

        //Called when something is closing the form for editing abilities.
        public void ClosingEditingAbilities()
        {
            EditAbilitiesForm = null;
        }

        //Called when something is closing the form for editing characters.
        public void ClosingEditingCharacter()
        {
            EditCharacterForm = null;
        }

        //Called when something is closing the form for editing spells.
        public void ClosingEditingSpells()
        {
            EditSpellsForm = null;
        }

        //Takes an updated version of an ability and puts it into the abilities library and the loaded character (as appropriate).
        public void SaveUpdatedAbility (Ability ability)
        {
            string id = ability.ID;

            //Make sure the ability is in the abilities library.
            if (AbilitiesLibrary.ContainsKey(id))
            {
                AbilitiesLibrary[id] = ability;
            } else
            {
                AbilitiesLibrary.Add(id, ability);
            }

            //Make sure the ability is in the character, as appropriate.
            LoadedCharacter.AddOrUpdateAbility(ability);

            //Save the changes to the abilities library to disk.
            SaveAbilitiesLibrary();

            //Also save the character? Maybe not.

            //Update the list of abilities, in case something relevant changed.
            DisplayAbilities(LoadedCharacter);
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        #region Dice Tab

        //The function that adds or removes dice
        private void AddDice(object sender, EventArgs e)
        {
            //Things we might get:
            //Add / remove 1 die of a particular size.
            //Add / remove multiple dice of a particular size?
            //Add / remove 1 die of advantage or disadvantage in a particular size.
            //Subtract a die from the result?
            //Format:
            //Starts with basic command. Tells this what to do with the rest of it.
            //add - adds the specified dice to the pile
            //remove - removes the specified dice from the pile
            //set? - sets the pile to what's in the dice string
            //Then adv/dis modifier, if applicable
            //Ends with dice string
            //positive / no symbol to make them regular dice, negative to make them negative.



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
            //MessageBox.Show(tag);
            //comes through as string, set the buttons up with dice strings in the standard format in their Tag attribute
            tag = tag.Trim();

            //Parse the relevant bits out of the string.
            //What we're looking for:
            //Starts with a command, probably add or subtract but might be set.
            //May contain adv or dis next, but not necessarily.
            //Then there will be either dX or X
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
                    //MessageBox.Show("Doing advantage");
                }
                else if (advdis == "dis")
                {
                    advdisint = -1;
                    //MessageBox.Show("Doing disadvantage");
                }
                else
                {
                    //MessageBox.Show("Doing regular dice");
                }

                bool subtract = false;
                if (command == "add")
                {
                    //Great
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

                //error checking?
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
            //dice.roll();
            //outputTotal.Text = "The total is " + dice.getTotal();
            //outputTotal.Text = "Rolled " + dice.getTotal();
            outputTotal.Text = "Rolled " + resp.Total;
            //logMessage (dice.getDescription());
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

        #region Character Tab: General

        //The onclick even for the save button. Saves the character.
        private void Button_SaveCharacter_Click(object sender, EventArgs e)
        {
            bool resp = LoadedCharacter.Save();

            //Display a message indicating success / failure.
            if (resp)
            {
                LogMessage("Character saved successfully.");
            }
            else
            {
                LogMessage("Could not save character.");
            }
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

            //...
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
            //MessageBox.Show("Rolling dice based off tag " + tag);

            //comes through as string, set the buttons up with dice strings in the standard format in their Tag attribute
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
                //MessageBox.Show("Found command " + command + " and dice string " + diceString);

                command = command.Trim();
                command = command.ToLower();

                if (command == "roll")
                {
                    DiceCollection d = new DiceCollection(diceString);
                    DiceResponse resp = LoadedCharacter.RollForCharacter(d);

                    outputTotal.Text = "Rolled " + resp.Total;
                    LogMessage(resp.Description);

                    //rolls for d20+0 seem to be 1-19, investigate
                    //make sure there's stuff to support 'prof bonus if proficient' type commands in dice strings
                    //whatever + profifprof(thing) + whatever ?
                    //thing would then be the name of a skill, or a stat + 'save' ie 'strsave', or a weapon name, or whatever

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
            //MessageBox.Show(tag);
            //comes through as string, set the buttons up with dice strings in the standard format in their Tag attribute
            tag = tag.Trim();
            bool onlyIncrease = (tag == "give") ? true : false;
            //to do: flip this around?


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
            Button_AnimalHandling.Text = "Animal Handling " + LoadedCharacter.GetBonusForRollAsString("Animal Handling");
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

        #region Character Tab: Classes Area
        //This region covers the list of classes on the character tab.

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
                //MessageBox.Show ("Found class ID " + className + " (" + subclassName + ")");
            }
            catch
            {
                className = "";
                subclassName = "";
            }
            className ??= "";
            //MessageBox.Show("Button clicked is " + colName + " column for class " + className + " (" + subclassName + ")");

            if (colName == "SpendHDButton")
            {
                DiceResponse resp = LoadedCharacter.SpendHDByClass(className, 1, false);
                if (resp.Success)
                {
                    //MessageBox.Show("HD response:" + resp.Total);
                }
                else
                {
                    //MessageBox.Show("HD spending failed.");
                }

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

            //update the display.
            DisplayClassList(LoadedCharacter);

            //it's doing 3 healing every time
            //it's not decrementing the hd
        }

        //Displays the currently loaded character's classes in the form.
        private void DisplayClassList(Character.Character character)
        {
            ClassesArea.Rows.Clear();

            for (int a = 0; a < character.Classes.Count; a++)
            {
                ClassLevel thisClass = character.Classes[a];

                string name = thisClass.Name;
                //MessageBox.Show("Processing class " + name);
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

        #region Character Tab: Recharge Area

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

            //MessageBox.Show("Button clicked is " + colName + " column for class " + className + " (" + subclassName + ")");

            if (colName == "Recharge_Button")
            {
                LoadedCharacter.RechargeAbilities(rechargeCondition);
            }

            DisplayClassList(LoadedCharacter);
            DisplayAbilities(LoadedCharacter);
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        #region Abilities Tab

        //Handlers for the buttons in the abilities list.
        private void AbilitiesArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Do we need to manually do something to make the row selected?

            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //Complain to a log file?
                return;
            }
            string colName = AbilitiesArea.Columns[colIndex].Name;

            //MessageBox.Show("Button clicked: col is " + colName);

            int rowNum = e.RowIndex;
            string? abilityID = AbilitiesArea.Rows[rowNum].Cells[1].Value.ToString();
            if (abilityID == null)
            {
                //Complain to a log file?
                return;
            }

            //MessageBox.Show("Button clicked: col is " + abilityID);

            //to do: consider doing this by the column index instead.
            if (colName == "Abilities_UseButtonCol")
            {
                DiceResponse resp = LoadedCharacter.UseAbility(abilityID);
                LogMessage(resp.Description);
                //MessageBox.Show(resp.description);
                //to do: check if description is actually present.
                //maybe change the class and it's getters so it'll do something special if I call getDescription() when there isn't a description?
                //with monotype variables I don't think that's going to do a lot, so maybe not.
                if (resp.Success)
                {
                    //MessageBox.Show("Ability used: total is " + resp.total + " and string is " + resp.description);
                }
                else
                {
                    //MessageBox.Show("Failed to use ability");
                }

            }
            else if (colName == "Abilities_Plus1Col")
            {
                bool resp = LoadedCharacter.ChangeAbilityUses(abilityID, 1);
                if (resp)
                {
                    //MessageBox.Show("Successfully added 1 use to " + abilityID);
                }
                else
                {
                    //MessageBox.Show("Failed to add 1 use to " + abilityID);
                }
            }
            else if (colName == "Abilities_Minus1Col")
            {
                bool resp = LoadedCharacter.ChangeAbilityUses(abilityID, -1);
                if (resp)
                {
                    //MessageBox.Show("Successfully subtracted 1 use from " + abilityID);
                }
                else
                {
                    //MessageBox.Show("Failed to subtract 1 use from " + abilityID);
                }
            }
            else
            {
                //We don't want to redraw everything.
                return;
            }

            //Update everything in case stuff changed.
            //to do:
            //redo any sorting or whatever that was done, after running these.
            //sorting
            //row selection
            //row height changing
            //Move DisplayAbility or whatever outside of DisplayCharacter, and just call them all separately?
            DisplayCharacter(LoadedCharacter);
            DisplayClassList(LoadedCharacter);
            UpdateHealthDisplay();
        }

        //Used to trigger the expansion / contraction of rows for the details views.
        private void AbilitiesArea_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //If the user clicks and drags, this triggers on the one where they let go.
            //Leave that alone?
            //Also watch where they start clicking and require that they be the same row?
            //Make this do nothing if the user moves the mouse more than x px while the button is down?

            //Check if this row is currently selected.
            int rowNum = e.RowIndex;
            if (rowNum < 0)
            {
                return;
            }
            //MessageBox.Show("Clicked on row " + rowNum);

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

            //MessageBox.Show("Sorting by column " + sortcol + " in direction " + sortdir);

            //to do: is reversing the direction supposed to be handled here, or is it done automatically?
            //that answer will change how we need to sort stuff re: display tiers.

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
            //grid.Rows[rowNum].Cells["Abilities_TextCol"].Value.ToString();
            newLabel.Left = 6;
            newLabel.Top = 6;

            //set height and width to the panel's height and width -12 each
            newLabel.Width = AbilitiesArea.Rows[0].AccessibilityObject.Bounds.Width - 12;
            newLabel.Height = 88; //hardcoded 100px height - 12px for margins = 88px
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

                //string[] allOfRow = { (a + 1).ToString(), id, name, text, recharge, dice, usesString, "Use", "+1", "-1" };
                //AbilitiesArea.Rows.Add(allOfRow);

                AbilitiesArea.Rows.Insert(a, a + 1, id, name, text, recharge, dice, usesString, usesChange, "Use", "+1", "-1");

                //Clicks handled by AbilitiesArea_CellContentClick()

                //To do:
                //Consider making the default sort order column hidden.
                //How will the user indicate they want the grid sorted by it, then?
                //Actually hook up the abilities.
                //Add an invisible column for the ID and do it by that?
                //Eventually: hide the text column and make it appear when the user clicks the row.
                //And disappear when they click another row.
                //Make sure the user can scroll horizontally when relevant.
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

            //MessageBox.Show("UpdateAbilitiesAreaDetails()");
            foreach (var thingy in AbilitiesAreaDetails)
            {
                string id = thingy.Key;
                SetAndPositionDetailPanel("Abilities", id);
            }
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        #region Magic Tab: Spells List

        //Populate the details panel for the spells list when it appears.
        private void CreateSpellDetailPanel(Panel p, DataGridViewRow row)
        {
            Label descriptionLabel = new Label();
            descriptionLabel.Text = row.Cells["Spells_DescriptionCol"].Value.ToString();
            descriptionLabel.Left = 6;
            descriptionLabel.Top = 21;  //place it 1 row down

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
                //write a function for this
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
            //MessageBox.Show("Clicked on row " + rowNum);

            //Exclude the 3 button columns.
            int colIndex = e.ColumnIndex;
            if (colIndex < 0)
            {
                //how to handle this?
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

            //MessageBox.Show("Button clicked: col is " + colName);

            int rowNum = e.RowIndex;
            string? spellID = SpellsArea.Rows[rowNum].Cells["Spells_IDCol"].Value.ToString();
            if (spellID == null)
            {
                //Complain to a log file?
                return;
            }

            //We'll want the name for when we put a message in the log.
            string? spellName = SpellsArea.Rows[rowNum].Cells["Spells_NameCol"].Value.ToString();

            //MessageBox.Show("Button clicked: col is " + abilityID);

            //to do: consider doing this by the column index instead.
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
                //trigger a popup with buttons for each possible upcast level.
                //also the description of what's gained by upcasting.
            }
            else
            {
                //We don't want to redraw everything.
                return;
            }

            DisplayAbilities(LoadedCharacter);
            //DisplayCharacter(LoadedCharacter);
            //DisplayClassList(LoadedCharacter);
            //UpdateHealthDisplay();
        }

        private void SpellsArea_Click(object sender, EventArgs e)
        {
            //Not currently used, EventArgs doesn't include the info we need from DataGridViewCellEventArgs.
        }

        private void SpellsArea_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateSpellsAreaDetails();
        }

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

            //to do: is reversing the direction supposed to be handled here, or is it done automatically?
            //that answer will change how we need to sort stuff re: display tiers.

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

            //MessageBox.Show("UpdateAbilitiesAreaDetails()");
            foreach (var thingy in SpellsAreaDetails)
            {
                string id = thingy.Key;

                SetAndPositionDetailPanel("Spells", id);

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