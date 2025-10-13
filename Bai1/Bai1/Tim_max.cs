using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tim_max
    {
         public static void Run()
        {
            Console.Write("Nhap vao so nguyen a: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nhap vao so nguyen b: ");
            int b = Convert.ToInt32(Console.ReadLine());
            int max = 0;
            if (a > b){
                max = a;
            }
            if (a < b){
                max = b;
            }
            Console.WriteLine("So lon hon trong 2 so la: " + max);
        }
    }
}
