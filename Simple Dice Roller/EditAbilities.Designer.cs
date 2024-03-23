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
            AbilitiesLibraryList = new DataGridView();
            Library_IDCol = new DataGridViewTextBoxColumn();
            Library_NameCol = new DataGridViewTextBoxColumn();
            Label_ExistingAbilities = new Label();
            Label_AssignedAbilities = new Label();
            AssignedAbilitiesList = new DataGridView();
            Assigned_IDCol = new DataGridViewTextBoxColumn();
            Assigned_NameCol = new DataGridViewTextBoxColumn();
            Label_Name = new Label();
            Input_Name = new TextBox();
            Input_ID = new TextBox();
            Label_ID = new Label();
            Button_Remove = new Button();
            Button_Add = new Button();
            Input_Text = new TextBox();
            Button_Close = new Button();
            Button_Save = new Button();
            Label_Text = new Label();
            Label_Uses = new Label();
            Label_MaxUses = new Label();
            Label_UsesChange = new Label();
            Label_RechargeCondition = new Label();
            Label_RechargeAmount = new Label();
            Input_Uses = new TextBox();
            Input_MaxUses = new TextBox();
            Input_UsesChange = new TextBox();
            Input_RechargeCondition = new TextBox();
            Input_RechargeAmount = new TextBox();
            Label_Action = new Label();
            Label_Dice = new Label();
            Label_DisplayTier = new Label();
            Input_Action = new TextBox();
            Input_Dice = new TextBox();
            Input_DisplayTier = new TextBox();
            Button_Delete = new Button();
            Button_Test = new Button();
            Button_NewAbility = new Button();
            Button_SaveAbility = new Button();
            ((System.ComponentModel.ISupportInitialize)AbilitiesLibraryList).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AssignedAbilitiesList).BeginInit();
            SuspendLayout();
            // 
            // AbilitiesLibraryList
            // 
            AbilitiesLibraryList.AllowUserToAddRows = false;
            AbilitiesLibraryList.AllowUserToDeleteRows = false;
            AbilitiesLibraryList.AllowUserToResizeColumns = false;
            AbilitiesLibraryList.AllowUserToResizeRows = false;
            AbilitiesLibraryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AbilitiesLibraryList.Columns.AddRange(new DataGridViewColumn[] { Library_IDCol, Library_NameCol });
            AbilitiesLibraryList.EditMode = DataGridViewEditMode.EditProgrammatically;
            AbilitiesLibraryList.Location = new Point(12, 27);
            AbilitiesLibraryList.MultiSelect = false;
            AbilitiesLibraryList.Name = "AbilitiesLibraryList";
            AbilitiesLibraryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AbilitiesLibraryList.Size = new Size(384, 447);
            AbilitiesLibraryList.TabIndex = 17;
            AbilitiesLibraryList.SelectionChanged += AbilitiesLibraryList_SelectionChanged;
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
            // AssignedAbilitiesList
            // 
            AssignedAbilitiesList.AllowUserToAddRows = false;
            AssignedAbilitiesList.AllowUserToDeleteRows = false;
            AssignedAbilitiesList.AllowUserToResizeColumns = false;
            AssignedAbilitiesList.AllowUserToResizeRows = false;
            AssignedAbilitiesList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AssignedAbilitiesList.Columns.AddRange(new DataGridViewColumn[] { Assigned_IDCol, Assigned_NameCol });
            AssignedAbilitiesList.EditMode = DataGridViewEditMode.EditProgrammatically;
            AssignedAbilitiesList.Location = new Point(402, 27);
            AssignedAbilitiesList.MultiSelect = false;
            AssignedAbilitiesList.Name = "AssignedAbilitiesList";
            AssignedAbilitiesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AssignedAbilitiesList.Size = new Size(386, 447);
            AssignedAbilitiesList.TabIndex = 18;
            AssignedAbilitiesList.SelectionChanged += AssignedAbilitiesList_SelectionChanged;
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
            // Label_Name
            // 
            Label_Name.AutoSize = true;
            Label_Name.Location = new Point(12, 512);
            Label_Name.Name = "Label_Name";
            Label_Name.Size = new Size(39, 15);
            Label_Name.TabIndex = 4;
            Label_Name.Text = "Name";
            // 
            // Input_Name
            // 
            Input_Name.Location = new Point(62, 509);
            Input_Name.Name = "Input_Name";
            Input_Name.Size = new Size(100, 23);
            Input_Name.TabIndex = 1;
            // 
            // Input_ID
            // 
            Input_ID.Location = new Point(62, 538);
            Input_ID.Name = "Input_ID";
            Input_ID.Size = new Size(100, 23);
            Input_ID.TabIndex = 2;
            // 
            // Label_ID
            // 
            Label_ID.AutoSize = true;
            Label_ID.Location = new Point(12, 541);
            Label_ID.Name = "Label_ID";
            Label_ID.Size = new Size(18, 15);
            Label_ID.TabIndex = 7;
            Label_ID.Text = "ID";
            // 
            // Button_Remove
            // 
            Button_Remove.Location = new Point(246, 480);
            Button_Remove.Name = "Button_Remove";
            Button_Remove.Size = new Size(150, 23);
            Button_Remove.TabIndex = 15;
            Button_Remove.Text = "Remove from Character";
            Button_Remove.UseVisualStyleBackColor = true;
            Button_Remove.Click += Button_Remove_Click;
            // 
            // Button_Add
            // 
            Button_Add.Location = new Point(402, 480);
            Button_Add.Name = "Button_Add";
            Button_Add.Size = new Size(150, 23);
            Button_Add.TabIndex = 16;
            Button_Add.Text = "Add to Character";
            Button_Add.UseVisualStyleBackColor = true;
            Button_Add.Click += Button_Add_Click;
            // 
            // Input_Text
            // 
            Input_Text.Location = new Point(12, 599);
            Input_Text.Multiline = true;
            Input_Text.Name = "Input_Text";
            Input_Text.Size = new Size(600, 200);
            Input_Text.TabIndex = 11;
            // 
            // Button_Close
            // 
            Button_Close.Location = new Point(713, 772);
            Button_Close.Name = "Button_Close";
            Button_Close.Size = new Size(75, 23);
            Button_Close.TabIndex = 19;
            Button_Close.Text = "Close";
            Button_Close.UseVisualStyleBackColor = true;
            Button_Close.Click += Button_Close_Click;
            // 
            // Button_Save
            // 
            Button_Save.Location = new Point(713, 743);
            Button_Save.Name = "Button_Save";
            Button_Save.Size = new Size(75, 23);
            Button_Save.TabIndex = 102;
            Button_Save.Text = "remove? Save";
            Button_Save.UseVisualStyleBackColor = true;
            // 
            // Label_Text
            // 
            Label_Text.AutoSize = true;
            Label_Text.Location = new Point(13, 581);
            Label_Text.Name = "Label_Text";
            Label_Text.Size = new Size(67, 15);
            Label_Text.TabIndex = 13;
            Label_Text.Text = "Description";
            // 
            // Label_Uses
            // 
            Label_Uses.AutoSize = true;
            Label_Uses.Location = new Point(173, 515);
            Label_Uses.Name = "Label_Uses";
            Label_Uses.Size = new Size(31, 15);
            Label_Uses.TabIndex = 14;
            Label_Uses.Text = "Uses";
            // 
            // Label_MaxUses
            // 
            Label_MaxUses.AutoSize = true;
            Label_MaxUses.Location = new Point(173, 544);
            Label_MaxUses.Name = "Label_MaxUses";
            Label_MaxUses.Size = new Size(57, 15);
            Label_MaxUses.TabIndex = 15;
            Label_MaxUses.Text = "Max Uses";
            // 
            // Label_UsesChange
            // 
            Label_UsesChange.AutoSize = true;
            Label_UsesChange.Location = new Point(173, 573);
            Label_UsesChange.Name = "Label_UsesChange";
            Label_UsesChange.Size = new Size(75, 15);
            Label_UsesChange.TabIndex = 16;
            Label_UsesChange.Text = "Uses Change";
            // 
            // Label_RechargeCondition
            // 
            Label_RechargeCondition.AutoSize = true;
            Label_RechargeCondition.Location = new Point(367, 512);
            Label_RechargeCondition.Name = "Label_RechargeCondition";
            Label_RechargeCondition.Size = new Size(112, 15);
            Label_RechargeCondition.TabIndex = 17;
            Label_RechargeCondition.Text = "Recharge Condition";
            // 
            // Label_RechargeAmount
            // 
            Label_RechargeAmount.AutoSize = true;
            Label_RechargeAmount.Location = new Point(367, 539);
            Label_RechargeAmount.Name = "Label_RechargeAmount";
            Label_RechargeAmount.Size = new Size(103, 15);
            Label_RechargeAmount.TabIndex = 18;
            Label_RechargeAmount.Text = "Recharge Amount";
            // 
            // Input_Uses
            // 
            Input_Uses.Location = new Point(251, 512);
            Input_Uses.Name = "Input_Uses";
            Input_Uses.Size = new Size(100, 23);
            Input_Uses.TabIndex = 3;
            // 
            // Input_MaxUses
            // 
            Input_MaxUses.Location = new Point(251, 541);
            Input_MaxUses.Name = "Input_MaxUses";
            Input_MaxUses.Size = new Size(100, 23);
            Input_MaxUses.TabIndex = 4;
            // 
            // Input_UsesChange
            // 
            Input_UsesChange.Location = new Point(251, 570);
            Input_UsesChange.Name = "Input_UsesChange";
            Input_UsesChange.Size = new Size(100, 23);
            Input_UsesChange.TabIndex = 5;
            // 
            // Input_RechargeCondition
            // 
            Input_RechargeCondition.Location = new Point(485, 507);
            Input_RechargeCondition.Name = "Input_RechargeCondition";
            Input_RechargeCondition.Size = new Size(100, 23);
            Input_RechargeCondition.TabIndex = 6;
            // 
            // Input_RechargeAmount
            // 
            Input_RechargeAmount.Location = new Point(485, 536);
            Input_RechargeAmount.Name = "Input_RechargeAmount";
            Input_RechargeAmount.Size = new Size(100, 23);
            Input_RechargeAmount.TabIndex = 7;
            // 
            // Label_Action
            // 
            Label_Action.AutoSize = true;
            Label_Action.Location = new Point(598, 507);
            Label_Action.Name = "Label_Action";
            Label_Action.Size = new Size(42, 15);
            Label_Action.TabIndex = 24;
            Label_Action.Text = "Action";
            // 
            // Label_Dice
            // 
            Label_Dice.AutoSize = true;
            Label_Dice.Location = new Point(598, 536);
            Label_Dice.Name = "Label_Dice";
            Label_Dice.Size = new Size(30, 15);
            Label_Dice.TabIndex = 25;
            Label_Dice.Text = "Dice";
            // 
            // Label_DisplayTier
            // 
            Label_DisplayTier.AutoSize = true;
            Label_DisplayTier.Location = new Point(598, 565);
            Label_DisplayTier.Name = "Label_DisplayTier";
            Label_DisplayTier.Size = new Size(64, 15);
            Label_DisplayTier.TabIndex = 26;
            Label_DisplayTier.Text = "DisplayTier";
            // 
            // Input_Action
            // 
            Input_Action.Location = new Point(688, 504);
            Input_Action.Name = "Input_Action";
            Input_Action.Size = new Size(100, 23);
            Input_Action.TabIndex = 8;
            // 
            // Input_Dice
            // 
            Input_Dice.Location = new Point(688, 533);
            Input_Dice.Name = "Input_Dice";
            Input_Dice.Size = new Size(100, 23);
            Input_Dice.TabIndex = 9;
            // 
            // Input_DisplayTier
            // 
            Input_DisplayTier.Location = new Point(688, 562);
            Input_DisplayTier.Name = "Input_DisplayTier";
            Input_DisplayTier.Size = new Size(100, 23);
            Input_DisplayTier.TabIndex = 10;
            // 
            // Button_Delete
            // 
            Button_Delete.Location = new Point(12, 480);
            Button_Delete.Name = "Button_Delete";
            Button_Delete.Size = new Size(150, 23);
            Button_Delete.TabIndex = 14;
            Button_Delete.Text = "Delete Ability";
            Button_Delete.UseVisualStyleBackColor = true;
            Button_Delete.Click += Button_Delete_Click;
            // 
            // Button_Test
            // 
            Button_Test.Location = new Point(713, 714);
            Button_Test.Name = "Button_Test";
            Button_Test.Size = new Size(75, 23);
            Button_Test.TabIndex = 101;
            Button_Test.Text = "Test";
            Button_Test.UseVisualStyleBackColor = true;
            Button_Test.Click += Button_Test_Click;
            // 
            // Button_NewAbility
            // 
            Button_NewAbility.Location = new Point(688, 591);
            Button_NewAbility.Name = "Button_NewAbility";
            Button_NewAbility.Size = new Size(100, 23);
            Button_NewAbility.TabIndex = 13;
            Button_NewAbility.Text = "New Ability";
            Button_NewAbility.UseVisualStyleBackColor = true;
            Button_NewAbility.Click += Button_NewAbility_Click;
            // 
            // Button_SaveAbility
            // 
            Button_SaveAbility.Location = new Point(688, 620);
            Button_SaveAbility.Name = "Button_SaveAbility";
            Button_SaveAbility.Size = new Size(100, 23);
            Button_SaveAbility.TabIndex = 12;
            Button_SaveAbility.Text = "Save Ability";
            Button_SaveAbility.UseVisualStyleBackColor = true;
            Button_SaveAbility.Click += Button_SaveAbility_Click;
            // 
            // EditAbilities
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 807);
            Controls.Add(Button_SaveAbility);
            Controls.Add(Button_NewAbility);
            Controls.Add(Button_Test);
            Controls.Add(Button_Delete);
            Controls.Add(Input_DisplayTier);
            Controls.Add(Input_Dice);
            Controls.Add(Input_Action);
            Controls.Add(Label_DisplayTier);
            Controls.Add(Label_Dice);
            Controls.Add(Label_Action);
            Controls.Add(Input_RechargeAmount);
            Controls.Add(Input_RechargeCondition);
            Controls.Add(Input_UsesChange);
            Controls.Add(Input_MaxUses);
            Controls.Add(Input_Uses);
            Controls.Add(Label_RechargeAmount);
            Controls.Add(Label_RechargeCondition);
            Controls.Add(Label_UsesChange);
            Controls.Add(Label_MaxUses);
            Controls.Add(Label_Uses);
            Controls.Add(Label_Text);
            Controls.Add(Button_Save);
            Controls.Add(Button_Close);
            Controls.Add(Input_Text);
            Controls.Add(Button_Add);
            Controls.Add(Button_Remove);
            Controls.Add(Label_ID);
            Controls.Add(Input_ID);
            Controls.Add(Input_Name);
            Controls.Add(Label_Name);
            Controls.Add(AssignedAbilitiesList);
            Controls.Add(Label_AssignedAbilities);
            Controls.Add(Label_ExistingAbilities);
            Controls.Add(AbilitiesLibraryList);
            Name = "EditAbilities";
            Text = "EditAbilities";
            FormClosing += EditAbilities_FormClosing;
            Load += EditAbilities_Load;
            ((System.ComponentModel.ISupportInitialize)AbilitiesLibraryList).EndInit();
            ((System.ComponentModel.ISupportInitialize)AssignedAbilitiesList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView AbilitiesLibraryList;
        private Label Label_ExistingAbilities;
        private Label Label_AssignedAbilities;
        private DataGridView AssignedAbilitiesList;
        private Label Label_Name;
        private TextBox Input_Name;
        private TextBox Input_ID;
        private Label Label_ID;
        private DataGridViewTextBoxColumn Library_IDCol;
        private DataGridViewTextBoxColumn Library_NameCol;
        private DataGridViewTextBoxColumn Assigned_IDCol;
        private DataGridViewTextBoxColumn Assigned_NameCol;
        private Button Button_Remove;
        private Button Button_Add;
        private TextBox Input_Text;
        private Button Button_Close;
        private Button Button_Save;
        private Label Label_Text;
        private Label Label_Uses;
        private Label Label_MaxUses;
        private Label Label_UsesChange;
        private Label Label_RechargeCondition;
        private Label Label_RechargeAmount;
        private TextBox Input_Uses;
        private TextBox Input_MaxUses;
        private TextBox Input_UsesChange;
        private TextBox Input_RechargeCondition;
        private TextBox Input_RechargeAmount;
        private Label Label_Action;
        private Label Label_Dice;
        private Label Label_DisplayTier;
        private TextBox Input_Action;
        private TextBox Input_Dice;
        private TextBox Input_DisplayTier;
        private Button Button_Delete;
        private Button Button_Test;
        private Button Button_NewAbility;
        private Button Button_SaveAbility;
    }
}