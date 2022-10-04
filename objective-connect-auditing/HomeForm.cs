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
        public static string token = "";
        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            token = LoginForm.token;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkspaceAuditForm wsAuditForm = new WorkspaceAuditForm();
            wsAuditForm.Show();
            this.Hide();
        }
    }
}
