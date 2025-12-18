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
            back_btn = new Button();
            extractName = new TextBox();
            selectSave = new TextBox();
            brws_btn = new Button();
            extract_btn = new Button();
            zip_items = new ListBox();
            insert = new TextBox();
            add_btn = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // back_btn
            // 
            back_btn.BackColor = Color.Silver;
            back_btn.Cursor = Cursors.Hand;
            back_btn.FlatStyle = FlatStyle.Popup;
            back_btn.Location = new Point(203, 420);
            back_btn.Name = "back_btn";
            back_btn.Size = new Size(75, 23);
            back_btn.TabIndex = 18;
            back_btn.Text = "Back";
            back_btn.UseVisualStyleBackColor = false;
            back_btn.Click += button5_Click;
            // 
            // extractName
            // 
            extractName.BackColor = SystemColors.Control;
            extractName.BorderStyle = BorderStyle.None;
            extractName.Font = new Font("Poppins", 9.75F);
            extractName.Location = new Point(19, 14);
            extractName.Multiline = true;
            extractName.Name = "extractName";
            extractName.PlaceholderText = "Extract name";
            extractName.Size = new Size(184, 26);
            extractName.TabIndex = 17;
            // 
            // selectSave
            // 
            selectSave.BorderStyle = BorderStyle.None;
            selectSave.Font = new Font("Poppins", 9.75F);
            selectSave.Location = new Point(19, 58);
            selectSave.Multiline = true;
            selectSave.Name = "selectSave";
            selectSave.PlaceholderText = "Select where to save";
            selectSave.ReadOnly = true;
            selectSave.Size = new Size(184, 26);
            selectSave.TabIndex = 16;
            // 
            // brws_btn
            // 
            brws_btn.BackColor = Color.Silver;
            brws_btn.Cursor = Cursors.Hand;
            brws_btn.FlatStyle = FlatStyle.Popup;
            brws_btn.Font = new Font("Poppins", 9.75F);
            brws_btn.Location = new Point(210, 54);
            brws_btn.Name = "brws_btn";
            brws_btn.Size = new Size(68, 33);
            brws_btn.TabIndex = 15;
            brws_btn.Text = "Browse";
            brws_btn.UseVisualStyleBackColor = false;
            // 
            // extract_btn
            // 
            extract_btn.BackColor = Color.Silver;
            extract_btn.Cursor = Cursors.Hand;
            extract_btn.FlatStyle = FlatStyle.Popup;
            extract_btn.Font = new Font("Poppins", 9.75F);
            extract_btn.Location = new Point(19, 103);
            extract_btn.Name = "extract_btn";
            extract_btn.Size = new Size(140, 33);
            extract_btn.TabIndex = 14;
            extract_btn.Text = "Extract";
            extract_btn.UseVisualStyleBackColor = false;
            // 
            // zip_items
            // 
            zip_items.BackColor = SystemColors.Control;
            zip_items.BorderStyle = BorderStyle.None;
            zip_items.Font = new Font("Poppins", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            zip_items.FormattingEnabled = true;
            zip_items.ItemHeight = 23;
            zip_items.Location = new Point(29, 132);
            zip_items.Name = "zip_items";
            zip_items.Size = new Size(740, 460);
            zip_items.TabIndex = 13;
            // 
            // insert
            // 
            insert.BorderStyle = BorderStyle.None;
            insert.Font = new Font("Poppins", 9.75F);
            insert.Location = new Point(200, 32);
            insert.Multiline = true;
            insert.Name = "insert";
            insert.PlaceholderText = "Insert file to extarct";
            insert.ReadOnly = true;
            insert.Size = new Size(368, 48);
            insert.TabIndex = 12;
            insert.TextAlign = HorizontalAlignment.Center;
            // 
            // add_btn
            // 
            add_btn.BackColor = Color.Silver;
            add_btn.BackgroundImageLayout = ImageLayout.None;
            add_btn.Cursor = Cursors.Hand;
            add_btn.FlatStyle = FlatStyle.Popup;
            add_btn.Font = new Font("Poppins", 9.75F);
            add_btn.Location = new Point(585, 41);
            add_btn.Name = "add_btn";
            add_btn.Size = new Size(140, 33);
            add_btn.TabIndex = 11;
            add_btn.Text = "Add";
            add_btn.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Khaki;
            panel1.Controls.Add(back_btn);
            panel1.Controls.Add(extract_btn);
            panel1.Controls.Add(extractName);
            panel1.Controls.Add(brws_btn);
            panel1.Controls.Add(selectSave);
            panel1.Location = new Point(775, 132);
            panel1.Name = "panel1";
            panel1.Size = new Size(295, 460);
            panel1.TabIndex = 19;
            // 
            // extract
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1082, 623);
            Controls.Add(zip_items);
            Controls.Add(insert);
            Controls.Add(add_btn);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "extract";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EXTRACT";
            Load += Form3_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button back_btn;
        private TextBox extractName;
        private TextBox selectSave;
        private Button brws_btn;
        private Button extract_btn;
        private ListBox zip_items;
        private TextBox insert;
        private Button add_btn;
        private Panel panel1;
    }
}