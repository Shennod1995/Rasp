using System;
using System.IO;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder log1 = new Pathfinder(new FileLogWritter("log.txt"));
            Pathfinder log2 = new Pathfinder(new ConsoleLogWritter());
            Pathfinder log3 = new Pathfinder(new SpecificDayOfWeekLogWriter(DayOfWeek.Friday, new FileLogWritter("log.txt")));
            Pathfinder log4 = new Pathfinder(new SpecificDayOfWeekLogWriter(DayOfWeek.Friday, new ConsoleLogWritter()));
            Pathfinder log5 = new Pathfinder(new CombineLogWriter(log2, log3));
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder
    {
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Find(string message)
        {
            _logger.WriteError("Что-то пишет в лог");
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        private readonly string _path;

        public FileLogWritter(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public void WriteError(string message)
        {
            File.WriteAllText(_path, message);
        }
    }

    class SpecificDayOfWeekLogWriter : ILogger
    {
        private readonly DayOfWeek _dayOfWeek;
        private readonly ILogger _logger;

        public SpecificDayOfWeekLogWriter(DayOfWeek dayOfWeek, ILogger logger)
        {
            _dayOfWeek = dayOfWeek;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _dayOfWeek)
            {
                _logger.WriteError(message);
            }
        }
    }

    class CombineLogWriter : ILogger
    {
        private readonly Pathfinder _consoleLogWritter;
        private readonly Pathfinder _specificDayOfWeekLogWriter;

        public CombineLogWriter(Pathfinder consoleLogWritter, Pathfinder specificDayOfWeekLogWriter)
        {
            _consoleLogWritter = consoleLogWritter ?? throw new ArgumentNullException(nameof(consoleLogWritter));
            _specificDayOfWeekLogWriter = specificDayOfWeekLogWriter ?? throw new ArgumentNullException(nameof(specificDayOfWeekLogWriter));
        }

        public void WriteError(string message)
        {
            _consoleLogWritter.Find(message);
            _specificDayOfWeekLogWriter.Find(message);
        }
    }
}