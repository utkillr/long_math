using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongNumbers
{
    class Program
    {
        public static void Main(string[] args) {
            Console.WriteLine("Enter action (+,-,*,/)");
            String action = Console.ReadLine();
            switch (action)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    break;
                default:
                    Console.WriteLine("Can't interpretate, error");
                    return;
            }
            try
            {
                Console.WriteLine("Enter number A:");
                longNumber a = new longNumber(Console.ReadLine());
                Console.WriteLine("Enter number B:");
                longNumber b = new longNumber(Console.ReadLine());
                longNumber res;
                switch (action)
                {
                    case "+":
                        res = a + b;
                        break;
                    case "-":
                        res = a - b;
                        break;
                    case "*":
                        res = a * b;
                        break;
                    case "/":
                        res = a / b;
                        break;
                    default:
                        res = new longNumber("0");
                        break;
                }
                Console.WriteLine("a " + action + " b = ");
                Console.WriteLine(res);
            }
            catch (error e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
