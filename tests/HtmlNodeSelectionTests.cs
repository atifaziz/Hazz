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
    using Systems.HtmlAgilityPack;
    using HtmlAgilityPack;
    using NUnit.Framework;

    [TestFixture]
    public class HtmlNodeSelectionTests
    {
        [Test]
        public void QuerySelectorAllWithNullNodeThrows()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                HtmlNodeSelection.QuerySelectorAll(null, "body"));
            Assert.That(e.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void QuerySelectorAllWithNullSelectorThrows()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml("<html></html>");
            var e = Assert.Throws<ArgumentNullException>(() =>
                doc.DocumentNode.QuerySelectorAll(null));
            Assert.That(e.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void CompileWithNullSelectorThrows()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                HtmlNodeSelection.Compile(null));
            Assert.That(e.ParamName, Is.EqualTo("selector"));
        }
    }
}
