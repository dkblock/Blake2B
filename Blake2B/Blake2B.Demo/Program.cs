using Blake2B.Core;
using System;

namespace Blake2B.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var blake = new Blake2B_Algorithm();
            var message = "abc";
            var hash = blake.ComputeHash(message);

            Console.WriteLine(hash);
            Console.ReadLine();
        }
    }
}
