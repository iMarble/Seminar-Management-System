using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace seminar
{
    public partial class AddSeminar : Form
    {
        private string _userType;
        private DataAccessAdmin DataAccessAdmin;
        private List<Topic> topics;
        private List<User> speakers;

        public AddSeminar(string userType)
        {
            InitializeComponent();
            DataAccessAdmin = new DataAccessAdmin(Settings.Default.connection);
            _userType = userType;
            topics = DataAccessAdmin.GetAllTopics();
            speakers = DataAccessAdmin.GetAllSpeakers();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show((comboBox1.SelectedItem as dynamic).Value.ToString());
                if (DataAccessAdmin.AddSeminar(textBox2.Text, dateTimePicker1.Value, comboBox2.Text, Convert.ToInt32((comboBox1.SelectedItem as dynamic).Value), "Scheduled"))
                {
                    MessageBox.Show("Seminar Added!");
                }
                else
                {
                    MessageBox.Show("Failed to Edit the seminar!");
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
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";

            foreach (Topic topic in topics)
            {
                comboBox2.Items.Add(topic.TopicName);
            }

            foreach (User speaker in speakers)
            {
                comboBox1.Items.Add(new { Text = speaker.FirstName + " " + speaker.LastName, Value = speaker.UserId });
            }
        }

        private void clear()
        {
            textBox2.Clear();
            comboBox2.Text = "";
            comboBox1.Text = "";
        }


    }
}
