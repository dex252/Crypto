using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Lab8
{
    class RSA
    {
        private int p;
        private int q;
        private List<int> message = new List<int>();
        private string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        System.Diagnostics.Stopwatch sw = new Stopwatch();
        
        public RSA(int p, int q, string word)
        {
            sw.Start();

            this.p = p;
            this.q = q;
            word = word.ToUpper();
            foreach (var w in word)
            {
                message.Add(alphabet.IndexOf(w) + 1);
            }
            Console.WriteLine("Поехали!  : " + (sw.ElapsedMilliseconds / 100.0) + " сек");
            Crypto(word);
            sw.Stop();
        }

        private void Crypto(string word)
        {
            int n = p * q;

            int _n = (p - 1) * (q - 1);

            int d = Extensions.GetSimple(_n);

            int e = GetOpenKey(_n, d);

            Console.WriteLine($"Первое простое число p: {p,8}");
            Console.WriteLine($"Второе простое число q: {q,8}");
            Console.WriteLine($"Число d               : {d,8}");
            Console.WriteLine($"Число e               : {e,8}");
            Console.WriteLine($"Открытый ключ         : ({d}, {n})");
            Console.WriteLine($"Закрытый ключ         : ({e}, {n})");

            var encrypt = Encrypt(d,n);
            var decrypt = Decrypt(encrypt, e, n);
            var outMessage = GetMessage(decrypt);

            PrintResult(encrypt, decrypt, outMessage, word);
        }

        private void PrintResult(List<BigInteger> encrypt, List<BigInteger> decrypt, string outMessage, string word)
        {
            Console.WriteLine();
            Console.Write($"Исходное сообщение текстом      : ");
            foreach (var w in word)
            {
                Console.Write($" {w,7}");
            }
            Console.WriteLine();

            Console.Write($"Исходное сообщение              : ");
            foreach (var m in message)
            {
                Console.Write($" {m,7}");
            }
            Console.WriteLine();

            Console.Write($"Зашифрованное сообщение         : ");
            foreach (var e in encrypt)
            {
                Console.Write($" {e,7}");
            }
            Console.WriteLine();

            Console.Write($"Расшифрованное сообщение        : ");
            foreach (var d in decrypt)
            {
                Console.Write($" {d,7}");
            }
            Console.WriteLine();

            Console.Write($"Расшифрованное сообщение текстом: ");
            foreach (var o in outMessage)
            {
                Console.Write($" {o,7}");
            }
        }

        /// <summary>
        /// Перевод дешифрованного сообщения в текст
        /// </summary>
        /// <param name="decrypt"></param>
        /// <returns></returns>
        private string GetMessage(List<BigInteger> decrypt)
        {
            string outMessage = "";

            foreach (var d in decrypt)
            {
                outMessage += alphabet[(int)d - 1];
            }

            return outMessage;
        }

        /// <summary>
        /// Дешифровка сообщения
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="i"></param>
        /// <param name="i1"></param>
        /// <returns></returns>
        private List<BigInteger> Decrypt(List<BigInteger> encrypt, int d, int n)
        {
            var decrypt = new List<BigInteger>();

            Console.WriteLine();
            Console.WriteLine("Дешифровка началась: " + (sw.ElapsedMilliseconds / 100.0) + " сек");
            Console.Write($"Дешифруем   :");
            foreach (var c in encrypt)
            {
                var m = BigInteger.Pow(c, d) % n;

                Console.Write($"  {m,7}");
                decrypt.Add(m);
            }

            Console.WriteLine();
            Console.WriteLine("Дешифровка закончилась: " + (sw.ElapsedMilliseconds / 100.0) + " сек");

            return decrypt;
        }

        /// <summary>
        /// Получить зашифрованное сообщение
        /// </summary>
        /// <param name="i"></param>
        /// <param name="i1"></param>
        /// <returns></returns>
        private List<BigInteger> Encrypt(int e, int n)
        {
            var encrypt = new List<BigInteger>();

            Console.WriteLine();
            Console.WriteLine("Шифрование началось: " + (sw.ElapsedMilliseconds / 100.0) + " сек");
            Console.Write($"Шифруем     :");
            foreach (var m in message)
            {
                var c = BigInteger.Pow(m, e) % n;

                Console.Write($"  {c, 7}");
                encrypt.Add(c);
            }

            Console.WriteLine();
            Console.WriteLine("Шифрование закончилось: " + (sw.ElapsedMilliseconds / 100.0) + " сек");

            return encrypt;
        }
        /// <summary>
        /// Получить открытый ключ
        /// </summary>
        /// <param name="_n"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private int GetOpenKey(int _n, int d)
        {
            int e = 2;
            while ((e * d) % _n != 1) e++;

            return e;
        }
    }
}
