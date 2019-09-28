using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password
{
    class Program
    {
        public static string FiveHash;
        private static string CompleteHash;
        public static string Hash(string input)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            FiveHash =
            !String.IsNullOrWhiteSpace(sb.ToString()) && sb.ToString().Length >= 5
            ? sb.ToString().Substring(0, 5)
            : sb.ToString();
            CompleteHash = sb.ToString().Substring(5, hash.Length - 5);
            return sb.ToString();
        }

        public static string SendHTTP(string input)
        {
            HttpWebRequest client = WebRequest.Create("https://api.pwnedpasswords.com/range/" + input) as HttpWebRequest;

            HttpWebResponse response = (HttpWebResponse)client.GetResponse();

            var encoding = Encoding.UTF8;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                return responseText;
            }


        }

        //public static void WritetoFile(string input)
        //{

        //    var sb = new StringBuilder(CompleteHash.Length * 2);
        //    var cuthash =
        //    !String.IsNullOrWhiteSpace(sb.ToString()) && sb.ToString().Length >= 5
        //    ? sb.ToString().Substring(4, CompleteHash.Length)
        //    : sb.ToString();
        //    var path = Environment.CurrentDirectory+"/foundHashes.txt";

        //    if (!File.Exists(path))
        //    {
        //        File.WriteAllText(path,SendHTTP(FiveHash));
        //    }
        //    Console.WriteLine("All found hashes have been written to the File 'foundHashes.txt' at : " + path);
        //    Console.WriteLine("to check if your Password is unsafe use CTRL+F in a text editor and search for : " + cuthash);
        //}

        static void Main(string[] args)
        {
            Console.WriteLine(Hash(Console.ReadLine()));
            Console.WriteLine(FiveHash);
            Console.WriteLine(SendHTTP(FiveHash));
            Console.ReadLine();
        }
    }
}
