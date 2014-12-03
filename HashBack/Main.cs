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
            // Simple example that makes a hash tree which verifies some
            // strings
            var myStrings = new string[] { "foo", "bar", "baz", "qux" };
            var data = new List<byte[]>();
            foreach (var myString in myStrings)
            {
                data.Add(Encoding.UTF8.GetBytes(myString));
            }
            var tree = new HashTree(data);
            Console.WriteLine(tree.RootHash);
            Console.ReadLine();

            return 0;
        }
    }
}
