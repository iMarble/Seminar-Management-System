using seminar.Properties;
using seminar.Utilities;
using System;
using System.Windows.Forms;

namespace seminar
{
    public partial class EditUser : Form
    {
        private int _userid;
        private GeneralAccess generalAccess;

        public EditUser(int userId)
        {
            InitializeComponent();
            generalAccess = new GeneralAccess(Settings.Default.connection);
            this._userid = userId;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox4.Text))
            {
                if (generalAccess.EditUserProfile(_userid, textBox1.Text, textBox2.Text, textBox4.Text, Convert.ToInt64(textBox3.Text)))
                {
                    MessageBox.Show("Profile Updated!");
                }
                else
                {
                    MessageBox.Show("Failed to Edit the Profile!");
                }
            }
            if (!string.IsNullOrEmpty(textBox6.Text))
            {
                if (generalAccess.EditUserPassword(_userid, textBox6.Text))
                {
                    MessageBox.Show("Password Updated!");
                }
                else
                {
                    MessageBox.Show("Failed to Update password");
                }
            }
            else
            {
                MessageBox.Show("Please provide all required values");
            }
        }

        private void AddSeminar_Load(object sender, EventArgs e)
        {
            UserLogin userdetails = generalAccess.GetUserProfile(_userid);
            textBox2.Text = userdetails.Luser.FirstName;
            textBox1.Text = userdetails.Luser.LastName;
            textBox4.Text = userdetails.Luser.Email;
            textBox3.Text = userdetails.Luser.ContactNo.ToString();
        }

        private void clear()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
