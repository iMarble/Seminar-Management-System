using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class feedback : UserControl
    {
        private readonly int UserId;
        private readonly string userType;
        private readonly int SeminarId;

        private GeneralAccess GeneralAccess;
        private DataAccessAdmin AdminAccess;
        private List<object> RequestsData;

        public feedback(int UserId, string UserType, [Optional] int seminarId)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.userType = UserType;
            SeminarId = seminarId;

            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            AdminAccess = new DataAccessAdmin(Settings.Default.connection);
            this.Load += ViewRequests_Load;
        }

        private void ViewRequests_Load(object sender, EventArgs e)
        {
            switch (userType)
            {
                case "Admin":
                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks());
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    break;
                case "Speaker":
                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks(isUser: true, userId: UserId));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    break;
                case "Attendee":
                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks(isUser: true, userId: UserId));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            switch (userType)
            {
                case "Admin":

                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks(filter: true, keyword: textBox1.Text));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch { }
                    break;
                case "Speaker":
                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks(isUser: true, userId: UserId, filter: true, keyword: textBox1.Text));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch { }
                    break;
                case "Attendee":
                    RequestsData = new Datasources().UserFeedbacksDataSource(GeneralAccess.GetUserFeedbacks(isUser: true, userId: UserId, filter: true, keyword: textBox1.Text));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch { }
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }
    }
}
