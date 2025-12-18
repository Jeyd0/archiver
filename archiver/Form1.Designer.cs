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
            add_btn = new Button();
            insertfile = new TextBox();
            list_of_item_selected = new ListBox();
            clear_btn = new Button();
            compress_btn = new Button();
            brws_btn = new Button();
            selectSave = new TextBox();
            zipname = new TextBox();
            search = new TextBox();
            back_btn = new Button();
            type = new ComboBox();
            panel1 = new Panel();
            showpass = new CheckBox();
            password = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // add_btn
            // 
            add_btn.BackColor = Color.Silver;
            add_btn.BackgroundImageLayout = ImageLayout.None;
            add_btn.Cursor = Cursors.Hand;
            add_btn.FlatStyle = FlatStyle.Popup;
            add_btn.Font = new Font("Poppins", 9.75F);
            add_btn.Location = new Point(559, 22);
            add_btn.Name = "add_btn";
            add_btn.Size = new Size(140, 33);
            add_btn.TabIndex = 0;
            add_btn.Text = "Add";
            add_btn.UseVisualStyleBackColor = false;
            add_btn.Click += button1_Click_1;
            // 
            // insertfile
            // 
            insertfile.BorderStyle = BorderStyle.None;
            insertfile.Font = new Font("Poppins", 9.75F);
            insertfile.Location = new Point(185, 15);
            insertfile.Multiline = true;
            insertfile.Name = "insertfile";
            insertfile.PlaceholderText = "Insert file(jpg,png,pdf,docx,excel)";
            insertfile.ReadOnly = true;
            insertfile.Size = new Size(368, 48);
            insertfile.TabIndex = 1;
            insertfile.TextAlign = HorizontalAlignment.Center;
            insertfile.TextChanged += textBox1_TextChanged;
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
            // clear_btn
            // 
            clear_btn.BackColor = Color.Silver;
            clear_btn.Cursor = Cursors.Hand;
            clear_btn.FlatStyle = FlatStyle.Popup;
            clear_btn.Font = new Font("Poppins", 9.75F);
            clear_btn.Location = new Point(15, 22);
            clear_btn.Name = "clear_btn";
            clear_btn.Size = new Size(140, 33);
            clear_btn.TabIndex = 3;
            clear_btn.Text = "Clear all";
            clear_btn.UseVisualStyleBackColor = false;
            // 
            // compress_btn
            // 
            compress_btn.BackColor = Color.Silver;
            compress_btn.Cursor = Cursors.Hand;
            compress_btn.FlatStyle = FlatStyle.Popup;
            compress_btn.Font = new Font("Poppins", 9.75F);
            compress_btn.Location = new Point(15, 332);
            compress_btn.Name = "compress_btn";
            compress_btn.Size = new Size(140, 33);
            compress_btn.TabIndex = 5;
            compress_btn.Text = "Compress";
            compress_btn.UseVisualStyleBackColor = false;
            compress_btn.Click += compress_btn_Click;
            // 
            // brws_btn
            // 
            brws_btn.BackColor = Color.Silver;
            brws_btn.Cursor = Cursors.Hand;
            brws_btn.FlatStyle = FlatStyle.Popup;
            brws_btn.Font = new Font("Poppins", 9.75F);
            brws_btn.Location = new Point(206, 278);
            brws_btn.Name = "brws_btn";
            brws_btn.Size = new Size(68, 33);
            brws_btn.TabIndex = 6;
            brws_btn.Text = "Browse";
            brws_btn.UseVisualStyleBackColor = false;
            brws_btn.Click += button4_Click;
            // 
            // selectSave
            // 
            selectSave.BorderStyle = BorderStyle.None;
            selectSave.Font = new Font("Poppins", 9.75F);
            selectSave.Location = new Point(15, 280);
            selectSave.Multiline = true;
            selectSave.Name = "selectSave";
            selectSave.PlaceholderText = "Select where to save";
            selectSave.ReadOnly = true;
            selectSave.Size = new Size(184, 26);
            selectSave.TabIndex = 7;
            // 
            // zipname
            // 
            zipname.BackColor = SystemColors.Control;
            zipname.BorderStyle = BorderStyle.None;
            zipname.Font = new Font("Poppins", 9.75F);
            zipname.Location = new Point(15, 220);
            zipname.Multiline = true;
            zipname.Name = "zipname";
            zipname.PlaceholderText = "Zip Name";
            zipname.Size = new Size(184, 26);
            zipname.TabIndex = 8;
            zipname.TextChanged += zipname_TextChanged;
            // 
            // search
            // 
            search.BackColor = SystemColors.Control;
            search.BorderStyle = BorderStyle.None;
            search.Font = new Font("Poppins", 9.75F);
            search.Location = new Point(208, 94);
            search.Multiline = true;
            search.Name = "search";
            search.PlaceholderText = "Search file";
            search.Size = new Size(318, 32);
            search.TabIndex = 9;
            search.TextAlign = HorizontalAlignment.Center;
            // 
            // back_btn
            // 
            back_btn.BackColor = Color.Silver;
            back_btn.Cursor = Cursors.Hand;
            back_btn.FlatStyle = FlatStyle.Popup;
            back_btn.Location = new Point(206, 424);
            back_btn.Name = "back_btn";
            back_btn.Size = new Size(75, 23);
            back_btn.TabIndex = 10;
            back_btn.Text = "Back";
            back_btn.UseVisualStyleBackColor = false;
            back_btn.Click += button5_Click;
            // 
            // type
            // 
            type.BackColor = SystemColors.Control;
            type.FlatStyle = FlatStyle.Popup;
            type.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            type.FormattingEnabled = true;
            type.Items.AddRange(new object[] { "Select type", "Zip", "Tar", "ISO", "GZip", "BZip2", "LZip" });
            type.Location = new Point(15, 80);
            type.Name = "type";
            type.Size = new Size(140, 31);
            type.TabIndex = 11;
            type.Text = "Select type";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Khaki;
            panel1.Controls.Add(showpass);
            panel1.Controls.Add(password);
            panel1.Controls.Add(back_btn);
            panel1.Controls.Add(type);
            panel1.Controls.Add(clear_btn);
            panel1.Controls.Add(compress_btn);
            panel1.Controls.Add(zipname);
            panel1.Controls.Add(brws_btn);
            panel1.Controls.Add(selectSave);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(782, 132);
            panel1.Name = "panel1";
            panel1.Size = new Size(288, 460);
            panel1.TabIndex = 12;
            panel1.Paint += panel1_Paint;
            // 
            // showpass
            // 
            showpass.AutoSize = true;
            showpass.BackColor = Color.Transparent;
            showpass.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            showpass.Location = new Point(15, 166);
            showpass.Name = "showpass";
            showpass.Size = new Size(123, 26);
            showpass.TabIndex = 17;
            showpass.Text = "Show password";
            showpass.UseVisualStyleBackColor = false;
            showpass.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // password
            // 
            password.BackColor = SystemColors.Control;
            password.BorderStyle = BorderStyle.None;
            password.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            password.Location = new Point(15, 142);
            password.Multiline = true;
            password.Name = "password";
            password.PlaceholderText = "Optional";
            password.Size = new Size(184, 26);
            password.TabIndex = 12;
            password.UseSystemPasswordChar = true;
            password.TextChanged += password_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.FlatStyle = FlatStyle.Popup;
            label4.Font = new Font("Poppins", 9.75F);
            label4.Location = new Point(11, 261);
            label4.Name = "label4";
            label4.Size = new Size(42, 23);
            label4.TabIndex = 16;
            label4.Text = "Save";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(9, 59);
            label3.Name = "label3";
            label3.Size = new Size(41, 23);
            label3.TabIndex = 15;
            label3.Text = "Type";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Poppins", 9.75F);
            label2.Location = new Point(12, 201);
            label2.Name = "label2";
            label2.Size = new Size(49, 23);
            label2.TabIndex = 14;
            label2.Text = "Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Poppins", 9.75F);
            label1.Location = new Point(11, 122);
            label1.Name = "label1";
            label1.Size = new Size(74, 23);
            label1.TabIndex = 13;
            label1.Text = "Password";
            // 
            // compress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1082, 623);
            Controls.Add(search);
            Controls.Add(list_of_item_selected);
            Controls.Add(insertfile);
            Controls.Add(add_btn);
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

        private Button add_btn;
        private TextBox insertfile;
        private ListBox list_of_item_selected;
        private Button clear_btn;
        private Button compress_btn;
        private Button brws_btn;
        private TextBox selectSave;
        private TextBox zipname;
        private TextBox search;
        private Button back_btn;
        private ComboBox type;
        private Panel panel1;
        private TextBox password;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private CheckBox showpass;
    }
}
