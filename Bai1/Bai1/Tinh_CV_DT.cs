using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tinh_CV_DT
    {
        public static void Run() {
            Console.Write("Nhap vao chieu dai: ");
            double a = Convert.ToDouble(Console.ReadLine());
            Console.Write("Nhap vao chieu rong: ");
            double b = Convert.ToDouble(Console.ReadLine());

            double fl = (a + b) * 2;
            double S = a * b;

            Console.WriteLine("Chu vi la: " + fl);
            Console.WriteLine("Dien tich la: " + S);
        }
      
    }
}
