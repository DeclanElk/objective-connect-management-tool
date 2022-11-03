using Microsoft.VisualBasic.FileIO;
using System.Net;

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

        //Run report
        private void button1_Click(object sender, EventArgs e)
        {
            runReport();
        }

        private async void runReport()
        {
            //Clear alert
            label4.Text = "";

            //Change button to disabled and indicate function is running
            button1.Text = "Running...";
            button1.Enabled = false;

            try
            {
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
                progressBar1.Maximum = numWorkspaces;
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

                    DateTime dateTest;
                    if (DateTime.TryParse(fields[5].Substring(0, 8), out dateTest))
                    {
                        //Define dates for the individual audit report (hardcoded to 20 years prior)
                        long startTime = DateTimeOffset.Now.AddYears(-20).ToUnixTimeMilliseconds();
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

                            DateTime.TryParse(fields2[0], out mostRecentEvent);
                        }

                        int dormantMonths = Decimal.ToInt32(numericUpDown1.Value);

                        //After the CSV has been read, add workspace details and final recorded date to output
                        if (dormantMonths == 0)
                        {
                            //If a minimum dormant time period has not been specified, log all workspaces
                            if (mostRecentEvent == DateTime.MinValue)
                            {
                                outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], "", fields[11], fields[13] });
                            }
                            else
                            {
                                outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], mostRecentEvent.ToString("dd/MM/yyyy"), fields[11], fields[13] });
                            }
                        }
                        else
                        {
                            //Else, only log those with activity that is older than the curent time minus the number of months specified
                            if (mostRecentEvent != DateTime.MinValue && mostRecentEvent < DateTime.Now.AddMonths(-dormantMonths))
                            {
                                outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], mostRecentEvent.ToString("dd/MM/yyyy"), fields[11], fields[13] });
                            }
                        }
                    }

                    //Update progress bar
                    workspacesParsed++;
                    progressBar1.Value = workspacesParsed;
                }

                //Format each output workspace into comma delinieated string
                string[] csvOutputLines = new string[outputWorkspaces.Count + 1];
                csvOutputLines[0] = "Workspace Name,Owner,Open Date,Last Accessed,Connections,Participants";

                for (int i = 0; i < outputWorkspaces.Count; i++)
                {
                    //Cleaning up open date formatting
                    DateTime openDate;
                    if (DateTime.TryParse(outputWorkspaces[i][2].Substring(0, 8), out openDate))
                    {
                        csvOutputLines[i + 1] = "\"" + outputWorkspaces[i][0] + "\"" + "," + outputWorkspaces[i][1] + "," + openDate.ToString("dd/MM/yyyy") + "," + outputWorkspaces[i][3] + "," + outputWorkspaces[i][4] + "," + outputWorkspaces[i][5];
                    }
                    else
                    {
                        csvOutputLines[i + 1] = "\"" + outputWorkspaces[i][0] + "\"" + "," + outputWorkspaces[i][1] + "," + outputWorkspaces[i][2] + "," + outputWorkspaces[i][3] + "," + outputWorkspaces[i][4] + "," + outputWorkspaces[i][5];
                    }
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

                //Reset indicators
                button1.Text = "Run report";
                button1.Enabled = true;
                progressBar1.Value = 0;

            }
            catch
            {
                MessageBox.Show(
                    "Could complete report.\n\nPlease check your network connection, then try again.",
                    "Error running report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                //Reset indicators
                button1.Text = "Run report";
                button1.Enabled = true;
                label4.Text = "";
                progressBar1.Value = 0;
            }
        }

        //Return to home screen
        private void button2_Click(object sender, EventArgs e)
        {
            HomeForm homeForm = new HomeForm(token, workgroupUuid, accountUuid);
            homeForm.Show();
            this.Hide();
        }

        //Close application on form close
        private void WorkspaceAuditFormHistorical_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
