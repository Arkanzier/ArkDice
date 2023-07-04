namespace Simple_Dice_Roller
{
    partial class EditSpells
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
            SpellsLibraryList = new DataGridView();
            Library_ID = new DataGridViewTextBoxColumn();
            Library_Name = new DataGridViewTextBoxColumn();
            Library_Level = new DataGridViewTextBoxColumn();
            Label_SpellsLibrary = new Label();
            Label_KnownSpells = new Label();
            AssignedSpellsList = new DataGridView();
            Assigned_ID = new DataGridViewTextBoxColumn();
            Assigned_Name = new DataGridViewTextBoxColumn();
            Assigned_Level = new DataGridViewTextBoxColumn();
            Button_Delete = new Button();
            Button_Remove = new Button();
            Button_Add = new Button();
            Label_Name = new Label();
            Label_ID = new Label();
            Input_Name = new TextBox();
            Input_ID = new TextBox();
            Label_Level = new Label();
            Input_Level = new TextBox();
            Label_School = new Label();
            Input_School = new TextBox();
            Input_Vocal = new CheckBox();
            Input_Somatic = new CheckBox();
            Input_Material = new CheckBox();
            Input_ExpensiveMaterial = new TextBox();
            Label_ExpensiveMaterial = new Label();
            Input_UpcastingBenefit = new TextBox();
            Label_UpcastingBenefit = new Label();
            Input_Description = new TextBox();
            Label_Description = new Label();
            Input_Concentration = new CheckBox();
            Label_Duration = new Label();
            Input_Duration = new TextBox();
            Label_Action = new Label();
            Label_Range = new Label();
            Label_Book = new Label();
            Label_Page = new Label();
            Input_Range = new TextBox();
            Input_Action = new TextBox();
            Input_Book = new TextBox();
            Input_Page = new TextBox();
            Button_New = new Button();
            Button_Save = new Button();
            Button_Close = new Button();
            ((System.ComponentModel.ISupportInitialize)SpellsLibraryList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AssignedSpellsList).BeginInit();
            SuspendLayout();
            // 
            // SpellsLibraryList
            // 
            SpellsLibraryList.AllowUserToAddRows = false;
            SpellsLibraryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SpellsLibraryList.Columns.AddRange(new DataGridViewColumn[] { Library_ID, Library_Name, Library_Level });
            SpellsLibraryList.EditMode = DataGridViewEditMode.EditProgrammatically;
            SpellsLibraryList.Location = new Point(12, 27);
            SpellsLibraryList.MultiSelect = false;
            SpellsLibraryList.Name = "SpellsLibraryList";
            SpellsLibraryList.RowTemplate.Height = 25;
            SpellsLibraryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SpellsLibraryList.Size = new Size(390, 423);
            SpellsLibraryList.TabIndex = 0;
            SpellsLibraryList.SelectionChanged += SpellsLibraryList_SelectionChanged;
            SpellsLibraryList.SortCompare += SpellsLibraryList_SortCompare;
            // 
            // Library_ID
            // 
            Library_ID.HeaderText = "ID";
            Library_ID.Name = "Library_ID";
            Library_ID.Visible = false;
            // 
            // Library_Name
            // 
            Library_Name.HeaderText = "Name";
            Library_Name.Name = "Library_Name";
            // 
            // Library_Level
            // 
            Library_Level.HeaderText = "Level";
            Library_Level.Name = "Library_Level";
            // 
            // Label_SpellsLibrary
            // 
            Label_SpellsLibrary.AutoSize = true;
            Label_SpellsLibrary.Location = new Point(12, 9);
            Label_SpellsLibrary.Name = "Label_SpellsLibrary";
            Label_SpellsLibrary.Size = new Size(76, 15);
            Label_SpellsLibrary.TabIndex = 1;
            Label_SpellsLibrary.Text = "Spells Library";
            // 
            // Label_KnownSpells
            // 
            Label_KnownSpells.AutoSize = true;
            Label_KnownSpells.Location = new Point(408, 9);
            Label_KnownSpells.Name = "Label_KnownSpells";
            Label_KnownSpells.Size = new Size(77, 15);
            Label_KnownSpells.TabIndex = 2;
            Label_KnownSpells.Text = "Known Spells";
            // 
            // AssignedSpellsList
            // 
            AssignedSpellsList.AllowUserToAddRows = false;
            AssignedSpellsList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AssignedSpellsList.Columns.AddRange(new DataGridViewColumn[] { Assigned_ID, Assigned_Name, Assigned_Level });
            AssignedSpellsList.EditMode = DataGridViewEditMode.EditProgrammatically;
            AssignedSpellsList.Location = new Point(408, 27);
            AssignedSpellsList.MultiSelect = false;
            AssignedSpellsList.Name = "AssignedSpellsList";
            AssignedSpellsList.RowTemplate.Height = 25;
            AssignedSpellsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AssignedSpellsList.Size = new Size(380, 423);
            AssignedSpellsList.TabIndex = 3;
            AssignedSpellsList.SelectionChanged += AssignedSpellsList_SelectionChanged;
            AssignedSpellsList.SortCompare += AssignedSpellsList_SortCompare;
            // 
            // Assigned_ID
            // 
            Assigned_ID.HeaderText = "ID";
            Assigned_ID.Name = "Assigned_ID";
            Assigned_ID.Visible = false;
            // 
            // Assigned_Name
            // 
            Assigned_Name.HeaderText = "Name";
            Assigned_Name.Name = "Assigned_Name";
            // 
            // Assigned_Level
            // 
            Assigned_Level.HeaderText = "Level";
            Assigned_Level.Name = "Assigned_Level";
            // 
            // Button_Delete
            // 
            Button_Delete.Location = new Point(13, 456);
            Button_Delete.Name = "Button_Delete";
            Button_Delete.Size = new Size(100, 23);
            Button_Delete.TabIndex = 4;
            Button_Delete.Text = "Delete Spell";
            Button_Delete.UseVisualStyleBackColor = true;
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Button_Remove
            // 
            Button_Remove.Location = new Point(252, 456);
            Button_Remove.Name = "Button_Remove";
            Button_Remove.Size = new Size(150, 23);
            Button_Remove.TabIndex = 5;
            Button_Remove.Text = "Remove from Character";
            Button_Remove.UseVisualStyleBackColor = true;
            Button_Remove.Click += Button_Remove_Click;
            // 
            // Button_Add
            // 
            Button_Add.Location = new Point(408, 456);
            Button_Add.Name = "Button_Add";
            Button_Add.Size = new Size(150, 23);
            Button_Add.TabIndex = 6;
            Button_Add.Text = "Add to Character";
            Button_Add.UseVisualStyleBackColor = true;
            Button_Add.Click += Button_Add_Click;
            // 
            // Label_Name
            // 
            Label_Name.AutoSize = true;
            Label_Name.Location = new Point(13, 488);
            Label_Name.Name = "Label_Name";
            Label_Name.Size = new Size(39, 15);
            Label_Name.TabIndex = 7;
            Label_Name.Text = "Name";
            // 
            // Label_ID
            // 
            Label_ID.AutoSize = true;
            Label_ID.Location = new Point(13, 517);
            Label_ID.Name = "Label_ID";
            Label_ID.Size = new Size(18, 15);
            Label_ID.TabIndex = 8;
            Label_ID.Text = "ID";
            // 
            // Input_Name
            // 
            Input_Name.Location = new Point(68, 485);
            Input_Name.Name = "Input_Name";
            Input_Name.Size = new Size(100, 23);
            Input_Name.TabIndex = 9;
            // 
            // Input_ID
            // 
            Input_ID.Location = new Point(68, 514);
            Input_ID.Name = "Input_ID";
            Input_ID.Size = new Size(100, 23);
            Input_ID.TabIndex = 10;
            // 
            // Label_Level
            // 
            Label_Level.AutoSize = true;
            Label_Level.Location = new Point(196, 488);
            Label_Level.Name = "Label_Level";
            Label_Level.Size = new Size(34, 15);
            Label_Level.TabIndex = 11;
            Label_Level.Text = "Level";
            // 
            // Input_Level
            // 
            Input_Level.Location = new Point(236, 485);
            Input_Level.Name = "Input_Level";
            Input_Level.Size = new Size(100, 23);
            Input_Level.TabIndex = 12;
            // 
            // Label_School
            // 
            Label_School.AutoSize = true;
            Label_School.Location = new Point(187, 517);
            Label_School.Name = "Label_School";
            Label_School.Size = new Size(43, 15);
            Label_School.TabIndex = 13;
            Label_School.Text = "School";
            // 
            // Input_School
            // 
            Input_School.Location = new Point(236, 514);
            Input_School.Name = "Input_School";
            Input_School.Size = new Size(100, 23);
            Input_School.TabIndex = 14;
            // 
            // Input_Vocal
            // 
            Input_Vocal.AutoSize = true;
            Input_Vocal.Location = new Point(342, 488);
            Input_Vocal.Name = "Input_Vocal";
            Input_Vocal.Size = new Size(54, 19);
            Input_Vocal.TabIndex = 15;
            Input_Vocal.Text = "Vocal";
            Input_Vocal.UseVisualStyleBackColor = true;
            // 
            // Input_Somatic
            // 
            Input_Somatic.AutoSize = true;
            Input_Somatic.Location = new Point(342, 513);
            Input_Somatic.Name = "Input_Somatic";
            Input_Somatic.Size = new Size(69, 19);
            Input_Somatic.TabIndex = 16;
            Input_Somatic.Text = "Somatic";
            Input_Somatic.UseVisualStyleBackColor = true;
            // 
            // Input_Material
            // 
            Input_Material.AutoSize = true;
            Input_Material.Location = new Point(342, 538);
            Input_Material.Name = "Input_Material";
            Input_Material.Size = new Size(69, 19);
            Input_Material.TabIndex = 17;
            Input_Material.Text = "Material";
            Input_Material.UseVisualStyleBackColor = true;
            // 
            // Input_ExpensiveMaterial
            // 
            Input_ExpensiveMaterial.Location = new Point(342, 563);
            Input_ExpensiveMaterial.Name = "Input_ExpensiveMaterial";
            Input_ExpensiveMaterial.Size = new Size(100, 23);
            Input_ExpensiveMaterial.TabIndex = 18;
            // 
            // Label_ExpensiveMaterial
            // 
            Label_ExpensiveMaterial.AutoSize = true;
            Label_ExpensiveMaterial.Location = new Point(231, 566);
            Label_ExpensiveMaterial.Name = "Label_ExpensiveMaterial";
            Label_ExpensiveMaterial.Size = new Size(105, 15);
            Label_ExpensiveMaterial.TabIndex = 19;
            Label_ExpensiveMaterial.Text = "Expensive Material";
            // 
            // Input_UpcastingBenefit
            // 
            Input_UpcastingBenefit.Location = new Point(12, 705);
            Input_UpcastingBenefit.Multiline = true;
            Input_UpcastingBenefit.Name = "Input_UpcastingBenefit";
            Input_UpcastingBenefit.Size = new Size(600, 50);
            Input_UpcastingBenefit.TabIndex = 20;
            // 
            // Label_UpcastingBenefit
            // 
            Label_UpcastingBenefit.AutoSize = true;
            Label_UpcastingBenefit.Location = new Point(14, 687);
            Label_UpcastingBenefit.Name = "Label_UpcastingBenefit";
            Label_UpcastingBenefit.Size = new Size(100, 15);
            Label_UpcastingBenefit.TabIndex = 21;
            Label_UpcastingBenefit.Text = "Upcasting Benefit";
            // 
            // Input_Description
            // 
            Input_Description.Location = new Point(14, 592);
            Input_Description.Multiline = true;
            Input_Description.Name = "Input_Description";
            Input_Description.Size = new Size(600, 92);
            Input_Description.TabIndex = 22;
            // 
            // Label_Description
            // 
            Label_Description.AutoSize = true;
            Label_Description.Location = new Point(14, 574);
            Label_Description.Name = "Label_Description";
            Label_Description.Size = new Size(67, 15);
            Label_Description.TabIndex = 23;
            Label_Description.Text = "Description";
            // 
            // Input_Concentration
            // 
            Input_Concentration.AutoSize = true;
            Input_Concentration.Location = new Point(514, 516);
            Input_Concentration.Name = "Input_Concentration";
            Input_Concentration.Size = new Size(102, 19);
            Input_Concentration.TabIndex = 24;
            Input_Concentration.Text = "Concentration";
            Input_Concentration.UseVisualStyleBackColor = true;
            // 
            // Label_Duration
            // 
            Label_Duration.AutoSize = true;
            Label_Duration.Location = new Point(455, 489);
            Label_Duration.Name = "Label_Duration";
            Label_Duration.Size = new Size(53, 15);
            Label_Duration.TabIndex = 25;
            Label_Duration.Text = "Duration";
            // 
            // Input_Duration
            // 
            Input_Duration.Location = new Point(514, 486);
            Input_Duration.Name = "Input_Duration";
            Input_Duration.Size = new Size(100, 23);
            Input_Duration.TabIndex = 26;
            // 
            // Label_Action
            // 
            Label_Action.AutoSize = true;
            Label_Action.Location = new Point(187, 546);
            Label_Action.Name = "Label_Action";
            Label_Action.Size = new Size(42, 15);
            Label_Action.TabIndex = 27;
            Label_Action.Text = "Action";
            // 
            // Label_Range
            // 
            Label_Range.AutoSize = true;
            Label_Range.Location = new Point(466, 539);
            Label_Range.Name = "Label_Range";
            Label_Range.Size = new Size(40, 15);
            Label_Range.TabIndex = 28;
            Label_Range.Text = "Range";
            // 
            // Label_Book
            // 
            Label_Book.AutoSize = true;
            Label_Book.Location = new Point(648, 489);
            Label_Book.Name = "Label_Book";
            Label_Book.Size = new Size(34, 15);
            Label_Book.TabIndex = 29;
            Label_Book.Text = "Book";
            // 
            // Label_Page
            // 
            Label_Page.AutoSize = true;
            Label_Page.Location = new Point(648, 518);
            Label_Page.Name = "Label_Page";
            Label_Page.Size = new Size(33, 15);
            Label_Page.TabIndex = 30;
            Label_Page.Text = "Page";
            // 
            // Input_Range
            // 
            Input_Range.Location = new Point(512, 536);
            Input_Range.Name = "Input_Range";
            Input_Range.Size = new Size(100, 23);
            Input_Range.TabIndex = 31;
            // 
            // Input_Action
            // 
            Input_Action.Location = new Point(236, 543);
            Input_Action.Name = "Input_Action";
            Input_Action.Size = new Size(100, 23);
            Input_Action.TabIndex = 32;
            // 
            // Input_Book
            // 
            Input_Book.Location = new Point(688, 486);
            Input_Book.Name = "Input_Book";
            Input_Book.Size = new Size(100, 23);
            Input_Book.TabIndex = 33;
            // 
            // Input_Page
            // 
            Input_Page.Location = new Point(688, 515);
            Input_Page.Name = "Input_Page";
            Input_Page.Size = new Size(100, 23);
            Input_Page.TabIndex = 34;
            // 
            // Button_New
            // 
            Button_New.Location = new Point(713, 558);
            Button_New.Name = "Button_New";
            Button_New.Size = new Size(75, 23);
            Button_New.TabIndex = 35;
            Button_New.Text = "New Spell";
            Button_New.UseVisualStyleBackColor = true;
            Button_New.Click += Button_New_Click;
            // 
            // Button_Save
            // 
            Button_Save.Location = new Point(713, 587);
            Button_Save.Name = "Button_Save";
            Button_Save.Size = new Size(75, 23);
            Button_Save.TabIndex = 36;
            Button_Save.Text = "Save Spell";
            Button_Save.UseVisualStyleBackColor = true;
            Button_Save.Click += Button_Save_Click;
            // 
            // Button_Close
            // 
            Button_Close.Location = new Point(713, 732);
            Button_Close.Name = "Button_Close";
            Button_Close.Size = new Size(75, 23);
            Button_Close.TabIndex = 37;
            Button_Close.Text = "Close";
            Button_Close.UseVisualStyleBackColor = true;
            Button_Close.Click += Button_Close_Click;
            // 
            // EditSpells
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 767);
            Controls.Add(Button_Close);
            Controls.Add(Button_Save);
            Controls.Add(Button_New);
            Controls.Add(Input_Page);
            Controls.Add(Input_Book);
            Controls.Add(Input_Action);
            Controls.Add(Input_Range);
            Controls.Add(Label_Page);
            Controls.Add(Label_Book);
            Controls.Add(Label_Range);
            Controls.Add(Label_Action);
            Controls.Add(Input_Duration);
            Controls.Add(Label_Duration);
            Controls.Add(Input_Concentration);
            Controls.Add(Label_Description);
            Controls.Add(Input_Description);
            Controls.Add(Label_UpcastingBenefit);
            Controls.Add(Input_UpcastingBenefit);
            Controls.Add(Label_ExpensiveMaterial);
            Controls.Add(Input_ExpensiveMaterial);
            Controls.Add(Input_Material);
            Controls.Add(Input_Somatic);
            Controls.Add(Input_Vocal);
            Controls.Add(Input_School);
            Controls.Add(Label_School);
            Controls.Add(Input_Level);
            Controls.Add(Label_Level);
            Controls.Add(Input_ID);
            Controls.Add(Input_Name);
            Controls.Add(Label_ID);
            Controls.Add(Label_Name);
            Controls.Add(Button_Add);
            Controls.Add(Button_Remove);
            Controls.Add(Button_Delete);
            Controls.Add(AssignedSpellsList);
            Controls.Add(Label_KnownSpells);
            Controls.Add(Label_SpellsLibrary);
            Controls.Add(SpellsLibraryList);
            Name = "EditSpells";
            Text = "EditSpells";
            FormClosing += EditSpells_FormClosing;
            Load += EditSpells_Load;
            ((System.ComponentModel.ISupportInitialize)SpellsLibraryList).EndInit();
            ((System.ComponentModel.ISupportInitialize)AssignedSpellsList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView SpellsLibraryList;
        private Label Label_SpellsLibrary;
        private Label Label_KnownSpells;
        private DataGridView AssignedSpellsList;
        private Button Button_Delete;
        private Button Button_Remove;
        private Button Button_Add;
        private Label Label_Name;
        private Label Label_ID;
        private TextBox Input_Name;
        private TextBox Input_ID;
        private Label Label_Level;
        private TextBox Input_Level;
        private Label Label_School;
        private TextBox Input_School;
        private CheckBox Input_Vocal;
        private CheckBox Input_Somatic;
        private CheckBox Input_Material;
        private TextBox Input_ExpensiveMaterial;
        private Label Label_ExpensiveMaterial;
        private TextBox Input_UpcastingBenefit;
        private Label Label_UpcastingBenefit;
        private TextBox Input_Description;
        private Label Label_Description;
        private CheckBox Input_Concentration;
        private Label Label_Duration;
        private TextBox Input_Duration;
        private Label Label_Action;
        private Label Label_Range;
        private Label Label_Book;
        private Label Label_Page;
        private TextBox Input_Range;
        private TextBox Input_Action;
        private TextBox Input_Book;
        private TextBox Input_Page;
        private Button Button_New;
        private Button Button_Save;
        private Button Button_Close;
        private DataGridViewTextBoxColumn Library_ID;
        private DataGridViewTextBoxColumn Library_Name;
        private DataGridViewTextBoxColumn Library_Level;
        private DataGridViewTextBoxColumn Assigned_ID;
        private DataGridViewTextBoxColumn Assigned_Name;
        private DataGridViewTextBoxColumn Assigned_Level;
    }
}