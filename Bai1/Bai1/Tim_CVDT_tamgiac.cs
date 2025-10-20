using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tim_CVDT_tamgiac
    {
        public static void Run()
        {
            Console.Write("Nhap canh a: ");
            double a = double.Parse(Console.ReadLine());

            Console.Write("Nhap canh b: ");
            double b = double.Parse(Console.ReadLine());

            Console.Write("Nhap canh c: ");
            double c = double.Parse(Console.ReadLine());

           
          
            if (a + b > c && a + c > b && b + c > a)
            {
                double chuVi = a + b + c;

                double p = chuVi / 2;
                double dienTich = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

                Console.WriteLine("Ba canh lap duoc tam giac.");
                Console.WriteLine("Chu vi tam giac = " + chuVi);
                Console.WriteLine("DIen tich tam giac = " + dienTich);
            }
            else
            {
                Console.WriteLine("Ba canh khong lap duoc tam giac");
            }
        }
    }
}
