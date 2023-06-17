namespace Porblem1 {
    internal class Program {

        private const int MIN_SPEED = 1;
        private const int MAX_SPEED = 90;
        private const int MIN_HOURS = 1;
        private const int MAX_HOURS = 12;
        private const int MIN_NUMBER_OF_LINES = 1;
        private const int MAX_NUMBER_OF_LINES = 10;

        private static string _numberOfLinesMessage = $"Please, enter number of lines ({MIN_NUMBER_OF_LINES} to {MAX_NUMBER_OF_LINES}):";
        private static string _speedHoursMessage = $"Please, enter speed in miles per hour (from {MIN_SPEED} to {MAX_SPEED}) and total epalsed time in hours (from {MIN_HOURS} to {MAX_HOURS})";
        private static string _renderMessage = "{0} miles";

        private static readonly List<Dataset> _datasets = new();

        private struct DatasetRow {
            public int Speed { get; set; }
            public int Hours { get; set; }
        }

        private class Dataset {
            public List<DatasetRow> DatasetRows { get; set; } = new List<DatasetRow>();
        }

        static void Main(string[] args) {
            InputDialog();
            RenderResults();
            Console.ReadLine();
        }

        private static bool IsValidSpeed(int? speed)
            => speed >= MIN_SPEED && speed <= MAX_SPEED;

        private static bool IsValidHours(int? hours)
            => hours >= MIN_HOURS && hours <= MAX_HOURS;

        private static bool IsEndOfDataset(int? numberOfLines)
            => numberOfLines == -1;

        private static bool IsValidNumberOfLines(int? numberOfLines)
            => numberOfLines >= MIN_NUMBER_OF_LINES && numberOfLines <= MAX_NUMBER_OF_LINES;
        
        private static (int?, int?) GetSpeedAndHoursDataInput(string? input) 
        {
            int? speed = null;
            int? hours = null;

            if (string.IsNullOrEmpty(input)) 
            {
                return (speed, hours);
            }
            
            var inputItems = input.Split(" ", StringSplitOptions.TrimEntries);

            if (inputItems.Length < 2) 
            {
                return (speed, hours);
            }

            if (int.TryParse(inputItems[0], out int speedParsedResult)) 
            {
                speed = speedParsedResult;
            }

            if (int.TryParse(inputItems[1], out  int hoursParsedResult))
            {
                hours = hoursParsedResult;
            }

            return (speed, hours);

        }

        private static int? GetNumberOfLinesDataInput(string? input) 
        {
            int? numberOfLines = null;

            if (string.IsNullOrEmpty(input)) 
            {
                return numberOfLines;
            }

            if (int.TryParse(input, out int numberOfLinesResult)) 
            {
                numberOfLines = numberOfLinesResult;
            }

            return numberOfLines;

        }

        private static int CalculateDistance(Dataset dataset) 
        {
            int distance = 0;
            
            for (var i = 0; i < dataset.DatasetRows.Count; i ++) 
            {
                var currentElement = dataset.DatasetRows.ElementAt(i);

                if (i == 0)
                {
                    distance = currentElement.Speed * currentElement.Hours;
                    continue;
                }

                var previousElement = dataset.DatasetRows.ElementAt(i - 1);
                    distance += currentElement.Speed * (currentElement.Hours - previousElement.Hours);

            }

            return distance;
        }

        private static void InputDialog() 
        {
            int? numberOfLines = null;

            while (!IsEndOfDataset(numberOfLines)) 
            {
                while (!IsValidNumberOfLines(numberOfLines))
                {
                    Console.WriteLine(_numberOfLinesMessage);
                    numberOfLines = GetNumberOfLinesDataInput(Console.ReadLine());
                }
               
                var dataset = new Dataset();

                for (var i = 0; i < numberOfLines; i++) 
                {
                    Console.WriteLine(_speedHoursMessage);

                    var (speed, hours) = GetSpeedAndHoursDataInput(Console.ReadLine());

                    while (!IsValidSpeed(speed) || !IsValidHours(hours)) 
                    {
                        Console.WriteLine(_speedHoursMessage);
                        (speed, hours) = GetSpeedAndHoursDataInput(Console.ReadLine());
                    }

                    dataset.DatasetRows.Add(new DatasetRow 
                    {
                        Speed = speed!.Value,
                        Hours = hours!.Value,
                    });
                }

                _datasets.Add(dataset);

                Console.WriteLine(_numberOfLinesMessage);
                numberOfLines = GetNumberOfLinesDataInput(Console.ReadLine());
            }
        }
        private static void RenderResults() 
        {
            foreach (var dataset in _datasets) 
            {
                var distance = CalculateDistance(dataset);
                Console.WriteLine(string.Format(_renderMessage, distance));
            }
        }

    }
}