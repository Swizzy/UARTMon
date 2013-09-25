namespace launcher
{
    internal sealed partial class MainForm
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
            this.comport = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.logname = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dolog = new System.Windows.Forms.CheckBox();
            this.updbtn = new System.Windows.Forms.Button();
            this.lowspeed = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comport
            // 
            this.comport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comport.FormattingEnabled = true;
            this.comport.Location = new System.Drawing.Point(12, 35);
            this.comport.Name = "comport";
            this.comport.Size = new System.Drawing.Size(186, 21);
            this.comport.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(236, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "Launch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // logname
            // 
            this.logname.Location = new System.Drawing.Point(33, 67);
            this.logname.Name = "logname";
            this.logname.Size = new System.Drawing.Size(165, 20);
            this.logname.TabIndex = 2;
            this.logname.Text = "uart.log";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(204, 64);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // dolog
            // 
            this.dolog.AutoSize = true;
            this.dolog.Location = new System.Drawing.Point(12, 70);
            this.dolog.Name = "dolog";
            this.dolog.Size = new System.Drawing.Size(15, 14);
            this.dolog.TabIndex = 4;
            this.dolog.UseVisualStyleBackColor = true;
            // 
            // updbtn
            // 
            this.updbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updbtn.Location = new System.Drawing.Point(204, 35);
            this.updbtn.Name = "updbtn";
            this.updbtn.Size = new System.Drawing.Size(44, 23);
            this.updbtn.TabIndex = 3;
            this.updbtn.Text = "...";
            this.updbtn.UseVisualStyleBackColor = true;
            this.updbtn.Click += new System.EventHandler(this.UpdbtnClick);
            // 
            // lowspeed
            // 
            this.lowspeed.AutoSize = true;
            this.lowspeed.Location = new System.Drawing.Point(12, 12);
            this.lowspeed.Name = "lowspeed";
            this.lowspeed.Size = new System.Drawing.Size(190, 17);
            this.lowspeed.TabIndex = 5;
            this.lowspeed.Text = "Lowspeed (Cygnos/Demon speed)";
            this.lowspeed.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 145);
            this.Controls.Add(this.lowspeed);
            this.Controls.Add(this.dolog);
            this.Controls.Add(this.updbtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.logname);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comport);
            this.Name = "MainForm";
            this.Text = "UARTMon Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox logname;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox dolog;
        private System.Windows.Forms.Button updbtn;
        private System.Windows.Forms.CheckBox lowspeed;
    }
}

