using seminar.Properties;
using seminar.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace seminar.UserControls
{
    public partial class viewSeminars : UserControl
    {
        private readonly int UserId;
        private readonly string userType;
        private readonly bool attended;
        private readonly bool assigned;
        private GeneralAccess GeneralAccess;
        private DataAccessAdmin DataAccessAdmin;
        private List<object> SeminarsData;
        private DataGridViewButtonColumn Attendees;
        private DataGridViewButtonColumn Edit;
        private DataGridViewButtonColumn Delete;
        private DataGridViewButtonColumn Accept;
        private DataGridViewButtonColumn Reject;


        public viewSeminars(int UserId, string UserType, bool attended = false, bool assigned = false)
        {
            InitializeComponent();
            this.UserId = UserId;
            this.userType = UserType;
            this.attended = attended;
            this.assigned = assigned;
            GeneralAccess = new GeneralAccess(Settings.Default.connection);
            DataAccessAdmin = new DataAccessAdmin(Settings.Default.connection);
            this.Load += ViewSeminars_Load;
        }

        private void ViewSeminars_Load(object sender, EventArgs e)
        {
            switch (userType)
            {
                case "Admin":
                    SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails());
                    dataGridView1.DataSource = SeminarsData;
                    dataGridView1.ForeColor = Color.Black;

                    dataGridView1.Columns["SeminarId"].Visible = false;
                    AddAdminButtons();
                    break;
                case "Speaker":
                    if (assigned)
                    {
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(speaker_id: UserId, isAssigned: true));
                        dataGridView1.DataSource = SeminarsData;
                        dataGridView1.ForeColor = Color.Black;
                        AssignedButtons();
                    }
                    else
                    {
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(speaker_id: UserId, attendee: true));
                        dataGridView1.DataSource = SeminarsData;
                        dataGridView1.ForeColor = Color.Black;
                        AddSpeakerButtons();
                    }



                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }

                    break;
                case "Attendee":
                    if (!attended)
                    {
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(attendee: true));
                    }
                    else
                    {
                        textBox1.Visible = false;
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAttendedSeminarDetails(user_id: UserId));
                    }

                    dataGridView1.DataSource = SeminarsData;
                    dataGridView1.ForeColor = Color.Black;

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }

                    AddAttendeeButtons();
                    break;
                default:
                    MessageBox.Show("test");
                    break;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            update_grid();
        }

        private void update_grid()
        {
            dataGridView1.Columns.Clear();

            List<object> seminarsDataSource = new List<object>();
            switch (userType)
            {
                case "Admin":
                    seminarsDataSource = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(textBox1.Text));
                    dataGridView1.DataSource = seminarsDataSource;

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                        AddAdminButtons();
                    }

                    break;
                case "Speaker":
                    if (!assigned)
                    {
                        seminarsDataSource = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(textBox1.Text, speaker_id: UserId));
                        dataGridView1.DataSource = seminarsDataSource;
                        AddSpeakerButtons();
                    }
                    else
                    {
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(speaker_id: UserId, isAssigned: true));
                        dataGridView1.DataSource = SeminarsData;
                        dataGridView1.ForeColor = Color.Black;

                        AssignedButtons();
                    }

                    if (dataGridView1.Columns.Contains("SeminarId"))
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }

                    break;
                case "Attendee":
                    if (!attended)
                    {
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAllSeminarDetails(attendee: true));
                    }
                    else
                    {
                        textBox1.Visible = false;
                        SeminarsData = new Datasources().SeminarsDataSource(GeneralAccess.GetAttendedSeminarDetails(user_id: UserId));
                    }

                    dataGridView1.DataSource = SeminarsData;
                    dataGridView1.ForeColor = Color.Black;

                    try
                    {
                        dataGridView1.Columns["SeminarId"].Visible = false;
                    }
                    catch { }
                    AddAttendeeButtons();
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
                    if (e.ColumnIndex == dataGridView1.Columns["Edit"]?.Index)
                    {
                        Form formBackground = new Form();
                        try
                        {
                            using (EditSeminar es = new EditSeminar(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value), userType))
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
                                es.Owner = formBackground;
                                es.ShowDialog();
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
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this seminar", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            DataAccessAdmin.DeleteSeminar(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value));
                        }
                        update_grid();
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Feedback"]?.Index)
                    {
                        Form formBackground = new Form();
                        try
                        {
                            using (AddFeedback af = new AddFeedback(UserId, userType, dataGridView1.Rows[e.RowIndex].Cells["SeminarName"].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells["SpeakerName"].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells["Topic"].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells["Date"].Value.ToString(), Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
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
                                af.Owner = formBackground;
                                af.ShowDialog();
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
                    else if (e.ColumnIndex == dataGridView1.Columns["Attend"]?.Index)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to attend this seminar", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            if (!GeneralAccess.checkAttendance(UserId, Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
                            {
                                if (GeneralAccess.AttendSeminar(UserId, Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value), "Registered"))
                                {
                                    MessageBox.Show("Successfully Registered for the seminar");
                                }
                                else
                                {
                                    MessageBox.Show("An Error occured");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are already registered for this seminar, check status in attendance tab!");
                            }
                        }
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Attendance"]?.Index)
                    {
                        Form formBackground = new Form();
                        try
                        {
                            viewAttendance viewAttendance = new viewAttendance(UserId, userType, Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value));
                            using (popup uu = new popup(viewAttendance))
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
                                uu.Owner = formBackground;
                                uu.ShowDialog();
                                formBackground.Dispose();
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
                    else if (e.ColumnIndex == dataGridView1.Columns["Accept"]?.Index)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to Accept this seminar", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {

                            if (GeneralAccess.AcceptAssign(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
                            {
                                MessageBox.Show("Seminar Scheduled!");
                            }
                            else
                            {
                                MessageBox.Show("An Error occured");
                            }
                        }
                        update_grid();
                    }
                    else if (e.ColumnIndex == dataGridView1.Columns["Reject"]?.Index)
                    {
                        Form formBackground = new Form();
                        try
                        {
                            using (RejectSeminar uu = new RejectSeminar(UserId, Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["SeminarId"].Value)))
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
                                uu.Owner = formBackground;
                                uu.ShowDialog();
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
                }
                catch { }
            }
        }

        private void AddAdminButtons()
        {
            Attendees = new DataGridViewButtonColumn();
            Edit = new DataGridViewButtonColumn();
            Delete = new DataGridViewButtonColumn();

            //Attendees
            Attendees.HeaderText = "Attendance";
            Attendees.MinimumWidth = 6;
            Attendees.Name = "Attendance";
            Attendees.Text = "Attendance";
            Attendees.UseColumnTextForButtonValue = true;

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

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Attendees, Edit, Delete });
        }

        private void AddSpeakerButtons()
        {
            Attendees = new DataGridViewButtonColumn();
            Edit = new DataGridViewButtonColumn();

            //Attendees
            Attendees.HeaderText = "Attendance";
            Attendees.MinimumWidth = 6;
            Attendees.Name = "Attendance";
            Attendees.Text = "Attendance";
            Attendees.UseColumnTextForButtonValue = true;

            //Edit
            Edit.HeaderText = "Edit";
            Edit.MinimumWidth = 6;
            Edit.Name = "Edit";
            Edit.Text = "Edit";
            Edit.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Attendees, Edit });

        }

        private void AddAttendeeButtons()
        {
            Attendees = new DataGridViewButtonColumn();

            //Attendees
            if (!attended)
            {
                Attendees.HeaderText = "Attend";
                Attendees.MinimumWidth = 6;
                Attendees.Name = "Attend";
                Attendees.Text = "Attend";
                Attendees.UseColumnTextForButtonValue = true;
            }
            else
            {
                Attendees.HeaderText = "Feedback";
                Attendees.MinimumWidth = 6;
                Attendees.Name = "Feedback";
                Attendees.Text = "Feedback";
                Attendees.UseColumnTextForButtonValue = true;
            }

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Attendees });
        }

        private void AssignedButtons()
        {
            Accept = new DataGridViewButtonColumn();
            Reject = new DataGridViewButtonColumn();


            Accept.HeaderText = "Accept";
            Accept.MinimumWidth = 6;
            Accept.Name = "Accept";
            Accept.Text = "Accept";
            Accept.UseColumnTextForButtonValue = true;

            Reject.HeaderText = "Reject";
            Reject.MinimumWidth = 6;
            Reject.Name = "Reject";
            Reject.Text = "Reject";
            Reject.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Accept, Reject });
        }
    }
}
