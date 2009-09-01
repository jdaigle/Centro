using System;
using System.Security;
using System.Text.RegularExpressions;
using Centro.Core.Helpers;

namespace Centro.Core.Extensions
{
    public static class Strings
    {
        /// <summary>
        /// Determines if a string "matches" another string in a case insensitive comparison."
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="compare">The compare string.</param>
        /// <returns></returns>
        public static bool Matches(this string source, string compare)
        {
            return string.Equals(source, compare, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if a trimmed string "matches" another trimmed string in a case insensitive comparison."
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="compare">The compare string.</param>
        /// <returns></returns>
        public static bool MatchesTrimmed(this string source, string compare)
        {
            return string.Equals(source.Trim(), compare.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if the input string matches the input regex pattern.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="matchPattern">The regex match pattern.</param>
        /// <returns></returns>
        public static bool MatchesRegex(this string input, string matchPattern)
        {
            return Regex.IsMatch(input, matchPattern,
                                 RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// Creates a SecureString object from an input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns></returns>
        public static SecureString MakeSecure(this string input)
        {
            ContractUtils.Requires(!string.IsNullOrEmpty(input), "input");
            var chars = input.ToCharArray();
            var secure = new SecureString();
            foreach (var c in chars)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }
    }
}