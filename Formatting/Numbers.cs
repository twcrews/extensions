using System;
using System.Collections.Generic;

namespace Crews.Extensions.Formatting
{
	/// <summary>
	/// Contains extension functions for numeric values.
	/// </summary>
	public static class Numbers
	{
		private static readonly Dictionary<int, string> TextNumbers = new Dictionary<int, string>()
		{
			{90, "ninety"}, {80, "eighty"}, {70, "seventy"}, {60, "sixty"}, {50, "fifty"},
			{40, "forty"}, {30, "thirty"}, {20, "twenty"}, {0, "zero"}, {1, "one"}, {2, "two"}, {3, "three"},
			{4, "four"}, {5, "five"}, {6, "six"}, {7, "seven"}, {8, "eight"}, {9, "nine"},
			{10, "ten"}, {11, "eleven"}, {12, "twelve"}, {13, "thirteen"}, {14, "fourteen"},
			{15, "fifteen"}, {16, "sixteen"}, {17, "seventeen"}, {18, "eighteen"}, {19, "nineteen"}
		};

		private static readonly Dictionary<string, int> PrecisionMap =
			new Dictionary<string, int>()
		{
			{"year", 0}, {"month", 1}, {"week", 2}, {"day", 3 },
				{"hour", 4}, {"minute", 5}, {"second", 6}
		};

		/// <summary>
		/// Formats an integer between 0 and 99 (inclusive) as text. 
		/// Other integers return a ToString of themselves.
		/// </summary>
		/// <param name="input">The integer to format.</param>
		/// <returns>Returns a string representing the number.</returns>
		public static string ToWord(this int input)
		{
			if (input < 0 || input > 99)
			{
				return input.ToString();
			}

			if (TextNumbers.TryGetValue(input, out string returnString))
			{
				return returnString;
			}

			foreach (KeyValuePair<int, string> pair in TextNumbers)
			{
				if (input < pair.Key)
				{
					continue;
				}

				return pair.Value + "-" + TextNumbers[input - pair.Key];
			}

			return input.ToString();
		}

		/// <summary>
		/// Formats a double with a given unit.
		/// </summary>
		/// <param name="quantity">The value of the quantity.</param>
		/// <param name="unit">The unit of measurement.</param>
		/// <returns>
		/// Returns a string representing the formatted quantity and unit of measurement.
		/// </returns>
		public static string ToQuantity(this double quantity, string unit)
		{
			if (quantity != 1)
			{
				if (unit.EndsWith("y"))
				{
					unit = unit.TrimEnd('y') + "ies";
				}
				else
				{
					unit = unit + "s";
				}
			}

			return $"{quantity.ToString()} {unit}".Trim();
		}

		/// <summary>
		/// Formats an int with a given unit.
		/// </summary>
		/// <param name="quantity">The value of the quantity.</param>
		/// <param name="unit">The unit of measurement.</param>
		/// <returns>
		/// Returns a string representing the formatted quantity and unit of measurement.
		/// </returns>
		public static string ToQuantity(this int quantity, string unit)
			=> ToQuantity((double)quantity, unit);

