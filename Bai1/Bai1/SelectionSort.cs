using System;
using System.IO;
using System.Linq;

namespace Bai1
{
    internal class SelectionSort
    {
        public static void Run()
        {
            string filePath = "input_array.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Không tìm thấy file " + filePath);
                return;
            }

            // Đọc dữ liệu từ file, tách theo khoảng trắng, chuyển thành mảng số nguyên
            int[] arr = File.ReadAllText(filePath)
                            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            Console.WriteLine("Mảng ban đầu:");
            PrintArray(arr);

            // Sắp xếp bằng Selection Sort
            SortArray(arr);

            Console.WriteLine("Mảng sau khi sắp xếp tăng dần:");
            PrintArray(arr);

            // Ghi mảng đã sắp xếp ra file mới
            string outputPath = "output_array.txt";
            File.WriteAllText(outputPath, string.Join(" ", arr));
            Console.WriteLine("Đã ghi mảng đã sắp xếp vào file: " + outputPath);
        }

        // Hàm in mảng
        static void PrintArray(int[] arr)
        {
            Console.WriteLine(string.Join(" ", arr));
        }

        // Hàm sắp xếp chọn (Selection Sort)
        static void SortArray(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Tìm chỉ số phần tử nhỏ nhất trong đoạn [i, n-1]
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }

                // Hoán đổi arr[i] và arr[minIndex]
                if (minIndex != i)
                {
                    int temp = arr[i];
                    arr[i] = arr[minIndex];
                    arr[minIndex] = temp;
                }
            }
        }
    }
}
