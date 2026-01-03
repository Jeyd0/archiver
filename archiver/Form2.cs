using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace archiver
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form f3 = new extract();
            f3.FormClosed += (s, args) => this.Show();
            f3.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f1 = new compress();
            f1.FormClosed += (s, args) => this.Show();
            f1.Show();
            this.Hide();
        }

        private void home_Load(object sender, EventArgs e)
        {
            // Enable keyboard shortcuts
            this.KeyPreview = true;
            this.KeyDown += Home_KeyDown;
        }

        /// <summary>
        /// Handles keyboard shortcuts for the home form
        /// </summary>
        private void Home_KeyDown(object? sender, KeyEventArgs e)
        {
            // C key - Compress
            if (e.KeyCode == Keys.C && !e.Control && !e.Alt && !e.Shift)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button1_Click(this, EventArgs.Empty);
            }
            // E key - Extract
            else if (e.KeyCode == Keys.E && !e.Control && !e.Alt && !e.Shift)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button2_Click(this, EventArgs.Empty);
            }
            // F1 - About
            else if (e.KeyCode == Keys.F1)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button3_Click(this, EventArgs.Empty);
            }
            // Escape or Alt+F4 - Exit
            else if (e.KeyCode == Keys.Escape || (e.Alt && e.KeyCode == Keys.F4))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                button4_Click(this, EventArgs.Empty);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Archiver Application\n\nVersion 1.0\n\nA simple file compression and extraction tool.\n\nSupported formats:\n• Compress: ZIP, ISO, TAR, GZiP, LZiP, BZiP\n• Extract: ZIP, 7z, RAR, TAR, GZip, BZ2, CAB, ISO, ARJ*, LZH*, UUE*, ACE*\n\n* Limited support\n\nCreated By: Jeyd0",
                "About Archiver", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exit", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
