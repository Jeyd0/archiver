namespace archiver
{
    partial class extract
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
            button5 = new Button();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            button4 = new Button();
            button3 = new Button();
            zip_items = new ListBox();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // button5
            // 
            button5.BackColor = Color.Silver;
            button5.Cursor = Cursors.Hand;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Location = new Point(974, 569);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 18;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Font = new Font("Poppins", 9.75F);
            textBox3.Location = new Point(790, 132);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Extract name";
            textBox3.Size = new Size(184, 26);
            textBox3.TabIndex = 17;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Poppins", 9.75F);
            textBox2.Location = new Point(790, 176);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Select where to save";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(184, 26);
            textBox2.TabIndex = 16;
            // 
            // button4
            // 
            button4.BackColor = Color.Silver;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Poppins", 9.75F);
            button4.Location = new Point(981, 172);
            button4.Name = "button4";
            button4.Size = new Size(68, 33);
            button4.TabIndex = 15;
            button4.Text = "Browse";
            button4.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.Silver;
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Poppins", 9.75F);
            button3.Location = new Point(790, 221);
            button3.Name = "button3";
            button3.Size = new Size(140, 33);
            button3.TabIndex = 14;
            button3.Text = "Extract";
            button3.UseVisualStyleBackColor = false;
            // 
            // zip_items
            // 
            zip_items.BorderStyle = BorderStyle.None;
            zip_items.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            zip_items.FormattingEnabled = true;
            zip_items.ItemHeight = 23;
            zip_items.Location = new Point(29, 132);
            zip_items.Name = "zip_items";
            zip_items.Size = new Size(740, 460);
            zip_items.TabIndex = 13;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Poppins", 9.75F);
            textBox1.Location = new Point(200, 32);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Insert file to extarct";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(368, 48);
            textBox1.TabIndex = 12;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // button1
            // 
            button1.BackColor = Color.Silver;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Poppins", 9.75F);
            button1.Location = new Point(585, 41);
            button1.Name = "button1";
            button1.Size = new Size(140, 33);
            button1.TabIndex = 11;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = false;
            // 
            // extract
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1082, 623);
            Controls.Add(button5);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(zip_items);
            Controls.Add(textBox1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "extract";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EXTRACT";
            Load += Form3_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button5;
        private TextBox textBox3;
        private TextBox textBox2;
        private Button button4;
        private Button button3;
        private ListBox zip_items;
        private TextBox textBox1;
        private Button button1;
    }
}