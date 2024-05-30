using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace seminar
{
    public partial class RejectSeminar : Form
    {
        private int userId;
        private int seminarId;
        private GeneralAccess GeneralAccess;

        public RejectSeminar(int userId, int seminarId)
        {
            InitializeComponent();
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            this.userId = userId;
            this.seminarId = seminarId;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(feedbacktxt.Text))
            {
                if (GeneralAccess.RejectAssign(seminarId, userId, feedbacktxt.Text))
                {
                    MessageBox.Show("Request Rejected");
                }
                else
                {
                    MessageBox.Show("Failed to reject the request!");
                }
            }
            else
            {
                MessageBox.Show("Please provide all required values");
            }
            clear();
        }

        private void AddSeminar_Load(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            feedbacktxt.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
