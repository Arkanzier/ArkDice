﻿namespace Simple_Dice_Roller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rollDice = new Button();
            outputTotal = new Label();
            diceStringBox = new TextBox();
            outputDescription = new Label();
            MainTabArea = new TabControl();
            DiceTab = new TabPage();
            button42 = new Button();
            button41 = new Button();
            button40 = new Button();
            button39 = new Button();
            button38 = new Button();
            button37 = new Button();
            button33 = new Button();
            button34 = new Button();
            button35 = new Button();
            button36 = new Button();
            button29 = new Button();
            button30 = new Button();
            button31 = new Button();
            button32 = new Button();
            button25 = new Button();
            button26 = new Button();
            button27 = new Button();
            button28 = new Button();
            button21 = new Button();
            button22 = new Button();
            button23 = new Button();
            button24 = new Button();
            button17 = new Button();
            button18 = new Button();
            button19 = new Button();
            button20 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            button16 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button3 = new Button();
            button4 = new Button();
            button1 = new Button();
            button2 = new Button();
            label4 = new Label();
            label3 = new Label();
            RemoveD100Button = new Button();
            RemoveD20Button = new Button();
            RemoveD12Button = new Button();
            RemoveD10Button = new Button();
            RemoveD8Button = new Button();
            RemoveD6Button = new Button();
            RemoveD4Button = new Button();
            RemoveD3Button = new Button();
            RemoveD2Button = new Button();
            LastRollDiceString = new Label();
            LastRollDiceLabel = new Label();
            RerollButton = new Button();
            AddD100Button = new Button();
            AddD20Button = new Button();
            AddD12Button = new Button();
            AddD10Button = new Button();
            AddD8Button = new Button();
            AddD6Button = new Button();
            AddD4Button = new Button();
            AddD3Button = new Button();
            AddD2Button = new Button();
            DiceArrayDisplay = new Label();
            LastRollLabel = new Label();
            CharacterTab = new TabPage();
            TestButton = new Button();
            Button_SaveCharacter = new Button();
            Char_Prof = new Label();
            CharLabel_Prof = new Label();
            Button_Persuasion = new Button();
            Button_Performance = new Button();
            Button_Intimidation = new Button();
            Button_Deception = new Button();
            Button_ChaSave = new Button();
            Button_Cha = new Button();
            Button_Survival = new Button();
            Button_Perception = new Button();
            Button_Medicine = new Button();
            Button_Insight = new Button();
            Button_AnimalHandling = new Button();
            Button_WisSave = new Button();
            Button_Wis = new Button();
            Button_Religion = new Button();
            Button_Nature = new Button();
            label6 = new Label();
            label5 = new Label();
            Button_Investigation = new Button();
            Button_History = new Button();
            Button_Arcana = new Button();
            Button_IntSave = new Button();
            Button_Int = new Button();
            Button_ConSave = new Button();
            Button_Con = new Button();
            Button_Stealth = new Button();
            Button_SleightOfHand = new Button();
            Button_Acrobatics = new Button();
            Button_DexSave = new Button();
            Button_Dex = new Button();
            label2 = new Label();
            Button_Athletics = new Button();
            Button_StrSave = new Button();
            Button_Str = new Button();
            label1 = new Label();
            BasicAbilitiesArea = new DataGridView();
            Basic_NumberCol = new DataGridViewTextBoxColumn();
            Basic_IDCol = new DataGridViewTextBoxColumn();
            Basic_NameCol = new DataGridViewTextBoxColumn();
            Basic_TextCol = new DataGridViewTextBoxColumn();
            Basic_UseButtonCol = new DataGridViewButtonColumn();
            Char_Cha = new Label();
            Char_Wis = new Label();
            Char_Int = new Label();
            Char_Con = new Label();
            Char_Dex = new Label();
            Char_Str = new Label();
            Char_Race = new Label();
            CharLabel_Race = new Label();
            Char_Name = new Label();
            CharLabel_Name = new Label();
            Char_ID = new Label();
            CharLabel_ID = new Label();
            AbilitiesTab = new TabPage();
            AbilitiesArea = new DataGridView();
            Abilities_NumberCol = new DataGridViewTextBoxColumn();
            Abilities_IDCol = new DataGridViewTextBoxColumn();
            Abilities_NameCol = new DataGridViewTextBoxColumn();
            Abilities_TextCol = new DataGridViewTextBoxColumn();
            Abilities_RechargeCol = new DataGridViewTextBoxColumn();
            Abilities_DiceCol = new DataGridViewTextBoxColumn();
            Abilities_UsesCol = new DataGridViewTextBoxColumn();
            Abilities_UseButtonCol = new DataGridViewButtonColumn();
            Abilities_Plus1Col = new DataGridViewButtonColumn();
            Abilities_Minus1Col = new DataGridViewButtonColumn();
            MagicTab = new TabPage();
            label7 = new Label();
            label8 = new Label();
            MainTabArea.SuspendLayout();
            DiceTab.SuspendLayout();
            CharacterTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BasicAbilitiesArea).BeginInit();
            AbilitiesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AbilitiesArea).BeginInit();
            MagicTab.SuspendLayout();
            SuspendLayout();
            // 
            // rollDice
            // 
            rollDice.Location = new Point(112, 6);
            rollDice.Name = "rollDice";
            rollDice.Size = new Size(75, 23);
            rollDice.TabIndex = 0;
            rollDice.Text = "Roll Dice";
            rollDice.UseVisualStyleBackColor = true;
            rollDice.Click += rollDice_Click;
            // 
            // outputTotal
            // 
            outputTotal.AutoSize = true;
            outputTotal.Location = new Point(234, 10);
            outputTotal.Name = "outputTotal";
            outputTotal.Size = new Size(86, 15);
            outputTotal.TabIndex = 1;
            outputTotal.Text = "Total goes here";
            // 
            // diceStringBox
            // 
            diceStringBox.Location = new Point(6, 6);
            diceStringBox.Name = "diceStringBox";
            diceStringBox.Size = new Size(100, 23);
            diceStringBox.TabIndex = 2;
            diceStringBox.KeyDown += diceStringBox_KeyDown;
            // 
            // outputDescription
            // 
            outputDescription.AutoSize = true;
            outputDescription.Location = new Point(16, 575);
            outputDescription.Name = "outputDescription";
            outputDescription.Size = new Size(47, 15);
            outputDescription.TabIndex = 3;
            outputDescription.Text = "Roll log";
            // 
            // MainTabArea
            // 
            MainTabArea.Controls.Add(DiceTab);
            MainTabArea.Controls.Add(CharacterTab);
            MainTabArea.Controls.Add(AbilitiesTab);
            MainTabArea.Controls.Add(MagicTab);
            MainTabArea.Location = new Point(12, 12);
            MainTabArea.Name = "MainTabArea";
            MainTabArea.SelectedIndex = 0;
            MainTabArea.Size = new Size(749, 560);
            MainTabArea.TabIndex = 4;
            // 
            // DiceTab
            // 
            DiceTab.Controls.Add(button42);
            DiceTab.Controls.Add(button41);
            DiceTab.Controls.Add(button40);
            DiceTab.Controls.Add(button39);
            DiceTab.Controls.Add(button38);
            DiceTab.Controls.Add(button37);
            DiceTab.Controls.Add(button33);
            DiceTab.Controls.Add(button34);
            DiceTab.Controls.Add(button35);
            DiceTab.Controls.Add(button36);
            DiceTab.Controls.Add(button29);
            DiceTab.Controls.Add(button30);
            DiceTab.Controls.Add(button31);
            DiceTab.Controls.Add(button32);
            DiceTab.Controls.Add(button25);
            DiceTab.Controls.Add(button26);
            DiceTab.Controls.Add(button27);
            DiceTab.Controls.Add(button28);
            DiceTab.Controls.Add(button21);
            DiceTab.Controls.Add(button22);
            DiceTab.Controls.Add(button23);
            DiceTab.Controls.Add(button24);
            DiceTab.Controls.Add(button17);
            DiceTab.Controls.Add(button18);
            DiceTab.Controls.Add(button19);
            DiceTab.Controls.Add(button20);
            DiceTab.Controls.Add(button13);
            DiceTab.Controls.Add(button14);
            DiceTab.Controls.Add(button15);
            DiceTab.Controls.Add(button16);
            DiceTab.Controls.Add(button9);
            DiceTab.Controls.Add(button10);
            DiceTab.Controls.Add(button11);
            DiceTab.Controls.Add(button12);
            DiceTab.Controls.Add(button5);
            DiceTab.Controls.Add(button6);
            DiceTab.Controls.Add(button7);
            DiceTab.Controls.Add(button8);
            DiceTab.Controls.Add(button3);
            DiceTab.Controls.Add(button4);
            DiceTab.Controls.Add(button1);
            DiceTab.Controls.Add(button2);
            DiceTab.Controls.Add(label4);
            DiceTab.Controls.Add(label3);
            DiceTab.Controls.Add(RemoveD100Button);
            DiceTab.Controls.Add(RemoveD20Button);
            DiceTab.Controls.Add(RemoveD12Button);
            DiceTab.Controls.Add(RemoveD10Button);
            DiceTab.Controls.Add(RemoveD8Button);
            DiceTab.Controls.Add(RemoveD6Button);
            DiceTab.Controls.Add(RemoveD4Button);
            DiceTab.Controls.Add(RemoveD3Button);
            DiceTab.Controls.Add(RemoveD2Button);
            DiceTab.Controls.Add(LastRollDiceString);
            DiceTab.Controls.Add(LastRollDiceLabel);
            DiceTab.Controls.Add(RerollButton);
            DiceTab.Controls.Add(AddD100Button);
            DiceTab.Controls.Add(AddD20Button);
            DiceTab.Controls.Add(AddD12Button);
            DiceTab.Controls.Add(AddD10Button);
            DiceTab.Controls.Add(AddD8Button);
            DiceTab.Controls.Add(AddD6Button);
            DiceTab.Controls.Add(AddD4Button);
            DiceTab.Controls.Add(AddD3Button);
            DiceTab.Controls.Add(AddD2Button);
            DiceTab.Controls.Add(DiceArrayDisplay);
            DiceTab.Controls.Add(LastRollLabel);
            DiceTab.Controls.Add(diceStringBox);
            DiceTab.Controls.Add(rollDice);
            DiceTab.Controls.Add(outputTotal);
            DiceTab.Location = new Point(4, 24);
            DiceTab.Name = "DiceTab";
            DiceTab.Padding = new Padding(3);
            DiceTab.Size = new Size(741, 532);
            DiceTab.TabIndex = 0;
            DiceTab.Text = "Dice";
            DiceTab.UseVisualStyleBackColor = true;
            // 
            // button42
            // 
            button42.Location = new Point(342, 464);
            button42.Name = "button42";
            button42.Size = new Size(40, 40);
            button42.TabIndex = 72;
            button42.Tag = "subtract 10";
            button42.Text = "-10";
            button42.UseVisualStyleBackColor = true;
            button42.Click += AddDice;
            // 
            // button41
            // 
            button41.Location = new Point(296, 464);
            button41.Name = "button41";
            button41.Size = new Size(40, 40);
            button41.TabIndex = 71;
            button41.Tag = "subtract 5";
            button41.Text = "-5";
            button41.UseVisualStyleBackColor = true;
            button41.Click += AddDice;
            // 
            // button40
            // 
            button40.Location = new Point(250, 464);
            button40.Name = "button40";
            button40.Size = new Size(40, 40);
            button40.TabIndex = 70;
            button40.Tag = "subtract 1";
            button40.Text = "-1";
            button40.UseVisualStyleBackColor = true;
            button40.Click += AddDice;
            // 
            // button39
            // 
            button39.Location = new Point(204, 464);
            button39.Name = "button39";
            button39.Size = new Size(40, 40);
            button39.TabIndex = 69;
            button39.Tag = "add 10";
            button39.Text = "+10";
            button39.UseVisualStyleBackColor = true;
            button39.Click += AddDice;
            // 
            // button38
            // 
            button38.Location = new Point(158, 464);
            button38.Name = "button38";
            button38.Size = new Size(40, 40);
            button38.TabIndex = 68;
            button38.Tag = "add 5";
            button38.Text = "+5";
            button38.UseVisualStyleBackColor = true;
            button38.Click += AddDice;
            // 
            // button37
            // 
            button37.Location = new Point(112, 464);
            button37.Name = "button37";
            button37.Size = new Size(40, 40);
            button37.TabIndex = 67;
            button37.Tag = "add 1";
            button37.Text = "+1";
            button37.UseVisualStyleBackColor = true;
            button37.Click += AddDice;
            // 
            // button33
            // 
            button33.Location = new Point(442, 418);
            button33.Name = "button33";
            button33.Size = new Size(60, 40);
            button33.TabIndex = 66;
            button33.Tag = "subtract dis d100";
            button33.Text = "Remove dis d100";
            button33.UseVisualStyleBackColor = true;
            button33.Click += AddDice;
            // 
            // button34
            // 
            button34.Location = new Point(376, 418);
            button34.Name = "button34";
            button34.Size = new Size(60, 40);
            button34.TabIndex = 65;
            button34.Tag = "add dis d100";
            button34.Text = "Add dis d100";
            button34.UseVisualStyleBackColor = true;
            button34.Click += AddDice;
            // 
            // button35
            // 
            button35.Location = new Point(310, 418);
            button35.Name = "button35";
            button35.Size = new Size(60, 40);
            button35.TabIndex = 64;
            button35.Tag = "subtract adv d100";
            button35.Text = "Remove adv d100";
            button35.UseVisualStyleBackColor = true;
            button35.Click += AddDice;
            // 
            // button36
            // 
            button36.Location = new Point(244, 418);
            button36.Name = "button36";
            button36.Size = new Size(60, 40);
            button36.TabIndex = 63;
            button36.Tag = "add adv d100";
            button36.Text = "Add adv d100";
            button36.UseVisualStyleBackColor = true;
            button36.Click += AddDice;
            // 
            // button29
            // 
            button29.Location = new Point(442, 372);
            button29.Name = "button29";
            button29.Size = new Size(60, 40);
            button29.TabIndex = 62;
            button29.Tag = "subtract dis d20";
            button29.Text = "Remove dis d20";
            button29.UseVisualStyleBackColor = true;
            button29.Click += AddDice;
            // 
            // button30
            // 
            button30.Location = new Point(376, 372);
            button30.Name = "button30";
            button30.Size = new Size(60, 40);
            button30.TabIndex = 61;
            button30.Tag = "add dis d20";
            button30.Text = "Add dis d20";
            button30.UseVisualStyleBackColor = true;
            button30.Click += AddDice;
            // 
            // button31
            // 
            button31.Location = new Point(310, 372);
            button31.Name = "button31";
            button31.Size = new Size(60, 40);
            button31.TabIndex = 60;
            button31.Tag = "subtract adv d20";
            button31.Text = "Remove adv d20";
            button31.UseVisualStyleBackColor = true;
            button31.Click += AddDice;
            // 
            // button32
            // 
            button32.Location = new Point(244, 372);
            button32.Name = "button32";
            button32.Size = new Size(60, 40);
            button32.TabIndex = 59;
            button32.Tag = "add adv d20";
            button32.Text = "Add adv d20";
            button32.UseVisualStyleBackColor = true;
            button32.Click += AddDice;
            // 
            // button25
            // 
            button25.Location = new Point(442, 326);
            button25.Name = "button25";
            button25.Size = new Size(60, 40);
            button25.TabIndex = 58;
            button25.Tag = "subtract dis d12";
            button25.Text = "Remove dis d12";
            button25.UseVisualStyleBackColor = true;
            button25.Click += AddDice;
            // 
            // button26
            // 
            button26.Location = new Point(376, 326);
            button26.Name = "button26";
            button26.Size = new Size(60, 40);
            button26.TabIndex = 57;
            button26.Tag = "add dis d12";
            button26.Text = "Add dis d12";
            button26.UseVisualStyleBackColor = true;
            button26.Click += AddDice;
            // 
            // button27
            // 
            button27.Location = new Point(310, 326);
            button27.Name = "button27";
            button27.Size = new Size(60, 40);
            button27.TabIndex = 56;
            button27.Tag = "subtract adv d12";
            button27.Text = "Remove adv d12";
            button27.UseVisualStyleBackColor = true;
            button27.Click += AddDice;
            // 
            // button28
            // 
            button28.Location = new Point(244, 326);
            button28.Name = "button28";
            button28.Size = new Size(60, 40);
            button28.TabIndex = 55;
            button28.Tag = "add adv d12";
            button28.Text = "Add adv d12";
            button28.UseVisualStyleBackColor = true;
            button28.Click += AddDice;
            // 
            // button21
            // 
            button21.Location = new Point(442, 280);
            button21.Name = "button21";
            button21.Size = new Size(60, 40);
            button21.TabIndex = 54;
            button21.Tag = "subtract dis d10";
            button21.Text = "Remove dis d10";
            button21.UseVisualStyleBackColor = true;
            button21.Click += AddDice;
            // 
            // button22
            // 
            button22.Location = new Point(376, 280);
            button22.Name = "button22";
            button22.Size = new Size(60, 40);
            button22.TabIndex = 53;
            button22.Tag = "add dis d10";
            button22.Text = "Add dis d10";
            button22.UseVisualStyleBackColor = true;
            button22.Click += AddDice;
            // 
            // button23
            // 
            button23.Location = new Point(310, 280);
            button23.Name = "button23";
            button23.Size = new Size(60, 40);
            button23.TabIndex = 52;
            button23.Tag = "subtract adv d10";
            button23.Text = "Remove adv d10";
            button23.UseVisualStyleBackColor = true;
            button23.Click += AddDice;
            // 
            // button24
            // 
            button24.Location = new Point(244, 280);
            button24.Name = "button24";
            button24.Size = new Size(60, 40);
            button24.TabIndex = 51;
            button24.Tag = "add adv d10";
            button24.Text = "Add adv d10";
            button24.UseVisualStyleBackColor = true;
            button24.Click += AddDice;
            // 
            // button17
            // 
            button17.Location = new Point(442, 234);
            button17.Name = "button17";
            button17.Size = new Size(60, 40);
            button17.TabIndex = 50;
            button17.Tag = "subtract dis d8";
            button17.Text = "Remove dis d8";
            button17.UseVisualStyleBackColor = true;
            button17.Click += AddDice;
            // 
            // button18
            // 
            button18.Location = new Point(376, 234);
            button18.Name = "button18";
            button18.Size = new Size(60, 40);
            button18.TabIndex = 49;
            button18.Tag = "add dis d8";
            button18.Text = "Add dis d8";
            button18.UseVisualStyleBackColor = true;
            button18.Click += AddDice;
            // 
            // button19
            // 
            button19.Location = new Point(310, 234);
            button19.Name = "button19";
            button19.Size = new Size(60, 40);
            button19.TabIndex = 48;
            button19.Tag = "subtract adv d8";
            button19.Text = "Remove adv d8";
            button19.UseVisualStyleBackColor = true;
            button19.Click += AddDice;
            // 
            // button20
            // 
            button20.Location = new Point(244, 234);
            button20.Name = "button20";
            button20.Size = new Size(60, 40);
            button20.TabIndex = 47;
            button20.Tag = "add adv d8";
            button20.Text = "Add adv d8";
            button20.UseVisualStyleBackColor = true;
            button20.Click += AddDice;
            // 
            // button13
            // 
            button13.Location = new Point(442, 188);
            button13.Name = "button13";
            button13.Size = new Size(60, 40);
            button13.TabIndex = 46;
            button13.Tag = "subtract dis d6";
            button13.Text = "Remove dis d6";
            button13.UseVisualStyleBackColor = true;
            button13.Click += AddDice;
            // 
            // button14
            // 
            button14.Location = new Point(376, 188);
            button14.Name = "button14";
            button14.Size = new Size(60, 40);
            button14.TabIndex = 45;
            button14.Tag = "add dis d6";
            button14.Text = "Add dis d6";
            button14.UseVisualStyleBackColor = true;
            button14.Click += AddDice;
            // 
            // button15
            // 
            button15.Location = new Point(310, 188);
            button15.Name = "button15";
            button15.Size = new Size(60, 40);
            button15.TabIndex = 44;
            button15.Tag = "subtract adv d6";
            button15.Text = "Remove adv d6";
            button15.UseVisualStyleBackColor = true;
            button15.Click += AddDice;
            // 
            // button16
            // 
            button16.Location = new Point(244, 188);
            button16.Name = "button16";
            button16.Size = new Size(60, 40);
            button16.TabIndex = 43;
            button16.Tag = "add adv d6";
            button16.Text = "Add adv d6";
            button16.UseVisualStyleBackColor = true;
            button16.Click += AddDice;
            // 
            // button9
            // 
            button9.Location = new Point(442, 142);
            button9.Name = "button9";
            button9.Size = new Size(60, 40);
            button9.TabIndex = 42;
            button9.Tag = "subtract dis d4";
            button9.Text = "Remove dis d4";
            button9.UseVisualStyleBackColor = true;
            button9.Click += AddDice;
            // 
            // button10
            // 
            button10.Location = new Point(376, 142);
            button10.Name = "button10";
            button10.Size = new Size(60, 40);
            button10.TabIndex = 41;
            button10.Tag = "add dis d4";
            button10.Text = "Add dis d4";
            button10.UseVisualStyleBackColor = true;
            button10.Click += AddDice;
            // 
            // button11
            // 
            button11.Location = new Point(310, 142);
            button11.Name = "button11";
            button11.Size = new Size(60, 40);
            button11.TabIndex = 40;
            button11.Tag = "subtract adv d4";
            button11.Text = "Remove adv d4";
            button11.UseVisualStyleBackColor = true;
            button11.Click += AddDice;
            // 
            // button12
            // 
            button12.Location = new Point(244, 142);
            button12.Name = "button12";
            button12.Size = new Size(60, 40);
            button12.TabIndex = 39;
            button12.Tag = "add adv d4";
            button12.Text = "Add adv d4";
            button12.UseVisualStyleBackColor = true;
            button12.Click += AddDice;
            // 
            // button5
            // 
            button5.Location = new Point(442, 96);
            button5.Name = "button5";
            button5.Size = new Size(60, 40);
            button5.TabIndex = 38;
            button5.Tag = "subtract dis d3";
            button5.Text = "Remove dis d3";
            button5.UseVisualStyleBackColor = true;
            button5.Click += AddDice;
            // 
            // button6
            // 
            button6.Location = new Point(376, 96);
            button6.Name = "button6";
            button6.Size = new Size(60, 40);
            button6.TabIndex = 37;
            button6.Tag = "add dis d3";
            button6.Text = "Add dis d3";
            button6.UseVisualStyleBackColor = true;
            button6.Click += AddDice;
            // 
            // button7
            // 
            button7.Location = new Point(310, 96);
            button7.Name = "button7";
            button7.Size = new Size(60, 40);
            button7.TabIndex = 36;
            button7.Tag = "subtract adv d3";
            button7.Text = "Remove adv d3";
            button7.UseVisualStyleBackColor = true;
            button7.Click += AddDice;
            // 
            // button8
            // 
            button8.Location = new Point(244, 96);
            button8.Name = "button8";
            button8.Size = new Size(60, 40);
            button8.TabIndex = 35;
            button8.Tag = "add adv d3";
            button8.Text = "Add adv d3";
            button8.UseVisualStyleBackColor = true;
            button8.Click += AddDice;
            // 
            // button3
            // 
            button3.Location = new Point(442, 50);
            button3.Name = "button3";
            button3.Size = new Size(60, 40);
            button3.TabIndex = 34;
            button3.Tag = "subtract dis d2";
            button3.Text = "Remove dis d2";
            button3.UseVisualStyleBackColor = true;
            button3.Click += AddDice;
            // 
            // button4
            // 
            button4.Location = new Point(376, 50);
            button4.Name = "button4";
            button4.Size = new Size(60, 40);
            button4.TabIndex = 33;
            button4.Tag = "add dis d2";
            button4.Text = "Add dis d2";
            button4.UseVisualStyleBackColor = true;
            button4.Click += AddDice;
            // 
            // button1
            // 
            button1.Location = new Point(310, 50);
            button1.Name = "button1";
            button1.Size = new Size(60, 40);
            button1.TabIndex = 32;
            button1.Tag = "subtract adv d2";
            button1.Text = "Remove adv d2";
            button1.UseVisualStyleBackColor = true;
            button1.Click += AddDice;
            // 
            // button2
            // 
            button2.Location = new Point(244, 50);
            button2.Name = "button2";
            button2.Size = new Size(60, 40);
            button2.TabIndex = 31;
            button2.Tag = "add adv d2";
            button2.Text = "Add adv d2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += AddDice;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 109);
            label4.Name = "label4";
            label4.Size = new Size(20, 15);
            label4.TabIndex = 30;
            label4.Text = "d3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 63);
            label3.Name = "label3";
            label3.Size = new Size(20, 15);
            label3.TabIndex = 29;
            label3.Text = "d2";
            // 
            // RemoveD100Button
            // 
            RemoveD100Button.Location = new Point(178, 418);
            RemoveD100Button.Name = "RemoveD100Button";
            RemoveD100Button.Size = new Size(60, 40);
            RemoveD100Button.TabIndex = 27;
            RemoveD100Button.Tag = "subtract d100";
            RemoveD100Button.Text = "Remove d100";
            RemoveD100Button.UseVisualStyleBackColor = true;
            RemoveD100Button.Click += AddDice;
            // 
            // RemoveD20Button
            // 
            RemoveD20Button.Location = new Point(178, 372);
            RemoveD20Button.Name = "RemoveD20Button";
            RemoveD20Button.Size = new Size(60, 40);
            RemoveD20Button.TabIndex = 26;
            RemoveD20Button.Tag = "subtract d20";
            RemoveD20Button.Text = "Remove d20";
            RemoveD20Button.UseVisualStyleBackColor = true;
            RemoveD20Button.Click += AddDice;
            // 
            // RemoveD12Button
            // 
            RemoveD12Button.Location = new Point(178, 326);
            RemoveD12Button.Name = "RemoveD12Button";
            RemoveD12Button.Size = new Size(60, 40);
            RemoveD12Button.TabIndex = 25;
            RemoveD12Button.Tag = "subtract d12";
            RemoveD12Button.Text = "Remove d12";
            RemoveD12Button.UseVisualStyleBackColor = true;
            RemoveD12Button.Click += AddDice;
            // 
            // RemoveD10Button
            // 
            RemoveD10Button.Location = new Point(178, 280);
            RemoveD10Button.Name = "RemoveD10Button";
            RemoveD10Button.Size = new Size(60, 40);
            RemoveD10Button.TabIndex = 24;
            RemoveD10Button.Tag = "subtract d10";
            RemoveD10Button.Text = "Remove d10";
            RemoveD10Button.UseVisualStyleBackColor = true;
            RemoveD10Button.Click += AddDice;
            // 
            // RemoveD8Button
            // 
            RemoveD8Button.Location = new Point(178, 234);
            RemoveD8Button.Name = "RemoveD8Button";
            RemoveD8Button.Size = new Size(60, 40);
            RemoveD8Button.TabIndex = 23;
            RemoveD8Button.Tag = "subtract d8";
            RemoveD8Button.Text = "Remove d8";
            RemoveD8Button.UseVisualStyleBackColor = true;
            RemoveD8Button.Click += AddDice;
            // 
            // RemoveD6Button
            // 
            RemoveD6Button.Location = new Point(178, 188);
            RemoveD6Button.Name = "RemoveD6Button";
            RemoveD6Button.Size = new Size(60, 40);
            RemoveD6Button.TabIndex = 22;
            RemoveD6Button.Tag = "subtract d6";
            RemoveD6Button.Text = "Remove d6";
            RemoveD6Button.UseVisualStyleBackColor = true;
            RemoveD6Button.Click += AddDice;
            // 
            // RemoveD4Button
            // 
            RemoveD4Button.Location = new Point(178, 142);
            RemoveD4Button.Name = "RemoveD4Button";
            RemoveD4Button.Size = new Size(60, 40);
            RemoveD4Button.TabIndex = 21;
            RemoveD4Button.Tag = "subtract d4";
            RemoveD4Button.Text = "Remove d4";
            RemoveD4Button.UseVisualStyleBackColor = true;
            RemoveD4Button.Click += AddDice;
            // 
            // RemoveD3Button
            // 
            RemoveD3Button.Location = new Point(178, 96);
            RemoveD3Button.Name = "RemoveD3Button";
            RemoveD3Button.Size = new Size(60, 40);
            RemoveD3Button.TabIndex = 20;
            RemoveD3Button.Tag = "subtract d3";
            RemoveD3Button.Text = "Remove d3";
            RemoveD3Button.UseVisualStyleBackColor = true;
            RemoveD3Button.Click += AddDice;
            // 
            // RemoveD2Button
            // 
            RemoveD2Button.Location = new Point(178, 50);
            RemoveD2Button.Name = "RemoveD2Button";
            RemoveD2Button.Size = new Size(60, 40);
            RemoveD2Button.TabIndex = 19;
            RemoveD2Button.Tag = "subtract d2";
            RemoveD2Button.Text = "Remove d2";
            RemoveD2Button.UseVisualStyleBackColor = true;
            RemoveD2Button.Click += AddDice;
            // 
            // LastRollDiceString
            // 
            LastRollDiceString.AutoSize = true;
            LastRollDiceString.Location = new Point(486, 10);
            LastRollDiceString.Name = "LastRollDiceString";
            LastRollDiceString.Size = new Size(102, 15);
            LastRollDiceString.TabIndex = 17;
            LastRollDiceString.Text = "Last roll goes here";
            // 
            // LastRollDiceLabel
            // 
            LastRollDiceLabel.AutoSize = true;
            LastRollDiceLabel.Location = new Point(426, 9);
            LastRollDiceLabel.Name = "LastRollDiceLabel";
            LastRollDiceLabel.Size = new Size(54, 15);
            LastRollDiceLabel.TabIndex = 16;
            LastRollDiceLabel.Text = "Last Roll:";
            // 
            // RerollButton
            // 
            RerollButton.Location = new Point(345, 5);
            RerollButton.Name = "RerollButton";
            RerollButton.Size = new Size(75, 23);
            RerollButton.TabIndex = 15;
            RerollButton.Text = "Reroll";
            RerollButton.UseVisualStyleBackColor = true;
            RerollButton.Click += RerollButton_Click;
            // 
            // AddD100Button
            // 
            AddD100Button.Location = new Point(112, 418);
            AddD100Button.Name = "AddD100Button";
            AddD100Button.Size = new Size(60, 40);
            AddD100Button.TabIndex = 13;
            AddD100Button.Tag = "add d100";
            AddD100Button.Text = "Add d100";
            AddD100Button.UseVisualStyleBackColor = true;
            AddD100Button.Click += AddDice;
            // 
            // AddD20Button
            // 
            AddD20Button.Location = new Point(112, 372);
            AddD20Button.Name = "AddD20Button";
            AddD20Button.Size = new Size(60, 40);
            AddD20Button.TabIndex = 12;
            AddD20Button.Tag = "add d20";
            AddD20Button.Text = "Add d20";
            AddD20Button.UseVisualStyleBackColor = true;
            AddD20Button.Click += AddDice;
            // 
            // AddD12Button
            // 
            AddD12Button.Location = new Point(112, 326);
            AddD12Button.Name = "AddD12Button";
            AddD12Button.Size = new Size(60, 40);
            AddD12Button.TabIndex = 11;
            AddD12Button.Tag = "add d12";
            AddD12Button.Text = "Add d12";
            AddD12Button.UseVisualStyleBackColor = true;
            AddD12Button.Click += AddDice;
            // 
            // AddD10Button
            // 
            AddD10Button.Location = new Point(112, 280);
            AddD10Button.Name = "AddD10Button";
            AddD10Button.Size = new Size(60, 40);
            AddD10Button.TabIndex = 10;
            AddD10Button.Tag = "add d10";
            AddD10Button.Text = "Add d10";
            AddD10Button.UseVisualStyleBackColor = true;
            AddD10Button.Click += AddDice;
            // 
            // AddD8Button
            // 
            AddD8Button.Location = new Point(112, 234);
            AddD8Button.Name = "AddD8Button";
            AddD8Button.Size = new Size(60, 40);
            AddD8Button.TabIndex = 9;
            AddD8Button.Tag = "add d8";
            AddD8Button.Text = "Add d8";
            AddD8Button.UseVisualStyleBackColor = true;
            AddD8Button.Click += AddDice;
            // 
            // AddD6Button
            // 
            AddD6Button.Location = new Point(112, 188);
            AddD6Button.Name = "AddD6Button";
            AddD6Button.Size = new Size(60, 40);
            AddD6Button.TabIndex = 8;
            AddD6Button.Tag = "add d6";
            AddD6Button.Text = "Add d6";
            AddD6Button.UseVisualStyleBackColor = true;
            AddD6Button.Click += AddDice;
            // 
            // AddD4Button
            // 
            AddD4Button.Location = new Point(112, 142);
            AddD4Button.Name = "AddD4Button";
            AddD4Button.Size = new Size(60, 40);
            AddD4Button.TabIndex = 7;
            AddD4Button.Tag = "add d4";
            AddD4Button.Text = "Add d4";
            AddD4Button.UseVisualStyleBackColor = true;
            AddD4Button.Click += AddDice;
            // 
            // AddD3Button
            // 
            AddD3Button.Location = new Point(112, 96);
            AddD3Button.Name = "AddD3Button";
            AddD3Button.Size = new Size(60, 40);
            AddD3Button.TabIndex = 6;
            AddD3Button.Tag = "add d3";
            AddD3Button.Text = "Add d3";
            AddD3Button.UseVisualStyleBackColor = true;
            AddD3Button.Click += AddDice;
            // 
            // AddD2Button
            // 
            AddD2Button.Location = new Point(112, 50);
            AddD2Button.Name = "AddD2Button";
            AddD2Button.Size = new Size(60, 40);
            AddD2Button.TabIndex = 5;
            AddD2Button.Tag = "add d2";
            AddD2Button.Text = "Add d2";
            AddD2Button.UseVisualStyleBackColor = true;
            AddD2Button.Click += AddDice;
            // 
            // DiceArrayDisplay
            // 
            DiceArrayDisplay.AutoSize = true;
            DiceArrayDisplay.Location = new Point(6, 32);
            DiceArrayDisplay.Name = "DiceArrayDisplay";
            DiceArrayDisplay.Size = new Size(72, 15);
            DiceArrayDisplay.TabIndex = 4;
            DiceArrayDisplay.Text = "Current dice";
            // 
            // LastRollLabel
            // 
            LastRollLabel.AutoSize = true;
            LastRollLabel.Location = new Point(193, 10);
            LastRollLabel.Name = "LastRollLabel";
            LastRollLabel.Size = new Size(35, 15);
            LastRollLabel.TabIndex = 3;
            LastRollLabel.Text = "Total:";
            // 
            // CharacterTab
            // 
            CharacterTab.Controls.Add(TestButton);
            CharacterTab.Controls.Add(Button_SaveCharacter);
            CharacterTab.Controls.Add(Char_Prof);
            CharacterTab.Controls.Add(CharLabel_Prof);
            CharacterTab.Controls.Add(Button_Persuasion);
            CharacterTab.Controls.Add(Button_Performance);
            CharacterTab.Controls.Add(Button_Intimidation);
            CharacterTab.Controls.Add(Button_Deception);
            CharacterTab.Controls.Add(Button_ChaSave);
            CharacterTab.Controls.Add(Button_Cha);
            CharacterTab.Controls.Add(Button_Survival);
            CharacterTab.Controls.Add(Button_Perception);
            CharacterTab.Controls.Add(Button_Medicine);
            CharacterTab.Controls.Add(Button_Insight);
            CharacterTab.Controls.Add(Button_AnimalHandling);
            CharacterTab.Controls.Add(Button_WisSave);
            CharacterTab.Controls.Add(Button_Wis);
            CharacterTab.Controls.Add(Button_Religion);
            CharacterTab.Controls.Add(Button_Nature);
            CharacterTab.Controls.Add(label6);
            CharacterTab.Controls.Add(label5);
            CharacterTab.Controls.Add(Button_Investigation);
            CharacterTab.Controls.Add(Button_History);
            CharacterTab.Controls.Add(Button_Arcana);
            CharacterTab.Controls.Add(Button_IntSave);
            CharacterTab.Controls.Add(Button_Int);
            CharacterTab.Controls.Add(Button_ConSave);
            CharacterTab.Controls.Add(Button_Con);
            CharacterTab.Controls.Add(Button_Stealth);
            CharacterTab.Controls.Add(Button_SleightOfHand);
            CharacterTab.Controls.Add(Button_Acrobatics);
            CharacterTab.Controls.Add(Button_DexSave);
            CharacterTab.Controls.Add(Button_Dex);
            CharacterTab.Controls.Add(label2);
            CharacterTab.Controls.Add(Button_Athletics);
            CharacterTab.Controls.Add(Button_StrSave);
            CharacterTab.Controls.Add(Button_Str);
            CharacterTab.Controls.Add(label1);
            CharacterTab.Controls.Add(BasicAbilitiesArea);
            CharacterTab.Controls.Add(Char_Cha);
            CharacterTab.Controls.Add(Char_Wis);
            CharacterTab.Controls.Add(Char_Int);
            CharacterTab.Controls.Add(Char_Con);
            CharacterTab.Controls.Add(Char_Dex);
            CharacterTab.Controls.Add(Char_Str);
            CharacterTab.Controls.Add(Char_Race);
            CharacterTab.Controls.Add(CharLabel_Race);
            CharacterTab.Controls.Add(Char_Name);
            CharacterTab.Controls.Add(CharLabel_Name);
            CharacterTab.Controls.Add(Char_ID);
            CharacterTab.Controls.Add(CharLabel_ID);
            CharacterTab.Location = new Point(4, 24);
            CharacterTab.Name = "CharacterTab";
            CharacterTab.Padding = new Padding(3);
            CharacterTab.Size = new Size(741, 532);
            CharacterTab.TabIndex = 1;
            CharacterTab.Text = "Character";
            CharacterTab.UseVisualStyleBackColor = true;
            // 
            // TestButton
            // 
            TestButton.Location = new Point(554, 158);
            TestButton.Name = "TestButton";
            TestButton.Size = new Size(75, 23);
            TestButton.TabIndex = 58;
            TestButton.Text = "Test";
            TestButton.UseVisualStyleBackColor = true;
            TestButton.Click += TestButton_Click;
            // 
            // Button_SaveCharacter
            // 
            Button_SaveCharacter.Location = new Point(594, 229);
            Button_SaveCharacter.Name = "Button_SaveCharacter";
            Button_SaveCharacter.Size = new Size(75, 23);
            Button_SaveCharacter.TabIndex = 57;
            Button_SaveCharacter.Text = "Save";
            Button_SaveCharacter.UseVisualStyleBackColor = true;
            Button_SaveCharacter.Click += Button_SaveCharacter_Click;
            // 
            // Char_Prof
            // 
            Char_Prof.AutoSize = true;
            Char_Prof.Location = new Point(553, 54);
            Char_Prof.Name = "Char_Prof";
            Char_Prof.Size = new Size(29, 15);
            Char_Prof.TabIndex = 56;
            Char_Prof.Text = "prof";
            // 
            // CharLabel_Prof
            // 
            CharLabel_Prof.AutoSize = true;
            CharLabel_Prof.Location = new Point(530, 39);
            CharLabel_Prof.Name = "CharLabel_Prof";
            CharLabel_Prof.Size = new Size(102, 15);
            CharLabel_Prof.TabIndex = 55;
            CharLabel_Prof.Text = "Proficiency Bonus";
            // 
            // Button_Persuasion
            // 
            Button_Persuasion.Location = new Point(411, 227);
            Button_Persuasion.Name = "Button_Persuasion";
            Button_Persuasion.Size = new Size(75, 23);
            Button_Persuasion.TabIndex = 54;
            Button_Persuasion.Tag = "roll d20+chamod+profifprof(persuasion)";
            Button_Persuasion.Text = "Persuasion";
            Button_Persuasion.UseVisualStyleBackColor = true;
            Button_Persuasion.Click += RollDice;
            // 
            // Button_Performance
            // 
            Button_Performance.Location = new Point(411, 198);
            Button_Performance.Name = "Button_Performance";
            Button_Performance.Size = new Size(75, 23);
            Button_Performance.TabIndex = 53;
            Button_Performance.Tag = "roll d20+chamod+profifprof(performance)";
            Button_Performance.Text = "Performance";
            Button_Performance.UseVisualStyleBackColor = true;
            Button_Performance.Click += RollDice;
            // 
            // Button_Intimidation
            // 
            Button_Intimidation.Location = new Point(411, 169);
            Button_Intimidation.Name = "Button_Intimidation";
            Button_Intimidation.Size = new Size(75, 23);
            Button_Intimidation.TabIndex = 52;
            Button_Intimidation.Tag = "roll d20+chamod+profifprof(intimidation)";
            Button_Intimidation.Text = "Intimidation";
            Button_Intimidation.UseVisualStyleBackColor = true;
            Button_Intimidation.Click += RollDice;
            // 
            // Button_Deception
            // 
            Button_Deception.Location = new Point(411, 140);
            Button_Deception.Name = "Button_Deception";
            Button_Deception.Size = new Size(75, 23);
            Button_Deception.TabIndex = 51;
            Button_Deception.Tag = "roll d20+chamod+profifprof(deception)";
            Button_Deception.Text = "Deception";
            Button_Deception.UseVisualStyleBackColor = true;
            Button_Deception.Click += RollDice;
            // 
            // Button_ChaSave
            // 
            Button_ChaSave.Location = new Point(411, 111);
            Button_ChaSave.Name = "Button_ChaSave";
            Button_ChaSave.Size = new Size(75, 23);
            Button_ChaSave.TabIndex = 50;
            Button_ChaSave.Tag = "roll d20+chamod+profifprof(chasave)";
            Button_ChaSave.Text = "Cha Save";
            Button_ChaSave.UseVisualStyleBackColor = true;
            Button_ChaSave.Click += RollDice;
            // 
            // Button_Cha
            // 
            Button_Cha.Location = new Point(411, 82);
            Button_Cha.Name = "Button_Cha";
            Button_Cha.Size = new Size(75, 23);
            Button_Cha.TabIndex = 49;
            Button_Cha.Tag = "roll d20+chamod";
            Button_Cha.Text = "Charisma";
            Button_Cha.UseVisualStyleBackColor = true;
            Button_Cha.Click += RollDice;
            // 
            // Button_Survival
            // 
            Button_Survival.Location = new Point(330, 256);
            Button_Survival.Name = "Button_Survival";
            Button_Survival.Size = new Size(75, 23);
            Button_Survival.TabIndex = 48;
            Button_Survival.Tag = "roll d20+wismod+profifprof(survival)";
            Button_Survival.Text = "Survival";
            Button_Survival.UseVisualStyleBackColor = true;
            Button_Survival.Click += RollDice;
            // 
            // Button_Perception
            // 
            Button_Perception.Location = new Point(330, 227);
            Button_Perception.Name = "Button_Perception";
            Button_Perception.Size = new Size(75, 23);
            Button_Perception.TabIndex = 47;
            Button_Perception.Tag = "roll d20+wismod+profifprof(perception)";
            Button_Perception.Text = "Perception";
            Button_Perception.UseVisualStyleBackColor = true;
            Button_Perception.Click += RollDice;
            // 
            // Button_Medicine
            // 
            Button_Medicine.Location = new Point(330, 198);
            Button_Medicine.Name = "Button_Medicine";
            Button_Medicine.Size = new Size(75, 23);
            Button_Medicine.TabIndex = 46;
            Button_Medicine.Tag = "roll d20+wismod+profifprof(medicine)";
            Button_Medicine.Text = "Medicine";
            Button_Medicine.UseVisualStyleBackColor = true;
            Button_Medicine.Click += RollDice;
            // 
            // Button_Insight
            // 
            Button_Insight.Location = new Point(330, 169);
            Button_Insight.Name = "Button_Insight";
            Button_Insight.Size = new Size(75, 23);
            Button_Insight.TabIndex = 45;
            Button_Insight.Tag = "roll d20+wismod+profifprof(insight)";
            Button_Insight.Text = "Insight";
            Button_Insight.UseVisualStyleBackColor = true;
            Button_Insight.Click += RollDice;
            // 
            // Button_AnimalHandling
            // 
            Button_AnimalHandling.Location = new Point(330, 140);
            Button_AnimalHandling.Name = "Button_AnimalHandling";
            Button_AnimalHandling.Size = new Size(75, 23);
            Button_AnimalHandling.TabIndex = 44;
            Button_AnimalHandling.Tag = "roll d20+wismod+profifprof(animalhandling)";
            Button_AnimalHandling.Text = "Animal Handling";
            Button_AnimalHandling.UseVisualStyleBackColor = true;
            Button_AnimalHandling.Click += RollDice;
            // 
            // Button_WisSave
            // 
            Button_WisSave.Location = new Point(330, 111);
            Button_WisSave.Name = "Button_WisSave";
            Button_WisSave.Size = new Size(75, 23);
            Button_WisSave.TabIndex = 43;
            Button_WisSave.Tag = "roll d20+wismod+profifprof(wissave)";
            Button_WisSave.Text = "Wis Save";
            Button_WisSave.UseVisualStyleBackColor = true;
            Button_WisSave.Click += RollDice;
            // 
            // Button_Wis
            // 
            Button_Wis.Location = new Point(330, 82);
            Button_Wis.Name = "Button_Wis";
            Button_Wis.Size = new Size(75, 23);
            Button_Wis.TabIndex = 42;
            Button_Wis.Tag = "roll d20+wismod";
            Button_Wis.Text = "Wisdom";
            Button_Wis.UseVisualStyleBackColor = true;
            Button_Wis.Click += RollDice;
            // 
            // Button_Religion
            // 
            Button_Religion.Location = new Point(249, 256);
            Button_Religion.Name = "Button_Religion";
            Button_Religion.Size = new Size(75, 23);
            Button_Religion.TabIndex = 41;
            Button_Religion.Tag = "roll d20+intmod+profifprof(religion)";
            Button_Religion.Text = "Religion";
            Button_Religion.UseVisualStyleBackColor = true;
            Button_Religion.Click += RollDice;
            // 
            // Button_Nature
            // 
            Button_Nature.Location = new Point(249, 227);
            Button_Nature.Name = "Button_Nature";
            Button_Nature.Size = new Size(75, 23);
            Button_Nature.TabIndex = 40;
            Button_Nature.Tag = "roll d20+intmod+profifprof(nature)";
            Button_Nature.Text = "Nature";
            Button_Nature.UseVisualStyleBackColor = true;
            Button_Nature.Click += RollDice;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(281, 338);
            label6.Name = "label6";
            label6.Size = new Size(250, 15);
            label6.TabIndex = 39;
            label6.Text = "Add buttons for casting related ability checks?";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(281, 353);
            label5.Name = "label5";
            label5.Size = new Size(233, 15);
            label5.TabIndex = 38;
            label5.Text = "Need modifiers for bless/bane, adv/dis, etc";
            // 
            // Button_Investigation
            // 
            Button_Investigation.Location = new Point(249, 198);
            Button_Investigation.Name = "Button_Investigation";
            Button_Investigation.Size = new Size(75, 23);
            Button_Investigation.TabIndex = 37;
            Button_Investigation.Tag = "roll d20+intmod+profifprof(investigation)";
            Button_Investigation.Text = "Investigation";
            Button_Investigation.UseVisualStyleBackColor = true;
            Button_Investigation.Click += RollDice;
            // 
            // Button_History
            // 
            Button_History.Location = new Point(249, 169);
            Button_History.Name = "Button_History";
            Button_History.Size = new Size(75, 23);
            Button_History.TabIndex = 36;
            Button_History.Tag = "roll d20+intmod+profifprof(history)";
            Button_History.Text = "History";
            Button_History.UseVisualStyleBackColor = true;
            Button_History.Click += RollDice;
            // 
            // Button_Arcana
            // 
            Button_Arcana.Location = new Point(249, 140);
            Button_Arcana.Name = "Button_Arcana";
            Button_Arcana.Size = new Size(75, 23);
            Button_Arcana.TabIndex = 35;
            Button_Arcana.Tag = "roll d20+intmod+profifprof(arcana)";
            Button_Arcana.Text = "Arcana";
            Button_Arcana.UseVisualStyleBackColor = true;
            Button_Arcana.Click += RollDice;
            // 
            // Button_IntSave
            // 
            Button_IntSave.Location = new Point(249, 111);
            Button_IntSave.Name = "Button_IntSave";
            Button_IntSave.Size = new Size(75, 23);
            Button_IntSave.TabIndex = 34;
            Button_IntSave.Tag = "roll d20+intmod+profifprof(intsave)";
            Button_IntSave.Text = "Int Save";
            Button_IntSave.UseVisualStyleBackColor = true;
            Button_IntSave.Click += RollDice;
            // 
            // Button_Int
            // 
            Button_Int.Location = new Point(249, 82);
            Button_Int.Name = "Button_Int";
            Button_Int.Size = new Size(75, 23);
            Button_Int.TabIndex = 33;
            Button_Int.Tag = "roll d20+intmod";
            Button_Int.Text = "Intelligence";
            Button_Int.UseVisualStyleBackColor = true;
            Button_Int.Click += RollDice;
            // 
            // Button_ConSave
            // 
            Button_ConSave.Location = new Point(168, 111);
            Button_ConSave.Name = "Button_ConSave";
            Button_ConSave.Size = new Size(75, 23);
            Button_ConSave.TabIndex = 32;
            Button_ConSave.Tag = "roll d20+conmod+profifprof(consave)";
            Button_ConSave.Text = "Con Save";
            Button_ConSave.UseVisualStyleBackColor = true;
            Button_ConSave.Click += RollDice;
            // 
            // Button_Con
            // 
            Button_Con.Location = new Point(168, 82);
            Button_Con.Name = "Button_Con";
            Button_Con.Size = new Size(75, 23);
            Button_Con.TabIndex = 31;
            Button_Con.Tag = "roll d20+conmod";
            Button_Con.Text = "Constitution";
            Button_Con.UseVisualStyleBackColor = true;
            Button_Con.Click += RollDice;
            // 
            // Button_Stealth
            // 
            Button_Stealth.Location = new Point(87, 198);
            Button_Stealth.Name = "Button_Stealth";
            Button_Stealth.Size = new Size(75, 23);
            Button_Stealth.TabIndex = 30;
            Button_Stealth.Tag = "roll d20+dexmod+profifprof(stealth)";
            Button_Stealth.Text = "Stealth";
            Button_Stealth.UseVisualStyleBackColor = true;
            Button_Stealth.Click += RollDice;
            // 
            // Button_SleightOfHand
            // 
            Button_SleightOfHand.Location = new Point(87, 169);
            Button_SleightOfHand.Name = "Button_SleightOfHand";
            Button_SleightOfHand.Size = new Size(75, 23);
            Button_SleightOfHand.TabIndex = 29;
            Button_SleightOfHand.Tag = "roll d20+dexmod+profifprof(sleightofhand)";
            Button_SleightOfHand.Text = "Sleight of Hand";
            Button_SleightOfHand.UseVisualStyleBackColor = true;
            Button_SleightOfHand.Click += RollDice;
            // 
            // Button_Acrobatics
            // 
            Button_Acrobatics.Location = new Point(87, 140);
            Button_Acrobatics.Name = "Button_Acrobatics";
            Button_Acrobatics.Size = new Size(75, 23);
            Button_Acrobatics.TabIndex = 28;
            Button_Acrobatics.Tag = "roll d20+dexmod+profifprof(acrobatics)";
            Button_Acrobatics.Text = "Acrobatics";
            Button_Acrobatics.UseVisualStyleBackColor = true;
            Button_Acrobatics.Click += RollDice;
            // 
            // Button_DexSave
            // 
            Button_DexSave.Location = new Point(87, 111);
            Button_DexSave.Name = "Button_DexSave";
            Button_DexSave.Size = new Size(75, 23);
            Button_DexSave.TabIndex = 27;
            Button_DexSave.Tag = "roll d20+dexmod+profifprof(dexsave)";
            Button_DexSave.Text = "Dex Save";
            Button_DexSave.UseVisualStyleBackColor = true;
            Button_DexSave.Click += RollDice;
            // 
            // Button_Dex
            // 
            Button_Dex.Location = new Point(87, 82);
            Button_Dex.Name = "Button_Dex";
            Button_Dex.Size = new Size(75, 23);
            Button_Dex.TabIndex = 26;
            Button_Dex.Tag = "roll d20+dexmod";
            Button_Dex.Text = "Dexterity";
            Button_Dex.UseVisualStyleBackColor = true;
            Button_Dex.Click += RollDice;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 53);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 25;
            label2.Text = "strmod";
            // 
            // Button_Athletics
            // 
            Button_Athletics.Location = new Point(6, 140);
            Button_Athletics.Name = "Button_Athletics";
            Button_Athletics.Size = new Size(75, 23);
            Button_Athletics.TabIndex = 24;
            Button_Athletics.Tag = "roll d20+strmod+profifprof(athletics)";
            Button_Athletics.Text = "Athletics";
            Button_Athletics.UseVisualStyleBackColor = true;
            Button_Athletics.Click += RollDice;
            // 
            // Button_StrSave
            // 
            Button_StrSave.Location = new Point(6, 111);
            Button_StrSave.Name = "Button_StrSave";
            Button_StrSave.Size = new Size(75, 23);
            Button_StrSave.TabIndex = 23;
            Button_StrSave.Tag = "roll d20+strmod+profifprof(strsave)";
            Button_StrSave.Text = "Str Save";
            Button_StrSave.UseVisualStyleBackColor = true;
            Button_StrSave.Click += RollDice;
            // 
            // Button_Str
            // 
            Button_Str.Location = new Point(6, 82);
            Button_Str.Name = "Button_Str";
            Button_Str.Size = new Size(75, 23);
            Button_Str.TabIndex = 22;
            Button_Str.Tag = "roll d20+strmod";
            Button_Str.Text = "Strength";
            Button_Str.UseVisualStyleBackColor = true;
            Button_Str.Click += RollDice;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(281, 368);
            label1.Name = "label1";
            label1.Size = new Size(301, 15);
            label1.TabIndex = 21;
            label1.Text = "eventually set the button labels to include their bonuses";
            // 
            // BasicAbilitiesArea
            // 
            BasicAbilitiesArea.AllowUserToAddRows = false;
            BasicAbilitiesArea.AllowUserToDeleteRows = false;
            BasicAbilitiesArea.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            BasicAbilitiesArea.Columns.AddRange(new DataGridViewColumn[] { Basic_NumberCol, Basic_IDCol, Basic_NameCol, Basic_TextCol, Basic_UseButtonCol });
            BasicAbilitiesArea.Location = new Point(6, 403);
            BasicAbilitiesArea.Name = "BasicAbilitiesArea";
            BasicAbilitiesArea.ReadOnly = true;
            BasicAbilitiesArea.RowTemplate.Height = 25;
            BasicAbilitiesArea.Size = new Size(729, 123);
            BasicAbilitiesArea.TabIndex = 18;
            // 
            // Basic_NumberCol
            // 
            Basic_NumberCol.HeaderText = "";
            Basic_NumberCol.Name = "Basic_NumberCol";
            Basic_NumberCol.ReadOnly = true;
            // 
            // Basic_IDCol
            // 
            Basic_IDCol.HeaderText = "ID";
            Basic_IDCol.Name = "Basic_IDCol";
            Basic_IDCol.ReadOnly = true;
            Basic_IDCol.Visible = false;
            // 
            // Basic_NameCol
            // 
            Basic_NameCol.HeaderText = "Name";
            Basic_NameCol.Name = "Basic_NameCol";
            Basic_NameCol.ReadOnly = true;
            // 
            // Basic_TextCol
            // 
            Basic_TextCol.HeaderText = "Description";
            Basic_TextCol.Name = "Basic_TextCol";
            Basic_TextCol.ReadOnly = true;
            // 
            // Basic_UseButtonCol
            // 
            Basic_UseButtonCol.HeaderText = "";
            Basic_UseButtonCol.Name = "Basic_UseButtonCol";
            Basic_UseButtonCol.ReadOnly = true;
            // 
            // Char_Cha
            // 
            Char_Cha.AutoSize = true;
            Char_Cha.Location = new Point(433, 38);
            Char_Cha.Name = "Char_Cha";
            Char_Cha.Size = new Size(26, 15);
            Char_Cha.TabIndex = 17;
            Char_Cha.Text = "cha";
            Char_Cha.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Wis
            // 
            Char_Wis.AutoSize = true;
            Char_Wis.Location = new Point(351, 38);
            Char_Wis.Name = "Char_Wis";
            Char_Wis.Size = new Size(24, 15);
            Char_Wis.TabIndex = 15;
            Char_Wis.Text = "wis";
            Char_Wis.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Int
            // 
            Char_Int.AutoSize = true;
            Char_Int.Location = new Point(281, 38);
            Char_Int.Name = "Char_Int";
            Char_Int.Size = new Size(21, 15);
            Char_Int.TabIndex = 13;
            Char_Int.Text = "int";
            Char_Int.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Con
            // 
            Char_Con.AutoSize = true;
            Char_Con.Location = new Point(187, 38);
            Char_Con.Name = "Char_Con";
            Char_Con.Size = new Size(27, 15);
            Char_Con.TabIndex = 11;
            Char_Con.Text = "con";
            Char_Con.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Dex
            // 
            Char_Dex.AutoSize = true;
            Char_Dex.Location = new Point(106, 38);
            Char_Dex.Name = "Char_Dex";
            Char_Dex.Size = new Size(26, 15);
            Char_Dex.TabIndex = 9;
            Char_Dex.Text = "dex";
            Char_Dex.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Str
            // 
            Char_Str.AutoSize = true;
            Char_Str.Location = new Point(30, 38);
            Char_Str.Name = "Char_Str";
            Char_Str.Size = new Size(20, 15);
            Char_Str.TabIndex = 7;
            Char_Str.Text = "str";
            Char_Str.TextAlign = ContentAlignment.TopRight;
            // 
            // Char_Race
            // 
            Char_Race.AutoSize = true;
            Char_Race.Location = new Point(303, 12);
            Char_Race.Name = "Char_Race";
            Char_Race.Size = new Size(54, 15);
            Char_Race.TabIndex = 5;
            Char_Race.Text = "racelabel";
            Char_Race.Click += Char_Race_Click;
            // 
            // CharLabel_Race
            // 
            CharLabel_Race.AutoSize = true;
            CharLabel_Race.Location = new Point(265, 12);
            CharLabel_Race.Name = "CharLabel_Race";
            CharLabel_Race.Size = new Size(32, 15);
            CharLabel_Race.TabIndex = 4;
            CharLabel_Race.Text = "Race";
            // 
            // Char_Name
            // 
            Char_Name.AutoSize = true;
            Char_Name.Location = new Point(176, 12);
            Char_Name.Name = "Char_Name";
            Char_Name.Size = new Size(62, 15);
            Char_Name.TabIndex = 3;
            Char_Name.Text = "namelabel";
            Char_Name.Click += Char_Name_Click;
            // 
            // CharLabel_Name
            // 
            CharLabel_Name.AutoSize = true;
            CharLabel_Name.Location = new Point(120, 12);
            CharLabel_Name.Name = "CharLabel_Name";
            CharLabel_Name.Size = new Size(39, 15);
            CharLabel_Name.TabIndex = 2;
            CharLabel_Name.Text = "Name";
            // 
            // Char_ID
            // 
            Char_ID.AutoSize = true;
            Char_ID.Location = new Point(30, 12);
            Char_ID.Name = "Char_ID";
            Char_ID.Size = new Size(42, 15);
            Char_ID.TabIndex = 1;
            Char_ID.Text = "idlabel";
            // 
            // CharLabel_ID
            // 
            CharLabel_ID.AutoSize = true;
            CharLabel_ID.Location = new Point(6, 12);
            CharLabel_ID.Name = "CharLabel_ID";
            CharLabel_ID.Size = new Size(18, 15);
            CharLabel_ID.TabIndex = 0;
            CharLabel_ID.Text = "ID";
            // 
            // AbilitiesTab
            // 
            AbilitiesTab.Controls.Add(AbilitiesArea);
            AbilitiesTab.Location = new Point(4, 24);
            AbilitiesTab.Name = "AbilitiesTab";
            AbilitiesTab.Padding = new Padding(3);
            AbilitiesTab.Size = new Size(741, 532);
            AbilitiesTab.TabIndex = 2;
            AbilitiesTab.Text = "Abilities";
            AbilitiesTab.UseVisualStyleBackColor = true;
            // 
            // AbilitiesArea
            // 
            AbilitiesArea.AllowUserToAddRows = false;
            AbilitiesArea.AllowUserToDeleteRows = false;
            AbilitiesArea.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AbilitiesArea.Columns.AddRange(new DataGridViewColumn[] { Abilities_NumberCol, Abilities_IDCol, Abilities_NameCol, Abilities_TextCol, Abilities_RechargeCol, Abilities_DiceCol, Abilities_UsesCol, Abilities_UseButtonCol, Abilities_Plus1Col, Abilities_Minus1Col });
            AbilitiesArea.Location = new Point(6, 6);
            AbilitiesArea.Name = "AbilitiesArea";
            AbilitiesArea.RowTemplate.Height = 25;
            AbilitiesArea.Size = new Size(729, 520);
            AbilitiesArea.TabIndex = 20;
            AbilitiesArea.CellContentClick += AbilitiesArea_CellContentClick;
            // 
            // Abilities_NumberCol
            // 
            Abilities_NumberCol.HeaderText = "";
            Abilities_NumberCol.Name = "Abilities_NumberCol";
            Abilities_NumberCol.Width = 50;
            // 
            // Abilities_IDCol
            // 
            Abilities_IDCol.HeaderText = "ID";
            Abilities_IDCol.Name = "Abilities_IDCol";
            Abilities_IDCol.Visible = false;
            // 
            // Abilities_NameCol
            // 
            Abilities_NameCol.HeaderText = "Name";
            Abilities_NameCol.Name = "Abilities_NameCol";
            Abilities_NameCol.Width = 150;
            // 
            // Abilities_TextCol
            // 
            Abilities_TextCol.HeaderText = "Text";
            Abilities_TextCol.Name = "Abilities_TextCol";
            // 
            // Abilities_RechargeCol
            // 
            Abilities_RechargeCol.HeaderText = "Recharge";
            Abilities_RechargeCol.Name = "Abilities_RechargeCol";
            // 
            // Abilities_DiceCol
            // 
            Abilities_DiceCol.HeaderText = "Dice";
            Abilities_DiceCol.Name = "Abilities_DiceCol";
            // 
            // Abilities_UsesCol
            // 
            Abilities_UsesCol.HeaderText = "Uses";
            Abilities_UsesCol.Name = "Abilities_UsesCol";
            Abilities_UsesCol.Width = 75;
            // 
            // Abilities_UseButtonCol
            // 
            Abilities_UseButtonCol.HeaderText = "";
            Abilities_UseButtonCol.Name = "Abilities_UseButtonCol";
            Abilities_UseButtonCol.Width = 50;
            // 
            // Abilities_Plus1Col
            // 
            Abilities_Plus1Col.HeaderText = "";
            Abilities_Plus1Col.Name = "Abilities_Plus1Col";
            Abilities_Plus1Col.Resizable = DataGridViewTriState.True;
            Abilities_Plus1Col.SortMode = DataGridViewColumnSortMode.Automatic;
            Abilities_Plus1Col.Width = 25;
            // 
            // Abilities_Minus1Col
            // 
            Abilities_Minus1Col.HeaderText = "";
            Abilities_Minus1Col.Name = "Abilities_Minus1Col";
            Abilities_Minus1Col.Resizable = DataGridViewTriState.True;
            Abilities_Minus1Col.SortMode = DataGridViewColumnSortMode.Automatic;
            Abilities_Minus1Col.Width = 25;
            // 
            // MagicTab
            // 
            MagicTab.Controls.Add(label8);
            MagicTab.Controls.Add(label7);
            MagicTab.Location = new Point(4, 24);
            MagicTab.Name = "MagicTab";
            MagicTab.Padding = new Padding(3);
            MagicTab.Size = new Size(741, 532);
            MagicTab.TabIndex = 3;
            MagicTab.Text = "Magic";
            MagicTab.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(124, 58);
            label7.Name = "label7";
            label7.Size = new Size(102, 15);
            label7.TabIndex = 0;
            label7.Text = "Spell slots go here";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(168, 146);
            label8.Name = "label8";
            label8.Size = new Size(125, 15);
            label8.TabIndex = 1;
            label8.Text = "List of spells goes here";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(774, 759);
            Controls.Add(MainTabArea);
            Controls.Add(outputDescription);
            Name = "Form1";
            MainTabArea.ResumeLayout(false);
            DiceTab.ResumeLayout(false);
            DiceTab.PerformLayout();
            CharacterTab.ResumeLayout(false);
            CharacterTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)BasicAbilitiesArea).EndInit();
            AbilitiesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AbilitiesArea).EndInit();
            MagicTab.ResumeLayout(false);
            MagicTab.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button rollDice;
        private Label outputTotal;
        private TextBox diceStringBox;
        private Label outputDescription;
        private TabControl MainTabArea;
        private TabPage DiceTab;
        private TabPage CharacterTab;
        private Label Char_Race;
        private Label CharLabel_Race;
        private Label Char_Name;
        private Label CharLabel_Name;
        private Label Char_ID;
        private Label CharLabel_ID;
        private Label Char_Cha;
        private Label Char_Wis;
        private Label Char_Int;
        private Label Char_Con;
        private Label Char_Dex;
        private Label Char_Str;
        private Label LastRollLabel;
        private Label DiceArrayDisplay;
        private Button AddD100Button;
        private Button AddD20Button;
        private Button AddD12Button;
        private Button AddD10Button;
        private Button AddD8Button;
        private Button AddD6Button;
        private Button AddD4Button;
        private Button AddD3Button;
        private Button AddD2Button;
        private Button RerollButton;
        private Label LastRollDiceString;
        private Label LastRollDiceLabel;
        private Button RemoveD100Button;
        private Button RemoveD20Button;
        private Button RemoveD12Button;
        private Button RemoveD10Button;
        private Button RemoveD8Button;
        private Button RemoveD6Button;
        private Button RemoveD4Button;
        private Button RemoveD3Button;
        private Button RemoveD2Button;
        private TabPage AbilitiesTab;
        private DataGridView AbilitiesArea;
        private DataGridView BasicAbilitiesArea;
        private DataGridViewTextBoxColumn Abilities_NumberCol;
        private DataGridViewTextBoxColumn Abilities_IDCol;
        private DataGridViewTextBoxColumn Abilities_NameCol;
        private DataGridViewTextBoxColumn Abilities_TextCol;
        private DataGridViewTextBoxColumn Abilities_RechargeCol;
        private DataGridViewTextBoxColumn Abilities_DiceCol;
        private DataGridViewTextBoxColumn Abilities_UsesCol;
        private DataGridViewButtonColumn Abilities_UseButtonCol;
        private DataGridViewButtonColumn Abilities_Plus1Col;
        private DataGridViewButtonColumn Abilities_Minus1Col;
        private DataGridViewTextBoxColumn Basic_NumberCol;
        private DataGridViewTextBoxColumn Basic_IDCol;
        private DataGridViewTextBoxColumn Basic_NameCol;
        private DataGridViewTextBoxColumn Basic_TextCol;
        private DataGridViewButtonColumn Basic_UseButtonCol;
        private Label label4;
        private Label label3;
        private Button button33;
        private Button button34;
        private Button button35;
        private Button button36;
        private Button button29;
        private Button button30;
        private Button button31;
        private Button button32;
        private Button button25;
        private Button button26;
        private Button button27;
        private Button button28;
        private Button button21;
        private Button button22;
        private Button button23;
        private Button button24;
        private Button button17;
        private Button button18;
        private Button button19;
        private Button button20;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button16;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button3;
        private Button button4;
        private Button button1;
        private Button button2;
        private Button button37;
        private Button button38;
        private Button button42;
        private Button button41;
        private Button button40;
        private Button button39;
        private Label label1;
        private Button Button_Str;
        private Button Button_StrSave;
        private Button Button_Athletics;
        private Label label2;
        private Button Button_Acrobatics;
        private Button Button_DexSave;
        private Button Button_Dex;
        private Button Button_Stealth;
        private Button Button_SleightOfHand;
        private Button Button_ConSave;
        private Button Button_Con;
        private Button Button_Investigation;
        private Button Button_History;
        private Button Button_Arcana;
        private Button Button_IntSave;
        private Button Button_Int;
        private Label label5;
        private Label label6;
        private Button Button_Religion;
        private Button Button_Nature;
        private Button Button_Persuasion;
        private Button Button_Performance;
        private Button Button_Intimidation;
        private Button Button_Deception;
        private Button Button_ChaSave;
        private Button Button_Cha;
        private Button Button_Survival;
        private Button Button_Perception;
        private Button Button_Medicine;
        private Button Button_Insight;
        private Button Button_AnimalHandling;
        private Button Button_WisSave;
        private Button Button_Wis;
        private Label Char_Prof;
        private Label CharLabel_Prof;
        private Button Button_SaveCharacter;
        private Button TestButton;
        private TabPage MagicTab;
        private Label label7;
        private Label label8;
    }
}