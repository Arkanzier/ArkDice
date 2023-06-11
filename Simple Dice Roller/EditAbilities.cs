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
    public partial class EditAbilities : Form
    {
        internal string CharacterID;
        internal Character.Character EditingCharacter;

        public Form1? ParentForm;

        public EditAbilities()
        {
            CharacterID = "";
            EditingCharacter = new Character.Character();
            ParentForm = null;

            InitializeComponent();

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }
        public EditAbilities(Character.Character character)
            : this()
        {
            EditingCharacter = character;
        }
        public EditAbilities(Form1 form)
            : this()
        {
            ParentForm = form;
            EditingCharacter = ParentForm.LoadedCharacter;

            //MessageBox.Show("Loaded character " + EditingCharacter.Name);

            DrawAbilitiesLibrary();
            DrawAssignedAbilitiesList();
        }


        //Draws the list of existing abilities.
        private void DrawAbilitiesLibrary()
        {
            AbilitiesLibrary.Rows.Clear();

            //to do: replace with an abilities library
            //hide rows here that exist in the other list?

            for (int a = 0; a < EditingCharacter.GetAbilities().Count(); a++)
            {
                Ability thisAbility = EditingCharacter.GetAbilities()[a];
                string id = thisAbility.ID;
                string name = thisAbility.Name;
                AbilitiesLibrary.Rows.Insert(a, id, name);
            }
        }

        //Draws the list of abilities assigned to the current character.
        private void DrawAssignedAbilitiesList()
        {
            AssignedAbilities.Rows.Clear();

            //to do: replace with an abilities library

            for (int a = 0; a < EditingCharacter.GetAbilities().Count(); a++)
            {
                Ability thisAbility = EditingCharacter.GetAbilities()[a];
                string id = thisAbility.ID;
                string name = thisAbility.Name;
                AssignedAbilities.Rows.Insert(a, id, name);
            }
        }

        //Takes the selected ability and loads it into the text boxes and such.
        //To do: need a way to tell which list to load from.
        //select the same ability in both lists.
        private void SelectAbility(string id)
        {
            //Make the ability selected in both lists, if it exists in both.
            //It should definitely exist in the library side.
            foreach (DataGridViewRow row in AbilitiesLibrary.Rows)
            {
                string rowid = row.Cells["ID"].ToString();
            }

            //find that ability in both lists, if it exists, and select it both times.


        }
    }
}
