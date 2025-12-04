using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Task_5 {
    internal class Program {

        // METHODS TAKEN FROM PRACTICAL TASK 4 ===============================================================================================================================

        // Prints the matrix to the console
        private static void PrintSolution(double[] solution) {
            // print array solution with formating
            Console.Write("Solution(s):\n{ ");
            for (int i = 0; i < solution.Length; i++) {
                Console.Write($"a{i + 1}={solution[i]}, ");
            }
            Console.WriteLine("}");
        }

        // Prints the matrix to the console
        public static void PrintMatrix(double[,] S) {
            for (int i = 0; i < S.GetLength(0); i++) {
                for (int j = 0; j < S.GetLength(1); j++) {
                    Console.Write($"{S[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Swaps rows of a matrix in memory reference
        public static void SwapRows(double[,] S, int target, int destination) {
            // guard clause
            int height = S.GetLength(0);
            int width = S.GetLength(1);
            if (target >= height || destination >= height || target < 0 || destination < 0) throw new IndexOutOfRangeException("Row index out of range.");
            double[] tmp = new double[width];

            // swap rows
            for (int i = 0; i < width; i++) {
                tmp[i] = S[destination, i]; // evacuate destination row
                S[destination, i] = S[target, i]; // write target row
                S[target, i] = tmp[i]; // save destination row
            }
        }

        // Scales row of a matrix in memory reference
        public static void ScaleRow(double[,] S, int target, double factor) {
            // guard clause

            int height = S.GetLength(0);
            int width = S.GetLength(1);
            if (target >= height || target < 0) throw new IndexOutOfRangeException("Row index out of range.");
            if (factor == double.NaN || double.IsInfinity(factor)) throw new ArgumentException("Scale must be a valid number.");

            // scale row by a factor
            for (int i = 0; i < width; i++) {
                S[target, i] *= (double)factor; // scaled element
            }
        }

        // Adds two rows of a matrix in memory reference
        public static void AddRows(double[,] S, int target, int addition) {
            // guard clause
            int width = S.GetLength(1);
            int height = S.GetLength(0);
            if (target >= height || addition >= height || target < 0 || addition < 0) throw new IndexOutOfRangeException("Row index out of range.");

            // add rows
            for (int i = 0; i < width; i++) {
                S[target, i] += S[addition, i]; // add element
            }
        }

        // Adds two rows of a matrix in memory reference
        public static void AddRows(double[,] S, int target, double[] addition) {
            // guard clause
            int width = S.GetLength(1);
            int height = S.GetLength(0);
            if (target >= height || target < 0 || addition.Length != width) throw new IndexOutOfRangeException("Row index out of range.");

            // add rows
            for (int i = 0; i < width; i++) {
                S[target, i] += addition[i]; // add element
            }
        }

        // sorts the matrix into row echelon form
        public static void RowEchelonForm(double[,] S, int current) {
            // int row = 0;
            int height = S.GetLength(0);
            int index = current + 1;
            for (int row = current; row < height; row++) {

                while (S[row, row] == 0 && index < height) {
                    SwapRows(S, row, index);
                    index++;
                }
            }
        }

        // counts the number of zero rows in a matrix
        public static int ZeroRows(double[,] S) {
            int height = S.GetLength(0);
            int width = S.GetLength(1);
            int found = 0;

            for (int i = 0; i < height; i++) {
                bool allZero = true; // reset after each row
                for (int j = 0; j < width - 1; j++) {
                    // negative check => element found
                    if (S[i, j] != 0) {
                        allZero = false;
                        break;
                    }
                }
                if (allZero) found++;
            }
            return found;
        }

        // checks for inconsistent rows in a matrix
        public static bool ConsistentRows(double[,] S) {
            int height = S.GetLength(0);
            int width = S.GetLength(1);
            bool allZero;

            for (int i = 0; i < height; i++) {
                allZero = true;
                for (int j = 0; j < width - 1; j++) {
                    if (S[i, j] != 0) allZero = false; // non zero element found
                }
                if (S[i, width - 1] != 0 && allZero) return false; // all elements zero but constant non zero
            }
            return true;
        }

        public static double[] SystemSolve(double[,] S) {
            int height = S.GetLength(0);
            int width = S.GetLength(1);
            int row = 0;

            while (row < height && row < width - 1) {
                // row echelon form
                if (S[row, row] == 0) RowEchelonForm(S, row);

                try {
                    // Console.WriteLine($"Scaling row {row} by factor {S[row, row]}");
                    ScaleRow(S, row, 1 / S[row, row]); // scale to one
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    return new double[0]; // no solution
                }

                for (int i = 0; i < height; i++) {
                    if (i == row) continue; // dont eliminate self
                    if (S[i, row] == 0) continue; // skip zero coefficients

                    double[] tmpRow = new double[width]; // place holder for scaled row
                    for (int j = 0; j < width; j++) {
                        tmpRow[j] = S[row, j] * -S[i, row]; // copy and scale row
                    }

                    try {
                        AddRows(S, i, tmpRow); // eliminate above / below
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                        return new double[0]; // no solution
                    }
                }
                row++;
            }

            // number of linearly independent rows < number of variables => infinitely many solutions
            if (height - ZeroRows(S) < width - 1) {
                Console.WriteLine("Infinitely many solutions detected.");
                return new double[] { };
            }

            // inconsistency check
            if (!ConsistentRows(S)) {
                Console.WriteLine("Inconsistency detected.");
                PrintMatrix(S);
                return new double[] { };
            }

            double[] solution = new double[height - ZeroRows(S)];

            for (int i = 0; i < height - ZeroRows(S); i++) {
                solution[i] = S[i, width - 1]; // constant column
            }

            Console.WriteLine("Solved Matrix: ");
            PrintMatrix(S);

            return solution;
        }

        // ===================================================================================================================================================================

        // PRACTICAL TASK 5 METHODS

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
            Console.WriteLine();
        }

        // Constructs the matrix of coefficients for polynomial interpolation
        public static double[,] ConstructMatrix(double[,] data) {

            double[,] coefficients = new double[data.GetLength(0), data.GetLength(0)+1];
            int width = coefficients.GetLength(0);
            int height = coefficients.GetLength(1);

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

            return coefficients;
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
        // polynomial interpolation using the interpolation theorem
        public static Func<double, double> PolyInterpolation(double[,] DP) {

            // construct linear system matrix from data points
            double[,] linearSystemMatrix = ConstructMatrix(DP);
            // solve equations for coefficients
            double[] coefficients = SystemSolve(linearSystemMatrix);

            // smallest and largest X values
            int min;
            int max;



            PrintSolution(coefficients);

            // TODO find range of valid inputs
            return (x) => {
                // guard clause for out of bound arguments
                if(min > x || x > max) throw new ArgumentOutOfRangeException("Error: out of bound argument");

                double[] solutions = new double[coefficients.Length];

                // horner schema
                solutions[0] = coefficients[0];
                for (int i = 1; i < coefficients.Length; i++) {
                    solutions[i] = (solutions[i - 1] * x) + coefficients[i] ;
                }

                // return last element of horner schema calculation
                return solutions[solutions.Length-1];
            };
        }


        public static void Main(/* string[] args */) {

            double[,] dataPoints = ReadInputs();

            // DISPLAY CURRENT INPUT
            Console.Clear();
            dataPoints = FormatPoints(dataPoints);
            PrintDataPoints(dataPoints);
            
            Func<double,double> f = PolyInterpolation(dataPoints);
            Console.WriteLine(f(0));
            Console.WriteLine(f(-1));
            Console.WriteLine(f(2));



            // HOLD THE LINE (terminal window) !!!
            Console.ReadLine();
        }
    }
}