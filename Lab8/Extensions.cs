using System;
using System.Numerics;

namespace Lab8
{
    public static class Extensions
    {
        /// <summary>
        /// Найти первообразное число по модулю
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int GetAntiderivative(int p)
        {
            int g = 1;
            int value = p + 1;

            while (value >= p)
            {
                int f = Euler(p);
                value = Convert.ToInt32(Math.Pow(g, f) % p);

                g++;
            }
            
            return g;
        }
        /// <summary>
        /// Найти функцию Эйлера
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int Euler(int m)
        {
            int result = m;
            for (int i = 2; i*i <= m; ++i)
            {
                if (m % i == 0)
                {
                    while (m % i == 0) m /= i;
                    result -= result / i;
                }

            }

            if (m > 1) result -= result / m;
            return result;
        }

        /// <summary>
        /// Вычислить обратное числу значение
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
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
        /// Вычислить обратное числу значение
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigInteger GetBackValue(BigInteger m, BigInteger n)
        {
            BigInteger value = 1;

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
                    x -= y;
                else
                    y -= x;
            }
            return x;
        }
    }
}
