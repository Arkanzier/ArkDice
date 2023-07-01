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

        //to do:
        //hook up the Prepared flag somewhere
        //spells tab: can toggle between all spells and prepared spells?
        //can toggle spells when seeing all?
        //replaces the use buttons?
        //I might need custom sorting to get the spell levels working properly.
        //then again, as long as it breaks ties by name I'm good.




        #region Functions triggered by the form

        //
        private void AssignedSpellsList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                return;
            }

            //Figure out which spell was just selected.
            Spell selectedSpell;
            if (AssignedSpellsList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no spell selected.
                //We can mimic this by passing a blank spell to the appropriate places.
                selectedSpell = new Spell();
            }
            else
            {
                //Get the spell's info.
                string id = "";
                try
                {
                    id = AssignedSpellsList.SelectedRows[0].Cells["Assigned_ID"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                }
                if (id == "")
                {
                    selectedSpell = new Spell();
                }
                else
                {
                    //to do: needs error protection.
                    //if the spell isn't in the library, this will cause problems.
                    //library cannot load after being saved. figure out why
                    if (id != null && SpellsLibrary.ContainsKey(id))
                    {
                        selectedSpell = SpellsLibrary[id];
                    }
                    else
                    {
                        //Complain?
                        selectedSpell = new Spell();
                        id = "";
                    }
                }

                //Now we have the spell, or a blank one for no selection.
                if (id != "")
                {
                    //Mirror this selection into the other grid, if the spell exists there.
                    int assignedIndex = GetIndexOfRowWithSpell(id, SpellsLibraryList);

                    if (assignedIndex >= 0)
                    {
                        //This spell exists in the library list. Select it there.
                        DrawingLists = true;
                        SpellsLibraryList.Rows[assignedIndex].Selected = true;
                        DrawingLists = false;
                    }
                }
            }
            //Now display the spell's info in the text boxes.
            DisplaySpell(selectedSpell);
        }

        //Add the currently selected spell to the character.
        private void Button_Add_Click(object sender, EventArgs e)
        {
            string? id = null;
            try
            {
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
            }
            catch
            {
                //complain?
                //This shouldn't happen.
            }


            if (id == null)
            {
                return;
            }
            Spell spell = SpellsLibrary[id];
            ParentOfThisForm.LoadedCharacter.AddOrUpdateSpell(spell);

            DrawAssignedSpellsList();
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
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
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
            ParentOfThisForm.RemoveAbility(id);

            DrawSpellsLibraryList();
            DrawAssignedSpellsList();

            AssignedSpellsList.ClearSelection();
            SpellsLibraryList.ClearSelection();
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
                id = SpellsLibraryList.SelectedRows[0].Cells["Library_IDCol"].Value.ToString();
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

            ParentOfThisForm.RemoveSpell(id);

            DrawAssignedSpellsList();
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

            //to do: select the spell again?
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

        //
        private void SpellsLibraryList_SelectionChanged(object sender, EventArgs e)
        {
            if (DrawingLists == true)
            {
                //This was only triggered as a result of redrawing the grid, we don't want to respond to it.
                return;
            }

            //Figure out which spell was just selected.
            Spell selectedSpell;
            if (SpellsLibraryList.SelectedRows.Count == 0)
            {
                //The selection was just cleared, there is no spell selected.
                //We can mimic this by passing a blank spell to the appropriate places.
                selectedSpell = new Spell();
            }
            else
            {
                //Get the spell's info.
                string id = "";
                try
                {
                    id = SpellsLibraryList.SelectedRows[0].Cells["Library_ID"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //do nothing?
                }
                if (id == "")
                {
                    selectedSpell = new Spell();
                }
                else
                {
                    selectedSpell = SpellsLibrary[id];
                }

                //Now we have the spell, or a blank one for no selection.
                if (id != "")
                {
                    //Mirror this selection into the other grid, if the spell exists there.
                    int assignedIndex = GetIndexOfRowWithSpell(id, AssignedSpellsList);

                    if (assignedIndex >= 0)
                    {
                        //This spell exists in the assigned list. Select it there.
                        DrawingLists = true;
                        AssignedSpellsList.Rows[assignedIndex].Selected = true;
                        DrawingLists = false;
                        //to do: how do i move the little triangle?
                    }
                    else
                    {
                        //There is no matching spell, clear the selection.
                        DrawingLists = true;
                        //unselect the selected row
                        AssignedSpellsList.ClearSelection();
                        DrawingLists = false;
                    }
                }
            }
            //Now display the spell's info in the text boxes.
            DisplaySpell(selectedSpell);
        }

        #endregion
    }
}
