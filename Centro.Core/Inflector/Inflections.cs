using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Centro.Extensions;

namespace Centro.Inflector
{
    public class Inflections
    {
        private static Inflections defaultInstance;

        public static Inflections Default
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = new Inflections();
                return defaultInstance;
            }
        }

        private readonly IList<Rule> plurals = new List<Rule>();
        private readonly IList<Rule> singulars = new List<Rule>();
        private readonly IList<string> uncountables = new List<string>();
        private readonly IList<Rule> humans = new List<Rule>();

        private Inflections()
        {
        }

        static Inflections()
        {
            Default.AddPlural("$", "s");
            Default.AddPlural("s$", "s");
            Default.AddPlural("(ax|test)is$", "$1es");
            Default.AddPlural("(octop|vir)us$", "$1i");
            Default.AddPlural("(alias|status)$", "$1es");
            Default.AddPlural("(bu)s$", "$1ses");
            Default.AddPlural("(buffal|tomat)o$", "$1oes");
            Default.AddPlural("([ti])um$", "$1a");
            Default.AddPlural("sis$", "ses");
            Default.AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
            Default.AddPlural("(hive)$", "$1s");
            Default.AddPlural("([^aeiouy]|qu)y$", "$1ies");
            Default.AddPlural("(x|ch|ss|sh)$", "$1es");
            Default.AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
            Default.AddPlural("([m|l])ouse$", "$1ice");
            Default.AddPlural("^(ox)$", "$1en");
            Default.AddPlural("(quiz)$", "$1zes");

            Default.AddSingular("s$", "");
            Default.AddSingular("(n)ews$", "$1ews");
            Default.AddSingular("([ti])a$", "$1um");
            Default.AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            Default.AddSingular("(^analy)ses$", "$1sis");
            Default.AddSingular("([^f])ves$", "$1fe");
            Default.AddSingular("(hive)s$", "$1");
            Default.AddSingular("(tive)s$", "$1");
            Default.AddSingular("([lr])ves$", "$1f");
            Default.AddSingular("([^aeiouy]|qu)ies$", "$1y");
            Default.AddSingular("(s)eries$", "$1eries");
            Default.AddSingular("(m)ovies$", "$1ovie");
            Default.AddSingular("(x|ch|ss|sh)es$", "$1");
            Default.AddSingular("([m|l])ice$", "$1ouse");
            Default.AddSingular("(bus)es$", "$1");
            Default.AddSingular("(o)es$", "$1");
            Default.AddSingular("(shoe)s$", "$1");
            Default.AddSingular("(cris|ax|test)es$", "$1is");
            Default.AddSingular("(octop|vir)i$", "$1us");
            Default.AddSingular("(alias|status)es$", "$1");
            Default.AddSingular("^(ox)en", "$1");
            Default.AddSingular("(vert|ind)ices$", "$1ex");
            Default.AddSingular("(matr)ices$", "$1ix");
            Default.AddSingular("(quiz)zes$", "$1");
            Default.AddSingular("(database)s$", "$1");

            Default.AddIrregular("person", "people");
            Default.AddIrregular("man", "men");
            Default.AddIrregular("child", "children");
            Default.AddIrregular("sex", "sexes");
            Default.AddIrregular("move", "moves");
            Default.AddIrregular("cow", "kine");

            Default.AddUncountable("equipment");
            Default.AddUncountable("information");
            Default.AddUncountable("rice");
            Default.AddUncountable("money");
            Default.AddUncountable("species");
            Default.AddUncountable("series");
            Default.AddUncountable("fish");
            Default.AddUncountable("sheep");
        }

        public void AddPlural(string regexPattern, string replacement)
        {
            uncountables.Remove(regexPattern);
            uncountables.Remove(replacement);
            plurals.Add(new Rule(regexPattern, replacement));
        }

        public void AddSingular(string regexPattern, string replacement)
        {
            uncountables.Remove(regexPattern);
            uncountables.Remove(replacement);
            singulars.Add(new Rule(regexPattern, replacement));
        }

        public void AddUncountable(string word)
        {
            if (!uncountables.Contains(word.ToUpper()))
                uncountables.Add(word.ToUpper());
        }

        public void AddHuman(string regexPattern, string replacement)
        {
            humans.Add(new Rule(regexPattern, replacement));
        }

        public void AddIrregular(string singular, string plural)
        {
            uncountables.Remove(singular);
            uncountables.Remove(plural);
            if (singular.Substring(0, 1).Matches(plural.Substring(0, 1)))
            {
                AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
                AddPlural("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + plural.Substring(1));
                AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
            }
            else
            {
                AddPlural("(" + singular.Substring(0,1).ToUpper() + ")" + singular.Substring(1) + "$", plural.Substring(0,1).ToUpper() + plural.Substring(1));
                AddPlural("(" + singular.Substring(0, 1).ToLower() + ")" + singular.Substring(1) + "$", plural.Substring(0, 1).ToLower() + plural.Substring(1));
                AddPlural("(" + plural.Substring(0, 1).ToUpper() + ")" + plural.Substring(1) + "$", plural.Substring(0, 1).ToUpper() + plural.Substring(1));
                AddPlural("(" + plural.Substring(0, 1).ToLower() + ")" + plural.Substring(1) + "$", plural.Substring(0, 1).ToLower() + plural.Substring(1));
                AddSingular("(" + plural.Substring(0, 1).ToUpper() + ")" + plural.Substring(1) + "$", singular.Substring(0, 1).ToUpper() + singular.Substring(1));
                AddSingular("(" + plural.Substring(0, 1).ToLower() + ")" + plural.Substring(1) + "$", singular.Substring(0, 1).ToLower() + singular.Substring(1));
            }
            
        }

        public string Pluralize(string word)
        {
            if (string.IsNullOrEmpty(word) || uncountables.Contains(word.ToUpper()))
                return word;
            return ApplyRules(plurals, word);
        }

        public string Singularize(string word)
        {
            if (string.IsNullOrEmpty(word) || uncountables.Contains(word.ToUpper()))
                return word;
            return ApplyRules(singulars, word);
        }

        public string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public string Camelize(string word, bool firstLetterInUppercase)
        {
            if (firstLetterInUppercase)
                return word.Substitute("(^|_)(?<letter>.)", m => m.Groups["letter"].Value.ToUpper());

            var camelized = Camelize(word, true);
            return camelized.ToCharArray().First().ToString().ToLower() + camelized.Substring(1, camelized.Length - 1);
        }

        public string Titleize(string word)
        {
            return Humanize(Underscore(word)).Substitute("('?[a-z])(?<letter>.)", m => m.Groups["letter"].Value.ToUpper());
        }

        public string Underscore(string word)
        {
            return word.Substitute(@"([A-Z]+)([A-Z][a-z])", "_")
                       .Substitute(@"([a-z\d])([A-Z])", "_")
                       .Replace("-", "_")
                       .ToLower();
        }

        public string Dasherize(string word)
        {
            return word.Replace(' ', '-');
        }

        public string Humanize(string word)
        {
            var result = ApplyRules(humans, word);
            return Capitalize(result.Replace("_id", "").Replace("_", ""));
        }

        public string Ordinalize(string word)
        {
            var number = 0;
            if (!int.TryParse(word, out number))
                throw new ArgumentException("Word must be an integer", "word");
            if ((new List<int> { 1, 2, 3 }).Contains(number % 100))
                return number.ToString() + "th";
            else
                switch (number % 10)
                {
                    case 1: return number.ToString() + "st";
                    case 2: return number.ToString() + "nd";
                    case 3: return number.ToString() + "rd";
                    default: return number.ToString() + "th";
                }
        }

        private string ApplyRules(IList<Rule> rules, string word)
        {
            var result = word;
            foreach (var rule in rules.Reverse())
                if ((result = rule.Apply(word)) != null)
                    return result;
            return result;
        }

        private class Rule
        {
            private readonly Regex regex;
            private readonly string replacement;

            public Rule(string regexPattern, string replacement)
            {
                regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
                this.replacement = replacement;
            }

            public string Apply(string word)
            {
                if (!regex.IsMatch(word))
                    return null;

                return regex.Replace(word, replacement);
            }
        }

    }
}