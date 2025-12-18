namespace archiver
{
    partial class compress
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
            button1 = new Button();
            textBox1 = new TextBox();
            list_of_item_selected = new ListBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button5 = new Button();
            comboBox1 = new ComboBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Silver;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Poppins", 9.75F);
            button1.Location = new Point(559, 22);
            button1.Name = "button1";
            button1.Size = new Size(140, 33);
            button1.TabIndex = 0;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Poppins", 9.75F);
            textBox1.Location = new Point(185, 15);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Insert file(jpg,png,pdf,docx,excel)";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(368, 48);
            textBox1.TabIndex = 1;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // list_of_item_selected
            // 
            list_of_item_selected.BackColor = SystemColors.Control;
            list_of_item_selected.BorderStyle = BorderStyle.None;
            list_of_item_selected.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            list_of_item_selected.FormattingEnabled = true;
            list_of_item_selected.ItemHeight = 23;
            list_of_item_selected.Location = new Point(29, 132);
            list_of_item_selected.Name = "list_of_item_selected";
            list_of_item_selected.Size = new Size(740, 460);
            list_of_item_selected.TabIndex = 2;
            list_of_item_selected.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.Silver;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Poppins", 9.75F);
            button2.Location = new Point(15, 22);
            button2.Name = "button2";
            button2.Size = new Size(140, 33);
            button2.TabIndex = 3;
            button2.Text = "Clear all";
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.Silver;
            button3.Cursor = Cursors.Hand;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Poppins", 9.75F);
            button3.Location = new Point(15, 208);
            button3.Name = "button3";
            button3.Size = new Size(140, 33);
            button3.TabIndex = 5;
            button3.Text = "Compress";
            button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Silver;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Poppins", 9.75F);
            button4.Location = new Point(208, 161);
            button4.Name = "button4";
            button4.Size = new Size(68, 33);
            button4.TabIndex = 6;
            button4.Text = "Browse";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Poppins", 9.75F);
            textBox2.Location = new Point(15, 164);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Select where to save";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(184, 26);
            textBox2.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.Control;
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Font = new Font("Poppins", 9.75F);
            textBox3.Location = new Point(15, 117);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Zip Name";
            textBox3.Size = new Size(184, 26);
            textBox3.TabIndex = 8;
            // 
            // textBox4
            // 
            textBox4.BackColor = SystemColors.Control;
            textBox4.BorderStyle = BorderStyle.None;
            textBox4.Font = new Font("Poppins", 9.75F);
            textBox4.Location = new Point(208, 94);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Search file";
            textBox4.Size = new Size(318, 32);
            textBox4.TabIndex = 9;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // button5
            // 
            button5.BackColor = Color.Silver;
            button5.Cursor = Cursors.Hand;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Location = new Point(201, 423);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 10;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = SystemColors.Control;
            comboBox1.FlatStyle = FlatStyle.Popup;
            comboBox1.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Select type", "Zip", "Tar", "ISO", "GZip", "BZip2", "LZip" });
            comboBox1.Location = new Point(15, 70);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(140, 31);
            comboBox1.TabIndex = 11;
            comboBox1.Text = "Select type";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Khaki;
            panel1.Controls.Add(button5);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(textBox2);
            panel1.Location = new Point(782, 132);
            panel1.Name = "panel1";
            panel1.Size = new Size(288, 460);
            panel1.TabIndex = 12;
            // 
            // compress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1082, 623);
            Controls.Add(textBox4);
            Controls.Add(list_of_item_selected);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "compress";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "COMPRESS";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private ListBox list_of_item_selected;
        private Button button2;
        private Button button3;
        private Button button4;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button5;
        private ComboBox comboBox1;
        private Panel panel1;
    }
}
