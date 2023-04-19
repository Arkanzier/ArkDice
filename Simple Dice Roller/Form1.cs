using ArkDice;
using Character;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.ApplicationServices;

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

        internal Character.Character loadedCharacter;

        internal DiceCollection currentDice;

        internal DiceCollection lastDice;

        //something for storing the previous roll.
        //by string?
        //DiceCollection?
        //also add a button to trigger rolling the last roll.
        //put a display somewhere to indicate what it is, in case people forget?
        //something to store the dice being collected via the various dice buttons, once I add them.
        //also a button to trigger that.
        //make it the same button as the one hooked to the text box?
        //that would require deciding how to handle when there's stuff for both, but that should be doable enough.
        //i'll want some kind of visual representation of the collection of dice that's been added up.
        //since it'll be a string anyway, use this for the 'previous roll' variable?
        //how much effort do I want to put in toward compacting this?
        //ie: if the user hits d6 -> d4 -> d6, should this display d6+d4+d6 or 2d6+d4?
        //I like the second one, but that might be a pain if I'm actually storing things as a string only.


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
            string filepath = folderpath + "Tiriel.char";
            string contents = File.ReadAllText(filepath);

            //Stores the currently loaded character.
            Character.Character currentCharacter = new Character.Character(contents);
            loadedCharacter = currentCharacter;
            //MessageBox.Show(currentCharacter.ToString());
            DisplayCharacter(currentCharacter);

            //Currently selected dice for the dice roller.
            currentDice = new DiceCollection();

            //Stores the last-rolled collection of dice from the dice roller.
            lastDice = new DiceCollection();
        }


        //Functions that get called by the form:
        //-------- -------- -------- -------- -------- -------- -------- -------- 

        //Handlers for the buttons in the abilities list.
        private void AbilitiesArea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Do we need to manually do something to make the row selected?

            int colIndex = e.ColumnIndex;
            string colName = AbilitiesArea.Columns[colIndex].Name;

            //MessageBox.Show("Button clicked: col is " + colName);

            int rowNum = e.RowIndex;
            //string aaaa = AbilitiesArea2.Rows[rowNum].Cells["NameCol"].ToString();
            string abilityID = "init";
            try
            {
                abilityID = AbilitiesArea.Rows[rowNum].Cells[1].Value.ToString();
                //MessageBox.Show ("Found ability ID " + abilityID);
            }
            catch
            {
                abilityID = "null";
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
            else if (colName == "PlusButtonCol")
            {

            }
            else if (colName == "MinusButtonCol")
            {

            }

            //Update everything in case stuff changed.
            DisplayCharacter(loadedCharacter);
            DisplayClassList(loadedCharacter);
            UpdateHealthDisplay();
        }

        //Dice roller functions: adding dice.
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

                int dieSize = 0;
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

        //Redo the most recent roll.
        private void RerollButton_Click(object sender, EventArgs e)
        {
            DiceResponse resp = lastDice.Roll();
            outputTotal.Text = "Rolled " + resp.Total;
            LogMessage(resp.Description);
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

        private void TestButton_Click(object sender, EventArgs e)
        {
            //string resp = loadedCharacter.ToString();
            //MessageBox.Show(resp);
            //loadedCharacter.RegainHD("half");
            loadedCharacter.RechargeAbilities("long rest");
            DisplayClassList(loadedCharacter);
            DisplayAbilities(loadedCharacter);
        }


        //Functions that do stuff to the form:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
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

                string[] allOfRow = { (a + 1).ToString(), id, name, text, recharge, dice, usesString, "Use", "+1", "-1" };
                AbilitiesArea.Rows.Add(allOfRow);

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

        //
        private void UpdateDiceArrayDisplay()
        {
            DiceArrayDisplay.Text = currentDice.GetDiceString();
        }

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

        //
        private void UpdateLastDice(string diceString)
        {
            lastDice = new DiceCollection(diceString);
            LastRollDiceString.Text = lastDice.GetDiceString();
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

        //function to update the last die roll stored + indicator?


        //Underlying functions that other stuff calls:
        //-------- -------- -------- -------- -------- -------- -------- -------- 
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

        private void Char_Name_Click(object sender, EventArgs e)
        {

        }

        private void Char_Race_Click(object sender, EventArgs e)
        {

        }

        private void Button_SaveCharacter_Click(object sender, EventArgs e)
        {
            string filepath = "C:\\Users\\david\\Programs\\Simple Dice Roller\\Tiriel.char";
            loadedCharacter.Save(filepath);
        }

        private void CharacterTab_Click(object sender, EventArgs e)
        {

        }
    }
}