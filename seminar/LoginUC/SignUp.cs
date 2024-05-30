using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar.LoginUC
{
    public partial class SignUp : UserControl
    {
        private Panel Panel;
        private GeneralAccess GeneralAccess;

        public SignUp(Panel panel)
        {
            InitializeComponent();
            Panel = panel;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn(Panel);
            AddControlsToPanel(Panel, signIn);
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            this.button1.Enabled = false;
            this.textBox1.TextChanged += Text_Updated;
            textBox2.TextChanged += Text_Updated;
            textBox3.TextChanged += Text_Updated;
            textBox4.TextChanged += Text_Updated;
            textBox5.TextChanged += Text_Updated;
            textBox6.TextChanged += Text_Updated;
        }

        static bool CheckIfTextBoxesAreEmpty(string fname, string lname, string contact, string email, string username, string password)
        {
            if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            return true;
        }

        private void Text_Updated(object sender, EventArgs e)
        {
            if (CheckIfTextBoxesAreEmpty(textBox1.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox2.Text))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GeneralAccess.RegisterNewUser(textBox1.Text, textBox3.Text, textBox5.Text, Convert.ToInt64(textBox4.Text), "Attendee", textBox6.Text, textBox2.Text))
            {
                MessageBox.Show("Account Created!");
            }
            else
            {
                MessageBox.Show("Account Creation failed!");
            }
        }
    }
}
