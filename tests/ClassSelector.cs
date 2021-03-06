#region Copyright and License
//
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
#endregion

namespace Fizzler.Tests
{
    using System.Linq;
    using Systems.HtmlAgilityPack;
    using HtmlAgilityPack;
    using NUnit.Framework;

    [TestFixture]
    public class ClassSelector : SelectorBaseTest
    {
        [Test]
        public void Basic()
        {
            var result = SelectList(".checkit");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("div", result[1].Name);
        }

        [TestCase(":not(.checkit)")]
        [TestCase("*:not(.checkit)")]
        public void Not_Basic(string selector)
        {
            TestNot(selector, 14, DocumentNode.GetElementsByClassName("checkit"));
        }

        /// <summary>
        /// Should match class="omg ohyeah"
        /// </summary>
        [Test]
        public void Chained()
        {
            var result = SelectList(".omg.ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }

        [TestCase(":not(.omg):not(.ohyeah)")]
        [TestCase("*:not(.omg):not(.ohyeah)")]
        public void Not_Chained(string selector)
        {
            TestNot(selector.Trim(), 15, DocumentNode.GetElementsByClassName("omg", "ohyeah"));
        }

        [Test]
        public void With_Element()
        {
            var result = SelectList("p.ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }

        [TestCase("p:not(.ohyeah)")]
        public void Not_With_Element(string selector)
        {
            TestNot(selector, 2,
                    from p in DocumentNode.Descendants("p")
                    from e in p.GetElementsByClassName("ohyeah")
                    select e);
        }

        [Test]
        public void Parent_Class_Selector()
        {
            var result = SelectList("div .ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }

        [TestCase(".a")]
        [TestCase(".b")]
        [TestCase(".c")]
        [TestCase(".d")]
        [TestCase(".e")]
        [TestCase(".f")]
        [TestCase(".a.c")]
        [TestCase(".a.b.c")]
        [TestCase(".c.e")]
        [TestCase(".d.e.f")]
        public void WhiteSpaceSeparators(string selector)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(@"
<!doctype html>
<html>
<head>
    <title>Lorem Ipsum</title>
</head>
<body>
    <p class='" + "a b\tc\rd\ne\ff" + @"'>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
</body>
</html>
");
            var e = doc.DocumentNode.QuerySelector(selector);
            Assert.That(e, Is.Not.Null);
            Assert.That(e.Name, Is.EqualTo("p"));
        }

        [TestCase("div :not(.ohyeah)")]
        [TestCase("div *:not(.ohyeah)")]
        public void Parent_Not_Class_Selector(string selector)
        {
            TestNot(selector, 6,
                    from div in DocumentNode.Descendants("div")
                    from e in div.GetElementsByClassName("ohyeah")
                    select e);
        }
    }
}
