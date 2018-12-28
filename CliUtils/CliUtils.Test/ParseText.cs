using System;
using System.Threading.Tasks;
using Xunit;

namespace CliUtils.Test
{
	public class ParseText
	{
		[Theory]
		[MemberData(nameof(HtmlToMdDataSource.TestData), MemberType = typeof(HtmlToMdDataSource))]
		public async Task HtmlToMd(string html, string md)
		{
			var parsed = await Verbs.Parse.HtmlToMd(html);
			Assert.Equal(md, parsed);
		}
	}
}