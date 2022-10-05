using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace objective_connect_auditing
{
    public partial class HomeForm : Form
    {
        private string token = "";
        private string workgroupUuid = "";
        private string accountUuid = "";
        public HomeForm(string passedToken, string workgroupUuid, string accountUuid)
        {
            InitializeComponent();
            this.token = passedToken;
            this.workgroupUuid = workgroupUuid;
            this.accountUuid = accountUuid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkspaceAuditForm wsAuditForm = new WorkspaceAuditForm(token, workgroupUuid, accountUuid);
            wsAuditForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text = token;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkspaceAuditFormHistorical wsHistoricalAuditForm = new WorkspaceAuditFormHistorical(token, workgroupUuid, accountUuid);
            wsHistoricalAuditForm.Show();
            this.Hide();
        }
    }
}
