using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Task_5 {
    internal class Program {

        // reads input data points and returns them as a 2D double array
        private static double[,] ReadInputs() {
            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();
            string source;

            // loop to input polynomial coefficients
            do {
                // DISPLAY CURRENT INPUT
                Console.Clear();
                Console.Write("Please enter positive or negative datapoints to be able to interpolate a polynomial function: \nEnter 'Q' or 'q' to stop entry\nDatapoints so far: { ");
                for (int i = 0; i < dataX.Count(); i++) Console.Write("{" + $" {dataX[i]}, {dataY[i]} " + "}, ");

                // read input of X with guard clause
                Console.Write("}\n\nX:");
                source = Console.ReadLine();
                if (!double.TryParse(source, out double numberX)) continue;

                // read input of Y with guard clause
                Console.Write("Y:");
                source = Console.ReadLine();
                if (!double.TryParse(source, out double numberY)) continue;

                // only add after validation of both data points
                dataX.Add(numberX);
                dataY.Add(numberY);
            } while (source.ToLower() != "q");

            // checks if lists are equal length
            if(dataX.Count() != dataY.Count()) throw new Exception("Mismatched data points");
            double[,] array = new double[dataX.Count(), 2];

            // rewrite from lists to array
            for (int i = 0; i < dataX.Count(); i++) {
                array[i, 0] = dataX[i];
                array[i, 1] = dataY[i];
            }

            return array;
        }

        // formats inputed data points to remove duplicates and check for double assignments of Y values to same X values
        private static double[,] FormatPoints(double[,] data) {
            int width = data.GetLength(0);
            Dictionary<double, double> dataPairs = new Dictionary<double, double>();

            // check for duplicate X values
            for (int i = 0; i < width; i++) {
                double xValue = data[i, 0];
                double yValue = data[i, 1];

                // same X value same Y value => "trim"
                if (dataPairs.ContainsKey(xValue) && dataPairs[xValue] == yValue) continue;
                // same X value different Y value => error
                if (dataPairs.ContainsKey(xValue) && dataPairs[xValue] != yValue) throw new Exception("Error: incorrect data points");
                // unique X value
                dataPairs.Add(xValue, yValue);
            }

            double[,] formatedPairs = new double[dataPairs.Count, 2];

            int width1 = formatedPairs.GetLength(0);
            int height = formatedPairs.GetLength(1);
            // debug
            Console.WriteLine($"FormatedPairs width: {width1} height: {height}");

            for (int i = 0; i < dataPairs.Count; i++) {
                formatedPairs[i, 0] = dataPairs.ElementAt(i).Key;
                formatedPairs[i, 1] = dataPairs.ElementAt(i).Value;
            }



            return formatedPairs;
        }

        // display data points
        private static void PrintDataPoints(double[,] data) {
            int width = data.GetLength(0);
            Console.WriteLine("Data Points:");
            for (int i = 0; i < width; i++) {
                Console.WriteLine($"[ {data[i, 0]}, {data[i, 1]} ]");
            }
        }

        // Constructs the matrix of coefficients for polynomial interpolation
        public static double[,] ConstructMatrix(double[,] data) {

            double[,] coefficients = new double[data.GetLength(0), data.GetLength(0)+1];
            int width = coefficients.GetLength(0);
            int height = coefficients.GetLength(1);
            // debug
            Console.WriteLine($"width: {width} height: {height}");

            // copy data into "augmented" matrix, scaling to powers 
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    // last column is Y values
                    if (j == height - 1) coefficients[i, j] = data[i, 1];
                    // other columns are X values scaled
                    else coefficients[i, j] = Math.Pow(data[i, 0], width - (j +1));
                }
            }

            /*
            // display matrix
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Console.Write($"{coefficients[i, j]}, ");
                }
                Console.WriteLine();
            }
            */

            return null;
        }

        /*
            Func<double, double> PolyInterpolation(double[,] DP) {
                Input:
                    a two-dimensional array of data points' coordinates
                Output:
                    a function that uses Horner's scheme to calculate the value of the interpolating polynomial
                    for real arguments between the smallest and the largest of the provided data points’
                    first coordinates.
                Examples:
                    PolyInterpolate({{3, 6}, {0, 3}, {2, 1}}) -> f
                        f(0)    -> 3
                        f(2)    -> 1
                        f(2.75) -> 4.375
                        f(3)    -> 6
                        f(-1)   -> Error: out of bound argument
                    PolyInterpolate({{3, 6}, {3, 7}}) -> Error: incorrect data points
                    PolyInterpolate({{-1, 1.25}, {2, 3.5}}) -> f
                        f(1)    -> 2.75
                        f(-1)   -> 1.25
        */
        public static Func<double, double> PolyInterpolation(double[,] DP) {
            
            DP = null;

            /* Replace this with your code */
            return (x) => {
                return x;
            };
        }

        public static void Main(/* string[] args */) {

            double[,] dataPoints = ReadInputs();

            dataPoints = FormatPoints(dataPoints);
            PrintDataPoints(dataPoints);
            
            ConstructMatrix(dataPoints);


            PolyInterpolation(dataPoints);

            // HOLD THE LINE (terminal window) !!!
            Console.ReadLine();
        }
    }
}