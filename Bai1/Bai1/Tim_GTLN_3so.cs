using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tim_GTLN_3so
    {
        public static void Run()
        {
            Console.Write("Nhap vao so nguyen a: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nhap vao so nguyen b: ");
            int b = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nhap vao so nguyen c: ");
            int c = Convert.ToInt32(Console.ReadLine());
            int max = 0;
            if((a > b) && (a > c)){
                max = a;
            }
            else if ((b>a) && (b > c)) {
                max = b;
            }
            else{
                max = c;
            }
            Console.WriteLine("So lon nhat trong 3 so la: {0}", max);
        }
    }
}
