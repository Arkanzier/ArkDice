﻿using Character;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Dice_Roller
{
    public partial class EditAbilities : Form
    {
        //Information about the selected character.
        internal string CharacterID;
        internal Character.Character EditingCharacter;

        //A link back to the main form.
        internal Form1? ParentForm;

        //The master list of abilities.
        internal Dictionary<string, Ability> AbilitiesLibrary;

        //Used so that we don't respond to row selection events when we're still drawing the list.
        internal bool DrawingLists;

        public EditAbilities()
        {
            CharacterID = "";
            EditingCharacter = new Character.Character();
            ParentForm = null;

            AbilitiesLibrary = new Dictionary<string, Ability>();

            DrawingLists = false;

            InitializeComponent();

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }
        public EditAbilities(Form1 form)
            : this()
        {
            ParentForm = form;
            EditingCharacter = ParentForm.LoadedCharacter;

            AbilitiesLibrary = ParentForm.AbilitiesLibrary;

            //MessageBox.Show("Loaded character " + EditingCharacter.Name);

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }


        //When a row is selected in this list, also select it in the other (if possible) and put it's information into the text boxes.
        private void AbilitiesLibraryList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                //MessageBox.Show("Skipping ability loading");
                return;
            }

            //Figure out which ability was just selected.
            Ability selectedAbility;
            if (AbilitiesLibraryList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no ability selected.
                //We can mimic this by passing a blank ability to the appropriate places.
                selectedAbility = new Ability();
                //MessageBox.Show("Going to blank out the displays");
            }
            else
            {
                //Get the ability's info.
                //int rowindex = AbilitiesLibraryList.SelectedRows[0].Index;
                string id = "";
                //if (AbilitiesLibraryList.Columns.Contains ("Library_IDCol"))
                //{
                //    id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
                //}
                try
                {
                    id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                }
                if (id == "")
                {
                    selectedAbility = new Ability();
                }
                else
                {
                    selectedAbility = AbilitiesLibrary[id];
                }

                //Now we have the ability, or a blank one for no selection.
                if (id != "")
                {
                    //Mirror this selection into the other grid, if the ability exists there.
                    int assignedIndex = GetIndexOfRowWithAbility(id, AssignedAbilitiesList);

                    if (assignedIndex >= 0)
                    {
                        //This ability exists in the assigned list. Select it there.
                        DrawingLists = true;
                        AssignedAbilitiesList.Rows[assignedIndex].Selected = true;
                        DrawingLists = false;
                        //to do: how do i move the little triangle?
                    }
                }
            }
            //Now display the ability's info in the text boxes.
            DisplayAbility(selectedAbility);
        }

        //When a row is selected in this list, also select it in the other (if possible) and put it's information into the text boxes.
        private void AssignedAbilitiesList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                //MessageBox.Show("Skipping ability loading");
                return;
            }

            //Figure out which ability was just selected.
            Ability selectedAbility;
            if (AssignedAbilitiesList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no ability selected.
                //We can mimic this by passing a blank ability to the appropriate places.
                selectedAbility = new Ability();
            }
            else
            {
                //Get the ability's info.
                string id = "";
                try
                {
                    id = AssignedAbilitiesList.SelectedRows[0].Cells["Assigned_IDCol"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                }
                if (id == "")
                {
                    selectedAbility = new Ability();
                }
                else
                {
                    //to do: needs error protection.
                    //if the ability isn't in the library, this will cause problems.
                    //was here
                    //library cannot load after being saved. figure out why
                    if (id != null && AbilitiesLibrary.ContainsKey(id))
                    {
                        selectedAbility = AbilitiesLibrary[id];
                    } else
                    {
                        //Complain?
                        selectedAbility = new Ability();
                        id = "";
                    }
                }

                //Now we have the ability, or a blank one for no selection.
                if (id != "")
                {
                    //Mirror this selection into the other grid, if the ability exists there.
                    int assignedIndex = GetIndexOfRowWithAbility(id, AbilitiesLibraryList);

                    if (assignedIndex >= 0)
                    {
                        //This ability exists in the library list. Select it there.
                        DrawingLists = true;
                        AbilitiesLibraryList.Rows[assignedIndex].Selected = true;
                        DrawingLists = false;
                    }
                }
            }
            //Now display the ability's info in the text boxes.
            DisplayAbility(selectedAbility);
        }

        //Displays an ability's information in the text boxes at the bottom of the window.
        private void DisplayAbility(Ability ability)
        {
            Input_Name.Text = ability.Name;
            Input_ID.Text = ability.ID;

            //to do: get rid of uses and let that be managed within the program proper?
            Input_Uses.Text = ability.Uses.ToString();
            Input_MaxUses.Text = ability.MaxUses.ToString();
            Input_UsesChange.Text = ability.UsesChange.ToString();
            Input_RechargeCondition.Text = ability.RechargeCondition;
            Input_RechargeAmount.Text = ability.RechargeAmount.ToString();
            //

            //to do: change to selecting supported actions off a dropdown.
            Input_Action.Text = ability.Action;
            Input_Dice.Text = ability.Dice.GetDiceString();
            Input_DisplayTier.Text = ability.DisplayTier.ToString();
            //

            Input_Text.Text = ability.Text;
        }

        //Draws the list of existing abilities.
        private void DrawAbilitiesLibrary()
        {
            DrawingLists = true;
            AbilitiesLibraryList.Rows.Clear();

            //hide rows here that exist in the other list?
            //will need a quick way to check if an ability is in the other list.
            //store it as a dictionary too?

            int a = 0;
            foreach (var keyValuePair in AbilitiesLibrary.OrderBy(x => x.Key))
            {
                Ability ability = keyValuePair.Value;
                string id = ability.ID;
                string name = ability.Name;
                AbilitiesLibraryList.Rows.Insert(a, id, name);
                a++;
            }

            DrawingLists = false;
        }

        //Draws the list of abilities assigned to the current character.
        private void DrawAssignedAbilitiesList()
        {
            DrawingLists = true;
            AssignedAbilitiesList.Rows.Clear();

            //to do: replace with an abilities library

            for (int a = 0; a < EditingCharacter.GetAbilities().Count(); a++)
            {
                Ability thisAbility = EditingCharacter.GetAbilities()[a];
                string id = thisAbility.ID;
                string name = thisAbility.Name;
                AssignedAbilitiesList.Rows.Insert(a, id, name);
            }

            DrawingLists = false;
        }

        private void EditAbilities_Load(object sender, EventArgs e)
        {
            //For some reason, it's necessary to do this here in order to be able to actually clear the selections later.
            AbilitiesLibraryList.ClearSelection();
            AssignedAbilitiesList.ClearSelection();
        }

        //Gets the index of the row matching a particular ability.
        //Returns -1 if no such row exists.
        private int GetIndexOfRowWithAbility(string abilityID, DataGridView list)
        {
            foreach (DataGridViewRow row in list.Rows)
            {
                string? thisid = row.Cells[0].Value.ToString();
                if (thisid == abilityID)
                {
                    return row.Index;
                }
            }

            return -1;
        }

        //Retrieves the values from the various fields.
        private Ability GetValuesFromFields()
        {
            Ability ret = new Ability();

            ret.Name = Input_Name.Text.Trim();
            ret.ID = Input_ID.Text.Trim();

            int tempint;
            ret.Uses = (Int32.TryParse(Input_Uses.Text.Trim(), out tempint)) ? tempint : 0;
            ret.MaxUses = (Int32.TryParse(Input_MaxUses.Text.Trim(), out tempint)) ? tempint : 0;
            ret.UsesChange = (Int32.TryParse(Input_UsesChange.Text.Trim(), out tempint)) ? tempint : 0;
            ret.RechargeCondition = Input_RechargeCondition.Text.Trim();
            ret.RechargeAmount = Input_RechargeAmount.Text.Trim();

            ret.Action = Input_Action.Text.Trim();
            ret.Dice = new ArkDice.DiceCollection(Input_Dice.Text.Trim());
            ret.DisplayTier = (Int32.TryParse(Input_DisplayTier.Text.Trim(), out tempint) ? tempint : 0);

            ret.Text = Input_Text.Text.Trim();

            return ret;
        }





        private void Button_Test_Click(object sender, EventArgs e)
        {
            AbilitiesLibraryList.ClearSelection();
            AssignedAbilitiesList.ClearSelection();
        }

        //Saves the current info into the library.
        private void Button_SaveAbility_Click(object sender, EventArgs e)
        {
            Ability ability = GetValuesFromFields();

            if (AbilitiesLibrary.ContainsKey(ability.ID))
            {
                //This ability exists in the list, overwrite it.
                var confirmation = MessageBox.Show("You are about to overwrite an existing ability, do you want to continue?", "Confirm Delete!!", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    //AbilitiesLibrary.Add(ability.ID, ability);
                    //Overwrite the ability.
                    AbilitiesLibrary[ability.ID] = ability;
                }
                else
                {
                    //Don't overwrite the ability.
                }
            }
            else
            {
                //This ability does not exist in the list, add it.
                AbilitiesLibrary.Add(ability.ID, ability);
            }

            //Save library here
            ParentForm.SaveUpdatedAbility(ability);

            //Redraw the lists
            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditAbilities_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.ClosingEditingAbilities();
            }
        }
    }
}
