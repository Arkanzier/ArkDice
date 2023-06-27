﻿using System;
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
            //to do:
            //remove the ability to free type into these
            //set up a const above that determines the names
            //just use an array in order?
            //different labels for saves and skills?
            //convert all decimals to doubles, since apparently I got those backwards?
            //do these need to be objects?
            //could i pass in my const array?

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

            //i need a GetSkillProf function in Character
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

            //classes somehow

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

            changes["ProfBonusBonus"] = Input_ProfBonusBonus.ToString();

            //Do some extra error checking?

            //Save the info
            EditingCharacter.IncorporateChanges(changes);

            //Trigger a save here
            //need filepath
            //how to best standardize / store filepath?
            //store it in the character when opening the file?
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

        //Called when the save button is clicked.
        private void Button_Save_Click(object sender, EventArgs e)
        {
            bool resp = SaveCharacter();
            //Close the form? Make that a separate button / add a close button?
            //"Save and Close" button?
        }

        #endregion

        //Called when the form is closing for whatever reason.
        private void EditCharacter_FormClosing(object sender, FormClosingEventArgs e)
        {
            //need to hook into the calling form and call ClosingEditingCharacter();
            if (ParentForm != null)
            {
                ParentForm.ClosingEditingCharacter();
            }
        }

        private void Wrapper_Stats_Enter(object sender, EventArgs e)
        {

        }

        private void Label_StrSkills_Click(object sender, EventArgs e)
        {

        }
    }
}