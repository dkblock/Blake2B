using Blake2B.Core;
using System;

namespace Blake2B.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var testString = "Hello, world!";
            var testsDemo = new StatisticalTestsDemo(testString);

            testsDemo.Run();
            //var blake = new Blake2B_Algorithm();
            //var message = "abc";
            //var hash = blake.ComputeHash(message);

            //Console.WriteLine(hash);
            //Console.ReadLine();
        }
    }
}
