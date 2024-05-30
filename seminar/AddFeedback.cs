using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace seminar
{
    public partial class AddFeedback : Form
    {
        private int userId;
        private string _userType;
        private int seminarId;
        private string seminarName;
        private string speakerName;
        private string topic;
        private string semdate;
        private DataAccessAdmin DataAccessAdmin;
        private List<Topic> topics;
        private List<User> speakers;

        public AddFeedback(int userId, string userType, string seminarName, string speakerName, string topic, string date, int seminarId)
        {
            InitializeComponent();
            DataAccessAdmin = new DataAccessAdmin(Settings.Default.connection);
            _userType = userType;
            this.seminarName = seminarName;
            this.speakerName = speakerName;
            this.topic = topic;
            semdate = date;
            topics = DataAccessAdmin.GetAllTopics();
            speakers = DataAccessAdmin.GetAllSpeakers();
            this.userId = userId;
            this.seminarId = seminarId;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(feedbacktxt.Text))
            {
                if (new GeneralAccess(Settings.Default.connection).AddFeedback(userId, seminarId, feedbacktxt.Text))
                {
                    MessageBox.Show("Feedback Added!");
                }
                else
                {
                    MessageBox.Show("Failed to Add the Feedback!");
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
            semName.Text = seminarName;
            spName.Text = speakerName;
            tpName.Text = topic;
            date.Text = semdate;
        }

        private void clear()
        {
            feedbacktxt.Clear();
        }
    }
}
