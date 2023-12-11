
using System;

class GaussianElimination
{
    static void Main()
    {
        Console.WriteLine("Enter the number of equations:");
        int numOfEquations = int.Parse(Console.ReadLine());


        Console.WriteLine("Enter the augmented matrix coefficients row-wise:");
        double[,] augmentedMatrix = InputMatrix(numOfEquations);

        Console.WriteLine("Augmented Matrix before solving:");
        PrintMatrix(augmentedMatrix);

        Solve(augmentedMatrix);

    }

    //Function that takes number of eqns and creates a matrix and asks for the elements row-wise
    static double[,] InputMatrix(int numOfEquations)
    {
        double[,] createdMatrix = new double[numOfEquations, numOfEquations + 1];

        for (int rowsIterator = 0; rowsIterator < numOfEquations; rowsIterator++)
        {
            Console.WriteLine($"Equation {rowsIterator + 1}:");
            for (int columnsIterator = 0; columnsIterator <= numOfEquations; columnsIterator++)
            {
                Console.Write($"Enter coefficient {columnsIterator + 1}: ");
                createdMatrix[rowsIterator, columnsIterator] = double.Parse(Console.ReadLine());
            }
        }

        return createdMatrix;
    }

    static void PrintMatrix(double[,] matrixToPrint)
    {
        int rows = matrixToPrint.GetLength(0); //The 0 given to the GetLength function refers to the length of the 1D (Rows)
        int cols = matrixToPrint.GetLength(1); //The 1 given to the GetLength function refers to the length of the 2D (Columns)

        for (int rowsIterator = 0; rowsIterator < rows; rowsIterator++)
        {
            for (int columnsIterator = 0; columnsIterator < cols; columnsIterator++)
            {
                Console.Write(matrixToPrint[rowsIterator, columnsIterator] + "\t");
            }
            Console.WriteLine();
        }
    }

    //rowIndex1 and rowIndex2 are the indexes of rows to be swapped like swap row 2 with row 3 for ex.
    static void SwapRows(double[,] inputMatrix, int rowIndex1, int rowIndex2)
    {

        for (int iterator = 0; iterator < inputMatrix.GetLength(1); iterator++)
        {
            double swapVariable = inputMatrix[rowIndex1, iterator];
            inputMatrix[rowIndex1, iterator] = inputMatrix[rowIndex2, iterator];
            inputMatrix[rowIndex2, iterator] = swapVariable;
        }
    }
    static double[,] ForwardElimination(double[,] inputMatrix)
    {
        int numberOfRows = inputMatrix.GetLength(0);
        int numberOfColumns = inputMatrix.GetLength(1);
        //pivot column represents the column of current pivot and we iterate less that number of cols by 1 so we don't iterate on the constants column
        for (int pivotColumn = 0; pivotColumn < numberOfColumns - 1; pivotColumn++)
        {
            // If the first element in the pivot column is zero, find a row with a non-zero element in that column
            if (inputMatrix[pivotColumn, pivotColumn] == 0)
            {
                //variable to store the index of the row with the non zero entry to make it as a pivot
                int nonZeroRowIndex = -1;
                //loop to start searching for the entry that is not zero. we start from the row under the zero entry that we want to swap
                for (int i = pivotColumn + 1; i < numberOfRows; i++)
                {
                    //the condition states that if the element under our current zero entry is not zero then get its row index and break the loop(we found our non zero entry to make it a pivot)
                    if (inputMatrix[i, pivotColumn] != 0)
                    {
                        nonZeroRowIndex = i;
                        break;
                    }
                }
                //if no non-zero entry is found this means that the system is inconsistent
                if (nonZeroRowIndex == -1)
                {
                    Console.WriteLine("The system is inconsistent Or has infinetly many solutions");
                    return null;
                }

             
//       Swap the row with a non-zero element with the current row
                SwapRows(inputMatrix, pivotColumn, nonZeroRowIndex);

            }
            //normalizing the pivot to 1
            double pivotValue = inputMatrix[pivotColumn, pivotColumn];
            if (pivotValue != 1)
            {
                for (int col = pivotColumn; col < numberOfColumns; col++)
                {
                    inputMatrix[pivotColumn, col] /= pivotValue;
                }
            }




            // Make entries below the pivot zero
            for (int rowToUpdate = pivotColumn + 1; rowToUpdate < numberOfRows; rowToUpdate++)
            {
                double factor = inputMatrix[rowToUpdate, pivotColumn] / inputMatrix[pivotColumn, pivotColumn];

                for (int col = pivotColumn + 1; col < numberOfColumns; col++)
                {
                    inputMatrix[rowToUpdate, col] -= factor * inputMatrix[pivotColumn, col];
                }

                // Fill the lower triangular matrix with zeros
                inputMatrix[rowToUpdate, pivotColumn] = 0;

            }
        }

        return inputMatrix;
    }
    // Function to calculate the values of the unknowns
    static void BackSubstitution(double[,] augmentedMatrix)
    {
        int systemSize = augmentedMatrix.GetLength(0); // Number of equations or unknowns

        // An array to store the solution
        double[] solution = new double[systemSize];

        // Start calculating from the last equation up to the first
        for (int currentEquation = systemSize - 1; currentEquation >= 0; currentEquation--)
        {
            // Start with the RHS of the equation
            solution[currentEquation] = augmentedMatrix[currentEquation, systemSize];

            // Iterate over the variables to the right of the current one
            for (int variableToRight = currentEquation + 1; variableToRight < systemSize; variableToRight++)
            {
                // Subtract contributions of variables to the right
                solution[currentEquation] -= augmentedMatrix[currentEquation, variableToRight] * solution[variableToRight];
            }

            // Divide by the coefficient of the unknown being calculated
            solution[currentEquation] = solution[currentEquation] / augmentedMatrix[currentEquation, currentEquation];
        }

        Console.WriteLine();
        Console.WriteLine("Solution for the system:");

        // Print the solution
        for (int iterator = 0; iterator < systemSize; iterator++)
        {
            Console.WriteLine($"X{iterator + 1} = {Math.Round(solution[iterator])}");

        }
    }
    static int Solve(double[,] inputMatrix)
    {
        if (ForwardElimination(inputMatrix) == null)
        {
            return -1;
        }
        else
        {
            System.Console.WriteLine("System has one solution");
            double[,] REF = ForwardElimination(inputMatrix);
            BackSubstitution(REF);
            return 1;
        }

    }
}