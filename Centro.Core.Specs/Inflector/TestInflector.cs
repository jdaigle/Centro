using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.Inflector;

namespace Centro.Core.Specs.Inflector
{
    [TestFixture]
    public class TestInflector
    {
        #region Fixture Data

        public static readonly Dictionary<string, string> singularToPlural = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> pascalToUnderscore = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> underscoreToCamel = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> pascalToUnderscoreWithoutReverse =
          new Dictionary<string, string>();

        public static readonly Dictionary<string, string> camelWithModuleToUnderscoreWithSlash =
          new Dictionary<string, string>();

        public static readonly Dictionary<string, string> underscoreToHuman = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> mixtureToTitleCase = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> ordinalNumbers = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> underscoresToDashes = new Dictionary<string, string>();

        static TestInflector()
        {
            singularToPlural.Add("search", "searches");
            singularToPlural.Add("switch", "switches");
            singularToPlural.Add("fix", "fixes");
            singularToPlural.Add("box", "boxes");
            singularToPlural.Add("process", "processes");
            singularToPlural.Add("address", "addresses");
            singularToPlural.Add("case", "cases");
            singularToPlural.Add("stack", "stacks");
            singularToPlural.Add("wish", "wishes");
            singularToPlural.Add("fish", "fish");

            singularToPlural.Add("category", "categories");
            singularToPlural.Add("query", "queries");
            singularToPlural.Add("ability", "abilities");
            singularToPlural.Add("agency", "agencies");
            singularToPlural.Add("movie", "movies");

            singularToPlural.Add("archive", "archives");

            singularToPlural.Add("index", "indices");

            singularToPlural.Add("wife", "wives");
            singularToPlural.Add("safe", "saves");
            singularToPlural.Add("half", "halves");

            singularToPlural.Add("move", "moves");

            singularToPlural.Add("salesperson", "salespeople");
            singularToPlural.Add("person", "people");

            singularToPlural.Add("spokesman", "spokesmen");
            singularToPlural.Add("man", "men");
            singularToPlural.Add("woman", "women");

            singularToPlural.Add("basis", "bases");
            singularToPlural.Add("diagnosis", "diagnoses");

            singularToPlural.Add("datum", "data");
            singularToPlural.Add("medium", "media");
            singularToPlural.Add("analysis", "analyses");

            singularToPlural.Add("node_child", "node_children");
            singularToPlural.Add("child", "children");

            singularToPlural.Add("experience", "experiences");
            singularToPlural.Add("day", "days");

            singularToPlural.Add("comment", "comments");
            singularToPlural.Add("foobar", "foobars");
            singularToPlural.Add("newsletter", "newsletters");

            singularToPlural.Add("old_news", "old_news");
            singularToPlural.Add("news", "news");

            singularToPlural.Add("series", "series");
            singularToPlural.Add("species", "species");

            singularToPlural.Add("quiz", "quizzes");

            singularToPlural.Add("perspective", "perspectives");

            singularToPlural.Add("ox", "oxen");
            singularToPlural.Add("photo", "photos");
            singularToPlural.Add("buffalo", "buffaloes");
            singularToPlural.Add("tomato", "tomatoes");
            singularToPlural.Add("dwarf", "dwarves");
            singularToPlural.Add("elf", "elves");
            singularToPlural.Add("information", "information");
            singularToPlural.Add("equipment", "equipment");
            singularToPlural.Add("bus", "buses");
            singularToPlural.Add("status", "statuses");
            singularToPlural.Add("status_code", "status_codes");
            singularToPlural.Add("mouse", "mice");

            singularToPlural.Add("louse", "lice");
            singularToPlural.Add("house", "houses");
            singularToPlural.Add("octopus", "octopi");
            singularToPlural.Add("virus", "viri");
            singularToPlural.Add("alias", "aliases");
            singularToPlural.Add("portfolio", "portfolios");

            singularToPlural.Add("vertex", "vertices");
            singularToPlural.Add("matrix", "matrices");

            singularToPlural.Add("axis", "axes");
            singularToPlural.Add("testis", "testes");
            singularToPlural.Add("crisis", "crises");

            singularToPlural.Add("rice", "rice");
            singularToPlural.Add("shoe", "shoes");

            singularToPlural.Add("horse", "horses");
            singularToPlural.Add("prize", "prizes");
            singularToPlural.Add("edge", "edges");

            pascalToUnderscore.Add("Product", "product");
            pascalToUnderscore.Add("SpecialGuest", "special_guest");
            pascalToUnderscore.Add("ApplicationController", "application_controller");
            pascalToUnderscore.Add("Area51Controller", "area51_controller");

            underscoreToCamel.Add("product", "product");
            underscoreToCamel.Add("special_guest", "specialGuest");
            underscoreToCamel.Add("application_controller", "applicationController");
            underscoreToCamel.Add("area51_controller", "area51Controller");

            pascalToUnderscoreWithoutReverse.Add("HTMLTidy", "html_tidy");
            pascalToUnderscoreWithoutReverse.Add("HTMLTidyGenerator", "html_tidy_generator");
            pascalToUnderscoreWithoutReverse.Add("FreeBSD", "free_bsd");
            pascalToUnderscoreWithoutReverse.Add("HTML", "html");

            underscoreToHuman.Add("employee_salary", "Employee salary");
            underscoreToHuman.Add("employee_id", "Employee id");
            underscoreToHuman.Add("underground", "Underground");

            mixtureToTitleCase.Add("active_record", "Active Record");
            mixtureToTitleCase.Add("ActiveRecord", "Active Record");
            mixtureToTitleCase.Add("action web service", "Action Web Service");
            mixtureToTitleCase.Add("Action Web Service", "Action Web Service");
            mixtureToTitleCase.Add("Action web service", "Action Web Service");
            mixtureToTitleCase.Add("actionwebservice", "Actionwebservice");
            mixtureToTitleCase.Add("Actionwebservice", "Actionwebservice");

            ordinalNumbers.Add("0", "0th");
            ordinalNumbers.Add("1", "1st");
            ordinalNumbers.Add("2", "2nd");
            ordinalNumbers.Add("3", "3rd");
            ordinalNumbers.Add("4", "4th");
            ordinalNumbers.Add("5", "5th");
            ordinalNumbers.Add("6", "6th");
            ordinalNumbers.Add("7", "7th");
            ordinalNumbers.Add("8", "8th");
            ordinalNumbers.Add("9", "9th");
            ordinalNumbers.Add("10", "10th");
            ordinalNumbers.Add("11", "11th");
            ordinalNumbers.Add("12", "12th");
            ordinalNumbers.Add("13", "13th");
            ordinalNumbers.Add("14", "14th");
            ordinalNumbers.Add("20", "20th");
            ordinalNumbers.Add("21", "21st");
            ordinalNumbers.Add("22", "22nd");
            ordinalNumbers.Add("23", "23rd");
            ordinalNumbers.Add("24", "24th");
            ordinalNumbers.Add("100", "100th");
            ordinalNumbers.Add("101", "101st");
            ordinalNumbers.Add("102", "102nd");
            ordinalNumbers.Add("103", "103rd");
            ordinalNumbers.Add("104", "104th");
            ordinalNumbers.Add("110", "110th");
            ordinalNumbers.Add("1000", "1000th");
            ordinalNumbers.Add("1001", "1001st");

            underscoresToDashes.Add("street", "street");
            underscoresToDashes.Add("street_address", "street-address");
            underscoresToDashes.Add("person_street_address", "person-street-address");
        }

