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
            f3.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f1 = new compress();
            f1.Show();
            this.Hide();
        }

        private void home_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Archiver Application\n\nVersion 1.0\n\nA simple file compression and extraction tool.\n\nSupported formats:\n• Compress: ZIP\n• Extract: ZIP, 7z, RAR, TAR, GZip, BZ2, CAB, ISO, ARJ*, LZH*, UUE*, ACE*\n\n* Limited support\n\nCreated By: Jade Dajuela",
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
