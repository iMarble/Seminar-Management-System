using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace seminar
{
    public partial class EditSeminar : Form
    {
        private int _seminarid;
        private string _userType;
        private DataAccessAdmin DataAccessAdmin;
        private List<Topic> topics;

        public EditSeminar(int seminarid, string userType)
        {
            InitializeComponent();
            DataAccessAdmin = new DataAccessAdmin(Settings.Default.connection);
            _seminarid = seminarid;
            _userType = userType;
            topics = DataAccessAdmin.GetAllTopics();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrEmpty(comboBox3.Text))
            {
                if (DataAccessAdmin.EditSeminar(_seminarid, textBox2.Text, dateTimePicker1.Value, comboBox2.Text, comboBox3.Text))
                {
                    MessageBox.Show("Seminar Edited!");
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

        private void EditSeminar_Load(object sender, EventArgs e)
        {
            foreach (Topic topic in topics)
            {
                comboBox2.Items.Add(topic.TopicName);
            }
        }

        private void clear()
        {
            textBox2.Clear();
            comboBox2.Text = "";
            comboBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
