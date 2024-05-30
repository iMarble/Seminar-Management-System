using System;
using System.Windows.Forms;

namespace seminar.UserControls.sidepanels
{
    public partial class adminSidePanel : UserControl
    {
        private int UserId;
        private Panel panelControls;
        private Panel spacingPanel;

        public string UserType { get; private set; }

        /*private GeneralAccess generalAccess;
private DataAccessAdmin accessAdmin;
private SpeakerDataAccess speakerDataAccess;*/

        public adminSidePanel(Panel mainPanel, Panel spacingPanel, int UserId, string UserType)
        {
            InitializeComponent();
            panelControls = mainPanel;
            this.spacingPanel = spacingPanel;
            this.UserId = UserId;
            this.UserType = UserType;
            /*            generalAccess = new GeneralAccess(Settings.Default.connection);
                        accessAdmin = new DataAccessAdmin(Settings.Default.connection);
                        speakerDataAccess = new SpeakerDataAccess(Settings.Default.connection);*/
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

        private void Speaker_Click(object sender, EventArgs e)
        {
            spacingPanel.Visible = false;
            viewSpeakers viewSpeakers = new viewSpeakers(UserId, UserType);
            AddControlsToPanel(panelControls, viewSpeakers);
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
            using (AddSeminar ads = new AddSeminar(UserType))
            {
                ads.ShowDialog();
            }
        }
    }
}
