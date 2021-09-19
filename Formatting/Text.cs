using System;
using System.Text.RegularExpressions;

namespace Crews.Extensions.Formatting
{
	/// <summary>
	/// Contains extension functions for plain text.
	/// </summary>
	public static class Text
	{
		/// <summary>
		/// Removes all HTML tags and their attributes from a string (insecure).
		/// </summary>
		/// <param name="input">The string to capitalize.</param>
		/// <returns>Returns a formatted string.</returns>
		public static string NoHtml(this string input)
			=> Regex.Replace(input, "<[a-zA-Z/].*?>", String.Empty);

		/// <summary>
		/// Capitalizes one or all words in a string.
		/// </summary>
		/// <param name="input">The string to capitalize.</param>
		/// <param name="allWords">
		/// Indicates whether all words detected should be capitalized.
		/// </param>
		/// <returns>Returns a formatted string.</returns>
		public static string Capitalized(this string input, bool allWords)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException("String cannot be empty or null.", nameof(input));
			}

			if (allWords)
			{
				string textStr = string.Empty;
				foreach (string word in input.Trim().Split(' '))
				{
					textStr += CapitalizedWord(word) + " ";
				}
				return textStr.Trim();
			}
			return CapitalizedWord(input);
		}

		/// <summary>
		/// Capitalizes one or all words in a string.
		/// </summary>
		/// <param name="input">The string to capitalize.</param>
		/// <returns>Returns a formatted string.</returns>
		public static string Capitalized(this string input) => Capitalized(input, true);

		private static string CapitalizedWord(string input) =>
			char.ToUpper(input[0]).ToString() + (input.Length > 1 ?
			input.Substring(1) : string.Empty);
	}
}