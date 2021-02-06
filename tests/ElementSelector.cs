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
    using System;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ElementSelector : SelectorBaseTest
    {
        [Test]
        public void Star()
        {
            Assert.AreEqual(16, SelectList("*").Count);
        }

        [TestCase(":not(*)")]
        [TestCase("*:not(*)")]
        public void Not_Star(string selector)
        {
            Assert.AreEqual(0, SelectList(selector).Count);
        }

        [Test]
        public void Single_Tag_Name()
        {
            Assert.AreEqual(1, SelectList("body").Count);
            Assert.AreEqual("body", SelectList("body")[0].Name);
        }

        [TestCase(":not(body)")]
        [TestCase("*:not(body)")]
        public void Not_Single_Tag_Name(string selector)
        {
            TestNot(selector, 15, e => e.Name == "body");
        }

        [Test]
        public void Single_Uppercase_Tag_Name()
        {
            Assert.AreEqual(1, SelectList("BODY").Count);
            Assert.AreEqual("body", SelectList("BODY")[0].Name);
        }

        [Test]
        public void Single_Tag_Name_Matching_Multiple_Elements()
        {
            Assert.AreEqual(3, SelectList("p").Count);
            Assert.AreEqual("p", SelectList("p")[0].Name);
            Assert.AreEqual("p", SelectList("p")[1].Name);
            Assert.AreEqual("p", SelectList("p")[2].Name);
        }

        [TestCase(":not(p)")]
        [TestCase("*:not(p)")]
        public void Not_Single_Tag_Name_Matching_Multiple_Elements(string selector)
        {
            TestNot(selector, 13, e => e.Name == "p");
        }

        [Test]
        public void Basic_Negative_Precedence()
        {
            Assert.AreEqual(0, SelectList("head p").Count);
        }

        [TestCase("head :not(p)")]
        [TestCase("head *:not(p)")]
        public void Not_Basic_Negative_Precedence(string selector)
        {
            TestNot(selector, 0,
                    from head in DocumentNode.Descendants("head")
                    from p in head.Descendants("p")
                    select p);
        }

        [Test]
        public void Basic_Positive_Precedence_Two_Tags()
        {
            Assert.AreEqual(2, SelectList("div p").Count);
        }

        [TestCase("div :not(p)")]
        [TestCase("div *:not(p)")]
        public void Not_Basic_Positive_Precedence_Two_Tags(string selector)
        {
            TestNot(selector, 5,
                    from div in DocumentNode.Descendants("div")
                    from p in div.Descendants("p")
                    select p);
        }

        [Test]
        public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant()
        {
            Assert.AreEqual(2, SelectList("div a").Count);
        }

        [TestCase("div :not(a)")]
        [TestCase("div *:not(a)")]
        public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant(string selector)
        {
            TestNot(selector, 5,
                    from div in DocumentNode.Descendants("div")
                    from a in div.Descendants("a")
                    select a);
        }

        [Test]
        public void Basic_Positive_Precedence_Three_Tags()
        {
            Assert.AreEqual(1, SelectList("div p a").Count);
            Assert.AreEqual("a", SelectList("div p a")[0].Name);
        }

        [TestCase("div p :not(a)")]
        [TestCase("div p *:not(a)")]
        public void Not_Basic_Positive_Precedence_Three_Tags(string selector)
        {
            TestNot(selector, 2,
                    from div in DocumentNode.Descendants("div")
                    from p in div.Descendants("p")
                    from a in p.Descendants("a")
                    select a);
        }

        [Test]
        public void Basic_Positive_Precedence_With_Same_Tags()
        {
            Assert.AreEqual(1, SelectList("div div").Count);
        }

        [TestCase("div :not(div)")]
        [TestCase("div *:not(div)")]
        public void Not_Basic_Positive_Precedence_With_Same_Tags(string selector)
        {
            TestNot(selector, 6,
                    from div1 in DocumentNode.Descendants("div")
                    from div2 in div1.Descendants("div")
                    select div2);
        }

        /// <summary>
        /// This test covers an issue with HtmlAgilityPack where form childnodes().length == 0.
        /// </summary>
        [Test]
        public void Basic_Positive_Precedence_Within_Form()
        {
            Assert.AreEqual(1, SelectList("form input").Count);
        }

        [Test]
        public void Type_Star()
        {
            Assert.Throws<FormatException>(() => SelectList("a*"));
        }
    }
}
