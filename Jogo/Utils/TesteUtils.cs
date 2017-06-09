using System;

namespace Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            int start, end, intervals, value;
            Console.Write("Range start: ");
            start = Convert.ToInt32(Console.ReadLine());

            Console.Write("Range end: ");
            end = Convert.ToInt32(Console.ReadLine());

            Console.Write("Intervals: ");
            intervals = Convert.ToInt32(Console.ReadLine());

            Console.Write("Value: ");
            value = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Result: "+Util.Range(start, end, intervals, value));
            Console.ReadKey();
        }
    }
}
