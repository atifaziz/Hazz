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
    public class IDSelector : SelectorBaseTest
    {
        [Test]
        public void Basic_Selector()
        {
            var result = SelectList("#myDiv");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("div", result[0].Name);
        }

        [TestCase(":not(#myDiv)")]
        [TestCase("*:not(#myDiv)")]
        public void Not_Basic_Selector(string selector)
        {
            TestNot(selector, 15, e => e.Id == "myDiv");
        }

        [Test]
        public void With_Element()
        {
            var result = SelectList("div#myDiv");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("div", result[0].Name);
        }

        [Test]
        public void Not_With_Element()
        {
            TestNot("div:not(#myDiv)", 3, e => e.Name == "div" && e.Id == "myDiv");
        }

        [Test]
        public void With_Existing_ID_Descendant()
        {
            var result = SelectList("#theBody #myDiv");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("div", result[0].Name);
        }

        [TestCase("#theBody :not(#myDiv)")]
        [TestCase("#theBody *:not(#myDiv)")]
        public void With_Not_Existing_ID_Descendant(string selector)
        {
            TestNot(selector, 12,
                    DocumentNode.GetElementById("theBody").GetElementById("myDiv"));
        }

        [Test]
        public void With_Non_Existant_ID_Descendant()
        {
            var result = SelectList("#theBody #whatwhatwhat");

            Assert.AreEqual(0, result.Count);
        }

        [TestCase("#theBody :not(#whatwhatwhat)")]
        [TestCase("#theBody *:not(#whatwhatwhat)")]
        public void With_Not_Non_Existant_ID_Descendant(string selector)
        {
            TestNot(selector, 13);
        }

        [Test]
        public void With_Non_Existant_ID_Ancestor()
        {
            var result = SelectList("#whatwhatwhat #someOtherDiv");

            Assert.AreEqual(0, result.Count);
        }

        [TestCase("#whatwhatwhat :not(#someOtherDiv)")]
        [TestCase("#whatwhatwhat *:not(#someOtherDiv)")]
        public void With_Not_Non_Existant_ID_Ancestor(string selector)
        {
            var result = SelectList(selector);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void All_Descendants_Of_ID()
        {
            var result = SelectList("#myDiv *");

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("p", result[1].Name);
        }

        [Test]
        public void Not_All_Descendants_Of_ID()
        {
            var result = SelectList("#myDiv :not(*)");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Child_ID()
        {
            var result = SelectList("#theBody>#myDiv");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("div", result[0].Name);
        }

        [TestCase("#theBody>:not(#myDiv)")]
        [TestCase("#theBody>*:not(#myDiv)")]
        public void Not_Child_ID(string selector)
        {
            var div = DocumentNode.GetElementById("myDiv");
            Assert.AreEqual("theBody", div.ParentNode.Id);
            TestNot(selector, 4, div);
        }

        [Test]
        public void Not_A_Child_ID()
        {
            var result = SelectList("#theBody>#someOtherDiv");

            Assert.AreEqual(0, result.Count);
        }

        [TestCase("#theBody>:not(#someOtherDiv)")]
        [TestCase("#theBody>*:not(#someOtherDiv)")]
        public void Not_Not_A_Child_ID(string selector)
        {
            var div = DocumentNode.GetElementById("someOtherDiv");
            Assert.AreNotEqual("theBody", div.ParentNode.Id);
            TestNot(selector, 5, div);
        }

        [Test]
        public void All_Children_Of_ID()
        {
            var result = SelectList("#myDiv>*");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("p", result[1].Name);
        }

        [TestCase(":not(#myDiv)>*")]
        [TestCase("*:not(#myDiv)>*")]
        public void All_Children_Of_Not_ID(string selector)
        {
            TestNot(selector, 13,
                    from e in DocumentNode.Descendants().Elements()
                    where e.ParentNode.Id == "myDiv"
                    select e);
        }

        [Test]
        public void All_Children_of_ID_with_no_children()
        {
            var result = SelectList("#someOtherDiv>*");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Not_All_Children_of_ID_with_no_children()
        {
            var result = SelectList("#someOtherDiv>:not(*)");

            Assert.AreEqual(0, result.Count);
        }
    }
}
