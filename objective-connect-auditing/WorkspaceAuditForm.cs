using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Net;

namespace objective_connect_auditing
{
    public partial class WorkspaceAuditForm : Form
    {
        private string token = "";
        private string workgroupUuid = "";
        private string accountUuid = "";

        public WorkspaceAuditForm(string passedToken, string workgroupUuid, string accountUuid)
        {
            InitializeComponent();
            this.token = passedToken;
            this.workgroupUuid = workgroupUuid;
            this.accountUuid = accountUuid;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            runReport();
        }

        private async void runReport()
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

                //Parse the workspace's open date
                DateTime openDate;
                if (DateTime.TryParse(fields[5].Substring(0, 8), out openDate))
                {
                    //If the open date is more than 6-months ago run individual audit report
                    //for last activity date
                    if (DateTime.Now.AddMonths(-6) > openDate)
                    {
                        //Define dates for the individual audit report (6 months)
                        long startTime = DateTimeOffset.Now.AddMonths(-6).ToUnixTimeMilliseconds();
                        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                        string workspaceId = fields[0];

                        //Interpolate start/stop unix timestamps and workspace ID to API url
                        string wsUrl = $"https://secure.objectiveconnect.com/publicapi/1/workspaceauditcsv?workspaceUuid={workspaceId}&startTime={startTime}&endTime={endTime}&isDeletedWorkspace=false";

                        //Create new HTTP request object and set headers
                        HttpClient individualAuditClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                        individualAuditClient.DefaultRequestHeaders.Accept.Clear();
                        individualAuditClient.DefaultRequestHeaders.Add("Authorization", token);

                        //Asnychronously fetch individual workspace audit for previous 6 months
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

                            //If the first field is not blank
                            if (fields2[0] != "")
                            {
                                //Attempt to parse it into the Date object above, if it parses, this is the
                                //most recent activity, if it doesn't, the date object doesn't change
                                DateTime.TryParse(fields2[0].Substring(0, 10), out mostRecentEvent);
                            }
                        }

                        //After the CSV has been read, if the DateTime value hasn't changed, there has
                        //been no activity in the past 6 months, meaning we can add this workspace to the output
                        if (mostRecentEvent == DateTime.MinValue)
                        {
                            outputWorkspaces.Add(new string[] { fields[1], fields[2], fields[5], fields[11] });
                            label4.Text = "Workspaces found that meet the criteria: " + outputWorkspaces.Count;
                        }
                    }
                }
            }

            //Format each output workspace into comma delinieated string
            string[] csvOutputLines = new string[outputWorkspaces.Count + 1];
            csvOutputLines[0] = "Workspace Name,Author,Open Date,Connections";

            for (int i = 0; i < outputWorkspaces.Count; i++)
            {
                csvOutputLines[i + 1] = "\"" + outputWorkspaces[i][0] + "\"" + "," + outputWorkspaces[i][1] + "," + outputWorkspaces[i][2] + "," + outputWorkspaces[i][3];
            }

            //Show save dialog
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save output CSV";
            saveFileDialog1.ShowDialog();

            label4.Text = saveFileDialog1.FileName;

            //Write list of workspaces to CSV file
            if (saveFileDialog1.FileName != "")
            {
                await File.WriteAllLinesAsync(saveFileDialog1.FileName, csvOutputLines);
            }
            else
            {
                await File.WriteAllLinesAsync("Workspace Report.csv", csvOutputLines);
            }
            
            //Display count of workspaces and connections used 
            //int connectionsCount = 0;
            //foreach (string[] workspace in outputWorkspaces)
            //{
            //    connectionsCount += Int32.Parse(workspace[3]);
            //}
            //Console.WriteLine("Total Workspaces not used in the past {0}-months: {1}   Totalling {2} connections.", 6, outputWorkspaces.Count, connectionsCount);

        }
    }
}
