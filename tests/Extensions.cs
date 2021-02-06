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
    using System.Collections.Generic;
    using System.Linq;
    using Systems.HtmlAgilityPack;
    using HtmlAgilityPack;

    static class Extensions
    {
        public static IEnumerable<HtmlNode> GetElementsByTagName(this HtmlNode node, string name) =>
            from e in node.Descendants().Elements()
            where string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase)
            select e;

        static string[] SplitClassNames(string @class)
        {
            var names = @class.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);
            return names.Length > 0 && names[0].Length == 0 ? Array.Empty<string>() : names;
        }

        public static IEnumerable<HtmlNode> GetElementsByClassName(this HtmlNode node, string names) =>
            node.GetElementsByClassName(SplitClassNames(names));

        public static IEnumerable<HtmlNode> GetElementsByClassName(this HtmlNode node, params string[] names) =>
            from e in node.Descendants().Elements()
            where SplitClassNames(e.GetAttributeValue("class", string.Empty)).Intersect(names, StringComparer.Ordinal).Any()
            select e;

        public static HtmlNode FindElementById(this HtmlNode node, string id) =>
            node.Descendants().Elements().SingleOrDefault(e => e.Id == id);

        public static HtmlNode GetElementById(this HtmlNode node, string id) =>
            node.FindElementById(id) ?? throw new Exception($"Element with ID \"{id}\" not found.");
    }
}
