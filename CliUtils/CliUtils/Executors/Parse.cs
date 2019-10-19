using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;

namespace CliUtils.Executors
{
	public static class Parse
	{
		private static readonly IDictionary<string, Func<string, string>> FuncMap =
			new Dictionary<string, Func<string, string>>
			{
				{ "H2", ParseTags.Header },
				{ "P", ParseTags.Paragraph },
				{ "CODE", ParseTags.Code },
				{ "EM",ParseTags.Emphasis },
				{ "UL", ParseTags.UnorderedList},
				{ "A", ParseTags.Link },
				{ "SPAN", ParseTags.Span },
				{ "PRE", ParseTags.Pre }
			};

		private static string HandleNode(IElement node)
		{
			var res = FuncMap[node.TagName](node.OuterHtml);
			var middlewareTags = new [] {"SPAN", "A", "EM", "CODE", "P", "PRE"};
			res = middlewareTags.Aggregate(res, (current, tag) => FuncMap[tag](current));
			return res;
		}

		public static Task<string> GetChallengeAsync(int year, int day)
			=> Custom.HttpClient.Value.GetStringAsync(Custom.GetChallengeUrl(year, day));
		
		public static Task<string> GetInputAsync(int year, int day)
			=> Custom.HttpClient.Value.GetStringAsync($"{Custom.GetChallengeUrl(year, day)}/input");
		
		public static async Task<string> HtmlToMdAsync(string html)
		{
			var parser = new HtmlParser();
			var document = await parser.ParseAsync(html);
			var children = 
				document.QuerySelectorAll("article").SelectMany(r => r.Children).ToArray();
			var sb = new StringBuilder();
			foreach (var child in children)
			{
				sb.Append(HandleNode(child));
				sb.Append(Environment.NewLine + Environment.NewLine);
			}

			sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		} 
	}
}