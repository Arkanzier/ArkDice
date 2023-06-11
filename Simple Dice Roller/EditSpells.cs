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

        public Form1? parent;

        public EditSpells()
        {
            CharacterID = "";
            EditingCharacter = new Character.Character();
            parent = null;

            InitializeComponent();
        }
    }
}
