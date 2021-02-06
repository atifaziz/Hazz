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
    using NUnit.Framework;

    [TestFixture]
    public class AttributeSelectors : SelectorBaseTest
    {
        [Test]
        public void Element_Attr_Exists()
        {
            var results = SelectList("div[id]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.NotNull(results[0].Attributes["id"]);
            Assert.AreEqual("div", results[1].Name);
            Assert.NotNull(results[1].Attributes["id"]);
        }

        [Test]
        public void Element_Attr_Exists_Not()
        {
            TestNot("div:not([id])", 2, e => e.Name == "div" && e.Id == null);
        }

        [TestCase(":not(div):not([id])")]
        [TestCase("*:not(div):not([id])")]
        public void Not_Element_Not_Attr_Exists(string selector)
        {
            TestNot(selector, 11, e => e.Name != "div" && e.Id == null);
        }

        [Test]
        public void Element_Attr_Equals_With_Double_Quotes()
        {
            var results = SelectList("div[id=\"someOtherDiv\"]");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("someOtherDiv", results[0].Attributes["id"].Value);
        }

        [TestCase("div:not([id=\"someOtherDiv\"])")]
        [TestCase("div:not(#someOtherDiv)")]
        public void Element_Not_Attr_Equals_With_Double_Quotes(string selector)
        {
            TestNot(selector, 3, e => e.Name != "div" || e.Id == "someOtherDiv");
        }

        [TestCase(":not(div):not([id=\"someOtherDiv\"])")]
        [TestCase("*:not(div):not([id=\"someOtherDiv\"])")]
        public void Not_Element_Not_Attr_Equals_With_Double_Quotes(string selector)
        {
            TestNot(selector, 12, e => e.Name != "div" && e.Id == "someOtherDiv");
        }

        [Test]
        public void Element_Attr_Space_Separated_With_Double_Quotes()
        {
            var results = SelectList("p[class~=\"ohyeah\"]");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("p", results[0].Name);
            Assert.AreEqual("eeeee", results[0].InnerText);
        }

        [TestCase("p:not([class~=\"ohyeah\"])")]
        public void Element_Attr_Space_Separated_With_Double_Quotes_Not(string selector)
        {
            TestNot(selector, 2,
                    e => e.Name != "p"
                      || (e.Attributes["class"]?.Value.Split(' ').Contains("ohyeah") ?? false));
        }

        [Test]
        public void Element_Attr_Space_Separated_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("p[class~='']").Count);
        }

        [Test]
        public void Element_Attr_Space_Separated_With_Empty_Value_Not()
        {
            var results = SelectList("p:not([class~=''])");

            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("p", results[0].Name);
            Assert.AreEqual("hitestoh", results[0].InnerText);
            Assert.AreEqual("p", results[1].Name);
            Assert.AreEqual("hi!!", results[1].InnerText);
            Assert.AreEqual("p", results[2].Name);
            Assert.AreEqual("eeeee", results[2].InnerText);
        }

        [Test]
        public void Element_Attr_Hyphen_Separated_With_Double_Quotes()
        {
            var results = SelectList("span[class|=\"separated\"]");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("span", results[0].Name);
            Assert.AreEqual("test", results[0].InnerText);
        }

        [Test]
        public void Element_Attr_Hyphen_Separated_With_Double_Quotes_Not()
        {
            var results = SelectList("span:not([class|=\"separated\"])");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("span", results[0].Name);
            Assert.AreEqual("oh", results[0].InnerText);
        }

        [TestCase("[class=\"checkit\"]")]
        [TestCase("*[class=\"checkit\"]")]
        public void Universal_Attr_Exact_With_Double_Quotes(string selector)
        {
            var results = SelectList(selector);

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [TestCase(":not([class=\"checkit\"])")]
        [TestCase("*:not([class=\"checkit\"])")]
        [TestCase(":not(.checkit)")]
        [TestCase("*:not(.checkit)")]
        public void Universal_Not_Attr_Exact_With_Double_Quotes(string selector)
        {
            TestNot(selector, 14, DocumentNode.GetElementsByClassName("checkit"));
        }

        [Test]
        public void Star_Attr_Prefix()
        {
            var results = SelectList("*[class^=check]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [TestCase(":not([class^=check])")]
        [TestCase("*:not([class^=check])")]
        public void Star_Attr_Prefix_Not(string selector)
        {
            TestNot(selector, 14,
                    from e in DocumentNode.Descendants().Elements()
                    where e.GetAttributeValue("class", string.Empty).StartsWith("check")
                    select e);
        }

        [Test]
        public void Star_Attr_Prefix_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class^='']").Count);
        }

        [TestCase("*:not([class^=''])")]
        [TestCase(":not([class^=''])")]
        public void Star_Not_Attr_Prefix_With_Empty_Value(string selector)
        {
            Assert.AreEqual(16, SelectList(selector).Count);
        }

        [Test]
        public void Star_Attr_Suffix()
        {
            var results = SelectList("*[class$=it]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Suffix_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class$='']").Count);
        }

        [Test]
        public void Star_Attr_Substring()
        {
            var results = SelectList("*[class*=heck]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Substring_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class*='']").Count);
        }

        [TestCase("[class~='a']")]
        [TestCase("[class~='b']")]
        [TestCase("[class~='c']")]
        [TestCase("[class~='d']")]
        [TestCase("[class~='e']")]
        [TestCase("[class~='f']")]
        [TestCase("[class~='a'][class~='c']")]
        [TestCase("[class~='a'][class~='b'][class~='c']")]
        [TestCase("[class~='c'][class~='e']")]
        [TestCase("[class~='d'][class~='e'][class~='f']")]
        public void WhiteSpaceSeparators(string selector)
        {
            var fixture = new ClassSelector();
            fixture.WhiteSpaceSeparators(selector);
        }
    }
}
