using System;
using System.Windows.Forms;

namespace seminar.UserControls.sidepanels
{
    public partial class speakerSidePanel : UserControl
    {
        private readonly int UserId;
        private Panel panelControls;
        private Panel spacingPanel;

        public string UserType { get; private set; }

        public speakerSidePanel(Panel mainPanel, Panel spacingPanel, int UserId, string UserType)
        {
            InitializeComponent();
            panelControls = mainPanel;
            this.spacingPanel = spacingPanel;
            this.UserId = UserId;
            this.UserType = UserType;
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }

        private void My_profile_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = true;
            viewProfile viewProfile = new viewProfile(UserId);
            AddControlsToPanel(panelControls, viewProfile);
        }

        private void Seminars_Click(object sender, EventArgs e)
        {
            //spacingpanel.Visible = false;
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, UserType);
            AddControlsToPanel(panelControls, viewSeminars);
        }

        private void Attendees_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewAttendees viewAttendees = new viewAttendees(UserId, UserType);
            AddControlsToPanel(panelControls, viewAttendees);
        }

        private void Request_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSpeakerRequests viewRequests = new viewSpeakerRequests(UserId, UserType);
            AddControlsToPanel(panelControls, viewRequests);
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

        private void button1_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, "Attendee", attended: true);
            AddControlsToPanel(panelControls, viewSeminars);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, "Attendee");
            AddControlsToPanel(panelControls, viewSeminars);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSeminars viewSeminars = new viewSeminars(UserId, UserType, assigned: true);
            AddControlsToPanel(panelControls, viewSeminars);
        }
    }
}
