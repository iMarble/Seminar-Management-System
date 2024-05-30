using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class viewAttendance : UserControl
    {
        private readonly int UserId;
        private readonly int SeminarId;
        private readonly string userType;
        private GeneralAccess GeneralAccess;
        private DataAccessAdmin AdminAccess;
        private SpeakerDataAccess SpeakerDataAccess;
        private List<object> AttendeesData;
        private DataGridViewButtonColumn Present;
        private DataGridViewButtonColumn Absent;

        public viewAttendance(int UserId, string UserType, int seminarId = 0)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.userType = UserType;
            SeminarId = seminarId;
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            AdminAccess = new DataAccessAdmin(Settings.Default.connection);
            SpeakerDataAccess = new SpeakerDataAccess(Settings.Default.connection);
            this.Load += ViewAttendance_Load; ;
        }

        private void ViewAttendance_Load(object sender, EventArgs e)
        {
            switch (userType)
            {
                case "Admin":
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetSeminarAttendance(SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance());

                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch
                    {

                    }
                    AddAdminButtons();
                    break;
                case "Speaker":
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetSeminarAttendance(SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(SpeakerDataAccess.GetAllOwnSeminarAttendees(UserId));
                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch
                    {

                    }
                    AddAdminButtons();
                    break;
                case "Attendee":
                    AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(UserId));
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch
                    {

                    }
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }
        }

        private void AddAdminButtons()
        {
            Present = new DataGridViewButtonColumn();
            Absent = new DataGridViewButtonColumn();

            //Edit
            Present.HeaderText = "Present";
            Present.MinimumWidth = 6;
            Present.Name = "Present";
            Present.Text = "Present";
            Present.UseColumnTextForButtonValue = true;

            //Delete
            Absent.HeaderText = "Absent";
            Absent.MinimumWidth = 6;
            Absent.Name = "Absent";
            Absent.Text = "Absent";
            Absent.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Present, Absent });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            switch (userType)
            {
                case "Admin":
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(textBox1.Text, SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(textBox1.Text));

                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;

                    }
                    catch { }
                    if (dataGridView1.Rows.Count > 0)
                    {
                        AddAdminButtons();

                    }
                    break;
                case "Speaker":
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(textBox1.Text, SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(keyword: textBox1.Text, speakerId: UserId));

                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;

                    }
                    catch { }
                    if (dataGridView1.Rows.Count > 0)
                    {
                        AddAdminButtons();

                    }
                    break;
                case "Attendee":
                    AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance(UserId, keyword: textBox1.Text));

                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;

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
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetSeminarAttendance(SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetAttendance());

                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch
                    {

                    }
                    AddAdminButtons();
                    break;
                case "Speaker":
                    if (SeminarId != 0)
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(AdminAccess.GetSeminarAttendance(SeminarId));
                    }
                    else
                    {
                        AttendeesData = new Datasources().AttendanceDataSource(SpeakerDataAccess.GetAllOwnSeminarAttendees(UserId));
                    }
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    dataGridView1.Columns["UserId"].Visible = false;
                    try
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch
                    {

                    }
                    AddAdminButtons();
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
                    if (e.ColumnIndex == dataGridView1.Columns["Present"]?.Index)
                    {
                        if (AdminAccess.MarkPresent(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserId"].Value), Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
                        {
                            update_grid();
                        }
                        else
                        {
                        }
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Absent"]?.Index)
                    {
                        if (AdminAccess.MarkAbsent(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserId"].Value), Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
                        {
                            update_grid();
                        }
                        else
                        {
                        }
                    }
                }
                catch { }
            }
        }
    }
}
