using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace simplex2
{
    internal class Program
    {
        //выбор ведущей строки и столбца
        public static List<double[]> SelectRowColmn(List<double[]> list) {
            List<int> ints= new List<int>();
            for (int i = 0; i < list.Last().Length; i++)
            {
                if (Math.Round( list.Last()[i], 2)>0)
                {
                    ints.Add(i);
                }
            }
            List<double> intsPerem = new List<double>();
            List<double> intsItog = new List<double>();
            if (ints.Count() == 1)
            {
                intsPerem.Add(CalcColumn(list, ints[0]).Where(x => x != 0).Min() * list.Last()[ints[0]]);
            }
            else
            {
                for (int i = 0; i < ints.Count; i++)
                {
                    intsPerem.Add(CalcColumn(list, ints[i]).Where(x => x != 0).Min() * list.Last()[i]);
                    //Exit(CalcColumn(list, ints[i]).Select(x=>(int)x).ToArray());
                }
            }
            int collumn = ints[intsPerem.IndexOf(intsPerem.Max())];
            int row = Array.IndexOf(CalcColumn(list, collumn).ToArray(), CalcColumn(list, collumn).Where(x=>x!=0).Min());
            Console.WriteLine($"Col{collumn} row {row}");
            return CalcMatrix(list, row, collumn); 
        }
        /* ненужный шлак
        public static void Exit(int[] ints) {
            for (int i = 0; i < ints.Length; i++)
            {
                Console.WriteLine(ints[i]+" ");
            }
        }*/
        //вывод листа массивов
        public static void PrintMatr(List<double[]> list) {
            Console.WriteLine();
            for (int i = 0; i < list.Count; i++)
            {
                for (int a = 0; a < list[i].Length; a++)
                {
                    Console.Write(Math.Round( list[i][a],2)+" ");
                }
                Console.WriteLine();
            }
        }
        //выполнение итерации с матрицей
        public static List<double[]> CalcMatrix(List<double[]> list, int posRow, int posCol ) {
            if (list[posRow][posCol]!=1) //проверка равен ли 1 тот ведущий элемент
            {
                var number = list[posRow][posCol];
                for (int i = 0; i < list[posRow].Length; i++)
                {
                    list[posRow][i]= list[posRow][i]/number;
                }
            }
            List<double[]> result = list;   
            for (int i = 0; i < list.Count; i++)
            {
                if (i==posRow)
                {
                    continue;
                }
                else
                {
                    double number = list[i][posCol];
                    for (int a = 0; a < list[i].Length; a++)
                    {
                        result[i][a] = list[i][a] - list[posRow][a]*number;
                    }
                }
            }
            PrintMatr(list);


            return list;
        }
        //вычисление тех циферок в столбцах из которых выбирают какой элемент будет ведущий
        public static List<double> CalcColumn(List<double[]> list, int ColumnNumber) {
            List<double> result = new List<double>();
            for (int i = 0; i < list.Count()-1; i++)
            {
                if (list[i][ColumnNumber]<=0)
                {
                    result.Add(new double());
                }
                else
                {
                    result.Add(list[i].Last() / list[i][ColumnNumber]);
                }
            }
            return result;
        }
        //добавление элемента
        public static double[] Append(double[] doubles, double number) {
            double[] array = new double[doubles.Length+1];
            doubles.CopyTo(array, 0);
            array[doubles.Length] = number;
            return array;
        }
        public static bool IsMinusLast(List<double[]> list)
        {
            return list.Last().Where(x => Math.Round( x,2) > 0).Count() > 0;
        }
        static void Main(string[] args)
        {
            int numRows, numCol, numRes; //число строк и число столбцов
            List<double[]> list = new List<double[]>();//матрица
            List<double> results = new List<double>();//Ответы

            while (true) //"безопасный" ввод данных (защищен от неверных значений)
            {
                try
                {
                    numRows = int.Parse(Console.ReadLine());
                    numCol = int.Parse(Console.ReadLine());
                    numRes = int.Parse(Console.ReadLine());
                    for (int i = 0; i < numRows; i++)
                    {
                        list.Add(Console.ReadLine().Split().Select(x => double.Parse(x)).ToArray());
                    }
                    if (list.Count != numRows) { Console.WriteLine("Error rows"); continue; }
                    if (list.Select(x => x.Length).Where(x => x != numCol).Count() != 1)
                    { Console.WriteLine("Error columns"); continue; }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Syntax error");
                    continue;
                }
            }
            list[list.Count - 1] = Append(list.Last(), 0);//исправление последнего элемента в листе, а именно добавление переменной
            while (IsMinusLast(list)) {
                var array = SelectRowColmn(list);
                list = array;
            }
            for (int i = 0; i < numRes; i++)
            {
                if (list.Last()[i] == 0)
                {
                    for (int a = 0; a < numRows; a++)
                    {
                        if (list[a][i] == 1)
                        {
                            results.Add(list[a][list[a].Length-1]);
                        }
                    }
                }
                else
                {
                    results.Add(0);
                }
            }
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"x[{i}] = "+results[i]);
            }
            Console.WriteLine(Math.Abs(list.Last()[list.Last().Length-1]));
            Console.ReadLine();
        }
    }
}
