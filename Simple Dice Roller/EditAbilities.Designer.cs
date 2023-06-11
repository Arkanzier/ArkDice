namespace Simple_Dice_Roller
{
    partial class EditAbilities
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
            AbilitiesLibrary = new DataGridView();
            Label_ExistingAbilities = new Label();
            Label_AssignedAbilities = new Label();
            AssignedAbilities = new DataGridView();
            Label_Name = new Label();
            Input_Name = new TextBox();
            Input_ID = new TextBox();
            Label_ID = new Label();
            Library_IDCol = new DataGridViewTextBoxColumn();
            Library_NameCol = new DataGridViewTextBoxColumn();
            Assigned_IDCol = new DataGridViewTextBoxColumn();
            Assigned_NameCol = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)AbilitiesLibrary).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AssignedAbilities).BeginInit();
            SuspendLayout();
            // 
            // AbilitiesLibrary
            // 
            AbilitiesLibrary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AbilitiesLibrary.Columns.AddRange(new DataGridViewColumn[] { Library_IDCol, Library_NameCol });
            AbilitiesLibrary.EditMode = DataGridViewEditMode.EditProgrammatically;
            AbilitiesLibrary.Location = new Point(12, 27);
            AbilitiesLibrary.MultiSelect = false;
            AbilitiesLibrary.Name = "AbilitiesLibrary";
            AbilitiesLibrary.RowTemplate.Height = 25;
            AbilitiesLibrary.Size = new Size(384, 488);
            AbilitiesLibrary.TabIndex = 0;
            // 
            // Label_ExistingAbilities
            // 
            Label_ExistingAbilities.AutoSize = true;
            Label_ExistingAbilities.Location = new Point(12, 9);
            Label_ExistingAbilities.Name = "Label_ExistingAbilities";
            Label_ExistingAbilities.Size = new Size(93, 15);
            Label_ExistingAbilities.TabIndex = 1;
            Label_ExistingAbilities.Text = "Existing Abilities";
            // 
            // Label_AssignedAbilities
            // 
            Label_AssignedAbilities.AutoSize = true;
            Label_AssignedAbilities.Location = new Point(402, 9);
            Label_AssignedAbilities.Name = "Label_AssignedAbilities";
            Label_AssignedAbilities.Size = new Size(100, 15);
            Label_AssignedAbilities.TabIndex = 2;
            Label_AssignedAbilities.Text = "Assigned Abilities";
            // 
            // AssignedAbilities
            // 
            AssignedAbilities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AssignedAbilities.Columns.AddRange(new DataGridViewColumn[] { Assigned_IDCol, Assigned_NameCol });
            AssignedAbilities.EditMode = DataGridViewEditMode.EditProgrammatically;
            AssignedAbilities.Location = new Point(402, 27);
            AssignedAbilities.MultiSelect = false;
            AssignedAbilities.Name = "AssignedAbilities";
            AssignedAbilities.RowTemplate.Height = 25;
            AssignedAbilities.Size = new Size(386, 488);
            AssignedAbilities.TabIndex = 3;
            // 
            // Label_Name
            // 
            Label_Name.AutoSize = true;
            Label_Name.Location = new Point(12, 524);
            Label_Name.Name = "Label_Name";
            Label_Name.Size = new Size(39, 15);
            Label_Name.TabIndex = 4;
            Label_Name.Text = "Name";
            // 
            // Input_Name
            // 
            Input_Name.Location = new Point(100, 521);
            Input_Name.Name = "Input_Name";
            Input_Name.Size = new Size(100, 23);
            Input_Name.TabIndex = 5;
            // 
            // Input_ID
            // 
            Input_ID.Location = new Point(100, 550);
            Input_ID.Name = "Input_ID";
            Input_ID.Size = new Size(100, 23);
            Input_ID.TabIndex = 6;
            // 
            // Label_ID
            // 
            Label_ID.AutoSize = true;
            Label_ID.Location = new Point(13, 553);
            Label_ID.Name = "Label_ID";
            Label_ID.Size = new Size(18, 15);
            Label_ID.TabIndex = 7;
            Label_ID.Text = "ID";
            // 
            // Library_IDCol
            // 
            Library_IDCol.HeaderText = "ID";
            Library_IDCol.Name = "Library_IDCol";
            Library_IDCol.Visible = false;
            // 
            // Library_NameCol
            // 
            Library_NameCol.HeaderText = "Name";
            Library_NameCol.Name = "Library_NameCol";
            Library_NameCol.Width = 200;
            // 
            // Assigned_IDCol
            // 
            Assigned_IDCol.HeaderText = "ID";
            Assigned_IDCol.Name = "Assigned_IDCol";
            Assigned_IDCol.Visible = false;
            // 
            // Assigned_NameCol
            // 
            Assigned_NameCol.HeaderText = "Name";
            Assigned_NameCol.Name = "Assigned_NameCol";
            Assigned_NameCol.Width = 200;
            // 
            // EditAbilities
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 774);
            Controls.Add(Label_ID);
            Controls.Add(Input_ID);
            Controls.Add(Input_Name);
            Controls.Add(Label_Name);
            Controls.Add(AssignedAbilities);
            Controls.Add(Label_AssignedAbilities);
            Controls.Add(Label_ExistingAbilities);
            Controls.Add(AbilitiesLibrary);
            Name = "EditAbilities";
            Text = "EditAbilities";
            ((System.ComponentModel.ISupportInitialize)AbilitiesLibrary).EndInit();
            ((System.ComponentModel.ISupportInitialize)AssignedAbilities).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView AbilitiesLibrary;
        private Label Label_ExistingAbilities;
        private Label Label_AssignedAbilities;
        private DataGridView AssignedAbilities;
        private Label Label_Name;
        private TextBox Input_Name;
        private TextBox Input_ID;
        private Label Label_ID;
        private DataGridViewTextBoxColumn Library_IDCol;
        private DataGridViewTextBoxColumn Library_NameCol;
        private DataGridViewTextBoxColumn Assigned_IDCol;
        private DataGridViewTextBoxColumn Assigned_NameCol;
    }
}