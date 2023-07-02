﻿using Character;
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
    public partial class EditSpells : Form
    {
        internal string CharacterID;
        internal Character.Character EditingCharacter;

        public Form1? ParentOfThisForm;

        //The master list of abilities.
        internal Dictionary<string, Spell> SpellsLibrary;

        //Used so that we don't respond to row selection events when we're still drawing the list.
        internal bool DrawingLists;

        public EditSpells()
        {
            CharacterID = "";
            EditingCharacter = new Character.Character();
            ParentOfThisForm = null;

            DrawingLists = false;

            SpellsLibrary = new Dictionary<string, Spell>();

            InitializeComponent();
        }

        public EditSpells(Form1 form)
            : this()
        {
            ParentOfThisForm = form;
            EditingCharacter = ParentOfThisForm.LoadedCharacter;

            SpellsLibrary = ParentOfThisForm.SpellsLibrary;

            //MessageBox.Show("Loaded character " + EditingCharacter.Name);

            DrawSpellsLibraryList();
            DrawAssignedSpellsList();
        }

        //to do:
        //need custom sorting for the lists
        //sorting by level: break ties by name
        //also, trigger the sorting on load


        //Displays the provided spell in the inputs.
        private void DisplaySpell(Spell spell)
        {
            Input_Name.Text = spell.Name;
            Input_ID.Text = spell.ID;
            Input_Action.Text = spell.Action;

            Input_Level.Text = spell.Level.ToString();
            Input_School.Text = spell.School;

            Input_Vocal.Checked = spell.Vocal;
            Input_Somatic.Checked = spell.Somatic;
            Input_Material.Checked = spell.Material;
            Input_ExpensiveMaterial.Text = spell.ExpensiveMaterial;

            Input_Duration.Text = spell.Duration.ToString();
            Input_Concentration.Checked = spell.Concentration;
            Input_Range.Text = spell.Range;

            Input_Book.Text = spell.Book;
            Input_Page.Text = spell.Page.ToString();

            Input_Description.Text = spell.Description;
            Input_UpcastingBenefit.Text = spell.UpcastingBenefit;
        }

        //Populate the list of spells assigned to the current character.
        private void DrawAssignedSpellsList()
        {
            DrawingLists = true;
            AssignedSpellsList.Rows.Clear();

            List<Spell> list = EditingCharacter.GetSpells();

            for (int a = 0; a < list.Count(); a++)
            {
                Spell thisSpell = list[a];
                string id = thisSpell.ID;
                string name = thisSpell.Name;
                string level = thisSpell.Level.ToString();
                AssignedSpellsList.Rows.Insert(a, id, name, level);
            }

            DrawingLists = false;
        }

        //Populate the list of spells in the library.
        private void DrawSpellsLibraryList()
        {
            DrawingLists = true;
            SpellsLibraryList.Rows.Clear();

            //hide rows here that exist in the other list?
            //will need a quick way to check if a spell is in the other list.
            //store it as a dictionary too?

            int a = 0;
            foreach (var keyValuePair in SpellsLibrary.OrderBy(x => x.Key))
            {
                Spell spell = keyValuePair.Value;
                string id = spell.ID;
                string name = spell.Name;
                string level = spell.Level.ToString();
                SpellsLibraryList.Rows.Insert(a, id, name, level);
                a++;
            }

            DrawingLists = false;
        }

        //Gets the index of the row matching a particular spell.
        //Returns -1 if no such row exists.
        private int GetIndexOfRowWithSpell(string abilityID, DataGridView list)
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

        //Retrieve information about a spell from the various inputs.
        private Spell GetValuesFromFields()
        {
            Spell ret = new Spell();

            ret.Name = Input_Name.Text;
            ret.ID = Input_ID.Text;
            ret.Action = Input_Action.Text;
            //ret.Level = Input_Level.Text;
            ret.School = Input_School.Text;
            ret.Vocal = Input_Vocal.Checked;
            ret.Somatic = Input_Somatic.Checked;
            ret.Material = Input_Material.Checked;
            ret.ExpensiveMaterial = Input_ExpensiveMaterial.Text;
            //ret.Duration = Input_Duration.Text;
            ret.Concentration = Input_Concentration.Checked;
            ret.Range = Input_Range.Text;
            ret.Book = Input_Book.Text;
            //ret.Page = Input_Page.Text;

            ret.Description = Input_Description.Text;
            ret.UpcastingBenefit = Input_UpcastingBenefit.Text;

            int tempint;
            if (Int32.TryParse(Input_Level.Text, out tempint))
            {
                ret.Level = tempint;
            }
            if (Int32.TryParse(Input_Duration.Text, out tempint))
            {
                ret.Duration = tempint;
            }
            if (Int32.TryParse(Input_Page.Text, out tempint))
            {
                ret.Page = tempint;
            }

            return ret;
        }

        //Selects the provided ability in both forms and displays it's info in the inputs.
        private void SelectSpell(string id)
        {
            int libraryIndex = -1;
            int assignedIndex = -1;
            if (id != "")
            {
                libraryIndex = GetIndexOfRowWithSpell(id, SpellsLibraryList);
                assignedIndex = GetIndexOfRowWithSpell(id, AssignedSpellsList);
            }

            Spell? spell;
            if (id != "" && SpellsLibrary.ContainsKey(id))
            {
                spell = SpellsLibrary[id];
            }
            else
            {
                spell = new Spell();
            }

            DrawingLists = true;
            //Make it selected in the library list.
            if (libraryIndex == -1)
            {
                //There is no such spell in the library, somehow.
                SpellsLibraryList.ClearSelection();
            }
            else
            {
                //We found this spell in the library, select it.
                SpellsLibraryList.Rows[libraryIndex].Selected = true;
            }

            //Make it selected in the assigned list.
            if (assignedIndex == -1)
            {
                //There is no such spell in the library, somehow.
                AssignedSpellsList.ClearSelection();
            }
            else
            {
                //We found this spell in the library, select it.
                AssignedSpellsList.Rows[assignedIndex].Selected = true;
            }
            DrawingLists = false;

            //Spell will either be populated by this point, or be a blank new spell.
            DisplaySpell(spell);
        }

        #region Functions triggered by the form

        //A row in the assigned spells list was just clicked.
        //Select the corresponding row in the library list and display the spell's info.
        private void AssignedSpellsList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                return;
            }

            //Figure out which spell was just selected.
            string id = "";
            if (AssignedSpellsList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no spell selected.
                //Just leave id blank.
            }
            else
            {
                try
                {
                    id = AssignedSpellsList.SelectedRows[0].Cells["Assigned_ID"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                    id = "";
                }
            }

            SelectSpell(id);
        }

        //Add the currently selected spell to the character.
        private void Button_Add_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_ID"].Value.ToString();
            }
            catch
            {
                //This shouldn't happen.
                MessageBox.Show("Error: couldn't retrieve spell ID.");
            }


            if (id == null)
            {
                return;
            }
            Spell spell = SpellsLibrary[id];
            ParentOfThisForm.LoadedCharacter.AddOrUpdateSpell(spell);

            DrawAssignedSpellsList();

            SelectSpell(id);
        }

        //Close this form.
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Delete the currently selected spell.
        private void Button_Delete_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_ID"].Value.ToString();
            }
            catch
            {
                //complain?
                //This shouldn't happen.
                MessageBox.Show("Error: couldn't retrieve spell ID.");
            }

            if (id == null)
            {
                MessageBox.Show("Unable to find id");
                return;
            }
            Spell spell = SpellsLibrary[id];

            //Delete the ability from the library.
            SpellsLibrary.Remove(id);
            ParentOfThisForm.RemoveSpell(id);

            DrawSpellsLibraryList();
            DrawAssignedSpellsList();

            //Make sure nothing is selected, since we just deleted the only thing that was.
            SelectSpell("");
        }

        //Clear the inputs and get ready for a new spell to be entered.
        private void Button_New_Click(object sender, EventArgs e)
        {
            DisplaySpell(new Spell());
        }

        //Remove the currently selected spell from the character.
        private void Button_Remove_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_ID"].Value.ToString();
            }
            catch
            {
                //complain?
                //This shouldn't happen.
                MessageBox.Show("Error: couldn't retrieve spell ID.");
            }

            if (id == null)
            {
                return;
            }

            //Remove the spell from the current character.
            ParentOfThisForm.LoadedCharacter.RemoveSpell(id);

            DrawAssignedSpellsList();

            SelectSpell(id);
        }

        //Save the information from the inputs into a new spell.
        private void Button_Save_Click(object sender, EventArgs e)
        {
            Spell spell = GetValuesFromFields();

            //Make sure the new / updated ability meets certain requirements.
            if (spell.ID == "")
            {
                //complain?
                return;
            }
            //more?

            if (SpellsLibrary.ContainsKey(spell.ID))
            {
                //This ability exists in the list, overwrite it.
                var confirmation = MessageBox.Show("You are about to overwrite an existing ability, do you want to continue?", "Confirm Delete!!", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.Yes)
                {
                    //AbilitiesLibrary.Add(ability.ID, ability);
                    //Overwrite the ability.
                    SpellsLibrary[spell.ID] = spell;
                }
                else
                {
                    //Don't overwrite the ability.
                }
            }
            else
            {
                //This ability does not exist in the list, add it.
                SpellsLibrary.Add(spell.ID, spell);
            }

            //Save library here
            ParentOfThisForm.SaveUpdatedSpell(spell);

            //Redraw the lists
            DrawSpellsLibraryList();
            DrawAssignedSpellsList();

            SelectSpell(spell.ID);
        }

        //Clean things up when the form is closing.
        private void EditSpells_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ParentOfThisForm != null)
            {
                ParentOfThisForm.ClosingEditingSpells();
            }
        }

        //For some reason, it's necessary to do this here in order to be able to actually clear the selections later.
        private void EditSpells_Load(object sender, EventArgs e)
        {
            AssignedSpellsList.ClearSelection();
            SpellsLibraryList.ClearSelection();
        }

        //A row in the spells library list was just clicked.
        //Select the corresponding row in the assigned list and display the spell's info.
        private void SpellsLibraryList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                return;
            }

            //Figure out which spell was just selected.
            string id = "";
            if (SpellsLibraryList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no spell selected.
                //Just leave id blank.
            }
            else
            {
                try
                {
                    id = SpellsLibraryList.SelectedRows[0].Cells["Library_ID"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                    id = "";
                }
            }

            SelectSpell(id);
        }

        #endregion
    }
}
