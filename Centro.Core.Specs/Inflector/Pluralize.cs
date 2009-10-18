using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Centro.Inflector;

namespace Centro.Core.Specs.Inflector
{
    [TestFixture]
    public class Pluralize
    {
        [Test]
        public void Plurals()
        {
            Assert.AreEqual("plurals", "plurals".Pluralize());
            Assert.AreEqual("Plurals", "Plurals".Pluralize());
        }

        [Test]
        public void EmptyString()
        {
            Assert.AreEqual("", "".Pluralize());
        }

        [Test]
        public void TestCases()
        {
            foreach (var toPlural in testCases)
            {
                Assert.AreEqual(toPlural.Value, toPlural.Key.Pluralize());
            }
        }

        public static IList<KeyValuePair<string, string>> testCases = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("search"     ,"searches"), new KeyValuePair<string, string>(
    "switch"      , "switches"), new KeyValuePair<string, string>(
    "fix"         , "fixes"), new KeyValuePair<string, string>(
    "box"         , "boxes"), new KeyValuePair<string, string>(
    "process"     , "processes"), new KeyValuePair<string, string>(
    "address"     , "addresses"), new KeyValuePair<string, string>(
    "case"        , "cases"), new KeyValuePair<string, string>(
    "stack"       , "stacks"), new KeyValuePair<string, string>(
    "wish"        , "wishes"), new KeyValuePair<string, string>(
    "fish"        , "fish"), new KeyValuePair<string, string>(
 
    "category"    , "categories"), new KeyValuePair<string, string>(
    "query"       , "queries"), new KeyValuePair<string, string>(
    "ability"     , "abilities"), new KeyValuePair<string, string>(
    "agency"      , "agencies"), new KeyValuePair<string, string>(
    "movie"       , "movies"), new KeyValuePair<string, string>(
 
    "archive"     , "archives"), new KeyValuePair<string, string>(
 
    "index"       , "indices"), new KeyValuePair<string, string>(
 
    "wife"        , "wives"), new KeyValuePair<string, string>(
    "safe"        , "saves"), new KeyValuePair<string, string>(
    "half"        , "halves"), new KeyValuePair<string, string>(
 
    "move"        , "moves"), new KeyValuePair<string, string>(
 
    "salesperson" , "salespeople"), new KeyValuePair<string, string>(
    "person"      , "people"), new KeyValuePair<string, string>(
 
    "spokesman"   , "spokesmen"), new KeyValuePair<string, string>(
    "man"         , "men"), new KeyValuePair<string, string>(
    "woman"       , "women"), new KeyValuePair<string, string>(
 
    "basis"       , "bases"), new KeyValuePair<string, string>(
    "diagnosis"   , "diagnoses"), new KeyValuePair<string, string>(
    "diagnosis_a" , "diagnosis_as"), new KeyValuePair<string, string>(
 
    "datum"       , "data"), new KeyValuePair<string, string>(
    "medium"      , "media"), new KeyValuePair<string, string>(
    "analysis"    , "analyses"), new KeyValuePair<string, string>(
 
    "node_child"  , "node_children"), new KeyValuePair<string, string>(
    "child"       , "children"), new KeyValuePair<string, string>(
 
    "experience"  , "experiences"), new KeyValuePair<string, string>(
    "day"         , "days"), new KeyValuePair<string, string>(
 
    "comment"     , "comments"), new KeyValuePair<string, string>(
    "foobar"      , "foobars"), new KeyValuePair<string, string>(
    "newsletter"  , "newsletters"), new KeyValuePair<string, string>(
 
    "old_news"    , "old_news"), new KeyValuePair<string, string>(
    "news"        , "news"), new KeyValuePair<string, string>(
 
    "series"      , "series"), new KeyValuePair<string, string>(
    "species"     , "species"), new KeyValuePair<string, string>(
 
    "quiz"        , "quizzes"), new KeyValuePair<string, string>(
 
    "perspective" , "perspectives"), new KeyValuePair<string, string>(
 
    "ox"          , "oxen"), new KeyValuePair<string, string>(
    "photo"       , "photos"), new KeyValuePair<string, string>(
    "buffalo"     , "buffaloes"), new KeyValuePair<string, string>(
    "tomato"      , "tomatoes"), new KeyValuePair<string, string>(
    "dwarf"       , "dwarves"), new KeyValuePair<string, string>(
    "elf"         , "elves"), new KeyValuePair<string, string>(
    "information" , "information"), new KeyValuePair<string, string>(
    "equipment"   , "equipment"), new KeyValuePair<string, string>(
    "bus"         , "buses"), new KeyValuePair<string, string>(
    "status"      , "statuses"), new KeyValuePair<string, string>(
    "status_code" , "status_codes"), new KeyValuePair<string, string>(
    "mouse"       , "mice"), new KeyValuePair<string, string>(
 
    "louse"       , "lice"), new KeyValuePair<string, string>(
    "house"       , "houses"), new KeyValuePair<string, string>(
    "octopus"     , "octopi"), new KeyValuePair<string, string>(
    "virus"       , "viri"), new KeyValuePair<string, string>(
    "alias"       , "aliases"), new KeyValuePair<string, string>(
    "portfolio"   , "portfolios"), new KeyValuePair<string, string>(
 
    "vertex"      , "vertices"), new KeyValuePair<string, string>(
    "matrix"      , "matrices"), 
    //new KeyValuePair<string, string>("matrix_fu"   , "matrix_fus"),  // This test case is a bitch
    new KeyValuePair<string, string>(
 
    "axis"        , "axes"), new KeyValuePair<string, string>(
    "testis"      , "testes"), new KeyValuePair<string, string>(
    "crisis"      , "crises"), new KeyValuePair<string, string>(
 
    "rice"        , "rice"), new KeyValuePair<string, string>(
    "shoe"        , "shoes"), new KeyValuePair<string, string>(
 
    "horse"       , "horses"), new KeyValuePair<string, string>(
    "prize"       , "prizes"), new KeyValuePair<string, string>(
    "edge"        , "edges"),
    new KeyValuePair<string, string>("database"    , "databases"),
    new KeyValuePair<string, string>("cow"         , "kine"), 
        };
    }
}