        #endregion

        [Test]
        public void PluralizePlurals()
        {
            Assert.AreEqual("plurals", Inflections.Default.Pluralize("plurals"));
            Assert.AreEqual("Plurals", Inflections.Default.Pluralize("Plurals"));
        }

        [Test]
        public void Pluralize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in singularToPlural)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Pluralize(keyValuePair.Key));
                Assert.AreEqual(Inflections.Default.Capitalize(keyValuePair.Value),
                                Inflections.Default.Pluralize(Inflections.Default.Capitalize(keyValuePair.Key)));
            }
        }

        [Test]
        public void Singularize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in singularToPlural)
            {
                Assert.AreEqual(keyValuePair.Key, Inflections.Default.Singularize(keyValuePair.Value));
                Assert.AreEqual(Inflections.Default.Capitalize(keyValuePair.Key),
                                Inflections.Default.Singularize(Inflections.Default.Capitalize(keyValuePair.Value)));
            }
        }

        [Test]
        public void TitleCase()
        {
            foreach (KeyValuePair<string, string> keyValuePair in mixtureToTitleCase)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Titleize(keyValuePair.Key));
            }
        }

        [Test]
        public void Pascalize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in pascalToUnderscore)
            {
                Assert.AreEqual(keyValuePair.Key, Inflections.Default.Pascalize(keyValuePair.Value));
            }
        }

        [Test]
        public void Camelize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in underscoreToCamel)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Camelize(keyValuePair.Key, false));
            }
        }

        [Test]
        public void Underscore()
        {
            foreach (KeyValuePair<string, string> keyValuePair in pascalToUnderscore)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Underscore(keyValuePair.Key));
            }

            foreach (KeyValuePair<string, string> keyValuePair in pascalToUnderscoreWithoutReverse)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Underscore(keyValuePair.Key));
            }

            foreach (KeyValuePair<string, string> keyValuePair in underscoreToCamel)
            {
                Assert.AreEqual(keyValuePair.Key, Inflections.Default.Underscore(keyValuePair.Value));
            }

            foreach (KeyValuePair<string, string> keyValuePair in underscoreToHuman)
            {
                Assert.AreEqual(keyValuePair.Key, Inflections.Default.Underscore(keyValuePair.Value));
            }
        }

        [Test]
        public void Humanize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in underscoreToHuman)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Humanize(keyValuePair.Key));
            }
        }

        [Test]
        public void Ordinal()
        {
            foreach (KeyValuePair<string, string> keyValuePair in ordinalNumbers)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Ordinalize(keyValuePair.Key));
            }
        }

        [Test]
        public void Dasherize()
        {
            foreach (KeyValuePair<string, string> keyValuePair in underscoresToDashes)
            {
                Assert.AreEqual(keyValuePair.Value, Inflections.Default.Dasherize(keyValuePair.Key));
            }
        }
    }
}
