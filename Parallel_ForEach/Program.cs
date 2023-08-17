using System.Collections.Concurrent;

namespace Parallel_ForEach
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started..");

            //int number = 12;
            //if(IsPrime(number))
            //    Console.WriteLine("prime");
            //Console.WriteLine("non prime");

            //Console.ReadLine();

            Console.WriteLine("started Concurrent..");
            var numbers = Enumerable.Range(0, 100).ToList();
            var result = GetPrimeNumbersConcurrent(numbers);
            foreach (var number in result)
            {
                Console.WriteLine($"Prime Number: {string.Format("{0:0000}", number.Key)}, Managed Thread Id: {number.Value} ");
            }
             

            Console.WriteLine("end Concurrent..");

            Console.WriteLine("started Parallel..");
            var numbersParallel = Enumerable.Range(0, 100).ToList();
            var resultParallel = GetPrimeNumbersConcurrent(numbersParallel);
            foreach (var numberParallel in resultParallel)
            {
                Console.WriteLine($"Prime Number: {string.Format("{0:0000}", numberParallel.Key)}, Managed Thread Id: {numberParallel.Value} ");
            }

            Console.WriteLine("end Parallel..");

            Console.ReadLine();
        }

        #region Get PrimeNumbers Concurrent
        private static ConcurrentDictionary<int, int> GetPrimeNumbersConcurrent(IList<int> numbers)
        {
            var primes = new ConcurrentDictionary<int, int>();
            foreach (var number in numbers)
            {
                if (IsPrime(number))
                {
                    primes.TryAdd(number,
                    Thread.CurrentThread.ManagedThreadId);
                }
            }
            return primes;
        }

        #endregion

        #region GetPrimeNumbersParallel
        private static ConcurrentDictionary<int, int> GetPrimeNumbersParallel(IList<int> numbers)
        {
            var primes = new ConcurrentDictionary<int, int>();
            
            Parallel.ForEach(numbers, number =>
            {
                if (IsPrime(number))
                {
                    primes.TryAdd(number,
                    Thread.CurrentThread.ManagedThreadId);
                }
            });
            return primes;
        }

        #endregion

        #region IsPrime
        static bool IsPrime(int integer)
        {
            if (integer <= 1) return false;
            if (integer == 2) return true;
            var limit = Math.Ceiling(Math.Sqrt(integer));
            for (int i = 2; i <= limit; ++i)
                if (integer % i == 0)
                    return false;
            return true;
        }

        #endregion 
    }
}