using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password
{
    class Program
    {
        public static string FiveHash;
        private static string CompleteHash;
        //Turns the given string into a sha1 hash for the API
        public static string Hash(string input)
        {
            //Turn the given password into a sha1 hash
            SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            // Rest of the hash for filtering the list returned by the api
            CompleteHash = sb.ToString().Substring(5);
            //First five characters of the hash that gets send to the API
            FiveHash =
            !String.IsNullOrWhiteSpace(sb.ToString()) && sb.ToString().Length >= 5
            ? sb.ToString().Substring(0, 5)
            : sb.ToString();


            return sb.ToString();
        }
        //Sends a Http request to the API and returns the a list with found hashes
        public static async Task<string> SendHTTP(string input, bool fulloutput = false, bool forFile = false)
        {

            var client = new HttpClient();
            //if its for console output
            if (!forFile)
            {
                //send the request to the api with the first five characters of the hash
                var response = await client.GetStringAsync("https://api.pwnedpasswords.com/range/" + input);
                //replace the line breaks with semicolons
                response = response.Replace("\r\n", ";");
                //split the response into entries where the semicolons are
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
                if (FormattedText.ContainsKey(CompleteHash))
                {
                    //if the user only wants the filtered output 
                    if (!fulloutput)
                    {
                        if (Convert.ToInt32(FormattedText[CompleteHash]) > 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        if (Convert.ToInt32(FormattedText[CompleteHash]) > 10)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        response = $"Your password has been {FormattedText[CompleteHash]} times cracked";
                    }//if the user wants the full output
                    else
                    {
                        response += $"\nYour password has been {FormattedText[CompleteHash]} times cracked";
                    }
                    
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    response = "Your password has not been cracked yet!";
                }
                return response;
                
            }//if its for file output
            else
            {
                var response = await client.GetStringAsync("https://api.pwnedpasswords.com/range/" + input);
                return response;
            }

        }
        [Obsolete("Your password already gets filtered automatically if you still want to use it then please remember it might get removed in the future")]
        public static void WriteToFile(string input)
        {
            var path = Environment.CurrentDirectory + "/foundHashes.txt";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, input);
            }
            Console.WriteLine($"All found hashes have been written to the File 'foundHashes.txt' at : {path}");
            Console.WriteLine($"to check if your Password has been found use CTRL+F in a text editor and search for : {CompleteHash}");
        }

        static void Main(string[] args)
        {
            Console.Write("Enter a Password: ");
            Console.WriteLine("Complete Hash: " + Hash(Console.ReadLine()));
            Console.WriteLine("Part send to the API: " + FiveHash);
            Console.WriteLine("Result: \n" + SendHTTP(FiveHash).Result);
            Console.ReadLine();

        }
        /*[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }*/
    }
}
