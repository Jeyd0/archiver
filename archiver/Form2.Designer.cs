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
            compress_btn = new Button();
            extract_btn = new Button();
            about_btn = new Button();
            exit_btn = new Button();
            panel1 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // compress_btn
            // 
            compress_btn.BackColor = Color.Silver;
            compress_btn.Cursor = Cursors.Hand;
            compress_btn.FlatStyle = FlatStyle.Popup;
            compress_btn.Font = new Font("Poppins", 9.75F);
            compress_btn.Location = new Point(98, 104);
            compress_btn.Name = "compress_btn";
            compress_btn.Size = new Size(159, 51);
            compress_btn.TabIndex = 0;
            compress_btn.Text = "COMPRESS";
            compress_btn.UseVisualStyleBackColor = false;
            compress_btn.Click += button1_Click;
            // 
            // extract_btn
            // 
            extract_btn.BackColor = Color.Silver;
            extract_btn.Cursor = Cursors.Hand;
            extract_btn.FlatStyle = FlatStyle.Popup;
            extract_btn.Font = new Font("Poppins", 9.75F);
            extract_btn.Location = new Point(98, 203);
            extract_btn.Name = "extract_btn";
            extract_btn.Size = new Size(159, 51);
            extract_btn.TabIndex = 1;
            extract_btn.Text = "EXTRACT";
            extract_btn.UseVisualStyleBackColor = false;
            extract_btn.Click += button2_Click;
            // 
            // about_btn
            // 
            about_btn.BackColor = Color.Silver;
            about_btn.Cursor = Cursors.Hand;
            about_btn.FlatStyle = FlatStyle.Popup;
            about_btn.Font = new Font("Poppins", 9.75F);
            about_btn.Location = new Point(12, 315);
            about_btn.Name = "about_btn";
            about_btn.Size = new Size(80, 29);
            about_btn.TabIndex = 3;
            about_btn.Text = "About";
            about_btn.UseVisualStyleBackColor = false;
            about_btn.Click += button3_Click;
            // 
            // exit_btn
            // 
            exit_btn.BackColor = Color.IndianRed;
            exit_btn.Cursor = Cursors.Hand;
            exit_btn.FlatStyle = FlatStyle.Popup;
            exit_btn.Font = new Font("Poppins", 9.75F);
            exit_btn.Location = new Point(132, 270);
            exit_btn.Name = "exit_btn";
            exit_btn.Size = new Size(86, 28);
            exit_btn.TabIndex = 4;
            exit_btn.Text = "EXIT";
            exit_btn.UseVisualStyleBackColor = false;
            exit_btn.Click += button4_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Info;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(about_btn);
            panel1.Controls.Add(exit_btn);
            panel1.Controls.Add(extract_btn);
            panel1.Controls.Add(compress_btn);
            panel1.Location = new Point(33, 65);
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

        private Button compress_btn;
        private Button extract_btn;
        private Button about_btn;
        private Button exit_btn;
        private Panel panel1;
        private Label label1;
    }
}