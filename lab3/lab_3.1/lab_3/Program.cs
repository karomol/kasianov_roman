using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>() { 1, 3, -7, 99, -8, 7, 8, 8, 44, -4, 9 };
            numbers.Sort();
            numbers.Reverse();
            foreach (int a in numbers)
                Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
