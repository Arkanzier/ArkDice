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

        #region renameme

        private void DrawClassList()
        {
            //Get a list of all of this character's classes.
            List<ClassLevel> classes = EditingCharacter.Classes;

            EditCharacterClassesList.Rows.Clear();

            //Now put them into the list.
            //Need to be able to change (at least add) subclass.
            //Need to be able to change level.
            //Allow changing name and HD, since the others can be changed?
            //Need to be able to delete entries.
            //Something more elegant than just clearing out all the fields?
            for (int a = 0; a < classes.Count; a++)
            {
                string name = classes[a].Name;
                string subclass = classes[a].Subclass;
                int level = classes[a].Level;
                int hd = classes[a].HDSize;
                int currhd = classes[a].CurrentHD;

                string[] row = { name, subclass, level.ToString(), "d" + hd.ToString(), currhd.ToString(), "X" };
                EditCharacterClassesList.Rows.Add(row);
            }

            //there will be an empty row to add new ones.
            //need a function to take info from that row and add it to the character.
            //put it in save character?
        }

        private void EditCharacterClassesList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //First we figure out which button was clicked.
            //Column:
            int colIndex = e.ColumnIndex;
            string colName = EditCharacterClassesList.Columns[colIndex].Name;

            //Row:
            int rowNum = e.RowIndex;
            string className;
            string subclassName;

            try
            {
                className = EditCharacterClassesList.Rows[rowNum].Cells[0].Value.ToString();
                subclassName = EditCharacterClassesList.Rows[rowNum].Cells[1].Value.ToString();
                //MessageBox.Show ("Found class ID " + className + " (" + subclassName + ")");
            }
            catch
            {
                className = "";
                subclassName = "";
                //pop up an error message?
                //return?
            }
            className ??= "";

            //Check if the button was clicked
            if (colName == "ClassesTable_DeleteCol")
            {
                //We're supposed to delete this row.
                //Find all instances of this class + subclass together on the character and remove those entries.
                //There should only be one, but the list is short enough that we'll want to just check them all.
                List<ClassLevel> classes = EditingCharacter.Classes;
                for (int a = 0; a < classes.Count; a++)
                {
                    string thisclass = classes[a].Name;
                    string thissubclass = classes[a].Subclass;
                    //MessageBox.Show("Deleting row: going to process class " + a + " of " + classes.Count + "\r\nThis class is " + thisclass + " (" + thissubclass + ")");

                    if (thisclass == className && thissubclass == subclassName)
                    {
                        //This is a match, delete it.
                        EditingCharacter.Classes.RemoveAt(a);
                        //MessageBox.Show("Deleting " + thisclass + " (" + thissubclass + ")");

                        //to do: check if this is necessary.
                        //Set a back 1 so we don't accidentally skip anything.
                        a--;
                    }
                }

                DrawClassList();
            }
            //else: the thing clicked was not a button; do nothing.
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

            //complain?
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
            //to do
            //calculate proficiency bonus here and display it, including the prof bonus bonus.
            //decide on label for it. "Proficiency Bonus" ?

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

            //Make sure nothing is in focus.
            ActiveControl = null;
            //doesn't work


            //Load the appropriate file.
            //Error checking?
            //Dump the info into the text boxes / etc
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
            //changes takes only strings, so we'll JSON encode what we pull out of here.
            //This is super simple JSON, so we'll just build it as we go.
            string classesJson = "";
            for (int a = 0; a < EditCharacterClassesList.Rows.Count; a++)
            {
                //Make sure this won't process the empty row at the end that exists to let the user add new rows.
                if (EditCharacterClassesList.Rows[a].Cells[0].Value == null)
                {
                    break;
                    //to do: change to continue?
                    //is it possible to have one of these null rows in the middle of actual data?
                }

                string? classname = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Name"].Value.ToString();
                if (classname == null)
                {
                    classname = "";
                }
                //MessageBox.Show("Found class name " + classname);
                string? subclassname = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Subclass"].Value.ToString();
                string? classlevel = EditCharacterClassesList.Rows[a].Cells["ClassesTable_Level"].Value.ToString();
                string? classhd = EditCharacterClassesList.Rows[a].Cells["ClassesTable_HD"].Value.ToString();
                //We need to convert the HD size from 'd some number' to just 'some number'
                if (classhd == null)
                {
                    classhd = "0";
                } else if (classhd.StartsWith("d") || classhd.StartsWith("D"))
                {
                    classhd = classhd.Substring(1);
                }
                string? currenthd = EditCharacterClassesList.Rows[a].Cells["ClassesTable_CurrentHD"].Value.ToString();

                //to do
                //error checking on the values goes here
                    //name and subclass can be left as-is
                    //the other 3 need to be ints-as-strings

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
            MessageBox.Show("Using JSON " + classesJson);

            //Do some extra error checking here?

            //Save the info
            EditingCharacter.IncorporateChanges(changes);

            //Trigger a save here
            EditingCharacter.Save();

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
            //Close the form? Make that a separate button / add a close button?
            //"Save and Close" button?
        }

        //Called when the form is closing for whatever reason.
        private void EditCharacter_FormClosing(object sender, FormClosingEventArgs e)
        {
            //need to hook into the calling form and call ClosingEditingCharacter();
            if (ParentForm != null)
            {
                ParentForm.ClosingEditingCharacter();
            }
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
