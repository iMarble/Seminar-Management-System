using System.Windows.Forms;

namespace seminar
{
    public partial class popup : Form
    {
        public popup(Control c)
        {
            InitializeComponent();
            AddControlsToPanel(c);
        }

        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(c);
        }
    }
}
