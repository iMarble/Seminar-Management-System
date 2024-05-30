using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class viewSpeakerRequests : UserControl
    {
        private readonly int UserId;
        private readonly string userType;
        private GeneralAccess GeneralAccess;
        private DataAccessAdmin AdminAccess;
        private List<object> RequestsData;
        private DataGridViewButtonColumn Approve;
        private DataGridViewButtonColumn Reject;

        public viewSpeakerRequests(int UserId, string UserType)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.userType = UserType;
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            AdminAccess = new DataAccessAdmin(Settings.Default.connection);
            this.Load += ViewRequests_Load;
        }

        private void ViewRequests_Load(object sender, EventArgs e)
        {
            switch (userType)
            {
                case "Admin":
                    RequestsData = new Datasources().UserRequestsDataSource(GeneralAccess.GetUserRequests());
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    dataGridView1.Columns["UserId"].Visible = false;
                    dataGridView1.Columns["ReqId"].Visible = false;
                    AddAdminButtons();
                    break;
                case "Speaker":
                    RequestsData = new Datasources().UserRequestsDataSource(GeneralAccess.GetUserRequests(isUser: true, userId: UserId));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    dataGridView1.Columns["UserId"].Visible = false;
                    dataGridView1.Columns["ReqId"].Visible = false;
                    break;
                case "Attendee":
                    textBox1.Visible = false;
                    label8.Text = "Request Status";
                    RequestsData = new Datasources().UserRequestsDataSource(GeneralAccess.GetUserRequests(isUser: true, userId: UserId));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["ReqId"].Visible = false;
                    }
                    catch { }
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }

        private void update_grid()
        {
            dataGridView1.Columns.Clear();

            switch (userType)
            {
                case "Admin":
                    RequestsData = new Datasources().UserRequestsDataSource(GeneralAccess.GetUserRequests());
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    dataGridView1.Columns["UserId"].Visible = false;
                    dataGridView1.Columns["ReqId"].Visible = false;
                    AddAdminButtons();
                    break;
                case "Speaker":
                    break;
                case "Attendee":
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }

        private void AddAdminButtons()
        {
            Approve = new DataGridViewButtonColumn();
            Reject = new DataGridViewButtonColumn();

            //Approve
            Approve.HeaderText = "Approve";
            Approve.MinimumWidth = 6;
            Approve.Name = "Approve";
            Approve.Text = "Approve";
            Approve.UseColumnTextForButtonValue = true;

            //Reject
            Reject.HeaderText = "Reject";
            Reject.MinimumWidth = 6;
            Reject.Name = "Reject";
            Reject.Text = "Reject";
            Reject.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Approve, Reject });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            switch (userType)
            {
                case "Admin":
                    RequestsData = new Datasources().UserRequestsDataSource(GeneralAccess.GetUserRequests(filter: true, keyword: textBox1.Text));
                    dataGridView1.DataSource = RequestsData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["ReqId"].Visible = false;

                    }
                    catch { }
                    AddAdminButtons();
                    break;
                case "Speaker":
                    break;
                case "Attendee":
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (e.ColumnIndex == dataGridView1.Columns["Approve"]?.Index)
                    {
                        if (AdminAccess.ApproveSpeakerRequest(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserId"].Value), Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ReqId"].Value)))
                        {
                            MessageBox.Show("Request Approved!");
                            update_grid();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Approve request");
                        }
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Reject"]?.Index)
                    {
                        if (AdminAccess.RejectSpeakerRequest(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ReqId"].Value)))
                        {
                            MessageBox.Show("Request Rejected!");
                            update_grid();
                        }
                        else
                        {
                            MessageBox.Show("Failed to reject request!");
                        }
                    }
                }
                catch { }
            }
        }
    }
}
