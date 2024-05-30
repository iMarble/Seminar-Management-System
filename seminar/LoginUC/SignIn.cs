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
    public partial class SignIn : UserControl
    {
        private GeneralAccess GeneralAccess;
        private Panel _p;

        public SignIn(Panel p)
        {
            InitializeComponent();
            _p = p;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var (isAuthenticated, userType, userId) = GeneralAccess.AuthenticateUser(textBox1.Text, textBox2.Text);
            if (isAuthenticated)
            {
                Dashboard dashboard = new Dashboard((isAuthenticated, userType, userId));
                dashboard.ShowDialog();
            }
            else
            {
                MessageBox.Show("Incorrect User/Password!");
            }

        }

        static bool CheckIfTextBoxesAreEmpty(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            return true;
        }

        private void Text_Updated(object sender, EventArgs e)
        {
            if (CheckIfTextBoxesAreEmpty(textBox1.Text, textBox2.Text))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            this.textBox1.TextChanged += Text_Updated;
            textBox2.TextChanged += Text_Updated;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp(_p);
            AddControlsToPanel(_p, signUp);
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }
    }
}
