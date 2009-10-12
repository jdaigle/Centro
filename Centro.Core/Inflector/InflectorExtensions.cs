
namespace Centro.Inflector
{
    public static class InflectorExtensions
    {
        public static string Pluralize(this string word)
        {
            return Inflections.Default.Pluralize(word);
        }

        public static string Singularize(this string word)
        {
            return Inflections.Default.Singularize(word);
        }

        public static string Capitalize(this string word)
        {
            return Inflections.Default.Capitalize(word);
        }

        public static string Camelize(this string word, bool firstLetterInUppercase)
        {
            return Inflections.Default.Camelize(word, firstLetterInUppercase);
        }

        public static string Titleize(this string word)
        {
            return Inflections.Default.Titleize(word);
        }

        public static string Underscore(this string word)
        {
            return Inflections.Default.Underscore(word);
        }

        public static string Dasherize(this string word)
        {
            return Inflections.Default.Dasherize(word);
        }

        public static string Humanize(this string word)
        {
            return Inflections.Default.Humanize(word);
        }

        public static string Ordinalize(this string word)
        {
            return Inflections.Default.Ordinalize(word);
        }
    }
}
