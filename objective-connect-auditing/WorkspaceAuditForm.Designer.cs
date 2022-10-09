namespace objective_connect_auditing
{
    partial class WorkspaceAuditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkspaceAuditForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(211, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Workspace Auditing (Monthly)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(606, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "This report will return all open workspaces that have not been accessed in the nu" +
    "mber of months that you specify.";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.FileName = "Worksapce Audit.csv";
            this.saveFileDialog1.Filter = "CSV (*.csv)|*.csv";
            this.saveFileDialog1.Title = "Choose where to save the report";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "Run report";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(571, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Once the report has finished running, you will be prompted to save the resulting " +
    "CSV file to your computer.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(332, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Workspaces found:  0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "< Back";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(259, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Minimum time doormant (months):";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(462, 117);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 23);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(29, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 15);
            this.label6.TabIndex = 11;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // WorkspaceAuditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 271);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkspaceAuditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workspace Auditing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorkspaceAuditForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private SaveFileDialog saveFileDialog1;
        private Button button1;
        private Label label3;
        private Label label4;
        private Button button2;
        private Label label5;
        private NumericUpDown numericUpDown1;
        private Label label6;
    }
}