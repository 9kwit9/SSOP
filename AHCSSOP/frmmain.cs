using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace AHCSSOP
{
    public partial class frmmain : C1.Win.C1Ribbon.C1RibbonForm
    {
        frmmain _f;
        frmshow fDetail;
        frmSupra frmSub;

        Ctool_Control Ctrl;

        private Color warnColour = Color.Red;
        private Color normalColour = Color.FromKnownColor(KnownColor.ControlText);

        public frmmain()
        {
            InitializeComponent();

            _f = this;
            Ctrl = new Ctool_Control();
        }

        private void frmmain_Load(object sender, EventArgs e)
        {
            RibbonBar.Minimized = true;

            //sb1.Panels[0].Text = "      User : " + UserAccount.GetInstance().name + " [ " + UserAccount.GetInstance().AccountID + " ]      ";

            sb1.Panels[4].Text = GetRunningVersion();


            //frmpreview frmpre = new frmpreview(this);
            //frmpre.MdiParent = this;
            //frmpre.Show();
            CheckPage();
        }

        public void frmShow()
        {
            //sb1.Panels[0].Text = "      User : " + UserAccount.GetInstance().name + " [ " + UserAccount.GetInstance().AccountID + " ]      ";
           
            this.Show();
        }

        private string GetRunningVersion()
        {
            string version = "";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;

                version = "  Version: " + ad.CurrentVersion.ToString();
            }

            if (version.Trim() == "")
            {
                version = "  Version: " + this.ProductVersion;
            }

            return version;
        }

        private void frmmain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void MenuLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        void Logout()
        {
            
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
            GC.Collect();
            Application.Exit();
        }

        void CheckPage()
        {
            this.Cursor = Cursors.WaitCursor;
            if (fDetail == null)
            {
                fDetail = new frmshow();
                fDetail.MdiParent = this;
                fDetail.WindowState = FormWindowState.Maximized;
                fDetail.Refresh();
                fDetail.Focus();
                fDetail.Show();
            }
            else if (fDetail.IsDisposed)
            {
                fDetail = new frmshow();
                fDetail.MdiParent = this;
                fDetail.WindowState = FormWindowState.Maximized;
                fDetail.Refresh();
                fDetail.Focus();
                fDetail.Show();
            }
            else
            {
                fDetail.WindowState = FormWindowState.Maximized;
                fDetail.Refresh();
                fDetail.Focus();
                fDetail.Show();
            }
            this.Refresh();
            this.Cursor = Cursors.Default;
        }

        void LoadFrmSupra()
        {
            this.Cursor = Cursors.WaitCursor;
            if (frmSub == null)
            {
                frmSub = new frmSupra();
                frmSub.MdiParent = this;
                frmSub.WindowState = FormWindowState.Maximized;
                frmSub.Refresh();
                frmSub.Focus();
                frmSub.Show();
            }
            else if (frmSub.IsDisposed)
            {
                frmSub = new frmSupra();
                frmSub.MdiParent = this;
                frmSub.WindowState = FormWindowState.Maximized;
                frmSub.Refresh();
                frmSub.Focus();
                frmSub.Show();
            }
            else
            {
                frmSub.WindowState = FormWindowState.Maximized;
                frmSub.Refresh();
                frmSub.Focus();
                frmSub.Show();
            }
            this.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            CheckPage();
        }

        private void btnSupra_Click(object sender, EventArgs e)
        {
            LoadFrmSupra();
        }
    }
}
