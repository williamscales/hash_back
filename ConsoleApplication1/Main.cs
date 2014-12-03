using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace HashBack
{
    class MainClass
    {
        static int Main(string[] args)
        {
            // Simple example which takes strings as command line arguments
            // and hashes them.
            var myStrings = new string[] { "foo", "bar", "baz", "qux" };
            SHA256 hasher = new SHA256Managed();
            var hashes = new List<byte[]>();
            foreach (var myString in myStrings)
            {
                var toHash = Encoding.UTF8.GetBytes(myString);
                hashes.Add(hasher.ComputeHash(toHash));
            }
            var tree = new HashTree(hashes);
            Console.WriteLine(tree.RootHash);
            Console.ReadLine();

            return 0;
        }
    }
}
