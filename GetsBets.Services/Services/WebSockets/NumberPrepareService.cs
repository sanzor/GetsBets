using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class NumberPrepareService : INumberPrepareService
    {
        public Either<Error, List<SendNumber>> PrepareRandomizedNumbers(string numbers)
        {
            var list=Try(() =>
            {
                var sendNumbers = GetNumbers(numbers);
                var randomizedList = RandomizeNumbers(sendNumbers);
                return randomizedList;
            }).ToEither(Error.New);
            return list;

        }
        private List<SendNumber> GetNumbers(string numbers)
        {
            int position = 0;
            List<SendNumber> sendNumbers = new List<SendNumber>();
            foreach (var item in numbers.Split(','))
            {
                sendNumbers.Add(new SendNumber { Id = position++, Value = item });
            }
            return sendNumbers;

        }
        private static Random rnd = new Random();
        private List<SendNumber> RandomizeNumbers(List<SendNumber> originals)
        {
            
            int n = originals.Count;
            while (n > 1)
            {
                n--;
                int position = rnd.Next(n + 1);
                SendNumber number = originals[position];
                originals[position] = originals[n];
                originals[n] = number;
            }
            return originals;
        }
    }
}
