using System;
using System.Collections.Generic;

namespace CliUtils.Test
{
	public static class HtmlToMdDataSource
	{
			private static readonly List<object[]> Data 
				= new List<object[]>
				{
					new object[]
					{
						"<article>" +
						"<ul>" +
						"<li><code>Test</code></li>" +
						"<li><em>Test</em></li>" +
						"<ul>" +
						"</article>",
						"* ```Test```* *Test*"
					},
					new object[]
					{
						"<article><em>Test</em></article>",
						"*Test*"
					},
					new object[]
					{
						"<article><code>Test</code></article>",
						"```Test```"
					},
					new object[]
					{
						"<article><p>Words <code>Test</code> and <em>em</em></p></article>",
						"Words ```Test``` and *em*"
					},
					new object[]
					{
						"<article><p><a href=\"http://link.com\">Link</a></p></article>",
						"[Link](http://link.com)"
					},
					new object[]
					{
						"<article><p><span title=\"Easter Egg\">Gotcha</span></p></article>",
						"Gotcha(Easter Egg)"
					},
					new object[]
					{
						"<article><pre><code>lot of code</code></pre></article>",
						"```lot of code```"
					},
					new object[]
					{
						"<article><p><em><code>em in code</code></em></p></article>",
						"*```em in code```*"
					},
					new object[]
					{
						"<article><p><em class=\"star\">star</em></p></article>",
						"*star*"
					}
				};
 
			public static IEnumerable<object[]> TestData => Data;
	}
}