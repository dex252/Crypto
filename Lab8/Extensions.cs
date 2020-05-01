namespace Lab8
{
    public static class Extensions
    {
        public static int GetBackValue(int m, int n)
        {
            int value = 1;

            while ((n * value) % m != 1)
            {
                value++;
            }

            return value;
        }
        /// <summary>
        /// Получить взаимно простое число для n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int GetSimple(int n)
        {
            int d = 1;

            int nod = 0;

            while (nod != 1)
            {
                d++;
                nod = Nod(d, n);
            }

            return d;
        }
        /// <summary>
        /// Вычислить наибольший общий делитель
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static int Nod(int x, int y)
        {
            while (x != y)
            {
                if (x > y)
                    x = x - y;
                else
                    y = y - x;
            }
            return x;
        }
    }
}
