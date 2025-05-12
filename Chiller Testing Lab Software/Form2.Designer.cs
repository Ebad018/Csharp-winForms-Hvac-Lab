namespace Chiller_Testing_Lab_Software
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.EngineerNameTextBox = new System.Windows.Forms.TextBox();
            this.UnitNameTextBox = new System.Windows.Forms.TextBox();
            this.CompressorTextBox = new System.Windows.Forms.TextBox();
            this.CoilSizeTextBox = new System.Windows.Forms.TextBox();
            this.CapacityTextBox = new System.Windows.Forms.TextBox();
            this.FanDetailsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxS5 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxMotor = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxGas = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxInverter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(91, 39);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Location = new System.Drawing.Point(1152, 571);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Display ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EngineerNameTextBox
            // 
            this.EngineerNameTextBox.Location = new System.Drawing.Point(156, 160);
            this.EngineerNameTextBox.Name = "EngineerNameTextBox";
            this.EngineerNameTextBox.Size = new System.Drawing.Size(193, 20);
            this.EngineerNameTextBox.TabIndex = 2;
            this.EngineerNameTextBox.TextChanged += new System.EventHandler(this.UnitName_TextChanged);
            // 
            // UnitNameTextBox
            // 
            this.UnitNameTextBox.Location = new System.Drawing.Point(173, 190);
            this.UnitNameTextBox.Name = "UnitNameTextBox";
            this.UnitNameTextBox.Size = new System.Drawing.Size(176, 20);
            this.UnitNameTextBox.TabIndex = 3;
            this.UnitNameTextBox.TextChanged += new System.EventHandler(this.EngineerName_TextChanged);
            // 
            // CompressorTextBox
            // 
            this.CompressorTextBox.Location = new System.Drawing.Point(194, 220);
            this.CompressorTextBox.Name = "CompressorTextBox";
            this.CompressorTextBox.Size = new System.Drawing.Size(155, 20);
            this.CompressorTextBox.TabIndex = 4;
            this.CompressorTextBox.TextChanged += new System.EventHandler(this.Compressor_TextChanged);
            // 
            // CoilSizeTextBox
            // 
            this.CoilSizeTextBox.Location = new System.Drawing.Point(219, 250);
            this.CoilSizeTextBox.Name = "CoilSizeTextBox";
            this.CoilSizeTextBox.Size = new System.Drawing.Size(130, 20);
            this.CoilSizeTextBox.TabIndex = 5;
            this.CoilSizeTextBox.TextChanged += new System.EventHandler(this.CoilSize_TextChanged);
            // 
            // CapacityTextBox
            // 
            this.CapacityTextBox.Location = new System.Drawing.Point(241, 280);
            this.CapacityTextBox.Name = "CapacityTextBox";
            this.CapacityTextBox.Size = new System.Drawing.Size(108, 20);
            this.CapacityTextBox.TabIndex = 6;
            this.CapacityTextBox.TextChanged += new System.EventHandler(this.Capacity_TextChanged);
            // 
            // FanDetailsTextBox
            // 
            this.FanDetailsTextBox.Location = new System.Drawing.Point(255, 310);
            this.FanDetailsTextBox.Name = "FanDetailsTextBox";
            this.FanDetailsTextBox.Size = new System.Drawing.Size(94, 20);
            this.FanDetailsTextBox.TabIndex = 7;
            this.FanDetailsTextBox.TextChanged += new System.EventHandler(this.Fan_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Engr Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Unit Name";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Compressor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Coil Size";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 283);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Capacity";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(103, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Fan Details";
            // 
            // textBoxS5
            // 
            this.textBoxS5.Location = new System.Drawing.Point(272, 340);
            this.textBoxS5.Name = "textBoxS5";
            this.textBoxS5.Size = new System.Drawing.Size(77, 20);
            this.textBoxS5.TabIndex = 14;
            this.textBoxS5.TextChanged += new System.EventHandler(this.textBoxS5_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "CFM\r\n";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(170, 370);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Motor";
            // 
            // textBoxMotor
            // 
            this.textBoxMotor.Location = new System.Drawing.Point(289, 370);
            this.textBoxMotor.Name = "textBoxMotor";
            this.textBoxMotor.Size = new System.Drawing.Size(60, 20);
            this.textBoxMotor.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(181, 400);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Gas 410A";
            // 
            // textBoxGas
            // 
            this.textBoxGas.Location = new System.Drawing.Point(300, 400);
            this.textBoxGas.Name = "textBoxGas";
            this.textBoxGas.Size = new System.Drawing.Size(49, 20);
            this.textBoxGas.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(191, 426);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "I.C Details";
            // 
            // textBoxInverter
            // 
            this.textBoxInverter.Location = new System.Drawing.Point(300, 426);
            this.textBoxInverter.Name = "textBoxInverter";
            this.textBoxInverter.Size = new System.Drawing.Size(49, 20);
            this.textBoxInverter.TabIndex = 21;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1279, 606);
            this.Controls.Add(this.textBoxInverter);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxGas);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxMotor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxS5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FanDetailsTextBox);
            this.Controls.Add(this.CapacityTextBox);
            this.Controls.Add(this.CoilSizeTextBox);
            this.Controls.Add(this.CompressorTextBox);
            this.Controls.Add(this.UnitNameTextBox);
            this.Controls.Add(this.EngineerNameTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox EngineerNameTextBox;
        public System.Windows.Forms.TextBox UnitNameTextBox;
        public System.Windows.Forms.TextBox CompressorTextBox;
        public System.Windows.Forms.TextBox CoilSizeTextBox;
        public System.Windows.Forms.TextBox CapacityTextBox;
        public System.Windows.Forms.TextBox FanDetailsTextBox;
        public System.Windows.Forms.TextBox textBoxS5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox textBoxMotor;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox textBoxGas;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox textBoxInverter;
    }
}