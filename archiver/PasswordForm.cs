using System;
using System.Windows.Forms;

namespace archiver
{
    public partial class PasswordForm : Form
    {
        private TextBox txtPassword;
        private Button btnOk;
        private Button btnCancel;
        private Label lblMessage;
        private CheckBox chkShowPassword;

        /// <summary>
        /// Gets the entered password
        /// </summary>
        public string Password => txtPassword.Text;

        public PasswordForm(string archiveName = "")
        {
            InitializePasswordForm(archiveName);
        }

        private void InitializePasswordForm(string archiveName)
        {
            this.Text = "Password Required";
            this.Size = new Size(350, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = SystemColors.ActiveCaption;

            // Label
            lblMessage = new Label();
            lblMessage.Text = string.IsNullOrEmpty(archiveName) 
                ? "This archive is password protected.\nPlease enter the password:" 
                : $"'{archiveName}' is password protected.\nPlease enter the password:";
            lblMessage.Location = new Point(15, 15);
            lblMessage.Size = new Size(310, 40);
            lblMessage.Font = new Font("Poppins", 9F);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new Point(15, 60);
            txtPassword.Size = new Size(305, 26);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Font = new Font("Poppins", 9.75F);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            // Show password checkbox
            chkShowPassword = new CheckBox();
            chkShowPassword.Text = "Show password";
            chkShowPassword.Location = new Point(15, 90);
            chkShowPassword.Size = new Size(150, 23);
            chkShowPassword.Font = new Font("Poppins", 8F);
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            // OK Button
            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new Point(155, 115);
            btnOk.Size = new Size(75, 28);
            btnOk.BackColor = Color.Silver;
            btnOk.FlatStyle = FlatStyle.Popup;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Click += BtnOk_Click;

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(240, 115);
            btnCancel.Size = new Size(75, 28);
            btnCancel.BackColor = Color.Silver;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblMessage);
            this.Controls.Add(txtPassword);
            this.Controls.Add(chkShowPassword);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;

            // Focus the password textbox when form loads
            this.Load += (s, e) => txtPassword.Focus();
        }

        private void ChkShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Password Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                txtPassword.Focus();
            }
        }
    }
}
