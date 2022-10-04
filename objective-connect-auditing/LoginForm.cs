using System.CodeDom;
using System.Drawing.Drawing2D;

namespace objective_connect_auditing
{
    public partial class LoginForm : Form
    {
        public static string token = "";
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Combine username and password
            string credentials = textBox1.Text + "\n" + textBox2.Text;
            //Convert to byte array
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            //Convert to base64 encoded string
            string base64Token = System.Convert.ToBase64String(plainTextBytes);
            token = base64Token;
            //Move over to second page
            HomeForm homeForm = new HomeForm();
            homeForm.Show();
            this.Hide();
        }
    }
}