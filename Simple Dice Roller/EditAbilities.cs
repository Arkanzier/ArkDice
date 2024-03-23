using Character;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        internal Form1? ParentOfThisForm;

        //The master list of abilities.
        internal Dictionary<string, Ability> AbilitiesLibrary;

        //Used so that we don't respond to row selection events when we're still drawing the list.
        internal bool DrawingLists;

        public EditAbilities()
        {
            CharacterID = "";
            EditingCharacter = new Character.Character();
            ParentOfThisForm = null;

            AbilitiesLibrary = new Dictionary<string, Ability>();

            DrawingLists = false;

            InitializeComponent();

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }
        public EditAbilities(Form1 form)
            : this()
        {
            ParentOfThisForm = form;
            EditingCharacter = ParentOfThisForm.LoadedCharacter;

            AbilitiesLibrary = ParentOfThisForm.AbilitiesLibrary;

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }


        //Displays an ability's information in the text boxes at the bottom of the window.
        private void DisplayAbility(Ability ability)
        {
            Input_Name.Text = ability.Name;
            Input_ID.Text = ability.ID;

            Input_Uses.Text = ability.Uses.ToString();
            Input_MaxUses.Text = ability.MaxUses.ToString();
            Input_UsesChange.Text = ability.UsesChange.ToString();
            Input_RechargeCondition.Text = ability.RechargeCondition;
            Input_RechargeAmount.Text = ability.RechargeAmount.ToString();

            Input_Action.Text = ability.Action;
            Input_Dice.Text = ability.Dice.GetDiceString();
            Input_DisplayTier.Text = ability.DisplayTier.ToString();

            Input_Text.Text = ability.Text;
        }

        //Draws the list of existing abilities.
        private void DrawAbilitiesLibrary()
        {
            DrawingLists = true;
            AbilitiesLibraryList.Rows.Clear();

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

            List<Ability> list = EditingCharacter.GetAbilities();

            for (int a = 0; a < list.Count(); a++)
            {
                Ability thisAbility = list[a];
                string id = thisAbility.ID;
                string name = thisAbility.Name;
                AssignedAbilitiesList.Rows.Insert(a, id, name);
            }

            DrawingLists = false;
        }

        //Gets the index of the row matching a particular ability.
        //Returns -1 if no such row exists.
        private int GetIndexOfRowWithAbility(string abilityID, DataGridView list)
        {
            foreach (DataGridViewRow row in list.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    return -1;
                }
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


        #region Functions triggered by the form

        //When a row is selected in this list, also select it in the other (if possible) and put it's information into the text boxes.
        private void AbilitiesLibraryList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                return;
            }

            //Figure out which ability was just selected.
            Ability selectedAbility;
            if (AbilitiesLibraryList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no ability selected.
                //We can mimic this by passing a blank ability to the appropriate places.
                selectedAbility = new Ability();
            }
            else
            {
                //Back out if there's no row selected.
                if (AbilitiesLibraryList.SelectedRows.Count == 0)
                {
                    return;
                }
                //Back out if this row is blank.
                if (AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value == null)
                {
                    return;
                }

                //Get the ability's info.
                string? id = "";
                try
                {
                    id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
                    if (id == null)
                    {
                        id = "";
                    }
                }
                catch (Exception ex)
                {
                    //do nothing?
                    return;
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
                    }
                    else
                    {
                        //There is no matching ability, clear the selection.
                        DrawingLists = true;
                        //Unselect the selected row?
                        AssignedAbilitiesList.ClearSelection();
                        DrawingLists = false;
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
                    if (id != null && AbilitiesLibrary.ContainsKey(id))
                    {
                        selectedAbility = AbilitiesLibrary[id];
                    }
                    else
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

        //Add the selected ability to the current character.
        private void Button_Add_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
            } catch
            {
                //complain to a log file?
                //This shouldn't be possible.
            }
            

            if (id == null)
            {
                return;
            }
            Ability ability = AbilitiesLibrary[id];
            ParentOfThisForm.LoadedCharacter.AddOrUpdateAbility (ability);

            DrawAssignedAbilitiesList();
        }

        //Close the form.
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Delete the selected ability.
        private void Button_Delete_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
            }
            catch
            {
                //complain?
                //This shouldn't be possible.
                MessageBox.Show("Error: couldn't retrieve ability ID.");
            }

            if (id == null)
            {
                MessageBox.Show("Unable to find id");
                return;
            }
            Ability ability = AbilitiesLibrary[id];

            //Delete the ability from the library.
            AbilitiesLibrary.Remove(id);
            ParentOfThisForm.RemoveAbility(id);

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();

            AssignedAbilitiesList.ClearSelection();
            AbilitiesLibraryList.ClearSelection();
        }

        //Clear the form
        private void Button_NewAbility_Click(object sender, EventArgs e)
        {
            DisplayAbility(new Ability());
        }

        //Remove the selected ability from the current character.
        private void Button_Remove_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = AbilitiesLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
            }
            catch
            {
                //complain?
                //This shouldn't be possible.
                MessageBox.Show("Error: couldn't retrieve ability ID.");
            }

            if (id == null)
            {
                return;
            }

            ParentOfThisForm.LoadedCharacter.RemoveAbility(id);

            DrawAssignedAbilitiesList();
        }

        //Saves the current info into the library.
        private void Button_SaveAbility_Click(object sender, EventArgs e)
        {
            Ability ability = GetValuesFromFields();

            //Make sure the new / updated ability meets certain requirements.
            if (ability.ID == "")
            {
                //complain?
                return;
            }

            if (AbilitiesLibrary.ContainsKey(ability.ID))
            {
                //This ability exists in the list, overwrite it.
                var confirmation = MessageBox.Show("You are about to overwrite an existing ability, do you want to continue?", "Confirm Delete!!", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
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
            ParentOfThisForm.SaveUpdatedAbility(ability);

            //Redraw the lists
            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }

        //Clean things up as the form gets closed.
        private void EditAbilities_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ParentOfThisForm != null)
            {
                ParentOfThisForm.ClosingEditingAbilities();
            }
        }

        //For some reason, it's necessary to do this here in order to be able to actually clear the selections later.
        private void EditAbilities_Load(object sender, EventArgs e)
        {
            AbilitiesLibraryList.ClearSelection();
            AssignedAbilitiesList.ClearSelection();
        }

        #endregion

        private void Button_Test_Click(object sender, EventArgs e)
        {
            AbilitiesLibraryList.ClearSelection();
            AssignedAbilitiesList.ClearSelection();
        }
    }
}
