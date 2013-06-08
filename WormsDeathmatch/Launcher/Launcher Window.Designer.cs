namespace Launcher
{
    partial class LauncherWindow
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRefreshMice = new System.Windows.Forms.Button();
            this.lbMice = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLastMouseName = new System.Windows.Forms.TextBox();
            this.bStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbLevels = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.clbWeapons = new System.Windows.Forms.CheckedListBox();
            this.cxbConsole = new System.Windows.Forms.CheckBox();
            this.cxbLimitedWeapons = new System.Windows.Forms.CheckBox();
            this.nudWeaponsLimit = new System.Windows.Forms.NumericUpDown();
            this.playerSelection1 = new Launcher.PlayerSelection();
            this.playerSelection2 = new Launcher.PlayerSelection();
            this.playerSelection3 = new Launcher.PlayerSelection();
            this.playerSelection4 = new Launcher.PlayerSelection();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeaponsLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(51, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Local";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(69, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(75, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Multiplayer";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.playerSelection1);
            this.panel1.Controls.Add(this.playerSelection2);
            this.panel1.Controls.Add(this.playerSelection3);
            this.panel1.Controls.Add(this.playerSelection4);
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 241);
            this.panel1.TabIndex = 6;
            // 
            // bRefreshMice
            // 
            this.bRefreshMice.Location = new System.Drawing.Point(247, 224);
            this.bRefreshMice.Name = "bRefreshMice";
            this.bRefreshMice.Size = new System.Drawing.Size(76, 38);
            this.bRefreshMice.TabIndex = 7;
            this.bRefreshMice.Text = "Refresh";
            this.bRefreshMice.UseVisualStyleBackColor = true;
            this.bRefreshMice.Click += new System.EventHandler(this.RefreshMices_Click);
            // 
            // lbMice
            // 
            this.lbMice.FormattingEnabled = true;
            this.lbMice.Location = new System.Drawing.Point(6, 19);
            this.lbMice.Name = "lbMice";
            this.lbMice.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbMice.Size = new System.Drawing.Size(317, 199);
            this.lbMice.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbLastMouseName);
            this.groupBox1.Controls.Add(this.lbMice);
            this.groupBox1.Controls.Add(this.bRefreshMice);
            this.groupBox1.Location = new System.Drawing.Point(12, 282);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 268);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mice";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Last Clicked";
            // 
            // tbLastMouseName
            // 
            this.tbLastMouseName.Location = new System.Drawing.Point(6, 242);
            this.tbLastMouseName.Name = "tbLastMouseName";
            this.tbLastMouseName.ReadOnly = true;
            this.tbLastMouseName.Size = new System.Drawing.Size(175, 20);
            this.tbLastMouseName.TabIndex = 9;
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(981, 524);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(161, 64);
            this.bStart.TabIndex = 10;
            this.bStart.Text = "Start Game";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbLevels);
            this.groupBox2.Location = new System.Drawing.Point(347, 282);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 287);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Level";
            // 
            // lbLevels
            // 
            this.lbLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLevels.FormattingEnabled = true;
            this.lbLevels.Location = new System.Drawing.Point(3, 16);
            this.lbLevels.Name = "lbLevels";
            this.lbLevels.Size = new System.Drawing.Size(304, 268);
            this.lbLevels.Sorted = true;
            this.lbLevels.TabIndex = 0;
            this.lbLevels.SelectedIndexChanged += new System.EventHandler(this.lbLevels_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.clbWeapons);
            this.groupBox3.Location = new System.Drawing.Point(660, 282);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 287);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Weapons";
            // 
            // clbWeapons
            // 
            this.clbWeapons.CheckOnClick = true;
            this.clbWeapons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbWeapons.Enabled = false;
            this.clbWeapons.FormattingEnabled = true;
            this.clbWeapons.Location = new System.Drawing.Point(3, 16);
            this.clbWeapons.Name = "clbWeapons";
            this.clbWeapons.Size = new System.Drawing.Size(304, 268);
            this.clbWeapons.Sorted = true;
            this.clbWeapons.TabIndex = 0;
            // 
            // cxbConsole
            // 
            this.cxbConsole.AutoSize = true;
            this.cxbConsole.Location = new System.Drawing.Point(981, 501);
            this.cxbConsole.Name = "cxbConsole";
            this.cxbConsole.Size = new System.Drawing.Size(94, 17);
            this.cxbConsole.TabIndex = 13;
            this.cxbConsole.Text = "Show Console";
            this.cxbConsole.UseVisualStyleBackColor = true;
            // 
            // cxbLimitedWeapons
            // 
            this.cxbLimitedWeapons.AutoSize = true;
            this.cxbLimitedWeapons.Enabled = false;
            this.cxbLimitedWeapons.Location = new System.Drawing.Point(981, 282);
            this.cxbLimitedWeapons.Name = "cxbLimitedWeapons";
            this.cxbLimitedWeapons.Size = new System.Drawing.Size(108, 17);
            this.cxbLimitedWeapons.TabIndex = 14;
            this.cxbLimitedWeapons.Text = "Limited Weapons";
            this.cxbLimitedWeapons.UseVisualStyleBackColor = true;
            this.cxbLimitedWeapons.CheckedChanged += new System.EventHandler(this.cxbLimitedWeapons_CheckedChanged);
            // 
            // nudWeaponsLimit
            // 
            this.nudWeaponsLimit.Location = new System.Drawing.Point(981, 305);
            this.nudWeaponsLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWeaponsLimit.Name = "nudWeaponsLimit";
            this.nudWeaponsLimit.Size = new System.Drawing.Size(120, 20);
            this.nudWeaponsLimit.TabIndex = 15;
            this.nudWeaponsLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // playerSelection1
            // 
            this.playerSelection1.Location = new System.Drawing.Point(3, 3);
            this.playerSelection1.Name = "playerSelection1";
            this.playerSelection1.Size = new System.Drawing.Size(276, 236);
            this.playerSelection1.TabIndex = 0;
            // 
            // playerSelection2
            // 
            this.playerSelection2.Location = new System.Drawing.Point(285, 3);
            this.playerSelection2.Name = "playerSelection2";
            this.playerSelection2.Size = new System.Drawing.Size(273, 236);
            this.playerSelection2.TabIndex = 1;
            // 
            // playerSelection3
            // 
            this.playerSelection3.Location = new System.Drawing.Point(564, 3);
            this.playerSelection3.Name = "playerSelection3";
            this.playerSelection3.Size = new System.Drawing.Size(274, 236);
            this.playerSelection3.TabIndex = 2;
            // 
            // playerSelection4
            // 
            this.playerSelection4.Location = new System.Drawing.Point(844, 3);
            this.playerSelection4.Name = "playerSelection4";
            this.playerSelection4.Size = new System.Drawing.Size(276, 236);
            this.playerSelection4.TabIndex = 3;
            // 
            // LauncherWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 600);
            this.Controls.Add(this.nudWeaponsLimit);
            this.Controls.Add(this.cxbLimitedWeapons);
            this.Controls.Add(this.cxbConsole);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Name = "LauncherWindow";
            this.Text = "Launcher";
            this.Load += new System.EventHandler(this.LauncherWindow_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudWeaponsLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PlayerSelection playerSelection1;
        private PlayerSelection playerSelection2;
        private PlayerSelection playerSelection3;
        private PlayerSelection playerSelection4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bRefreshMice;
        private System.Windows.Forms.ListBox lbMice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLastMouseName;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbLevels;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox clbWeapons;
        private System.Windows.Forms.CheckBox cxbConsole;
        private System.Windows.Forms.CheckBox cxbLimitedWeapons;
        private System.Windows.Forms.NumericUpDown nudWeaponsLimit;
    }
}

