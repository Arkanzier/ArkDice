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

        public Form1? parent;

        public EditCharacter()
        {
            InitializeComponent();

            CharacterID = "";
            EditingCharacter = new Character.Character();
            parent = null;
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

        //Loads the character's info into the various spots in the form.
        private void LoadCharacter()
        {
            Input_Str.Text = EditingCharacter.GetStr().ToString();
            Input_Dex.Text = EditingCharacter.GetDex().ToString();
            Input_Con.Text = EditingCharacter.GetCon().ToString();
            Input_Int.Text = EditingCharacter.GetInt().ToString();
            Input_Wis.Text = EditingCharacter.GetWis().ToString();
            Input_Cha.Text = EditingCharacter.GetCha().ToString();

            Input_Name.Text = EditingCharacter.Name;
            Input_ID.Text = EditingCharacter.ID;

            Input_Race.Text = EditingCharacter.Race;
            Input_Subrace.Text = EditingCharacter.Subrace;

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

            //Do some extra error checking?

            //Save the info
            EditingCharacter.IncorporateChanges(changes);

            return true;
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
            if (parent != null)
            {
                parent.ClosingEditingCharacter();
            }
        }

        private void Wrapper_Stats_Enter(object sender, EventArgs e)
        {

        }
    }
}
