using seminar.UserControls;
using seminar.UserControls.sidepanels;
using System;
using System.Windows.Forms;

namespace seminar
{
    public partial class Dashboard : Form
    {
        private (bool isAuthenticated, string userType, int userId) userData;

        public Dashboard((bool isAuthenticated, string userType, int userId) authenticatedUser)
        {
            InitializeComponent();
            this.userData = authenticatedUser;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            spacingpanel.Visible = false;
            Welcome welcome = new Welcome();
            switch (userData.userType)
            {
                case "Admin":
                    AddControlsToPanel(panelControls, welcome);
                    adminSidePanel adminSidePanel = new adminSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panel1, adminSidePanel);
                    break;
                case "Speaker":
                    speakerSidePanel speakerSidePanel = new speakerSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panelControls, welcome);
                    AddControlsToPanel(panel1, speakerSidePanel);
                    break;
                case "Attendee":
                    attendeeSidePanel attendeeSidePanel = new attendeeSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panelControls, welcome);
                    AddControlsToPanel(panel1, attendeeSidePanel);
                    break;
            }
        }

        private void AddControlsToPanel(Panel p, Control c)
        {
            c.Dock = DockStyle.Fill;
            p.Controls.Clear();
            p.Controls.Add(c);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Gotodashboard_Click(object sender, EventArgs e)
        {
            spacingpanel.Visible = true;
            switch (userData.userType)
            {
                case "Admin":
                    adminHome adminHome = new adminHome();
                    AddControlsToPanel(panelControls, adminHome);
                    adminSidePanel adminSidePanel = new adminSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panel1, adminSidePanel);
                    break;
                case "Speaker":
                    speakerHome speaker = new speakerHome();
                    speakerSidePanel speakerSidePanel = new speakerSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panelControls, speaker);
                    AddControlsToPanel(panel1, speakerSidePanel);
                    break;
                case "Attendee":
                    attendeeHome attendeeHome = new attendeeHome();
                    attendeeSidePanel attendeeSidePanel = new attendeeSidePanel(panelControls, spacingpanel, userData.userId, userData.userType);
                    AddControlsToPanel(panelControls, attendeeHome);
                    AddControlsToPanel(panel1, attendeeSidePanel);
                    break;
            }
        }
    }
}
