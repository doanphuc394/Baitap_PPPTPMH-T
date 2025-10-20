using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tim_CL_AD
    {
        public static void Run()
        {
            Console.Write("Nhap so nguyen n: ");
            int n = int.Parse(Console.ReadLine());
            if(n % 2 == 0)
            {
                Console.WriteLine(" n la so chan");
            }
            else
            {
                Console.WriteLine(" n la so le");
            }
            if(n < 0)
            {
                Console.WriteLine("n la so nguyen am");
            }
            else
            {
                Console.WriteLine("n la so nguyen duong");
            }
        }
    }
}
