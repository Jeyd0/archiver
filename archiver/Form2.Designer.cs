namespace archiver
{
    partial class home
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            panel1 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Silver;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Poppins", 9.75F);
            button1.Location = new Point(98, 104);
            button1.Name = "button1";
            button1.Size = new Size(159, 51);
            button1.TabIndex = 0;
            button1.Text = "COMPRESS";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Silver;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Poppins", 9.75F);
            button2.Location = new Point(98, 203);
            button2.Name = "button2";
            button2.Size = new Size(159, 51);
            button2.TabIndex = 1;
            button2.Text = "EXTRACT";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Silver;
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Poppins", 9.75F);
            button3.Location = new Point(12, 441);
            button3.Name = "button3";
            button3.Size = new Size(80, 29);
            button3.TabIndex = 3;
            button3.Text = "About";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.IndianRed;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Poppins", 9.75F);
            button4.Location = new Point(132, 270);
            button4.Name = "button4";
            button4.Size = new Size(86, 28);
            button4.TabIndex = 4;
            button4.Text = "EXIT";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Info;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(36, 46);
            panel1.Name = "panel1";
            panel1.Size = new Size(361, 357);
            panel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Poppins Medium", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(122, 35);
            label1.Name = "label1";
            label1.Size = new Size(116, 34);
            label1.TabIndex = 6;
            label1.Text = "WELCOME";
            // 
            // home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(435, 482);
            Controls.Add(panel1);
            Controls.Add(button3);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "home";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HOME";
            Load += home_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Panel panel1;
        private Label label1;
    }
}