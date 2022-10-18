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

            //Store passed values from login
            this.token = passedToken;
            this.workgroupUuid = workgroupUuid;
            this.accountUuid = accountUuid;
        }

        //Navigate to individual report form
        private void button2_Click(object sender, EventArgs e)
        {
            WorkspaceAuditFormHistorical wsHistoricalAuditForm = new WorkspaceAuditFormHistorical(token, workgroupUuid, accountUuid);
            wsHistoricalAuditForm.Show();
            this.Hide();
        }

        //Close application on form close
        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
