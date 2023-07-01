namespace Helpers
{
    public static class HelperExtensions
    {
        public static int GetWidth<T>(this T[,] arr)
        {
            return arr.GetLength(0);
        }

        public static int GetHeight<T>(this T[,] arr)
        {
            return arr.GetLength(1);
        }
    }
}