namespace AoC2020.Day20
{
    public static class MatrixHelper
    {
        public static T[][] Rotate<T>(T[][] arr, int n)
        {
            var res = new T[n][];

            for (int i = 0; i < n; ++i)
            {
                res[i] = new T[n];

                for (int j = 0; j < n; ++j)
                    res[i][j] = arr[n - j - 1][i];
            }

            return res;
        }

        public static T[][] Flip<T>(T[][] arr, int n)
        {
            var res = new T[n][];

            for (var i = 0; i < n; i++)
            {
                res[i] = new T[n];

                for (var j = 0; j < n; j++)
                    res[i][j] = arr[n - i - 1][j];
            }

            return res;
        }

        public static T[][] Transpose<T>(T[][] arr, int n)
        {
            var res = new T[n][];

            for (var i = 0; i < n; i++)
            {
                res[i] = new T[n];

                for (var j = 0; j < n; j++)
                    res[i][j] = arr[j][i];
            }

            return res;
        }
    }
}