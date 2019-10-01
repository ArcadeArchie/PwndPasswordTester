using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password
{
    public partial class Form1 : Form
    {
        private RichTextBox richTextBox1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private TextBox textBox2;
        private string CompleteHash;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private TextBox textBox3;
        private Label label4;
        private string FiveHash;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 91);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(704, 190);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Your Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password hash";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(122, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(133, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(261, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 21);
            this.button1.TabIndex = 6;
            this.button1.Text = "Have i been Pwnd?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(382, 29);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(77, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Full Output";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(456, 29);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(85, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Write to file?";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(547, 27);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 9;
            this.textBox3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(547, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Path";
            this.label4.Visible = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(728, 290);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                richTextBox1.Text = SendHTTP(FiveHash, checkBox1.Checked);
            }
            else
            {
                textBox3.Visible = checkBox2.Checked;
                label4.Visible = checkBox2.Checked;
            }
        }

        private string SendHTTP(string input, bool fulloutput = false, bool forFile = false)
        {

            var client = new HttpClient();
            //if its for console output
            if (!forFile)
            {
                //send the request to the api with the first five characters of the hash
                var response = client.GetStringAsync("https://api.pwnedpasswords.com/range/" + input).Result;
                //replace the line breaks with semicolons
                response = response.Replace("\r\n", ";");
                //split the response into single entries where the semicolons are
                var Array = response.Split(';');
                //Dictinary for the entries Key: Hash, Value: the number of times it has been cracked
                Dictionary<string, string> FormattedText = new Dictionary<string, string>();
                foreach (var item in Array)
                {
                    //Get the Hash and Number
                    var temp = item.Split(':');
                    //Add the hash and number to the dictionary
                    FormattedText.Add(temp[0], temp[1]);
                }
                //if the user only wants the filtered output 
                if (!fulloutput)
                {
                    response = $"\nYour password has been {FormattedText[CompleteHash]} times cracked";
                }//if the user wants the full output
                else
                {
                    response += $"\nYour password has been {FormattedText[CompleteHash]} times cracked";
                }
                return response;
            }//if its for file output
            else
            {
                var response = client.GetStringAsync("https://api.pwnedpasswords.com/range/" + input).Result;
                return response;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            textBox2.Text = sb.ToString();
            // Rest of the hash for filtering the list returned by the api
            CompleteHash = sb.ToString().Substring(5);
            //First five characters of the hash that gets send to the API
            FiveHash =
            !String.IsNullOrWhiteSpace(sb.ToString()) && sb.ToString().Length >= 5
            ? sb.ToString().Substring(0, 5)
            : sb.ToString();
        }
    }
}
