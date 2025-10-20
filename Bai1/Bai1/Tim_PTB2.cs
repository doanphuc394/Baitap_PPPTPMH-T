using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tim_PTB2
    {
        public static void Run()
        {
            Console.WriteLine("Giai phuong trinh bac 2: ax^2 + bx + c = 0");

            
            Console.Write("Nhap a: ");
            double a = double.Parse(Console.ReadLine());

            Console.Write("Nhap b: ");
            double b = double.Parse(Console.ReadLine());

            Console.Write("Nhap c: ");
            double c = double.Parse(Console.ReadLine());

            if (a == 0)
            {
                // Phương trình trở thành bậc 1: bx + c = 0
                if (b == 0)
                {
                    if (c == 0)
                        Console.WriteLine("Phuong trinh co vo so nghiem");
                    else
                        Console.WriteLine("Phuong trinh vo nghiem.");
                }
                else
                {
                    double x = -c / b;
                    Console.WriteLine("Phương trinh co nghiem duy nhat: x = " + x);
                }
            }
            else
            {
                // Tính delta
                double delta = b * b - 4 * a * c;

                if (delta < 0)
                {
                    Console.WriteLine("Phuong trinh vo nghiem.");
                }
                else if (delta == 0)
                {
                    double x = -b / (2 * a);
                    Console.WriteLine("Phuong trinh co nghiem kep: x1 = x2 = " + x);
                }
                else
                {
                    double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                    Console.WriteLine("Phuong trinh co 2 nghiem phan biet:");
                    Console.WriteLine("x1 = " + x1);
                    Console.WriteLine("x2 = " + x2);
                }
            }
        }
    }
}
