using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace seminar
{
    public partial class AssignSeminar : Form
    {
        private int _userid;
        private DataAccessAdmin DataAccessAdmin;
        private List<Topic> topics;

        public AssignSeminar(int userid)
        {
            InitializeComponent();
            DataAccessAdmin = new DataAccessAdmin(Settings.Default.connection);
            _userid = userid;
            topics = DataAccessAdmin.GetAllTopics();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(comboBox2.Text))
            {
                if (DataAccessAdmin.AddSeminar(textBox2.Text, dateTimePicker1.Value, comboBox2.Text, _userid, "Assigned"))
                {
                    MessageBox.Show("Seminar Assign!");
                }
                else
                {
                    MessageBox.Show("Failed to assign the seminar!");
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
