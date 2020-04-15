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

        [Test]
        public void With_Element()
        {
            var result = SelectList("p.ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
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
    }
}