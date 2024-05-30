using seminar.Properties;
using seminar.Utilities;
using System;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class viewProfile : UserControl
    {
        private int UserId;
        private GeneralAccess generalAccess;
        private UserLogin UserLogin;

        public viewProfile(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
            generalAccess = new GeneralAccess(Settings.Default.connection);

        }

        private void viewProfile_Load(object sender, EventArgs e)
        {
            UserLogin = generalAccess.GetUserProfile(UserId);
            Fnametxt.Text = UserLogin.Luser.FirstName;
            Lnametxt.Text = UserLogin.Luser.LastName;
            contcttxt.Text = UserLogin.Luser.ContactNo.ToString();
            emailtxt.Text = UserLogin.Luser.Email;
            unametxt.Text = UserLogin.LLogin.Username;
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            try
            {
                if (passtxt.Text != "")
                {
                    if (generalAccess.EditUserProfile(UserId, Fnametxt.Text, Lnametxt.Text, emailtxt.Text, Convert.ToInt64(contcttxt.Text)) && generalAccess.EditUserPassword(UserId, passtxt.Text))
                    {
                        MessageBox.Show("Updated Your Profile And Password");
                    }
                }
                else
                {
                    if (generalAccess.EditUserProfile(UserId, Fnametxt.Text, Lnametxt.Text, emailtxt.Text, Convert.ToInt64(contcttxt.Text)))
                    {
                        MessageBox.Show("Updated Your Profile");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            UserLogin = generalAccess.GetUserProfile(UserId);
            Fnametxt.Text = UserLogin.Luser.FirstName;
            Lnametxt.Text = UserLogin.Luser.LastName;
            contcttxt.Text = UserLogin.Luser.ContactNo.ToString();
            emailtxt.Text = UserLogin.Luser.Email;
            unametxt.Text = UserLogin.LLogin.Username;
        }
    }
}
