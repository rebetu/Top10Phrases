using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopPhrases;
using System.Collections.Generic;
using System.Linq;

namespace PhrasesTests
{
    //A lot more tests could be added but this is a start.
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void TestTopResultAccuracy()
        {
            string document = "a b c d.b c d.j k l b c d a.a j k l b e. a b c d e f g. a b. a b.";
            Program program = new TopPhrases.Program();
            var result = Program.GetTop10Phrases(document);
            KeyValuePair<string, int> expectedResult = new KeyValuePair<string, int>("b c d", 4);
            Assert.AreEqual(expectedResult, result.FirstOrDefault());
        }

        [TestMethod]
        public void TestRepeatedSubsetsGetRemoved()
        {
            string document = "Those people are so calm, cool, and collected. I am only cool and collected. I should be more calm so I can be calm, cool, and collected.";
            Program program = new TopPhrases.Program();
            var result = Program.GetTop10Phrases(document);
            Assert.IsFalse(result.ContainsKey("cool and collected"));
        }
    }
}
