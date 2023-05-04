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
        internal List<string> logMessages;
        //sooner or later, swap this to displaying on a DataGridView
        //Columns: name -> total -> description
        //maybe timestamp too?

        //Hold the character that's loaded.
        internal Character.Character loadedCharacter;

        //Holds the collection of dice that are currently selected via the dice roller tab.
        internal DiceCollection currentDice;

        //Hold the most recently rolled collection of dice from the dice roller tab.
        internal DiceCollection lastDice;

        //Holds information about which rows in AbilitiesArea to display extra information for.
        //The key is the row's ID value.
        //The other one is the chunk of text to display.

        Dictionary<string, Panel> AbilitiesAreaDetails;

        //Stores a list of spells to make them easy to load later on.
        Dictionary<string, Character.Spell> SpellsLibrary;

        //Constructor(s):
        //-------- -------- -------- -------- -------- -------- -------- -------- 
        public Form1()
        {
            //Used to display messages to the user near the bottom of the page.
            logMessages = new List<string>();

            InitializeComponent();

            //Directory that this stuff runs out of:
            //C:\Users\david\source\repos\ArkDice\Simple Dice Roller\bin\Debug\net6.0-windows

            string folderpath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\";
            //to do:
            //load this from a file or something.
            //make this a global variable?

            //A collection of spell information so we can load them by ID.
            SpellsLibrary = new Dictionary<string, Spell>();

            LoadSpellsLibrary(folderpath);

            //function to parse the json and load the spells.
            //idea: change this setup to there being one file per spell, and then load that file on demand.
            //is that going to be worth the performance hit for loading a bunch of files at once?

            string filepath = folderpath + "Tiriel.char";
            string contents = File.ReadAllText(filepath);

            AbilitiesAreaDetails = new Dictionary<string, Panel>();

            //Currently selected dice for the dice roller.
            currentDice = new DiceCollection();

            //Stores the last-rolled collection of dice from the dice roller.
            lastDice = new DiceCollection();

            //Stores the currently loaded character.
            Character.Character currentCharacter = new Character.Character(contents);
            loadedCharacter = currentCharacter;
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

            foreach (string message in logMessages)
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
        private int GetIndexForRow(string id)
        {
            //Look up the index of the id column
            int colIndex = AbilitiesArea.Columns["Abilities_IDCol"].Index;
            //MessageBox.Show("Column index is " + colIndex);
            if (colIndex < 0)
            {
                //complain
                return -1;
            }

            for (int a = 0; a < AbilitiesArea.Rows.Count; a++)
            {
                if (AbilitiesArea.Rows[a].Cells[colIndex].Value.ToString() == id)
                {
                    return a;
                }
            }

            return -1;
        }

        //
        private bool LoadSpellsLibrary (string folderpath)
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
                    } else
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
            logMessages.Insert(0, message);

            //We'll limit the log to the most recent 20 rows for readability.
            //This will also prevent ballooning memory usage if the program is used very heavily.
            while (logMessages.Count > 20)
            {
                logMessages.RemoveAt(logMessages.Count - 1);
            }

            DisplayMessages();
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

            //----------------
            //Testing creating a second form as a popup
            /*
            var formPopup = new Form();
            formPopup.Show(this);
            */
            //Creates a new window, I don't want that.

            //Testing creating a panel as a popup
            /*
            Panel myPanel = new Panel();
            myPanel.Width = 100;
            myPanel.Height = 100;
            myPanel.Location = new Point (1, 1);
            myPanel.BringToFront();
            
            myPanel.Show();
            myPanel.Visible = true;
            */
            //Does nothing, apparently

            /*
            //Getting ahold of the row location and dimensions:
            //double top = AbilitiesArea.CurrentRow.AccessibilityObject.Bounds.Top;
            //double right = AbilitiesArea.CurrentRow.AccessibilityObject.Bounds.Right;
            //double bottom = AbilitiesArea.CurrentRow.AccessibilityObject.Bounds.Bottom;
            //double left = AbilitiesArea.CurrentRow.AccessibilityObject.Bounds.Left;
            double top = AbilitiesArea.Rows[1].AccessibilityObject.Bounds.Top;
            double right = AbilitiesArea.Rows[1].AccessibilityObject.Bounds.Right;
            double bottom = AbilitiesArea.Rows[1].AccessibilityObject.Bounds.Bottom;
            double left = AbilitiesArea.Rows[1].AccessibilityObject.Bounds.Left;

            //MessageBox.Show("This row's edges are " + top + "-" + bottom + " vertically and " + left + "-" + right + " horizontally.");

            double width = right - left;
            double height = bottom - top;

            left = 0;
            top = 0;
            height = 100;
            width = 100;

            //MessageBox.Show("Going to draw box starting at " + left + ", " + top + " and it will be " + width + " px wide and " + height + " px tall.");

            //Draw a box on top of that row, except the 25px at the top.
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            Rectangle rect = new Rectangle((int)left, (int)top, (int)width, (int)height);
            formGraphics.FillRectangle(brush, rect);
            //rect.BringToFront();
            //This works, but it draws the box behind the datagridview.

            brush.Dispose();
            formGraphics.Dispose();

            //DataGridView dgv = new DataGridView();
            //dgv.Columns.Add("theone", "the one");
            //AbilitiesArea.Rows[1].CreateCells(dgv);
                //row provided already belongs to a datagridview
                //fuckin duh
            */


            //This works, but:
            //it fills one cell's position.
            //it doesn't scroll with the table.
            /*
            DateTimePicker dtp = new DateTimePicker();
            dtp.Value = DateTime.Now.AddDays(-10);
            //add DateTimePicker into the control collection of the DataGridView
            this.AbilitiesArea.Controls.Add(dtp);
            //set its location and size to fit the cell
            dtp.Location = this.AbilitiesArea.GetCellDisplayRectangle(0, 3, true).Location;
            dtp.Size = this.AbilitiesArea.GetCellDisplayRectangle(0, 3, true).Size;
            */

            //Testing big single detail panel
            /*
            if (AbilityDetailPanel.Visible)
            {
                AbilityDetailPanel.Visible = false;
            }
            else
            {
                DisplayAbilityDetails("FontofMagic");
            }
            */
        }

        //Displays or hides the detail view for any given row in AbilitiesArea.
        //to do: pass in a grid for this to reference.
        private void ToggleGridExpand(int rowNum)
        {
            //Expand the row.
            if (AbilitiesArea.Rows[rowNum].Height > 100)
            {
                //Shrink the row back to normal.
                //to do: there's probably a function or something for getting this thing's preferred height.
                //look that up and use it.
                AbilitiesArea.Rows[rowNum].Height = AbilitiesArea.Rows[rowNum].GetPreferredHeight(rowNum, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);

                //Remove this panel from the list.
                string? id = AbilitiesArea.Rows[rowNum].Cells["Abilities_IDCol"].Value.ToString();
                if (id == null)
                {
                    //complain
                    return;
                }
                AbilitiesAreaDetails[id].Dispose();
            }
            else
            {

                string? id = AbilitiesArea.Rows[rowNum].Cells["Abilities_IDCol"].Value.ToString();
                string? text = AbilitiesArea.Rows[rowNum].Cells["Abilities_TextCol"].Value.ToString();

                if (id == null || text == null)
                {
                    //Log something?
                    return;
                }

                //Expand the row.
                AbilitiesArea.Rows[rowNum].Height = AbilitiesArea.Rows[rowNum].Height + 100;

                //Create an appropriate panel and add it to the list.
                //to do: write a function for this
                //to do: set up a function for when the panel is clicked: treat that like clicking the row again.
                Panel newPanel = new Panel();
                Label newLabel = new Label();
                newLabel.Text = text;
                newLabel.Left = 6;
                newLabel.Top = 6;
                //set height and width to the panel's height and width -12 each
                newLabel.Width = AbilitiesArea.Rows[0].AccessibilityObject.Bounds.Width - 12;
                newLabel.Height = 88; //hardcoded 100px height - 12px for margins = 88px
                newPanel.Controls.Add(newLabel);
                AbilitiesArea.Controls.Add(newPanel);
                AbilitiesAreaDetails[id] = newPanel;
            }

            UpdateAbilitiesAreaDetails();
        }


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

                bool temp = currentDice.AddOneDie(dieSize, subtract, advdisint, flatBonus);

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
            currentDice = new DiceCollection();

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
            DiceResponse resp = lastDice.Roll();
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
                DiceResponse resp = currentDice.Roll();
                outputTotal.Text = "Rolled " + resp.Total;
                LogMessage(resp.Description);

                UpdateLastDice(currentDice.GetDiceString());

                currentDice = new DiceCollection();
                UpdateDiceArrayDisplay();
            }
        }

        //Displays the collection of dice currently selected in the dice roller.
        private void UpdateDiceArrayDisplay()
        {
            DiceArrayDisplay.Text = currentDice.GetDiceString();
        }

        //Updates the display for the most recent collection of dice.
        private void UpdateLastDice(string diceString)
        {
            lastDice = new DiceCollection(diceString);
            LastRollDiceString.Text = lastDice.GetDiceString();
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        #region Character Tab: General

        //The onclick even for the save button. Saves the character.
        private void Button_SaveCharacter_Click(object sender, EventArgs e)
        {
            string filepath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\Tiriel.char";
            loadedCharacter.Save(filepath);
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

            loadedCharacter.Damage(amount);
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

            Char_Prof.Text = character.GetProf().ToString();

            DisplayClassList(character);

            DisplayAbilities(character);

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

            loadedCharacter.Heal(amount);
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
                    DiceResponse resp = loadedCharacter.RollForCharacter(d);

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
            loadedCharacter.SetTempHP(number, onlyIncrease);
            UpdateHealthDisplay();
        }

        //Displays the character's current + temp HP out of their max HP.
        private void UpdateHealthDisplay()
        {
            string healthString;
            if (loadedCharacter.TempHP > 0)
            {
                healthString = loadedCharacter.CurrentHP + " + " + loadedCharacter.TempHP + " (" + (loadedCharacter.CurrentHP + loadedCharacter.TempHP) + ") / " + loadedCharacter.MaxHP;
            }
            else
            {
                healthString = loadedCharacter.CurrentHP + " / " + loadedCharacter.MaxHP;
            }

            Char_Health.Text = healthString;
        }

        //Sets the appropriate bonuses into the labels for the various stat, save, and skill buttons.
        private void UpdateStatButtonLabels()
        {
            //Stats:
            Button_Str.Text = "Strength " + loadedCharacter.GetBonusForRollAsString("str");
            Button_Dex.Text = "Dexterity " + loadedCharacter.GetBonusForRollAsString("dex");
            Button_Con.Text = "Constitution " + loadedCharacter.GetBonusForRollAsString("con");
            Button_Int.Text = "Intelligence " + loadedCharacter.GetBonusForRollAsString("int");
            Button_Wis.Text = "Wisdom " + loadedCharacter.GetBonusForRollAsString("wis");
            Button_Cha.Text = "Charisma " + loadedCharacter.GetBonusForRollAsString("cha");

            //Saves:
            Button_StrSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("strsave");
            Button_DexSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("dexsave");
            Button_ConSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("consave");
            Button_IntSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("intsave");
            Button_WisSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("wissave");
            Button_ChaSave.Text = "Save " + loadedCharacter.GetBonusForRollAsString("chasave");

            //Skills:
            Button_Athletics.Text = "Athletics " + loadedCharacter.GetBonusForRollAsString("Athletics");
            Button_Acrobatics.Text = "Acrobatics " + loadedCharacter.GetBonusForRollAsString("Acrobatics");
            Button_SleightOfHand.Text = "Sleight of Hand " + loadedCharacter.GetBonusForRollAsString("Sleight of hand");
            Button_Stealth.Text = "Stealth " + loadedCharacter.GetBonusForRollAsString("Stealth");
            Button_Arcana.Text = "Arcana " + loadedCharacter.GetBonusForRollAsString("Arcana");
            Button_History.Text = "History " + loadedCharacter.GetBonusForRollAsString("History");
            Button_Investigation.Text = "Investigation " + loadedCharacter.GetBonusForRollAsString("Investigation");
            Button_Nature.Text = "Nature " + loadedCharacter.GetBonusForRollAsString("Nature");
            Button_Religion.Text = "Religion " + loadedCharacter.GetBonusForRollAsString("Religion");
            Button_AnimalHandling.Text = "Animal Handling " + loadedCharacter.GetBonusForRollAsString("Animal Handling");
            Button_Insight.Text = "Insight " + loadedCharacter.GetBonusForRollAsString("Insight");
            Button_Medicine.Text = "Medicine " + loadedCharacter.GetBonusForRollAsString("Medicine");
            Button_Perception.Text = "Perception " + loadedCharacter.GetBonusForRollAsString("Perception");
            Button_Survival.Text = "Survival " + loadedCharacter.GetBonusForRollAsString("Survival");
            Button_Deception.Text = "Deception " + loadedCharacter.GetBonusForRollAsString("Deception");
            Button_Intimidation.Text = "Intimidation " + loadedCharacter.GetBonusForRollAsString("Intimidation");
            Button_Performance.Text = "Performance " + loadedCharacter.GetBonusForRollAsString("Performance");
            Button_Persuasion.Text = "Persuasion " + loadedCharacter.GetBonusForRollAsString("Persuasion");


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
                DiceResponse resp = loadedCharacter.SpendHDByClass(className, 1, false);
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
                loadedCharacter.AddOrSubtractHDForClass(className, 1);
            }
            else if (colName == "SubtractHDButton")
            {
                loadedCharacter.AddOrSubtractHDForClass(className, -1);
            }

            //update the display.
            DisplayClassList(loadedCharacter);

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

            List<string> recharges = loadedCharacter.GetAbilityRechargeConditions();

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
                loadedCharacter.RechargeAbilities(rechargeCondition);
            }

            DisplayClassList(loadedCharacter);
            DisplayAbilities(loadedCharacter);
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
                DiceResponse resp = loadedCharacter.UseAbility(abilityID);
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
                bool resp = loadedCharacter.ChangeAbilityUses(abilityID, 1);
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
                bool resp = loadedCharacter.ChangeAbilityUses(abilityID, -1);
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
            DisplayCharacter(loadedCharacter);
            DisplayClassList(loadedCharacter);
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
                ToggleGridExpand(rowNum);
            }
            else
            {
                //This row just got selected.
                ToggleGridExpand(rowNum);
            }
        }

        //Triggered when the abilities list scrolls. Updates the positions of any ability detail panels.
        private void AbilitiesArea_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateAbilitiesAreaDetails();
        }

        private void AbilitiesArea_Sorted(object sender, EventArgs e)
        {
            UpdateAbilitiesAreaDetails();
        }

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

            Ability ability1 = loadedCharacter.GetAbilityByID(id1);
            Ability ability2 = loadedCharacter.GetAbilityByID(id2);

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

                //DataGridViewRow row = new DataGridViewRow();
                //row.Cells.Add(new DataGridViewTextBoxCell());
                //row.Cells[0].Value = a + 1;
                //row.Cells.Add(new DataGridViewCell (id));

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

            //Prevent the program from auto-selecting the first row.
            //Doesn't seem to work. Multiselect only?
            //AbilitiesArea.ClearSelection();
            //AbilitiesArea.CurrentCell = null;
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
                Panel detailArea = thingy.Value;

                int rowIndex = GetIndexForRow(id);

                //Get the location and dimensions to set this.
                int top = AbilitiesArea.Rows[rowIndex].AccessibilityObject.Bounds.Top;
                int left = AbilitiesArea.Rows[rowIndex].AccessibilityObject.Bounds.Left;
                int bottom = AbilitiesArea.Rows[rowIndex].AccessibilityObject.Bounds.Bottom;
                int right = AbilitiesArea.Rows[rowIndex].AccessibilityObject.Bounds.Right;

                //test: get the grid's location and subtract that
                int gridTop = AbilitiesArea.AccessibilityObject.Bounds.Top;
                int gridLeft = AbilitiesArea.AccessibilityObject.Bounds.Left;

                if (top > bottom)
                {
                    //This row is too short for this.
                    //complain?
                    return;
                    //to do: change to just hiding this one and moving on?
                }

                int height = bottom - top;
                int width = right - left;
                //to do: add up all column widths?
                //this is going to the edge of the grid, not to the visible edge of the row.

                //We need to account for the fact that we set position relative to the grid.
                top -= gridTop;
                left -= gridLeft;

                //Leave ourselves 25px of row to click on.
                height -= 25;
                top += 25;

                //MessageBox.Show("Setting panel to " + left + "," + top + " and " + width + "x" + height);

                Rectangle rect = new Rectangle(left, top, width, height);

                //Now set the panel's size and location.
                detailArea.Bounds = rect;
                detailArea.Visible = true;
                detailArea.BringToFront();
            }
        }

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 

        #region Magic Tab

        #endregion

        //-------- -------- -------- -------- -------- -------- -------- -------- 







        //Things to move elsewhere
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
    }
}