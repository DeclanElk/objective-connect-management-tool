using Newtonsoft.Json;
using System.CodeDom;
using System.Drawing.Drawing2D;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace objective_connect_auditing
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Combine username and password
                string credentials = textBox1.Text + ":" + textBox2.Text;
                //Convert to byte array
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(credentials);
                //Convert to base64 encoded string
                string base64Token = System.Convert.ToBase64String(plainTextBytes);
                base64Token = "Basic " + base64Token;

                //Retrieve Workgroup and Account UUID from /me endpoint
                string url = "https://secure.objectiveconnect.com/publicapi/1/me?includeActions=false";

                //Proxy pre-authentication setup
                //Use default credentials of signed in user for proxy authentication
                var proxy = new WebProxy
                {
                    UseDefaultCredentials = true
                };

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy
                };

                //Pre-authenticate proxy so that auth headers aren't overwritten
                httpClientHandler.PreAuthenticate = true;
                httpClientHandler.UseDefaultCredentials = true;

                //Primary HTTP request object
                HttpClient wsAuditclient = new HttpClient(handler: httpClientHandler, disposeHandler: true);

                //Set HTTP request headers
                wsAuditclient.DefaultRequestHeaders.Accept.Clear();
                wsAuditclient.DefaultRequestHeaders.Add("Authorization", base64Token);

                //Asynchronously fetch list of all workspaces and store resulting CSV in a string
                Task<string> stringTask = wsAuditclient.GetStringAsync(url);
                string response = await stringTask;

                //Deserialize JSON response
                dynamic userDetails = JsonConvert.DeserializeObject(response);

                string workgroupUuid = userDetails.workgroup.uuid;
                string accountUuid = userDetails.workgroup.accountUuid;

                //Move over to second page
                HomeForm homeForm = new HomeForm(base64Token, workgroupUuid, accountUuid);
                homeForm.Show();
                this.Hide();
            }
            catch
            {
                MessageBox.Show(
                    "Could not sign in.\nPlease check your credentials and network connection, then try again",
                    "Error signing in",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}