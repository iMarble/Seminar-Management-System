using seminar.Properties;
using seminar.Utilities;
using System;
using System.Windows.Forms;

namespace seminar.UserControls.sidepanels
{
    public partial class attendeeSidePanel : UserControl
    {
        private int UserId;
        private Panel panelControls;
        private Panel spacingPanel;

        public string UserType { get; private set; }

        public attendeeSidePanel(Panel mainPanel, Panel spacingPanel, int UserId, string userType)
        {
            InitializeComponent();
            panelControls = mainPanel;
            this.spacingPanel = spacingPanel;
            this.UserId = UserId;
            this.UserType = userType;
        }

        private void My_profile_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = true;
            viewProfile viewProfile = new viewProfile(UserId);
            AddControlsToPanel(panelControls, viewProfile);
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }

        private void Seminars_Click(object sender, EventArgs e)
        {
            //spacingpanel.Visible = false;
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, UserType);
            AddControlsToPanel(panelControls, viewSeminars);
        }

        private void FeedBack_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            feedback feedback = new feedback(UserId, UserType);
            AddControlsToPanel(panelControls, feedback);
        }

        private void Attandance_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewAttendance viewAttendance = new viewAttendance(UserId, UserType);
            AddControlsToPanel(panelControls, viewAttendance);
        }

        private void Request_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSpeakerRequests viewRequests = new viewSpeakerRequests(UserId, UserType);
            AddControlsToPanel(panelControls, viewRequests);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, UserType, attended:true);
            AddControlsToPanel(panelControls, viewSeminars);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to become speaker?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if (new DataAccessAdmin(Settings.Default.connection).AddRequest(UserId))
                {
                    MessageBox.Show("Request Submitted");
                }
                else
                {
                    MessageBox.Show("Could not submit request this time");
                }
            }
        }
    }
}
