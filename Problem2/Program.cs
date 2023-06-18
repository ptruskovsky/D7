using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Problem2 
{
    internal class Program 
    {

        public const int MIN_NUMBER_OF_CLEARINGS = 2;
        public const int MAX_NUMBER_OF_CLEARINGS = 20;
        public const int MIN_NUMBER_OF_PATHS = 1;
        public const int ONE_ATTEMPT_TIME_MINUTES = 1;
        static void Main(string[] args) 
        {
            Dialog();
            Console.ReadLine();
        }

        private struct WayPoint 
        {
            public readonly string Id 
            {
                get 
                {
                    return $"X={X}, Y={Y}";
                }
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        // TODO !
        private static double GetAverageTime(List<WayPoint> dataset) 
        {
            var starting = GetStartingPoint(dataset);
            var exit = GetWayoutPoint(dataset);
            int totalMinutes = 0;
            int totalAttempts = 0;
            
            var zeroPoints = dataset.Where(el => el.X == starting).ToList();

            foreach (var zeroPoint in zeroPoints) 
            {
                totalAttempts++;
                Go(dataset, zeroPoint, exit, ref totalMinutes, ref totalAttempts);
            }

            return (double)totalMinutes / totalAttempts;    
        }
        
        // TODO !
        private static WayPoint Go(List<WayPoint> dataset, WayPoint currentPoint, int exit, ref int totalMinutes, ref int totalAttempts) 
        {
            totalMinutes += ONE_ATTEMPT_TIME_MINUTES;
    
            if (!IsWayout(exit, currentPoint)) 
            {
                var nextPoint = dataset.FirstOrDefault(el => el.X == currentPoint.Y);
                currentPoint = Go(dataset, nextPoint, exit, ref totalMinutes, ref totalAttempts);
            }

            return currentPoint;
        }

        private static bool IsWayout(int endPoint, WayPoint wayPoint) 
            => wayPoint.Y == endPoint;

        private static int GetWayoutPoint(List<WayPoint> dataset) 
        {
            var maxX = dataset.Max(el => el.X);
            var maxY = dataset.Max(el => el.Y);

            return maxY > maxX ? maxY : maxX;
        }

        private static int GetStartingPoint(List<WayPoint> dataset) 
        {
            var minX = dataset.Min(el => el.X);
            var minY = dataset.Min(el => el.Y);

            return minY < minX ? minY : minX;
        }

        private static (int x, int y) GetXYFromInput(string? input) 
        {
            int x = 0;
            int y = 0;

            if (string.IsNullOrEmpty(input)) 
            {
                return (x, y);
            }

            var xy = input.Split(" ", StringSplitOptions.TrimEntries);

            if (xy.Length < 2) 
            {
                return (x, y);
            }

            if (int.TryParse(xy[0], out int xResult)) 
            {
                x = xResult;
            }

            if (int.TryParse(xy[1], out int yResult))
            {
                y = yResult;
            }

            return (x, y);
        }

        public static void Dialog() 
        {
            var numberOfClearings = 0;
            var numberOfPaths = 0;

            while (!(numberOfClearings >= MIN_NUMBER_OF_CLEARINGS && numberOfClearings <= MAX_NUMBER_OF_CLEARINGS) ||
                 !(numberOfPaths >= MIN_NUMBER_OF_PATHS)) {
                (numberOfClearings, numberOfPaths) = GetInitialParams(Console.ReadLine());
            }

            var data = new List<WayPoint>(numberOfPaths);

            for (var i = 0; i < numberOfPaths; i++) 
            {
                var (x, y) = GetXYFromInput(Console.ReadLine());

                data.Add(new WayPoint {
                    X = x,
                    Y = y,
                });
            }

            //data.Add(new WayPoint { X = 0, Y = 1 });
            //data.Add(new WayPoint { X = 1, Y = 2 });
            //data.Add(new WayPoint { X = 0, Y = 2 });

            var time = GetAverageTime(data);

            Console.Clear();
            Console.WriteLine(time.ToString("0.000000"));
        }

        private static (int numberOfClearings, int numberOfPaths) GetInitialParams(string? input) 
        {
            int paramNumberOfClearings = 0;
            int paramNumberOfPaths = 0;
         
            if (string.IsNullOrEmpty(input)) 
            {
                return (paramNumberOfClearings, paramNumberOfPaths);
            }

            var topNumbers = input.Split(" ", StringSplitOptions.TrimEntries);

            if (topNumbers.Length < 2) 
            {
                return (paramNumberOfClearings, paramNumberOfPaths);
            }

            if (int.TryParse(topNumbers[0], out int numberOfClearingsResult)) 
            {
                paramNumberOfClearings = numberOfClearingsResult;
            }

            if (int.TryParse(topNumbers[1], out int paramNumberOfPathsResult))
            {
                paramNumberOfPaths = paramNumberOfPathsResult;
            }

            return (paramNumberOfClearings, paramNumberOfPaths);
        }
    }
}