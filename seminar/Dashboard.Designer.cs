namespace seminar
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dashbord = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Gotodashboard = new System.Windows.Forms.Button();
            this.spacingpanel = new System.Windows.Forms.Panel();
            this.panelControls = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 822);
            this.panel1.TabIndex = 0;
            // 
            // dashbord
            // 
            this.dashbord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.dashbord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dashbord.FlatAppearance.BorderSize = 0;
            this.dashbord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dashbord.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashbord.Image = ((System.Drawing.Image)(resources.GetObject("dashbord.Image")));
            this.dashbord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dashbord.Location = new System.Drawing.Point(0, 4);
            this.dashbord.Margin = new System.Windows.Forms.Padding(4);
            this.dashbord.Name = "dashbord";
            this.dashbord.Size = new System.Drawing.Size(267, 62);
            this.dashbord.TabIndex = 7;
            this.dashbord.Text = "Dashboard";
            this.dashbord.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(936, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 48);
            this.button2.TabIndex = 2;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel2.Controls.Add(this.button2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(267, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(991, 48);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Gotodashboard);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(267, 48);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(991, 150);
            this.panel3.TabIndex = 3;
            // 
            // Gotodashboard
            // 
            this.Gotodashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.Gotodashboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Gotodashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.Gotodashboard.FlatAppearance.BorderSize = 0;
            this.Gotodashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Gotodashboard.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Gotodashboard.ForeColor = System.Drawing.Color.White;
            this.Gotodashboard.Image = ((System.Drawing.Image)(resources.GetObject("Gotodashboard.Image")));
            this.Gotodashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Gotodashboard.Location = new System.Drawing.Point(0, 0);
            this.Gotodashboard.Margin = new System.Windows.Forms.Padding(4);
            this.Gotodashboard.Name = "Gotodashboard";
            this.Gotodashboard.Size = new System.Drawing.Size(991, 62);
            this.Gotodashboard.TabIndex = 8;
            this.Gotodashboard.Text = "    Go to Dashboard";
            this.Gotodashboard.UseVisualStyleBackColor = false;
            this.Gotodashboard.Click += new System.EventHandler(this.Gotodashboard_Click);
            // 
            // spacingpanel
            // 
            this.spacingpanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.spacingpanel.Location = new System.Drawing.Point(267, 198);
            this.spacingpanel.Name = "spacingpanel";
            this.spacingpanel.Size = new System.Drawing.Size(343, 624);
            this.spacingpanel.TabIndex = 0;
            // 
            // panelControls
            // 
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(610, 198);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(648, 624);
            this.panelControls.TabIndex = 4;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1258, 822);
            this.ControlBox = false;
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.spacingpanel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button dashbord;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel spacingpanel;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Button Gotodashboard;
    }
}