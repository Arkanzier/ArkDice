namespace Simple_Dice_Roller
{
    partial class EditCharacter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label_Strength = new Label();
            Label_Dexterity = new Label();
            Label_Constitution = new Label();
            Label_Intelligence = new Label();
            Label_Wisdom = new Label();
            Label_Charisma = new Label();
            Input_Str = new TextBox();
            Input_Dex = new TextBox();
            Input_Con = new TextBox();
            Input_Int = new TextBox();
            Input_Wis = new TextBox();
            Input_Cha = new TextBox();
            Button_Save = new Button();
            Wrapper_BasicAttributes = new GroupBox();
            Wrapper_Stats = new GroupBox();
            Wrapper_Classes = new GroupBox();
            Label_Name = new Label();
            Label_ID = new Label();
            Input_Name = new TextBox();
            Input_ID = new TextBox();
            Label_Race = new Label();
            Input_Race = new TextBox();
            Label_Subrace = new Label();
            Input_Subrace = new TextBox();
            Wrapper_BasicAttributes.SuspendLayout();
            Wrapper_Stats.SuspendLayout();
            SuspendLayout();
            // 
            // Label_Strength
            // 
            Label_Strength.AutoSize = true;
            Label_Strength.Location = new Point(6, 19);
            Label_Strength.Name = "Label_Strength";
            Label_Strength.Size = new Size(52, 15);
            Label_Strength.TabIndex = 1;
            Label_Strength.Text = "Strength";
            // 
            // Label_Dexterity
            // 
            Label_Dexterity.AutoSize = true;
            Label_Dexterity.Location = new Point(6, 48);
            Label_Dexterity.Name = "Label_Dexterity";
            Label_Dexterity.Size = new Size(54, 15);
            Label_Dexterity.TabIndex = 2;
            Label_Dexterity.Text = "Dexterity";
            // 
            // Label_Constitution
            // 
            Label_Constitution.AutoSize = true;
            Label_Constitution.Location = new Point(6, 77);
            Label_Constitution.Name = "Label_Constitution";
            Label_Constitution.Size = new Size(73, 15);
            Label_Constitution.TabIndex = 3;
            Label_Constitution.Text = "Constitution";
            // 
            // Label_Intelligence
            // 
            Label_Intelligence.AutoSize = true;
            Label_Intelligence.Location = new Point(6, 106);
            Label_Intelligence.Name = "Label_Intelligence";
            Label_Intelligence.Size = new Size(68, 15);
            Label_Intelligence.TabIndex = 4;
            Label_Intelligence.Text = "Intelligence";
            // 
            // Label_Wisdom
            // 
            Label_Wisdom.AutoSize = true;
            Label_Wisdom.Location = new Point(7, 135);
            Label_Wisdom.Name = "Label_Wisdom";
            Label_Wisdom.Size = new Size(51, 15);
            Label_Wisdom.TabIndex = 5;
            Label_Wisdom.Text = "Wisdom";
            // 
            // Label_Charisma
            // 
            Label_Charisma.AutoSize = true;
            Label_Charisma.Location = new Point(7, 164);
            Label_Charisma.Name = "Label_Charisma";
            Label_Charisma.Size = new Size(57, 15);
            Label_Charisma.TabIndex = 6;
            Label_Charisma.Text = "Charisma";
            // 
            // Input_Str
            // 
            Input_Str.Location = new Point(100, 19);
            Input_Str.Name = "Input_Str";
            Input_Str.Size = new Size(100, 23);
            Input_Str.TabIndex = 7;
            // 
            // Input_Dex
            // 
            Input_Dex.Location = new Point(100, 48);
            Input_Dex.Name = "Input_Dex";
            Input_Dex.Size = new Size(100, 23);
            Input_Dex.TabIndex = 8;
            // 
            // Input_Con
            // 
            Input_Con.Location = new Point(100, 77);
            Input_Con.Name = "Input_Con";
            Input_Con.Size = new Size(100, 23);
            Input_Con.TabIndex = 9;
            // 
            // Input_Int
            // 
            Input_Int.Location = new Point(100, 106);
            Input_Int.Name = "Input_Int";
            Input_Int.Size = new Size(100, 23);
            Input_Int.TabIndex = 10;
            // 
            // Input_Wis
            // 
            Input_Wis.Location = new Point(100, 135);
            Input_Wis.Name = "Input_Wis";
            Input_Wis.Size = new Size(100, 23);
            Input_Wis.TabIndex = 11;
            // 
            // Input_Cha
            // 
            Input_Cha.Location = new Point(100, 164);
            Input_Cha.Name = "Input_Cha";
            Input_Cha.Size = new Size(100, 23);
            Input_Cha.TabIndex = 12;
            // 
            // Button_Save
            // 
            Button_Save.Location = new Point(713, 415);
            Button_Save.Name = "Button_Save";
            Button_Save.Size = new Size(75, 23);
            Button_Save.TabIndex = 13;
            Button_Save.Text = "Save";
            Button_Save.UseVisualStyleBackColor = true;
            Button_Save.Click += Button_Save_Click;
            // 
            // Wrapper_BasicAttributes
            // 
            Wrapper_BasicAttributes.Controls.Add(Label_Subrace);
            Wrapper_BasicAttributes.Controls.Add(Input_Subrace);
            Wrapper_BasicAttributes.Controls.Add(Input_ID);
            Wrapper_BasicAttributes.Controls.Add(Input_Name);
            Wrapper_BasicAttributes.Controls.Add(Label_Race);
            Wrapper_BasicAttributes.Controls.Add(Input_Race);
            Wrapper_BasicAttributes.Controls.Add(Label_ID);
            Wrapper_BasicAttributes.Controls.Add(Label_Name);
            Wrapper_BasicAttributes.Location = new Point(13, 12);
            Wrapper_BasicAttributes.Name = "Wrapper_BasicAttributes";
            Wrapper_BasicAttributes.Size = new Size(340, 80);
            Wrapper_BasicAttributes.TabIndex = 14;
            Wrapper_BasicAttributes.TabStop = false;
            Wrapper_BasicAttributes.Text = "Basic Attributes";
            // 
            // Wrapper_Stats
            // 
            Wrapper_Stats.Controls.Add(Input_Str);
            Wrapper_Stats.Controls.Add(Label_Strength);
            Wrapper_Stats.Controls.Add(Input_Dex);
            Wrapper_Stats.Controls.Add(Label_Charisma);
            Wrapper_Stats.Controls.Add(Input_Cha);
            Wrapper_Stats.Controls.Add(Label_Wisdom);
            Wrapper_Stats.Controls.Add(Input_Con);
            Wrapper_Stats.Controls.Add(Label_Intelligence);
            Wrapper_Stats.Controls.Add(Input_Wis);
            Wrapper_Stats.Controls.Add(Label_Constitution);
            Wrapper_Stats.Controls.Add(Input_Int);
            Wrapper_Stats.Controls.Add(Label_Dexterity);
            Wrapper_Stats.Location = new Point(13, 98);
            Wrapper_Stats.Name = "Wrapper_Stats";
            Wrapper_Stats.Size = new Size(221, 201);
            Wrapper_Stats.TabIndex = 15;
            Wrapper_Stats.TabStop = false;
            Wrapper_Stats.Text = "Stats";
            Wrapper_Stats.Enter += Wrapper_Stats_Enter;
            // 
            // Wrapper_Classes
            // 
            Wrapper_Classes.Location = new Point(359, 12);
            Wrapper_Classes.Name = "Wrapper_Classes";
            Wrapper_Classes.Size = new Size(429, 149);
            Wrapper_Classes.TabIndex = 16;
            Wrapper_Classes.TabStop = false;
            Wrapper_Classes.Text = "Classes";
            // 
            // Label_Name
            // 
            Label_Name.AutoSize = true;
            Label_Name.Location = new Point(7, 19);
            Label_Name.Name = "Label_Name";
            Label_Name.Size = new Size(39, 15);
            Label_Name.TabIndex = 0;
            Label_Name.Text = "Name";
            // 
            // Label_ID
            // 
            Label_ID.AutoSize = true;
            Label_ID.Location = new Point(7, 48);
            Label_ID.Name = "Label_ID";
            Label_ID.Size = new Size(18, 15);
            Label_ID.TabIndex = 1;
            Label_ID.Text = "ID";
            // 
            // Input_Name
            // 
            Input_Name.Location = new Point(52, 19);
            Input_Name.Name = "Input_Name";
            Input_Name.Size = new Size(100, 23);
            Input_Name.TabIndex = 2;
            // 
            // Input_ID
            // 
            Input_ID.Location = new Point(52, 48);
            Input_ID.Name = "Input_ID";
            Input_ID.Size = new Size(100, 23);
            Input_ID.TabIndex = 3;
            // 
            // Label_Race
            // 
            Label_Race.AutoSize = true;
            Label_Race.Location = new Point(189, 19);
            Label_Race.Name = "Label_Race";
            Label_Race.Size = new Size(32, 15);
            Label_Race.TabIndex = 17;
            Label_Race.Text = "Race";
            // 
            // Input_Race
            // 
            Input_Race.Location = new Point(227, 19);
            Input_Race.Name = "Input_Race";
            Input_Race.Size = new Size(100, 23);
            Input_Race.TabIndex = 18;
            // 
            // Label_Subrace
            // 
            Label_Subrace.AutoSize = true;
            Label_Subrace.Location = new Point(172, 48);
            Label_Subrace.Name = "Label_Subrace";
            Label_Subrace.Size = new Size(49, 15);
            Label_Subrace.TabIndex = 19;
            Label_Subrace.Text = "Subrace";
            // 
            // Input_Subrace
            // 
            Input_Subrace.Location = new Point(227, 48);
            Input_Subrace.Name = "Input_Subrace";
            Input_Subrace.Size = new Size(100, 23);
            Input_Subrace.TabIndex = 20;
            // 
            // EditCharacter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Wrapper_Classes);
            Controls.Add(Wrapper_Stats);
            Controls.Add(Wrapper_BasicAttributes);
            Controls.Add(Button_Save);
            Name = "EditCharacter";
            Text = "EditCharacter";
            FormClosing += EditCharacter_FormClosing;
            Wrapper_BasicAttributes.ResumeLayout(false);
            Wrapper_BasicAttributes.PerformLayout();
            Wrapper_Stats.ResumeLayout(false);
            Wrapper_Stats.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label Label_Strength;
        private Label Label_Dexterity;
        private Label Label_Constitution;
        private Label Label_Intelligence;
        private Label Label_Wisdom;
        private Label Label_Charisma;
        private TextBox Input_Str;
        private TextBox Input_Dex;
        private TextBox Input_Con;
        private TextBox Input_Int;
        private TextBox Input_Wis;
        private TextBox Input_Cha;
        private Button Button_Save;
        private GroupBox Wrapper_BasicAttributes;
        private GroupBox Wrapper_Stats;
        private GroupBox Wrapper_Classes;
        private Label Label_Name;
        private Label Label_ID;
        private TextBox Input_ID;
        private TextBox Input_Name;
        private Label Label_Race;
        private TextBox Input_Race;
        private Label Label_Subrace;
        private TextBox Input_Subrace;
    }
}