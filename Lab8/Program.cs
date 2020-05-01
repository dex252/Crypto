using System;

namespace Lab8
{
    class Program
    {
        static void Main()
        {
            // Backpack backpack = new Backpack("Солодков", 4);
            ElGamal elGamal = new ElGamal("Солодков", 37);
            // RSA rsa = new RSA(1447, 3191, "Солодков");
            
            Console.ReadKey();
        }
    }
}
