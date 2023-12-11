namespace AdventOfCode2023;

public static class Utils
{
    public static void InitMatrix<T>(T[,] matrix, T value)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = value;
            }
        }
    }

    public static bool IsOutsideMatrix<T>(int i, int j, T[,] matrix)
    {
        return i < 0 || i >= matrix.GetLength(0) || j < 0 || j >= matrix.GetLength(1);
    }

    public static T[,] To2DimensionalArray<T>(List<List<T>> matrix)
    {
        var result = new T[matrix.Count, matrix[0].Count];

        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[0].Count; j++)
            {
                result[i, j] = matrix[i][j];
            }
        }

        return result;
    }

    public static char[,] To2DimensionalArray(string[] matrix)
    {
        var result = new char[matrix.Length, matrix[0].Length];

        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                result[i, j] = matrix[i][j];
            }
        }

        return result;
    }

    public static void Print(string[] matrix, int sleepMs = 0)
    {
        Print(To2DimensionalArray(matrix), sleepMs);
    }

    public static void Print<T>(List<List<T>> matrix, int sleepMs = 0)
    {
        Print(To2DimensionalArray(matrix), sleepMs);
    }

    public static void Print<T>(T[,] matrix, int sleepMs = 0)
    {
        Console.Clear();

        for (int i = 0; i < matrix.GetLength(1); i++)
            Console.Write('-');
        Console.WriteLine();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write(matrix[i, j]);

            Console.WriteLine();
        }

        for (int i = 0; i < matrix.GetLength(1); i++)
            Console.Write('-');
        Console.WriteLine();

        if (sleepMs > 0)
            Thread.Sleep(sleepMs);
    }
}

