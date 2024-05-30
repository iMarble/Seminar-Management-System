using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class viewAttendees : UserControl
    {
        private readonly int UserId;
        private readonly string userType;
        private GeneralAccess GeneralAccess;
        private DataAccessAdmin AdminAccess;
        private List<User> AttendeesData;
        private DataGridViewButtonColumn Edit;
        private DataGridViewButtonColumn Delete;

        public viewAttendees(int UserId, string UserType)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.userType = UserType;
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            AdminAccess = new DataAccessAdmin(Settings.Default.connection);
            this.Load += ViewAttendees_Load;
        }

        private void ViewAttendees_Load(object sender, EventArgs e)
        {
            switch (userType)
            {
                case "Admin":
                    AttendeesData = AdminAccess.GetAllUsers(attendee: true);
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
                case "Speaker":
                    break;
                case "Attendee":
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
                    AttendeesData = AdminAccess.GetAllUsers(attendee: true);
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
            Edit = new DataGridViewButtonColumn();
            Delete = new DataGridViewButtonColumn();

            //Edit
            Edit.HeaderText = "Edit";
            Edit.MinimumWidth = 6;
            Edit.Name = "Edit";
            Edit.Text = "Edit";
            Edit.UseColumnTextForButtonValue = true;

            //Delete
            Delete.HeaderText = "Delete";
            Delete.MinimumWidth = 6;
            Delete.Name = "Delete";
            Delete.Text = "Delete";
            Delete.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Edit, Delete });
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (e.ColumnIndex == dataGridView1.Columns["Edit"]?.Index)
                    {
                        Form formBackground = new Form();
                        try
                        {
                            using (EditUser eu = new EditUser(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserId"].Value)))
                            {
                                formBackground.StartPosition = FormStartPosition.Manual;
                                formBackground.FormBorderStyle = FormBorderStyle.None;
                                formBackground.Opacity = .70d;
                                formBackground.BackColor = Color.Black;
                                formBackground.WindowState = FormWindowState.Maximized;
                                formBackground.TopMost = true;
                                formBackground.Location = this.Location;
                                formBackground.ShowInTaskbar = false;
                                formBackground.Show();
                                eu.Owner = formBackground;
                                eu.ShowDialog();
                                formBackground.Dispose();
                                update_grid();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            formBackground.Dispose();
                        }
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Delete"]?.Index)
                    {
                        if (AdminAccess.DeleteUser(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserId"].Value)))
                        {
                            MessageBox.Show("User Deleted");
                            update_grid();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Delete User");
                        }
                    }
                }
                catch { }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            switch (userType)
            {
                case "Admin":
                    AttendeesData = AdminAccess.GetAllUsers(attendee: true, keyword: textBox1.Text);
                    dataGridView1.DataSource = AttendeesData;
                    dataGridView1.ForeColor = Color.Black;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    try
                    {
                        dataGridView1.Columns["UserId"].Visible = false;
                        dataGridView1.Columns["SeminarId"].Visible = false;

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
    }
}
