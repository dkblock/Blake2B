using Blake2B.Core;
using System;
using System.Text;

namespace Blake2B.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var testString = "Hello, world!";
            var hash = GetHash(testString);            
            
            TestsDemo(hash);
        }

        private static byte[] GetHash(string testString)
        {
            var blake = new Blake2B_Algorithm();
            var testValue = Encoding.ASCII.GetBytes(testString);

            var hash = blake.ComputeHash(testValue);
            var hashToDisplay = blake.ComputeHash(testString);

            Console.WriteLine(hashToDisplay);
            Console.ReadLine();

            return hash;
        }

        private static void TestsDemo(byte[] hash)
        {
            var testsDemo = new StatisticalTestsDemo(hash);
            testsDemo.Run();
        }        
    }
}
