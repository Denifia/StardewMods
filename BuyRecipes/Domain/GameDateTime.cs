using System;

namespace Denifia.Stardew.BuyRecipes.Domain
{
    internal class GameDateTime : IComparable<GameDateTime>
    {
        /// <summary>
        /// Deserialise a GameDateTime from its individual components.
        /// </summary>
        /// <param name="timeOfDay">Time of day.</param>
        /// <param name="dayOfMonth">Day of the month.</param>
        /// <param name="currentSeason">String representation of the Season.</param>
        /// <param name="year">Year.</param>
        /// <returns>GameDateTime representing an in game DateTime.</returns>
        public static GameDateTime Deserialise(int timeOfDay, int dayOfMonth, string currentSeason, int year)
        {
            return new GameDateTime(timeOfDay, dayOfMonth, SeasonAsInt(currentSeason), year);
        }

        private int _timeOfDay;
        private int _dayOfMonth;
        private int _season;
        private int _year;

        public int TimeOfDay { get => _timeOfDay; }
        public int DayOfMonth { get => _dayOfMonth; }
        public int Season { get => _season; }
        public int Year { get => _year; }

        public GameDateTime(int timeOfDay, int dayOfMonth, int season, int year)
        {
            _timeOfDay = timeOfDay;
            _dayOfMonth = dayOfMonth;
            _season = season;
            _year = year;
        }

        /// <summary>
        /// Gets the start of the current week.
        /// </summary>
        /// <returns>The start of the current week (sunday).</returns>
        public GameDateTime GetStartOfWeek()
        {
            var startDayOfWeek = (((_dayOfMonth / 7) + 1) * 7) - 6;
            return new GameDateTime(_timeOfDay, startDayOfWeek, _season, _year);
        }

        private static int SeasonAsInt(string season)
        {
            switch (season)
            {
                case "spring":
                    return 1;
                case "summer":
                    return 2;
                case "fall":
                    return 3;
                case "winter":
                    return 4;
                default:
                    return 0;
            }
        }

        /****
         * IComparable<GameDateTime>
         ****/

        public int CompareTo(GameDateTime other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            if (Year != other.Year) return Year.CompareTo(other.Year);
            if (Season != other.Season) return Season.CompareTo(other.Season);
            if (DayOfMonth != other.DayOfMonth) return DayOfMonth.CompareTo(other.DayOfMonth);
            if (TimeOfDay != other.TimeOfDay) return TimeOfDay.CompareTo(other.TimeOfDay);

            return 0;
        }

        public static int Compare(GameDateTime left, GameDateTime right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }
            if (ReferenceEquals(left, null))
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        public override bool Equals(object obj)
        {
            GameDateTime other = obj as GameDateTime;
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            var stringRepresentation = $"{TimeOfDay}{DayOfMonth}{Season}{Year}";
            return stringRepresentation.GetHashCode();
        }

        public static bool operator ==(GameDateTime left, GameDateTime right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(GameDateTime left, GameDateTime right)
        {
            return !(left == right);
        }
        public static bool operator <(GameDateTime left, GameDateTime right)
        {
            return (Compare(left, right) < 0);
        }
        public static bool operator >(GameDateTime left, GameDateTime right)
        {
            return (Compare(left, right) > 0);
        }
    }
}
