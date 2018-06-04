using System;

namespace Api
{
    public class Dep : IDep
    {
        public void Duck()
        {
            Console.WriteLine("DUUUCK!");
        }
    }
}