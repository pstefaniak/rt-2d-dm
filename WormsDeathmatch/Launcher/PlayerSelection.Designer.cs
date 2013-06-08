namespace Launcher
{
    partial class PlayerSelection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbName = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btSaveProfile = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.cbKeyboard = new System.Windows.Forms.ComboBox();
            this.cbMouse = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bWpns = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(3, 23);
            this.tbName.MaxLength = 15;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(164, 20);
            this.tbName.TabIndex = 0;
            this.tbName.Text = "Unnamed";
            this.tbName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbName_KeyPress);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(182, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // btSaveProfile
            // 
            this.btSaveProfile.Location = new System.Drawing.Point(191, 16);
            this.btSaveProfile.Name = "btSaveProfile";
            this.btSaveProfile.Size = new System.Drawing.Size(53, 21);
            this.btSaveProfile.TabIndex = 2;
            this.btSaveProfile.Text = "Save";
            this.btSaveProfile.UseVisualStyleBackColor = true;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(6, 7);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(35, 13);
            this.lbName.TabIndex = 3;
            this.lbName.Text = "Name";
            // 
            // colorDialog1
            // 
            this.colorDialog1.AllowFullOpen = false;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // pbColor
            // 
            this.pbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbColor.Location = new System.Drawing.Point(173, 23);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(20, 20);
            this.pbColor.TabIndex = 4;
            this.pbColor.TabStop = false;
            this.pbColor.Click += new System.EventHandler(this.pbColor_Click);
            // 
            // cbKeyboard
            // 
            this.cbKeyboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeyboard.FormattingEnabled = true;
            this.cbKeyboard.Items.AddRange(new object[] {
            "Up, Left, Right",
            "W, A, D",
            "I, J, L",
            "Num 5, Num 1, Num 3"});
            this.cbKeyboard.Location = new System.Drawing.Point(6, 16);
            this.cbKeyboard.Name = "cbKeyboard";
            this.cbKeyboard.Size = new System.Drawing.Size(238, 21);
            this.cbKeyboard.TabIndex = 0;
            // 
            // cbMouse
            // 
            this.cbMouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMouse.FormattingEnabled = true;
            this.cbMouse.Location = new System.Drawing.Point(6, 43);
            this.cbMouse.Name = "cbMouse";
            this.cbMouse.Size = new System.Drawing.Size(238, 21);
            this.cbMouse.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panel3);
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(265, 227);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Player";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.bWpns);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.cbKeyboard);
            this.panel3.Controls.Add(this.cbMouse);
            this.panel3.Location = new System.Drawing.Point(6, 127);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(253, 94);
            this.panel3.TabIndex = 9;
            // 
            // bWpns
            // 
            this.bWpns.Location = new System.Drawing.Point(158, 69);
            this.bWpns.Name = "bWpns";
            this.bWpns.Size = new System.Drawing.Size(85, 22);
            this.bWpns.TabIndex = 8;
            this.bWpns.Text = "Weapons";
            this.bWpns.UseVisualStyleBackColor = true;
            this.bWpns.Click += new System.EventHandler(this.bWpns_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Controls";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbName);
            this.panel2.Controls.Add(this.pbColor);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tbName);
            this.panel2.Location = new System.Drawing.Point(6, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 51);
            this.panel2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Color";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btSaveProfile);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 45);
            this.panel1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Profile";
            // 
            // PlayerSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "PlayerSelection";
            this.Size = new System.Drawing.Size(273, 240);
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btSaveProfile;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.ComboBox cbMouse;
        private System.Windows.Forms.ComboBox cbKeyboard;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Button bWpns;
    }
}
