using System;
using System.Collections;
using System.Collections.Generic;

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

        private static readonly Dataset _dataset = new();

        private class Dataset {
            public List<DatasetRow> DatasetRows { get; set; } = new List<DatasetRow>();
        }

        private struct DatasetRow {
            public int Radius { get; set; }
            public int NumberOfPieces { get; set;}
            public double ThettaInRadians { get; set; }
        }


        static void Main(string[] args) {
            Dialog();
            var results = Calculate();
            RenderResults(results);
            Console.ReadLine();
        }

        private static double DegreesToRadians(int angle, int minutes, int seconds) 
            => (angle * Math.PI / 180) + (minutes * (Math.PI / (180 * 60))) + (seconds * (Math.PI / (180 * 60 * 60)));

        private static double AreaOfSector(double angleInRadians, int radius)
            => 0.5 * Math.Pow(radius,2) * angleInRadians;

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

        private static bool IsRadiusValid(int? radius) 
            => radius > MIN_RADIUS && radius <= MAX_RADIUS;

        private static bool IsNumberOfPiecesValid(int? numberOfPiceses)
            => numberOfPiceses > MIN_NUMBER_OF_PIECES && numberOfPiceses <= MAX_NUMBER_OF_PIECES;

        private static bool IsAngleValid(int? degrees, int? minutes, int? seconds)
            => degrees >= MIN_DEGREES && degrees <= MAX_DEGREES && minutes >= MIN_MINUTES && minutes <= MAX_MINUTES && seconds >= MIN_SECONDS && seconds <= MAX_SECONDS;

        private static bool IsNumberOfTestsValid(int? numberOfTests)
            => numberOfTests >= MIN_NUMBER_OF_TESTS && numberOfTests <= MAX_NUMBER_OF_TESTS;


        public static void Dialog() 
        {
            int? numberOfTests = null;

            while (!IsNumberOfTestsValid(numberOfTests)) 
            {
                numberOfTests = GetNumberOfTestsFromInput(Console.ReadLine());
            }

            for (var i = 0; i < numberOfTests; i++) 
            {
                var (radius, numberOfPieces, thettaDegrees, thettaMinutes, thettaSeconds) = GetPizzaParametersFromInput(Console.ReadLine());

                while (!IsRadiusValid(radius) || !IsNumberOfPiecesValid(numberOfPieces) 
                        || !IsAngleValid(thettaDegrees, thettaMinutes, thettaSeconds)) 
                {
                    (radius, numberOfPieces, thettaDegrees, thettaMinutes, thettaSeconds) = GetPizzaParametersFromInput(Console.ReadLine());
                }

                var datasetRow = new DatasetRow {
                    Radius = radius!.Value,
                    NumberOfPieces = numberOfPieces!.Value,
                    ThettaInRadians = DegreesToRadians(thettaDegrees!.Value, thettaMinutes!.Value, thettaSeconds!.Value)
                };

                _dataset.DatasetRows.Add(datasetRow);
            }

        }

        private static IEnumerable<double> Calculate() 
        {
            foreach(var test in _dataset.DatasetRows) 
            {
                var numberOfPieces = test.NumberOfPieces;
                double[] cuts = new double[numberOfPieces];
                
                for (var n = 0; n < test.NumberOfPieces; n++) 
                {
                    if (n == 0) 
                    {
                        cuts[0] = 0;
                        continue;
                    }

                    var testValue = cuts[n - 1] + test.ThettaInRadians;

                    cuts[n] = testValue > Math.PI * 2 ?
                              testValue - Math.PI * 2 :
                              testValue; 
;
                }

                var orderedCuts = cuts.OrderBy(c => c);
                var cutsArray = new List<double> { Math.PI * 2 }.Concat(orderedCuts).OrderBy(c => c).ToArray();
                var maxAngle = MaxSortedAdjacentDiff(cutsArray, cutsArray.Length);
                var maxArea = AreaOfSector(maxAngle, test.Radius);

                yield return maxArea;
            }
        }

        private static double MaxSortedAdjacentDiff(double[] arr, int n) {
  
            double maxVal = arr[0];
            double minVal = arr[0];
            for (int i = 1; i < n; i++) 
            {
                maxVal = Math.Max(maxVal, arr[i]);
                minVal = Math.Min(minVal, arr[i]);
            }

            double[] maxBucket = new double[n - 1];
            double[] minBucket = new double[n - 1];
            maxBucket = maxBucket.Select(i => double.MinValue).ToArray();
            minBucket = minBucket.Select(i => double.MaxValue).ToArray();

            double delta = (double)(maxVal - minVal) / (double)(n - 1);

            for (int i = 0; i < n; i++)
            {
                if (arr[i] == maxVal || arr[i] == minVal) 
                {
                    continue;
                }

                var indexTest = (Math.Floor((arr[i] - minVal) / delta));
                int index = (int)(Math.Floor((arr[i] - minVal) / delta));

                if (maxBucket[index] == double.MinValue) 
                {
                    maxBucket[index] = arr[i];
                }
                else 
                {
                    maxBucket[index] = Math.Max(maxBucket[index], arr[i]);
                }

                if (minBucket[index] == double.MaxValue) 
                {
                    minBucket[index] = arr[i];
                }
                else 
                {
                    minBucket[index] = Math.Min(minBucket[index], arr[i]);
                }
            }

            double prev_val = minVal;
            double max_gap = 0;
            for (int i = 0; i < n - 1; i++) 
            {
                if (minBucket[i] == double.MaxValue) 
                {
                    continue;
                }
                max_gap = Math.Max(max_gap, minBucket[i] - prev_val);
                prev_val = maxBucket[i];
            }

            max_gap = Math.Max(max_gap, maxVal - prev_val);

            return max_gap;
        }


        private static void RenderResults(IEnumerable<double> results) {
            foreach(var answer in results) 
            {
                Console.WriteLine(answer.ToString("0.000000"));
            }
        }
    }
}