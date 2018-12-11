namespace TactPlay
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.buttonMode1 = new System.Windows.Forms.Button();
            this.buttonMode2 = new System.Windows.Forms.Button();
            this.buttonMode3 = new System.Windows.Forms.Button();
            this.buttonMode0 = new System.Windows.Forms.Button();
            this.comboBoxCOMPort = new System.Windows.Forms.ComboBox();
            this.buttonUpdateCOMPorts = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(256, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(238, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop LogFile Reading";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 255);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(238, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "ConnectToSerialPort";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(256, 226);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 20);
            this.textBox1.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(256, 197);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(238, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Send Text to VS";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(344, 255);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Motor Test";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // buttonMode1
            // 
            this.buttonMode1.Location = new System.Drawing.Point(12, 70);
            this.buttonMode1.Name = "buttonMode1";
            this.buttonMode1.Size = new System.Drawing.Size(238, 23);
            this.buttonMode1.TabIndex = 6;
            this.buttonMode1.Text = "Ammo EventHandler enabled";
            this.buttonMode1.UseVisualStyleBackColor = true;
            this.buttonMode1.Click += new System.EventHandler(this.buttonMode1_Click);
            // 
            // buttonMode2
            // 
            this.buttonMode2.Location = new System.Drawing.Point(12, 99);
            this.buttonMode2.Name = "buttonMode2";
            this.buttonMode2.Size = new System.Drawing.Size(238, 23);
            this.buttonMode2.TabIndex = 7;
            this.buttonMode2.Text = "Stam EventHandler enabled";
            this.buttonMode2.UseVisualStyleBackColor = true;
            this.buttonMode2.Click += new System.EventHandler(this.buttonMode2_Click);
            // 
            // buttonMode3
            // 
            this.buttonMode3.Location = new System.Drawing.Point(12, 128);
            this.buttonMode3.Name = "buttonMode3";
            this.buttonMode3.Size = new System.Drawing.Size(238, 23);
            this.buttonMode3.TabIndex = 8;
            this.buttonMode3.Text = "Ammo and Stam EventHandlers enabled";
            this.buttonMode3.UseVisualStyleBackColor = true;
            this.buttonMode3.Click += new System.EventHandler(this.buttonMode3_Click);
            // 
            // buttonMode0
            // 
            this.buttonMode0.Location = new System.Drawing.Point(12, 41);
            this.buttonMode0.Name = "buttonMode0";
            this.buttonMode0.Size = new System.Drawing.Size(238, 23);
            this.buttonMode0.TabIndex = 9;
            this.buttonMode0.Text = "Disable all EventHandlers";
            this.buttonMode0.UseVisualStyleBackColor = true;
            this.buttonMode0.Click += new System.EventHandler(this.buttonMode0_Click);
            // 
            // comboBoxCOMPort
            // 
            this.comboBoxCOMPort.FormattingEnabled = true;
            this.comboBoxCOMPort.Location = new System.Drawing.Point(12, 226);
            this.comboBoxCOMPort.Name = "comboBoxCOMPort";
            this.comboBoxCOMPort.Size = new System.Drawing.Size(124, 21);
            this.comboBoxCOMPort.TabIndex = 10;
            this.comboBoxCOMPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxCOMPort_SelectedIndexChanged);
            // 
            // buttonUpdateCOMPorts
            // 
            this.buttonUpdateCOMPorts.Location = new System.Drawing.Point(142, 226);
            this.buttonUpdateCOMPorts.Name = "buttonUpdateCOMPorts";
            this.buttonUpdateCOMPorts.Size = new System.Drawing.Size(108, 23);
            this.buttonUpdateCOMPorts.TabIndex = 11;
            this.buttonUpdateCOMPorts.Text = "Update ComboBox";
            this.buttonUpdateCOMPorts.UseVisualStyleBackColor = true;
            this.buttonUpdateCOMPorts.Click += new System.EventHandler(this.buttonUpdateCOMPorts_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(238, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start LogFile Reading";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(256, 41);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(238, 23);
            this.button6.TabIndex = 12;
            this.button6.Text = "Send Stam Event Timer";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(256, 99);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(238, 23);
            this.button7.TabIndex = 13;
            this.button7.Text = "HealthReader";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(256, 157);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(238, 20);
            this.textBox2.TabIndex = 14;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(256, 128);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(238, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = "Stop HealthReader";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 336);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.buttonUpdateCOMPorts);
            this.Controls.Add(this.comboBoxCOMPort);
            this.Controls.Add(this.buttonMode0);
            this.Controls.Add(this.buttonMode3);
            this.Controls.Add(this.buttonMode2);
            this.Controls.Add(this.buttonMode1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button buttonMode1;
        private System.Windows.Forms.Button buttonMode2;
        private System.Windows.Forms.Button buttonMode3;
        private System.Windows.Forms.Button buttonMode0;
        private System.Windows.Forms.ComboBox comboBoxCOMPort;
        private System.Windows.Forms.Button buttonUpdateCOMPorts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button8;
    }
}

