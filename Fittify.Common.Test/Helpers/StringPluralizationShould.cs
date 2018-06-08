using System.Collections.Generic;
using Fittify.Common.Helpers;
using NUnit.Framework;

namespace Fittify.Common.Test.Helpers
{
    [TestFixture]
    class StringPluralizationShould
    {
        [Test]
        public void StandardPluralizationTests()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("sausage", "sausages");  // Most words - Just add an 's'
            dictionary.Add("status", "statuses");   // Words that end in 's' - Add 'es'
            dictionary.Add("ax", "axes");           // Words that end in 'x' - Add 'es'
            dictionary.Add("octopus", "octopi");    // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("virus", "viri");        // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("crush", "crushes");     // Words that end in 'sh' - Add 'es'
            dictionary.Add("crutch", "crutches");   // Words that end in 'ch' - Add 'es'
            dictionary.Add("matrix", "matrices");   // Words that end in 'ix' - Replace with 'ices'
            dictionary.Add("index", "indices");     // Words that end in 'ex' - Replace with 'ices'
            dictionary.Add("mouse", "mice");        // Some Words that end in 'ouse' - Replace with 'ice'
            dictionary.Add("quiz", "quizzes");      // Words that end in 'z' - Add 'zes'
            dictionary.Add("mailman", "mailmen");   // Words that end in 'man' - Replace with 'men'
            dictionary.Add("man", "men");           // Words that end in 'man' - Replace with 'men'
            dictionary.Add("wolf", "wolves");       // Words that end in 'f' - Replace with 'ves'
            dictionary.Add("wife", "wives");        // Words that end in 'fe' - Replace with 'ves'
            dictionary.Add("calf", "calves");        // Words that end in 'fe' - Replace with 'ves'
            dictionary.Add("thief", "thieves");        // Words that end in 'fe' - Replace with 'ves'
            dictionary.Add("day", "days");          // Words that end in '[vowel]y' - Replace with 'ys'
            dictionary.Add("sky", "skies");         // Words that end in '[consonant]y' - Replace with 'ies'

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];

                Assert.AreEqual(plural, StringPluralization.Pluralize(2, singular));
                Assert.AreEqual(singular, StringPluralization.Pluralize(1, singular));
            }
        }

        [Test]
        public void IrregularPluralizationTests()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("person", "people");
            dictionary.Add("child", "children");
            dictionary.Add("ox", "oxen");

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];

                Assert.AreEqual(plural, StringPluralization.Pluralize(2, singular));
                Assert.AreEqual(singular, StringPluralization.Pluralize(1, singular));
            }
        }

        [Test]
        public void NonPluralizingPluralizationTests()
        {
            var nonPluralizingWords = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };

            foreach (var word in nonPluralizingWords)
            {
                Assert.AreEqual(word, StringPluralization.Pluralize(2, word));
                Assert.AreEqual(word, StringPluralization.Pluralize(1, word));
            }
        }
    }
}
