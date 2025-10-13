using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai1
{
    internal class Tinh_tongphantutrongmang
    {
        public static void Run()
        {
            Console.Write("Nhap so phan tu trong mang: ");
            int n = int.Parse(Console.ReadLine());

            int[] arr = new int[n];
            int tong = 0;

            // Nhập các phần tử mảng
            for (int i = 0; i < n; i++)
            {
                Console.Write("Nhap phan tu thu " + (i + 1) + ": ");
                arr[i] = int.Parse(Console.ReadLine());
                tong += arr[i]; // cộng dồn vào tổng
            }

            Console.WriteLine("Tong cac phan tu trong mang = " + tong);
        }
    }
}
