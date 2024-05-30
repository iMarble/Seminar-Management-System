using seminar.LoginUC;
using seminar.Properties;
using seminar.Utilities;
using System;
using System.Windows.Forms;

namespace seminar
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
            //button1.Enabled = false;
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn(panel2);
            AddControlsToPanel(panel2, signIn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
