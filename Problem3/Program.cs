using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable

namespace Problem3 {
    internal class Program {

        private const int MIN_RADIUS = 0;
        private const int MAX_RADIUS = 100;
        private const int MIN_NUMBER_OF_PIECES = 0;
        private const int MAX_NUMBER_OF_PIECES = 1_0000_0000;
        private const int MIN_DEGREES = 0;
        private const int MAX_DEGREES = 359;
        private const int MIN_MINUTES = 0;
        private const int MAX_MINUTES = 59;
        private const int MIN_SECONDS = 0;
        private const int MAX_SECONDS = 59;
        private const int MIN_NUMBER_OF_TESTS = 1;
        private const int MAX_NUMBER_OF_TESTS = 200;

        static void Main(string[] args) 
        {
            Calculate();
            Console.ReadLine();
        }

        private static (int? radius, int? numberOfPieces, int? thettaDegrees, int? thettaMinutes, int? thettaSeconds) GetPizzaParametersFromInput(string? inputPizzaParams) 
        {
            int? paramRadius = null;
            int? paramNumberOfPieces = null;
            int? paramThettaDegrees = null;
            int? paramThettaMinutes = null;
            int? paramThettaSeconds = null;

            if (string.IsNullOrWhiteSpace(inputPizzaParams)) 
            {
                return (paramRadius, paramNumberOfPieces, paramThettaDegrees, paramThettaMinutes, paramThettaSeconds);
            }

            var pizzaParams = inputPizzaParams.Split(" ", StringSplitOptions.TrimEntries);

            if (pizzaParams.Length < 5) 
            {
                return (paramRadius, paramNumberOfPieces, paramThettaDegrees, paramThettaMinutes, paramThettaSeconds);
            }

            if (int.TryParse(pizzaParams[0], out var radiusResult)) 
            {
                paramRadius = radiusResult;
            }

            if (int.TryParse(pizzaParams[1], out var numberOfPiecesResult)) 
            {
                paramNumberOfPieces = numberOfPiecesResult;
            }

            if (int.TryParse(pizzaParams[2], out var thettaDegreesResult))
            {
                paramThettaDegrees = thettaDegreesResult;
            }

            if (int.TryParse(pizzaParams[3], out var thettaMinutesResult)) 
            {
                paramThettaMinutes = thettaMinutesResult;
            }

            if (int.TryParse(pizzaParams[4], out var thettaSecondsResult)) 
            {
                paramThettaSeconds = thettaSecondsResult;
            }

            return (paramRadius, paramNumberOfPieces, paramThettaDegrees, paramThettaMinutes, paramThettaSeconds);
        }

        private static int? GetNumberOfTestsFromInput(string? numberOfTestsInput) 
        {
            int? numberOfTests = null;
            if (int.TryParse(numberOfTestsInput, out var numberOfTestsResult)) 
            {
                numberOfTests = numberOfTestsResult;
            }
            return numberOfTests;
        }

        public static void Calculate() 
        {
            int? numberOfTests = null;
            var pi2 = Math.PI * 2;
            var pi2List = new List<double>(1) { pi2 };
            

            while (!(numberOfTests >= MIN_NUMBER_OF_TESTS && numberOfTests <= MAX_NUMBER_OF_TESTS)) 
            {
                numberOfTests = GetNumberOfTestsFromInput(Console.ReadLine());
            }

            var result = new List<double>(numberOfTests.Value);

            for (var i = 0; i < numberOfTests; i++)
            {
                var (radius, numberOfPieces, thettaDegrees, thettaMinutes, thettaSeconds) = GetPizzaParametersFromInput(Console.ReadLine());

                while (!(radius > MIN_RADIUS && radius <= MAX_RADIUS) || !(numberOfPieces > MIN_NUMBER_OF_PIECES && numberOfPieces <= MAX_NUMBER_OF_PIECES)
                        || !(thettaDegrees >= MIN_DEGREES && thettaDegrees <= MAX_DEGREES && thettaMinutes >= MIN_MINUTES && thettaMinutes <= MAX_MINUTES && thettaSeconds >= MIN_SECONDS && thettaSeconds <= MAX_SECONDS)) 
                {
                    (radius, numberOfPieces, thettaDegrees, thettaMinutes, thettaSeconds) = GetPizzaParametersFromInput(Console.ReadLine());
                }

                var testNumberOfPieces = numberOfPieces!.Value;
                var testThettaInRadians = (thettaDegrees!.Value * Math.PI / 180) + (thettaMinutes!.Value * (Math.PI / (180 * 60))) + (thettaSeconds!.Value * (Math.PI / (180 * 60 * 60)));

                double[] cuts = new double[testNumberOfPieces];

                for (var n = 0; n < testNumberOfPieces; n++) 
                {
                    if (n == 0) 
                    {
                        cuts[0] = 0;
                        continue;
                    }

                    var testValue = cuts[n - 1] + testThettaInRadians;
                    cuts[n] = testValue > pi2 ? testValue - pi2 : testValue;
                }
           
                var cutsItems = pi2List.Concat(cuts).OrderBy(c => c).ToList();
                var maxArea = FindMaxDiff(cutsItems) * 0.5 * radius!.Value * radius!.Value;

                result.Add(maxArea);
            }

            foreach(var item in result) 
            {
                Console.WriteLine(item.ToString("0.000000"));
            }
        }

        private static double FindMaxDiff(List<double> items) 
        {
            double result = 0;
        
            for (var i = 1; i < items.Count; i++) 
            {
                var previous = items.ElementAt(i - 1);
                var current = items.ElementAt(i);

                if ((current - previous) > result) 
                {
                    result = current - previous;
                }
            }

            return result;
        }
    }
}