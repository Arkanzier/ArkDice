using Character;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Dice_Roller
{
    public partial class EditCharacter : Form
    {
        internal string CharacterID;
        internal Character.Character EditingCharacter;

        public Form1? ParentForm;

        //Definable settings
        private string[] ProficiencyLevelStrings = new[] { "None", "Half Proficiency", "Proficient", "1.5x Proficiency", "Expertise" };
        private double[] ProficiencyLevelNumbers = new[] { 0, 0.5, 1, 1.5, 2 };

        public EditCharacter()
        {
            InitializeComponent();

            CharacterID = "";
            EditingCharacter = new Character.Character();
            ParentForm = null;
        }

        public EditCharacter(string CharacterID)
            : this()
        {
            this.CharacterID = CharacterID;
            if (CharacterID != "")
            {
                LoadCharacter();
            }
            else
            {
                MessageBox.Show("Error: could not find character ID.");
            }
        }

        public EditCharacter(Character.Character character)
            : this()
        {
            EditingCharacter = character;

            LoadCharacter();
        }

        #region Main Functions

        private void DrawClassList()
        {
            //Get a list of all of this character's classes.
            List<ClassLevel> classes = EditingCharacter.Classes;

            EditCharacterClassesList.Rows.Clear();

            //Now put them into the list.
            for (int a = 0; a < classes.Count; a++)
            {
                string name = classes[a].Name;
                string subclass = classes[a].Subclass;
                int level = classes[a].Level;
                int hd = classes[a].HDSize;
                int currhd = classes[a].CurrentHD;

                string[] row = { name, subclass, level.ToString(), "d" + hd.ToString(), currhd.ToString() };
                EditCharacterClassesList.Rows.Add(row);
            }
        }

        private double GetProfFromDropdown(ComboBox dropdown)
        {
            string strval = dropdown.Text;

            for (int a = 0; a < ProficiencyLevelStrings.Length; a++)
            {
                if (strval == ProficiencyLevelStrings[a])
                {
                    return ProficiencyLevelNumbers[a];
                }
            }

            //complain to a log file?
            return 0;
        }

        //Loads the character's info into the various spots in the form.
        private void LoadCharacter()
        {
            //Some basic attributes.
            Input_Name.Text = EditingCharacter.Name;
            Input_ID.Text = EditingCharacter.ID;

            Input_Race.Text = EditingCharacter.Race;
            Input_Subrace.Text = EditingCharacter.Subrace;

            Input_MaxHealth.Text = EditingCharacter.MaxHP.ToString();

            //Stats
            Input_Str.Text = EditingCharacter.GetStr().ToString();
            Input_Dex.Text = EditingCharacter.GetDex().ToString();
            Input_Con.Text = EditingCharacter.GetCon().ToString();
            Input_Int.Text = EditingCharacter.GetInt().ToString();
            Input_Wis.Text = EditingCharacter.GetWis().ToString();
            Input_Cha.Text = EditingCharacter.GetCha().ToString();

            //Save Profs
            Input_StrSave.Items.AddRange(ProficiencyLevelStrings);
            Input_DexSave.Items.AddRange(ProficiencyLevelStrings);
            Input_ConSave.Items.AddRange(ProficiencyLevelStrings);
            Input_IntSave.Items.AddRange(ProficiencyLevelStrings);
            Input_WisSave.Items.AddRange(ProficiencyLevelStrings);
            Input_ChaSave.Items.AddRange(ProficiencyLevelStrings);

            SetProfDropdown(EditingCharacter.GetSaveProf("Str"), Input_StrSave);
            SetProfDropdown(EditingCharacter.GetSaveProf("Dex"), Input_DexSave);
            SetProfDropdown(EditingCharacter.GetSaveProf("Con"), Input_ConSave);
            SetProfDropdown(EditingCharacter.GetSaveProf("Int"), Input_IntSave);
            SetProfDropdown(EditingCharacter.GetSaveProf("Wis"), Input_WisSave);
            SetProfDropdown(EditingCharacter.GetSaveProf("Cha"), Input_ChaSave);

            //Skill Profs
            Input_Athletics.Items.AddRange(ProficiencyLevelStrings);
            Input_Acrobatics.Items.AddRange(ProficiencyLevelStrings);
            Input_SleightOfHand.Items.AddRange(ProficiencyLevelStrings);
            Input_Stealth.Items.AddRange(ProficiencyLevelStrings);
            Input_Arcana.Items.AddRange(ProficiencyLevelStrings);
            Input_History.Items.AddRange(ProficiencyLevelStrings);
            Input_Investigation.Items.AddRange(ProficiencyLevelStrings);
            Input_Nature.Items.AddRange(ProficiencyLevelStrings);
            Input_Religion.Items.AddRange(ProficiencyLevelStrings);
            Input_AnimalHandling.Items.AddRange(ProficiencyLevelStrings);
            Input_Insight.Items.AddRange(ProficiencyLevelStrings);
            Input_Medicine.Items.AddRange(ProficiencyLevelStrings);
            Input_Perception.Items.AddRange(ProficiencyLevelStrings);
            Input_Survival.Items.AddRange(ProficiencyLevelStrings);
            Input_Deception.Items.AddRange(ProficiencyLevelStrings);
            Input_Intimidation.Items.AddRange(ProficiencyLevelStrings);
            Input_Performance.Items.AddRange(ProficiencyLevelStrings);
            Input_Persuasion.Items.AddRange(ProficiencyLevelStrings);

            SetProfDropdown(EditingCharacter.GetSkillProf("Athletics"), Input_Athletics);
            SetProfDropdown(EditingCharacter.GetSkillProf("Acrobatics"), Input_Acrobatics);
            SetProfDropdown(EditingCharacter.GetSkillProf("SleightOfHand"), Input_SleightOfHand);
            SetProfDropdown(EditingCharacter.GetSkillProf("Stealth"), Input_Stealth);
            SetProfDropdown(EditingCharacter.GetSkillProf("Arcana"), Input_Arcana);
            SetProfDropdown(EditingCharacter.GetSkillProf("History"), Input_History);
            SetProfDropdown(EditingCharacter.GetSkillProf("Investigation"), Input_Investigation);
            SetProfDropdown(EditingCharacter.GetSkillProf("Nature"), Input_Nature);
            SetProfDropdown(EditingCharacter.GetSkillProf("Religion"), Input_Religion);
            SetProfDropdown(EditingCharacter.GetSkillProf("AnimalHandling"), Input_AnimalHandling);
            SetProfDropdown(EditingCharacter.GetSkillProf("Insight"), Input_Insight);
            SetProfDropdown(EditingCharacter.GetSkillProf("Medicine"), Input_Medicine);
            SetProfDropdown(EditingCharacter.GetSkillProf("Perception"), Input_Perception);
            SetProfDropdown(EditingCharacter.GetSkillProf("Survival"), Input_Survival);
            SetProfDropdown(EditingCharacter.GetSkillProf("Deception"), Input_Deception);
            SetProfDropdown(EditingCharacter.GetSkillProf("Intimidation"), Input_Intimidation);
            SetProfDropdown(EditingCharacter.GetSkillProf("Performance"), Input_Performance);
            SetProfDropdown(EditingCharacter.GetSkillProf("Persuasion"), Input_Persuasion);

            Input_ProfBonusBonus.Text = EditingCharacter.BonusToProf.ToString();

            //Spell slots
            Input_StandardSlots1.Text = EditingCharacter.GetSpellSlotsForLevel(1, false, "standard").ToString();
            Input_StandardSlots2.Text = EditingCharacter.GetSpellSlotsForLevel(2, false, "standard").ToString();
            Input_StandardSlots3.Text = EditingCharacter.GetSpellSlotsForLevel(3, false, "standard").ToString();
            Input_StandardSlots4.Text = EditingCharacter.GetSpellSlotsForLevel(4, false, "standard").ToString();
            Input_StandardSlots5.Text = EditingCharacter.GetSpellSlotsForLevel(5, false, "standard").ToString();
            Input_StandardSlots6.Text = EditingCharacter.GetSpellSlotsForLevel(6, false, "standard").ToString();
            Input_StandardSlots7.Text = EditingCharacter.GetSpellSlotsForLevel(7, false, "standard").ToString();
            Input_StandardSlots8.Text = EditingCharacter.GetSpellSlotsForLevel(8, false, "standard").ToString();
            Input_StandardSlots9.Text = EditingCharacter.GetSpellSlotsForLevel(9, false, "standard").ToString();
            Input_WarlockSlots1.Text = EditingCharacter.GetSpellSlotsForLevel(1, false, "warlock").ToString();
            Input_WarlockSlots2.Text = EditingCharacter.GetSpellSlotsForLevel(2, false, "warlock").ToString();
            Input_WarlockSlots3.Text = EditingCharacter.GetSpellSlotsForLevel(3, false, "warlock").ToString();
            Input_WarlockSlots4.Text = EditingCharacter.GetSpellSlotsForLevel(4, false, "warlock").ToString();
            Input_WarlockSlots5.Text = EditingCharacter.GetSpellSlotsForLevel(5, false, "warlock").ToString();

            //Classes and class levels.
            DrawClassList();
        }

        //Saves the character.
        private bool SaveCharacter()
        {
            //First we retrieve the info and store it in a Dictionary.
            //We'll smell check the new info as we go.
            Dictionary<string, string> changes = new Dictionary<string, string>();

            int temp;
            if (Int32.TryParse(Input_Str.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Str"] = temp.ToString();
                }
            }
            if (Int32.TryParse(Input_Dex.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Dex"] = temp.ToString();
                }
            }
            if (Int32.TryParse(Input_Con.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Con"] = temp.ToString();
                }
            }
            if (Int32.TryParse(Input_Int.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Int"] = temp.ToString();
                }
            }
            if (Int32.TryParse(Input_Wis.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Wis"] = temp.ToString();
                }
            }
            if (Int32.TryParse(Input_Cha.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["Cha"] = temp.ToString();
                }
            }

            if (Input_Name.Text != "")
            {
                changes["Name"] = Input_Name.Text;
            }
            if (Input_ID.Text != "")
            {
                changes["ID"] = Input_ID.Text;
            }
            if (Input_Race.Text != "")
            {
                changes["Race"] = Input_Race.Text;
            }
            if (Input_Subrace.Text != "")
            {
                changes["Subrace"] = Input_Subrace.Text;
            }

            if (Int32.TryParse(Input_MaxHealth.Text, out temp))
            {
                if (temp >= 0)
                {
                    changes["MaxHP"] = temp.ToString();
                }
            }

            //We'll blindly grab the values of all skill dropdowns, since we don't currently have a way to check if they've been changed.
            //We could add one, but this is cheap enough that it's probably not worth it.
            changes["Athletics"] = GetProfFromDropdown(Input_Athletics).ToString();
            changes["Acrobatics"] = GetProfFromDropdown(Input_Acrobatics).ToString();
            changes["SleightOfHand"] = GetProfFromDropdown(Input_SleightOfHand).ToString();
            changes["Stealth"] = GetProfFromDropdown(Input_Stealth).ToString();
            changes["Arcana"] = GetProfFromDropdown(Input_Arcana).ToString();
            changes["History"] = GetProfFromDropdown(Input_History).ToString();
            changes["Investigation"] = GetProfFromDropdown(Input_Investigation).ToString();
            changes["Nature"] = GetProfFromDropdown(Input_Nature).ToString();
            changes["Religion"] = GetProfFromDropdown(Input_Religion).ToString();
            changes["AnimalHandling"] = GetProfFromDropdown(Input_AnimalHandling).ToString();
            changes["Insight"] = GetProfFromDropdown(Input_Insight).ToString();
            changes["Medicine"] = GetProfFromDropdown(Input_Medicine).ToString();
            changes["Perception"] = GetProfFromDropdown(Input_Perception).ToString();
            changes["Survival"] = GetProfFromDropdown(Input_Survival).ToString();
            changes["Deception"] = GetProfFromDropdown(Input_Deception).ToString();
            changes["Intimidation"] = GetProfFromDropdown(Input_Intimidation).ToString();
            changes["Performance"] = GetProfFromDropdown(Input_Performance).ToString();
            changes["Persuasion"] = GetProfFromDropdown(Input_Persuasion).ToString();

            changes["ProfBonusBonus"] = Input_ProfBonusBonus.Text;

            changes["SpellsLvl1"] = Input_StandardSlots1.Text;
            changes["SpellsLvl2"] = Input_StandardSlots2.Text;
            changes["SpellsLvl3"] = Input_StandardSlots3.Text;
            changes["SpellsLvl4"] = Input_StandardSlots4.Text;
            changes["SpellsLvl5"] = Input_StandardSlots5.Text;
            changes["SpellsLvl6"] = Input_StandardSlots6.Text;
            changes["SpellsLvl7"] = Input_StandardSlots7.Text;
            changes["SpellsLvl8"] = Input_StandardSlots8.Text;
            changes["SpellsLvl9"] = Input_StandardSlots9.Text;

            changes["SpellsWarlockLvl1"] = Input_WarlockSlots1.Text;
            changes["SpellsWarlockLvl2"] = Input_WarlockSlots2.Text;
            changes["SpellsWarlockLvl3"] = Input_WarlockSlots3.Text;
            changes["SpellsWarlockLvl4"] = Input_WarlockSlots4.Text;
            changes["SpellsWarlockLvl5"] = Input_WarlockSlots5.Text;

            //Class changes
            //The changes variable takes only strings, so we'll JSON encode what we pull out of here.
            //This is super simple JSON, so we'll just build it as we go.
            string classesJson = "";
            List<ClassLevel> classesFound = new List<ClassLevel>();
            for (int a = 0; a < EditCharacterClassesList.Rows.Count; a++)
            {
                //Make sure this won't process the empty row at the end that exists to let the user add new rows.
                if (EditCharacterClassesList.Rows[a].Cells["ClassesTable_Name"].Value == null)
                {
                    //This row is blank, so we won't process it.
                    //We continue, rather than break, because this might be a blank row the user left in the middle of the list.
                    //They SHOULD use the X button to remove the row entirely, but you can't count on them doing any given thing.
                    continue;
                }

                //We want to make sure we're dealing with actual strings here, since apparently this stuff sends back null when a cell is empty.
                //We'll convert those nulls to empty strings or zeroes, as appropriate.
                string classname = "";
                object tempvalue = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Name"].Value;
                if (tempvalue != null && tempvalue.ToString() != null)
                {
                    classname = tempvalue.ToString();
                }

                string? subclassname = "";
                tempvalue = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Subclass"].Value;
                if (tempvalue != null)
                {
                    subclassname = tempvalue.ToString();
                }

                string? classlevel = "0";
                tempvalue = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Level"].Value;
                if (tempvalue != null)
                {
                    classlevel = tempvalue.ToString();
                }

                string? classhd = "1";
                tempvalue = EditCharacterClassesList.Rows[a].Cells["ClassesTable_HD"].Value;
                if (tempvalue != null)
                {
                    classhd = tempvalue.ToString();
                }

                string? currenthd = "0";
                tempvalue = EditCharacterClassesList.Rows[a].Cells["ClassesTable_CurrentHD"].Value;
                if (tempvalue != null)
                {
                    currenthd = tempvalue.ToString();
                }

                //We need to convert the HD size from 'd some number' to just 'some number'
                if (classhd == null || classhd == "")
                {
                    classhd = "0";
                }
                else if (classhd.StartsWith("d") || classhd.StartsWith("D"))
                {
                    classhd = classhd.Substring(1);
                }

                //We'll make sure none of the strings are null here to shut up the compiler.



                //Make sure everything we expect is present and in the right format.
                //We'll split these all out into separate conditions so we can log the exact problem.
                int levelint;
                int numhdint;
                int hdsizeint;

                //There must be a name, but we don't really care what it is.
                if (classname == "")
                {
                    MessageBox.Show("Error: each class must have a name.");
                    return false;
                }

                //The subclass name is optional, since some classes don't get them until level 3.

                //There must be a level, and it must be an int > 0.
                if (classlevel == "")
                {
                    MessageBox.Show("Error: each class must have a level.");
                    return false;
                }
                else if (classlevel == "0")
                {
                    MessageBox.Show("Error: class levels must be whole numbers greater than 0.");
                    return false;
                }
                else if (!int.TryParse(classlevel, out levelint))
                {
                    MessageBox.Show("Error: class levels must be whole numbers greater than 0.");
                    return false;
                }

                //There must be an HD size and at this point it must be an int > 0.
                if (classhd == "")
                {
                    MessageBox.Show("Error: each class must have an HD size.");
                    return false;
                }
                else if (classhd == "0")
                {
                    MessageBox.Show("Error: HD sizes must be whole numbers greater than 0.");
                    return false;
                }
                else if (!int.TryParse(classhd, out hdsizeint))
                {
                    MessageBox.Show("Error: HD sizes must be whole numbers greater than 0.");
                    return false;
                }

                //The user doesn't have access to the current HD column, so we'll just make it default to something reasonable-enough.
                if (!int.TryParse(currenthd, out numhdint))
                {
                    //The user doesn't have access to the current HD column, so we won't complain at them if something is wrong.
                    currenthd = "0";
                }

                //Now we check to see if this class + subclass combo already exists in the list.
                //We'll use some ClassLevel objects so that we can use their built in functions to check for sufficient levels of identicalness instead of having to re-code it here.

                //to do: need to convert the ints-as-strings to ints
                //do that here or in a new constructor?
                //set up the earlier stuff so we convert them once, check if they come out all right, and save the values to use here?
                //i like that plan, do that. It's just 3 conversions, but it's also just 3 ints.
                ClassLevel thisclasslevel = new ClassLevel(classname, subclassname, levelint, hdsizeint, numhdint);
                //ClassLevel thisclasslevel = new ClassLevel(classname, subclassname);
                for (int b = 0; b < classesFound.Count; b++)
                {
                    if (thisclasslevel.IsSameClass(classesFound[b]))
                    {
                        //They're identical, we'll complain and quit.
                        MessageBox.Show("Error: two identical classes found.");
                        return false;
                    }

                    classesFound.Add(thisclasslevel);
                }


                string json = "{\"Name\":\"" + classname + "\",\"Subclass\":\"" + subclassname + "\",\"Level\":" + classlevel + ",\"HDSize\":" + classhd + ",\"CurrentHD\":" + currenthd + "}";
                //Remember to leave the ints without quotes, because it gets picky about that.

                if (classesJson == "")
                {
                    //This is the first class we're adding.
                    classesJson = json;
                }
                else
                {
                    //This is not the first class we're adding.
                    classesJson += "," + json;
                }
            }
            //Wrap it up so that it properly represents a ClassLevelList object.
            classesJson = "{\"Levels\":[" + classesJson + "]}";
            changes["Classes"] = classesJson;

            //Do some extra error checking here?

            //Save the info
            EditingCharacter.IncorporateChanges(changes);

            return true;
        }

        //Sets a proficiency dropdown
        private void SetProfDropdown(double prof, ComboBox dropdown)
        {
            //Check if the combobox is present?

            switch (prof)
            {
                case 0:
                    dropdown.SelectedIndex = 0; break;
                case 0.5:
                    dropdown.SelectedIndex = 1; break;
                case 1:
                    dropdown.SelectedIndex = 2; break;
                case 1.5:
                    dropdown.SelectedIndex = 3; break;
                case 2:
                    dropdown.SelectedIndex = 4; break;
                default:
                    //Complain?
                    dropdown.SelectedIndex = 0; break;
            }
        }

        #endregion


        #region Called Functions
        //Functions that get called via the form

        //Closes this form when the Close button is clicked.
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Called when the save button is clicked.
        private void Button_Save_Click(object sender, EventArgs e)
        {
            bool resp = SaveCharacter();
        }

        //Called when the form is closing for whatever reason.
        private void EditCharacter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.ClosingEditingCharacter();
            }
        }

        //Handles people clicking on the rows.
        private void EditCharacterClassesList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //First we figure out what was clicked.
            //Column:
            int colIndex = e.ColumnIndex;
            string colName = EditCharacterClassesList.Columns[colIndex].Name;

            //Row:
            int rowNum = e.RowIndex;

            //We'll do a quick exit if this wasn't a click on the one button we care about.
            if (colName != "ClassesTable_DeleteCol")
            {
                return;
            }

            //Now we know the remove button was clicked, so we're looking to remove a row.

            //First we'll check if that row actually exists, since there is a blank row at the bottom that the user could have clicked.
            //We want to block clicks on index (Count - 1) because the blank row is included in Count.
            if (rowNum >= EditCharacterClassesList.Rows.Count - 1)
            {
                //This is the blank row at the bottom, so there's nothing for us to do.
                return;
            }

            //The row exists, so now we remove it.
            //We'll only remove the displayed row, not the corresponding row in the current character's class list.
            //The code that saves any changes will handle updating the character for us.
            EditCharacterClassesList.Rows.RemoveAt(rowNum);

            return;
        }

        #endregion

        private void Wrapper_Stats_Enter(object sender, EventArgs e)
        {

        }

        private void Label_StrSkills_Click(object sender, EventArgs e)
        {

        }

        private void EditCharacterClassesList_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void EditCharacterClassesList_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void EditCharacterClassesList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
