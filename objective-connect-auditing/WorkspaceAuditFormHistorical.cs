using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace objective_connect_auditing
{
    public partial class WorkspaceAuditFormHistorical : Form
    {
        private string token = "";
        private string workgroupUuid = "";
        private string accountUuid = "";
        public WorkspaceAuditFormHistorical(string passedToken, string workgroupUuid, string accountUuid)
        {
            InitializeComponent();
            this.token = passedToken;
            this.workgroupUuid = workgroupUuid;
            this.accountUuid = accountUuid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runReport();
        }

        private async void runReport()
        {
            //Clear alert
            label4.Text = "";

            //URL Formatting
            string url = $"https://secure.objectiveconnect.com/publicapi/1/workspacecsv?accountUuid={accountUuid}&workgroupUuid={workgroupUuid}";

            //Output list
            List<string[]> outputWorkspaces = new List<string[]>();

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
            wsAuditclient.DefaultRequestHeaders.Add("Authorization", token);

            //Asynchronously fetch list of all workspaces and store resulting CSV in a string
            Task<string> stringTask = wsAuditclient.GetStringAsync(url);
            string response = await stringTask;

            //Calculate number of workspaces
            int numWorkspaces = response.Split('\n').Length;
            int workspacesParsed = 0;

            //Read CSV as a stream to TextFieldParser (CSV reading tool)
            StringReader stringStream = new StringReader(response);
            TextFieldParser csvParser = new TextFieldParser(stringStream);

            //Set parameters of TextFieldParser
            csvParser.TextFieldType = FieldType.Delimited;
            csvParser.SetDelimiters(",");

            //Process the CSV data line by line
            while (!csvParser.EndOfData)
            {
                //Read the current line into a string array
                string[] fields = csvParser.ReadFields();

                //Define dates for the individual audit report (hardcoded to 20 years prior)
                long startTime = DateTimeOffset.Now.AddYears(-1).ToUnixTimeMilliseconds();
                long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                string workspaceId = fields[0];

                //Interpolate start/stop unix timestamps and workspace ID to API url
                string wsUrl = $"https://secure.objectiveconnect.com/publicapi/1/workspaceauditcsv?workspaceUuid={workspaceId}&startTime={startTime}&endTime={endTime}&isDeletedWorkspace=false";

                //Create new HTTP request object and set headers
                HttpClient individualAuditClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                individualAuditClient.DefaultRequestHeaders.Accept.Clear();
                individualAuditClient.DefaultRequestHeaders.Add("Authorization", token);

                //Asnychronously fetch individual workspace audit for previous x months
                Task<string> individualStringTask = individualAuditClient.GetStringAsync(wsUrl);
                string individualResponse = await individualStringTask;

                //Read CSV response as a stream to TextFieldParser (CSV reading tool)
                StringReader individualStringStream = new StringReader(individualResponse);
                TextFieldParser individualCsvParser = new TextFieldParser(individualStringStream);

                //Set parameters of TextFieldParser
                individualCsvParser.TextFieldType = FieldType.Delimited;
                individualCsvParser.SetDelimiters(",");

                //Object to hold the most recent activity recorded in the individual audit report
                DateTime mostRecentEvent = new DateTime();

                //Process the CSV data line by line
                while (!individualCsvParser.EndOfData)
                {
                    //Read the current line into a string array
                    string[] fields2 = individualCsvParser.ReadFields();

                    DateTime.TryParse(fields2[0].Substring(0, 10), out mostRecentEvent);
                }

                //After the CSV has been read, add workspace details and final recorded date to output
                if (mostRecentEvent == DateTime.MinValue)
                {
                    outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], fields[11], "" });
                }
                else
                {
                    outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], fields[11], mostRecentEvent.ToString() });
                }

                //Update progress bar
                workspacesParsed++;
                progressBar1.Value = workspacesParsed / numWorkspaces;
            }

            //Format each output workspace into comma delinieated string
            string[] csvOutputLines = new string[outputWorkspaces.Count + 1];
            csvOutputLines[0] = "Workspace Name,Author,Open Date,Connections,Last Accessed";

            for (int i = 0; i < outputWorkspaces.Count; i++)
            {
                csvOutputLines[i + 1] = "\"" + outputWorkspaces[i][0] + "\"" + "," + outputWorkspaces[i][1] + "," + outputWorkspaces[i][2] + "," + outputWorkspaces[i][3] + "," + outputWorkspaces[i][4];
            }

            //Show save file dialog and save results to CSV
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                await File.WriteAllLinesAsync(saveFileDialog1.FileName, csvOutputLines);
                label4.ForeColor = Color.Green;
                label4.Text = "File saved successfully.";
            }
            else
            {
                label4.Text = "File could not be saved.";
                label4.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeForm homeForm = new HomeForm(token, workgroupUuid, accountUuid);
            homeForm.Show();
            this.Hide();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
