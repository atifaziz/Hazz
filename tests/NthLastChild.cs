
namespace Fizzler.Tests
{
    using System.Linq;
    using Systems.HtmlAgilityPack;
    using NUnit.Framework;

    [TestFixture]
    public class NthLastChild : SelectorBaseTest
    {
        [Test]
        public void No_Prefix_With_Digit()
        {
            var result = SelectList(":nth-last-child(2)");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("head", result[0].Name);
            Assert.AreEqual("div", result[1].Name);
            Assert.AreEqual("span", result[2].Name);
            Assert.AreEqual("div", result[3].Name);
        }

        [TestCase(":not(:nth-last-child(2))")]
        [TestCase("*:not(:nth-last-child(2))")]
        public void Not_No_Prefix_With_Digit(string selector)
        {
            TestNot(selector, 12,
                    from e in DocumentNode.Descendants().Elements()
                    let elements = e.ParentNode.Elements().ToList()
                    where elements.IndexOf(e) == elements.Count - 2
                    select e);
        }

        [Test]
        public void Id_Prefix_With_Digit()
        {
            var result = SelectList("#myDiv :nth-last-child(2)");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("span", result[1].Name);
        }

        [TestCase("#myDiv :not(:nth-last-child(2))")]
        [TestCase("#myDiv *:not(:nth-last-child(2))")]
        public void Not_Id_Prefix_With_Digit(string selector)
        {
            TestNot(selector, 3,
                    from e in DocumentNode.GetElementById("myDiv")
                                          .Descendants().Elements()
                    let elements = e.ParentNode.Elements().ToList()
                    where elements.IndexOf(e) == elements.Count - 2
                    select e);
        }

        [Test]
        public void Element_Prefix_With_Digit()
        {
            var result = SelectList("span:nth-last-child(3)");

            Assert.AreEqual(0, result.Count);
        }

        [TestCase("span:not(:nth-last-child(3))")]
        public void Not_Element_Prefix_With_Digit(string selector)
        {
            TestNot(selector, 2,
                    from e in DocumentNode.GetElementsByTagName("span")
                    let elements = e.ParentNode.Elements().ToList()
                    where elements.IndexOf(e) == elements.Count - 3
                    select e);
        }

        [Test]
        public void Element_Prefix_With_Digit2()
        {
            var result = SelectList("span:nth-last-child(2)");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("span", result[0].Name);
        }

        [TestCase("span:not(:nth-last-child(2))")]
        public void Not_Element_Prefix_With_Digit2(string selector)
        {
            TestNot(selector, 1,
                    from e in DocumentNode.GetElementsByTagName("span")
                    let elements = e.ParentNode.Elements().ToList()
                    where elements.IndexOf(e) == elements.Count - 2
                    select e);
        }
    }
}
