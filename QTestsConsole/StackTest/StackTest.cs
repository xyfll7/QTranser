using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTests.StackTest
{
    class StackTest
    {
        public static void Mainserss()
        {
            var alphabet = new Stack<char>();
            alphabet.Push('A');
            alphabet.Push('B');
            alphabet.Push('C');

            foreach(char item in alphabet)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("::::::::::");
            while (alphabet.Count > 0)
            {
                Console.WriteLine(alphabet.Pop());
            }
            Console.WriteLine(alphabet.Count);

        }
    }
}
