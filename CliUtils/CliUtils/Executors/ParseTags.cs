using System.Linq;
using System.Text.RegularExpressions;

namespace CliUtils.Executors
{
	public static class ParseTags
	{
		public static string Header(string html)
			=> $"# {Regex.Replace(html, @"^(<h2.*?>)|(<\/h2>)$", string.Empty)}";
		
		public static string Paragraph(string html)
			=> Regex.Replace(html, @"(<p>)|(<\/p>)", string.Empty);
		
		public static string Code(string html)
		{
			if (!Regex.IsMatch(html, @"(<code.*?>)|(<\/code>)"))
				return html;
			
			
			var replaced = Regex.Replace(html, @"(<code.*?>)|(<\/code>)", "```");
			return string.Join(string.Empty, replaced.Split("```").Select((r, idx) =>
			{
				if (idx % 2 == 0)
					return r;
				
				return r.Contains("\n") ? $"```\n{r}```" : $"```{r}```";
			}));
		}
		
		public static string Emphasis(string html)
			=> Regex.Replace(html, "(<em(>|\\sclass=\"star\">))|(<\\/em>)", "*");
		
		public static string UnorderedList(string html)
			=> Regex.Replace(html, @"(<ul>)|(<\/ul>)", string.Empty)
				.Replace(@"<li>", "* ")
				.Replace("</li>", string.Empty);
		
		public static string Link(string html)
		{
			if (!Regex.IsMatch(html, @"(<a(.*))|(</a>)"))
				return html;
			
			var hrefMatch = Regex.Match(html, "href=\"(?<s>.+)\"");
			
			return Regex.Replace(html, "(<a(.*?)>)", "[")
				.Replace("</a>", $"]({hrefMatch.Groups["s"].Value})");
		}
		
		public static string Span(string html)
		{
			if (!Regex.IsMatch(html, @"(<span(.*))|(</span>)"))
				return html;

			var titleMatch = Regex.Match(html, "title=\"(?<s>.+?)\"");
			var res = html;
			if (titleMatch.Success)
				res = Regex.Replace(html, "<span title=\"(.+?)\">", $"({titleMatch.Groups["s"].Value}) ");
			
			res = Regex.Replace(res, "(<span(.+?)>)", string.Empty)
				.Replace("</span>", string.Empty);
			
			return res;
		}
		
		public static string Pre(string html)
			=> Regex.Replace(html, @"(<pre(.*?)>)|(<\/pre>)", "\n");
	}
}