﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrammarCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarCheck.Tests
{
    [TestClass()]
    public class Tests
    {
        [TestMethod()]
        public void Check_TheHelloWorldPhraseWontHaveMistakes()
        {
            var mistakes = Mistake.FindMistakes("Hello world", "en-US");
            Assert.AreEqual(mistakes.Count(), 0);
        }

        [TestMethod()]
        public void Check_TheHellowVorldPhraseWontHaveMistakes()
        {
            var mistakes = Mistake.FindMistakes("Hellow vorld", "en-US");
            Assert.AreEqual(mistakes.Count(), 2);
        }

        [TestMethod()]
        public void Check_TheMethodDoesntCheckUpperCaseInSentenceStart()
        {
            var mistakes = Mistake.FindMistakes("hello world", "en-US");
            Assert.AreEqual(mistakes.Count(), 0);
        }
    }

    public abstract class ABase
    {
        public Int32 A { get; set; }
    }

    public class A1 : ABase
    {
        public Int32 Method() { return 0; }
    }

    public class A2 : ABase
    {
        public Int32 Method()
        {
            var variabl = 3;
            variabl++;
            return variabl;
        }
    }

}