namespace Matrixes
{
    public static class MatrixOperations
    {
        public static List<List<int>> CreateEmptyMatrix(int rows, int cols = -1)
        {
            if (cols < 0)
                cols = rows;

            if (rows < 0)
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(rows),
                    message: "Matrix size cannot be negative");

            List<List<int>> matrix = new();

            for (int i = 0; i < rows; i++)
            {
                matrix.Add(new());
                for (int j = 0; j < cols; j++)
                    matrix[i].Add(default);
            }

            return matrix;
        }

        public static void CloneMatrix(out List<List<int>> destination, List<List<int>> source)
        {
            destination = new();
            for (int i = 0; i < source.Count; i++)
            {
                destination.Add(new());
                for (int j = 0; j < source[i].Count; j++)
                    destination[i].Add(source[i][j]);
            }
        }

        public static List<List<int>> Transponse(List<List<int>> matrix)
        {
            var transposed = CreateEmptyMatrix(matrix[0].Count, matrix.Count);

            for (int i = 0; i < matrix.Count; i++)
            {
                if (matrix[i].Count != transposed.Count)
                    throw new ArgumentException("Invalid matrix");

                for (int j = 0; j < matrix[i].Count; j++)
                    transposed[j][i] = matrix[i][j];
            }

            return transposed;
        }

        public static List<List<int>> LogicalSum(List<List<List<int>>> matrixes)
        {
            CloneMatrix(out List<List<int>> logicalSum, matrixes[0]);

            for (int k = 0; k < matrixes.Count; k++)
            {
                if (matrixes[k].Count != matrixes[0].Count)
                    throw new ArgumentException("Cannot logically add matrixes of different sizes");

                for (int i = 0; i < matrixes[k].Count; i++)
                {
                    if (matrixes[k][i].Count != matrixes[0][i].Count)
                        throw new ArgumentException("Cannot logically add matrixes of different sizes");

                    for (int j = 0; j < matrixes[k][i].Count; j++)
                        logicalSum[i][j] |= matrixes[k][i][j];
                }
            }

            return logicalSum;
        }

        public static List<List<int>> LogicalProduct(List<List<int>> matrix1, List<List<int>> matrix2)
        {
            if (matrix1.Count != matrix2.Count)
                throw new ArgumentException("Cannot logically multiply matrixes of different sizes");

            List<List<int>> logicalProduct = new();

            for (int i = 0; i < matrix1.Count; i++)
            {
                if (matrix1.Count != matrix1[i].Count || matrix2.Count != matrix2[i].Count)
                    throw new ArgumentException("Cannot logically multiply non-square matrixes");

                logicalProduct.Add(new());
                for (int j = 0; j < matrix1.Count; j++)
                {
                    logicalProduct[i].Add(0);
                    for (int k = 0; k < matrix1.Count; k++)
                        logicalProduct[i][j] |= matrix1[i][k] & matrix2[k][j];
                }
            }

            return logicalProduct;
        }

        public static List<List<int>> GetReachabilityMatrix_Powers(List<List<int>> adjacencyMatrix)
        {
            List<List<List<int>>> powers = new() { adjacencyMatrix };

            for (int i = 1; i < adjacencyMatrix.Count; i++)
                powers.Add(LogicalProduct(powers[^1], adjacencyMatrix));

            return LogicalSum(powers);
        }

        public static List<List<int>> GetReachabilityMatrix_Warshall(List<List<int>> adjacencyMatrix)
        {
            CloneMatrix(out List<List<int>> currentMatrix, adjacencyMatrix);
            CloneMatrix(out List<List<int>> reachabilityMatrix, currentMatrix);

            for (int k = 0; k < adjacencyMatrix.Count; k++)
            {
                for (int i = 0; i < adjacencyMatrix.Count; i++)
                    for (int j = 0; j < adjacencyMatrix.Count; j++)
                        currentMatrix[i][j] = currentMatrix[i][j] | (currentMatrix[i][k] & currentMatrix[k][j]);

                CloneMatrix(out reachabilityMatrix, currentMatrix);
            }

            return reachabilityMatrix;
        }

        public static void PrintMatrix(List<List<int>> matrix, List<string> vertexes)
        {
            Console.WriteLine();
            Console.Write("    ");
            Console.ForegroundColor = ConsoleColor.Cyan;

            for (int i = 0; i < matrix.Count; i++)
                Console.Write($"{vertexes[i],4}");
            Console.WriteLine();

            for (int i = 0; i < matrix.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{vertexes[i],4}");
                Console.ForegroundColor = ConsoleColor.White;

                for (int j = 0; j < matrix[i].Count; j++)
                    Console.Write($"{(matrix[i][j] == int.MaxValue ? "-" : matrix[i][j]),4}");
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
