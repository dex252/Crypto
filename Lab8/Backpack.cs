using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab8
{
    class Backpack
    {
        private string Message { get; }
        private int m;
        private int n;
        public Backpack(string message, int firstValueOfCloseKey)
        {
            Message = message.ToUpper();

            StartCrypto(firstValueOfCloseKey);
        }

        private void StartCrypto(int firstValueOfCloseKey)
        {
            if (firstValueOfCloseKey < 2) firstValueOfCloseKey = 2;
            var closeKey = GetCloseKey(firstValueOfCloseKey);
        
            var openKey = GetOpenKey(closeKey);
            var bin = GetBinWin1251();
            var encrypt = GetEncrypt(openKey, bin);
            var _n = Extensions.GetBackValue(m, n);

            var decrypt = GetDecrypt(encrypt, closeKey, _n);
            var message = GetDecryptText(decrypt);

            PrintResults(closeKey, openKey, bin, encrypt, decrypt, message);
        }

        private void PrintResults(List<int> closeKey, List<int> openKey, List<string> bin, List<int> encrypt, List<string> decrypt, string message)
        {
            Console.WriteLine();
            Console.WriteLine("Исходное сообщение: " + Message);
            
            Console.WriteLine();
            Console.Write("Закрытый ключ:   ");
            foreach (var c in closeKey)
            {
                Console.Write($"{c, 8}");
            }

            Console.WriteLine();
            Console.Write("Открытый ключ:   ");
            foreach (var c in openKey)
            {
                Console.Write($"{c,8}");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Текст исходного сообщения:        ");
            foreach (var c in Message)
            {
                Console.Write($"{c,10}");
            }

            Console.WriteLine();
            Console.Write("Bin коды  шифруемого сообщения:   ");
            foreach (var c in bin)
            {
                Console.Write($"{c,10}");
            }

            Console.WriteLine();
            Console.Write("Шифрограмма:                      ");
            foreach (var c in encrypt)
            {
                Console.Write($"{c,10}");
            }

            Console.WriteLine();
            Console.Write("Расшифровка:                      ");
            foreach (var c in decrypt)
            {
                Console.Write($"{c,10}");
            }


            Console.WriteLine();
            Console.Write("Расшифрованное сообщение:         ");
            foreach (var c in message)
            {
                Console.Write($"{c,10}");
            }
        }

        private string GetDecryptText(List<string> decrypt)
        {
            string message = "";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1251");

            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            foreach (var d in decrypt)
            {
                string dec = Convert.ToString(Convert.ToInt32(d, 2), 10);
                byte[] win1251Bytes = new byte[1]
                {
                    Convert.ToByte(dec)
                };
                byte[] utf8Bytes = Encoding.Convert(win1251, utf8, win1251Bytes);
                var sym = Encoding.UTF8.GetString(utf8Bytes); ;
                message += sym;
            }

            return message;
        }

        /// <summary>
        /// Получение расшифрованного сообщения в формате 1251
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="closeKey"></param>
        /// <param name="_n"></param>
        /// <returns></returns>
        private List<string> GetDecrypt(List<int> encrypt, List<int> closeKey, int _n)
        {
            var decrypt = new List<string>();

            foreach (var e in encrypt)
            {
                var backValue = (e * _n) % m;

                var value = 0; //это значение которое получится
                var binaryList = "00000000";
                var closeTemp = new List<int>(closeKey);
                while (backValue != value)
                {
                    int num = 0;
                    if (value == 0) num = closeTemp.Where(s => s < backValue).Max();
                    else num = closeTemp.Where(s => s <= backValue - value).Max();

                    var index = closeKey.IndexOf(num);
                    binaryList = binaryList.Substring(0, index) + '1' + binaryList.Substring(index + 1);
                    closeTemp.Remove(num);

                    value += num;
                }
                decrypt.Add(binaryList);
            }

            return decrypt;
        }

        /// <summary>
        /// Получаем зашифрованное сообщение
        /// </summary>
        /// <param name="openKey"></param>
        /// <param name="bin"></param>
        /// <returns></returns>
        private List<int> GetEncrypt(List<int> openKey, List<string> bin)
        {
            var encrypt = new List<int>();

            foreach (var b in bin)
            {
                int weight = 0;
                for (int i = 0; i < b.Length; i++)
                {
                    if (b[i] == '1') weight += openKey[i];
                }
                encrypt.Add(weight);
            }

            return encrypt;
        }

        /// <summary>
        /// Получаем бинарное представление входящего сообщения в кодировке 1251
        /// </summary>
        /// <returns></returns>
        private List<string> GetBinWin1251()
        {
            var bin = new List<string>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1251");

            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            foreach (var m in Message)
            {
                byte[] utf8Bytes = utf8.GetBytes(m.ToString());
                byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                string binary = Convert.ToString(win1251Bytes[0], 2);
                bin.Add(binary);
            }

            return bin;
        }

        /// <summary>
        /// Получение открытого ключа
        /// </summary>
        /// <param name="closeKey">Закрытый ключ</param>
        /// <returns></returns>
        private List<int> GetOpenKey(List<int> closeKey)
        {
            var key = new List<int>();
            int sum = 0;
            foreach (var num in closeKey)
            {
                sum += num;
            }

            m = sum + 1;

            n = Extensions.GetSimple(m);

            foreach (var num in closeKey)
            {
                int value = (num * n) % m;
                key.Add(value);
            }
            return key;
        }

        /// <summary>
        /// Получение закрытого ключа
        /// </summary>
        /// <param name="first">Первое число в последовательности</param>
        /// <returns></returns>
        private List<int> GetCloseKey(int first)
        {
            var key = new List<int>();
            key.Add(first);
            int sum = first;

            for (int i = 0; i < 7; i++)
            {
                int value = sum + new Random().Next(1, 5);
                key.Add(value);
                sum += value;
            }

            return key;
        }
    }
}
