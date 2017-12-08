# Hazz

Hazz implements CSS Selectors for [HTMLAgilityPack][hap]. It is based on
[Fizzler][fizzler], a generic CSS Selectors parser and generator library.

Hazz was previously known and distributed as
[Fizzler.Systems.HtmlAgilityPack][fizzhap].

## Examples

```c#
// Load the document using HTMLAgilityPack as normal
var html = new HtmlDocument();
html.LoadHtml(@"
  <html>
      <head></head>
      <body>
        <div>
          <p class='content'>Fizzler</p>
          <p>CSS Selector Engine</p></div>
      </body>
  </html>");

// Fizzler for HtmlAgilityPack is implemented as the 
// QuerySelectorAll extension method on HtmlNode

var document = html.DocumentNode;

// yields: [<p class="content">Fizzler</p>]
document.QuerySelectorAll(".content"); 

// yields: [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
document.QuerySelectorAll("p");

// yields empty sequence
document.QuerySelectorAll("body>p");

// yields [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
document.QuerySelectorAll("body p");

// yields [<p class="content">Fizzler</p>]
document.QuerySelectorAll("p:first-child");
```

  [fizzler]: https://github.com/atifaziz/Fizzler
  [fizzhap]: https://www.nuget.org/packages/Fizzler.Systems.HtmlAgilityPack/
  [hap]: http://html-agility-pack.net/