		/// <summary>
		/// Formats a TimeSpan as text.
		/// </summary>
		/// <param name="timeSpan">The TimeSpan object to format.</param>
		/// <param name="precision">The TimeUnit of precision to use.</param>
		/// <returns>Returns a string representation of the TimeSpan.</returns>
		public static string ToQuantity(this TimeSpan timeSpan, TimeUnit precision)
		{
			if (timeSpan.TotalSeconds <= 0)
			{
				throw new ArgumentException("Cannot format zero or negative TimeStamps.");
			}

			string returnString = string.Empty;

			int years = 0;
			int months = 0;
			int weeks = 0;

			if (timeSpan.TotalDays >= 365)
			{
				years = timeSpan.Days / 365;
				returnString += ToQuantity(years, "year") + ", ";
			}
			else if (precision == TimeUnit.Year)
			{
				return ToQuantity(timeSpan.TotalDays / 365, "year");
			}

			if (timeSpan.TotalDays >= 30 && precision >= TimeUnit.Month)
			{
				months = timeSpan.Days / 30 - years * 12;
				returnString += months > 0 ?
					ToQuantity(months, "month") + ", " : string.Empty;
			}
			else if (precision == TimeUnit.Month)
			{
				return ToQuantity(timeSpan.TotalDays / 30, "month");
			}

			if (timeSpan.TotalDays >= 7 && precision >= TimeUnit.Week)
			{
				weeks = timeSpan.Days / 7 - years * 52 - months * 4;
				returnString += weeks > 0 ?
					ToQuantity(weeks, "week") + ", " : string.Empty;
			}
			else if (precision == TimeUnit.Week)
			{
				return ToQuantity(timeSpan.TotalDays / 7, "week");
			}

			if (precision >= TimeUnit.Day)
			{
				int days = timeSpan.Days - years * 365 - months * 30 - weeks * 7;
				returnString += days > 0 ?
					ToQuantity(days, "day") + ", " : string.Empty;
				if (timeSpan.TotalDays < 1 && precision == TimeUnit.Day)
				{
					return ToQuantity(timeSpan.TotalDays, "day");
				}
			}

			if (precision >= TimeUnit.Hour)
			{
				returnString += timeSpan.Hours > 0 ?
					ToQuantity(timeSpan.Hours, "hour") +
					", " : string.Empty;
				if (timeSpan.TotalHours < 1 && precision == TimeUnit.Hour)
				{
					return ToQuantity(timeSpan.TotalHours, "hour");
				}
			}

			if (precision >= TimeUnit.Minute)
			{
				returnString += timeSpan.Minutes > 0 ?
					ToQuantity(timeSpan.Minutes, "minute") +
					", " : string.Empty;
				if (timeSpan.TotalMinutes < 1 && precision == TimeUnit.Minute)
				{
					return ToQuantity(timeSpan.TotalMinutes, "minute");
				}
			}

			if (precision >= TimeUnit.Second)
			{
				returnString += timeSpan.Seconds > 0 ?
					ToQuantity(timeSpan.Seconds, "second") : string.Empty;
				if (timeSpan.TotalSeconds < 1 && precision == TimeUnit.Second)
				{
					return ToQuantity(timeSpan.TotalSeconds, "second");
				}
			}

			returnString = returnString.TrimEnd(',', ' ');
			if (returnString.Contains(", "))
			{
				returnString = returnString.Remove(returnString.LastIndexOf(", "), 2)
					.Insert(returnString.LastIndexOf(", "), " and ");
			}
			return returnString;
		}

		/// <summary>
		/// Formats a TimeSpan as text.
		/// </summary>
		/// <param name="timeSpan">The TimeSpan object to format.</param>
		/// <returns>Returns a string representation of the TimeSpan.</returns>
		public static string ToQuantity(this TimeSpan timeSpan)
			=> ToQuantity(timeSpan, TimeUnit.Second);
	}

	/// <summary>
	/// Represents units of time.
	/// </summary>
	public enum TimeUnit
	{
		/// <summary>
		/// Represents years as a unit of time.
		/// </summary>
		Year,

		/// <summary>
		/// Represents months as a unit of time.
		/// </summary>
		Month,

		/// <summary>
		/// Represents weeks as a unit of time.
		/// </summary>
		Week,

		/// <summary>
		/// Represents days as a unit of time.
		/// </summary>
		Day,

		/// <summary>
		/// Represents hours as a unit of time.
		/// </summary>
		Hour,

		/// <summary>
		/// Represents minutes as a unit of time.
		/// </summary>
		Minute,

		/// <summary>
		/// Represents seconds as a unit of time.
		/// </summary>
		Second
	}
}
