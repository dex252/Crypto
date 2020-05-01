using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab8
{
    class ElGamal
    {

        private string Message { get; }
        private readonly string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private readonly List<int> messageList = new List<int>();
        /// <summary>
        /// Простое число. Часть открытого ключа.
        /// </summary>
        private int p;
        /// <summary>
        /// Первообразная число по модулю p, меньшее, чем p. Часть открытого ключа.
        /// </summary>
        private int g;
        /// <summary>
        /// Результат выражения y=g^x mod p. Часть открытого ключа.
        /// </summary>
        private int y;
        /// <summary>
        /// Случайное число, меньшее, чем p. Закрытый ключ.
        /// </summary>
        private int x;
        /// <summary>
        /// Случайное число.
        /// </summary>
        private int k = new Random().Next(1, 333);
        /// <summary>
        /// Первая часть шифрограммы.
        /// </summary>
        private int a;
        /// <summary>
        /// Вторая часть шифрограммы.
        /// </summary>
        List<BigInteger> b = new List<BigInteger>();
        /// <summary>
        /// Число p - простое!
        /// </summary>
        /// <param name="абрамов"></param>
        /// <param name="i"></param>
        public ElGamal(string message, int p)
        {
            Message = message.ToUpper();
            this.p = p;

            Crypto();
        }

        private void PrintResults(BigInteger ax, List<BigInteger> openMessage, string openText)
        {
            Console.WriteLine($"Шифруемое слово      :   {Message}");
            Console.WriteLine($"Закрытый ключ x      :   {x}");
            Console.WriteLine($"Открытый ключ (y,g,p):  ({y},{g},{p})\n");

            Console.Write($"Шифруемое сообщение текстом              :");
            foreach (var m in Message)
            {
                Console.Write($"{m,8}");
            }
            Console.WriteLine();

            Console.Write($"Шифруемое сообщение символами алфавита   :");
            foreach (var m in messageList)
            {
                Console.Write($"{m,8}");
            }
            Console.WriteLine();

            Console.Write($"Случайное число k                        :");
            foreach (var m in b)
            {
                Console.Write($"{k,8}");
            }
            Console.WriteLine();

            Console.Write($"Первая часть шифрограммы                 :");
            foreach (var m in b)
            {
                Console.Write($"{a,8}");
            }
            Console.WriteLine();

            Console.Write($"Вторая часть шифрограммы                 :");
            foreach (var m in b)
            {
                Console.Write($"{m,8}");
            }
            Console.WriteLine();

            Console.Write($"Обратное значение числа a^x              :");
            foreach (var m in b)
            {
                Console.Write($"{ax,8}");
            }
            Console.WriteLine();

            Console.Write($"Открытое сообщение                       :");
            foreach (var m in openMessage)
            {
                Console.Write($"{m,8}");
            }
            Console.WriteLine();

            Console.Write($"Открытое сообщение текстом               :");
            foreach (var m in openText)
            {
                Console.Write($"{m,8}");
            }
            Console.WriteLine("\n");
            Console.WriteLine($"Результат: {openText}");
        }

        private void Crypto()
        {
            g = Extensions.GetAntiderivative(p);
            
            x = 5; 
            y = Convert.ToInt32(Math.Pow(g, x) % p);

            foreach (var m in Message)
            {
                messageList.Add(alphabet.IndexOf(m) + 1);
            }
        
            k = 7;
            a = Convert.ToInt32(Math.Pow(g, k) % p);

            foreach (var m in messageList)
            {
                BigInteger value = BigInteger.Pow(y, k) * m % p;
                b.Add(value);
            }

            var ax = BigInteger.Pow(a, x);

            var backAx = g * (ax * g % p);

            List<BigInteger> openMessage = GetOpenMessage(backAx);

            string openText = GetMessage(openMessage);

            PrintResults(backAx, openMessage, openText);
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
        private List<BigInteger> GetOpenMessage(in BigInteger backAx)
        {
            var openMessage = new List<BigInteger>();

            foreach (var m in b)
            {
                var t = m * backAx % 37;
                openMessage.Add(t);
            }

            return openMessage;
        }
    }
}
