using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace CliUtils
{
	public static class Custom
	{
		public static readonly Lazy<HttpClient> HttpClient 
			= new(HttpClientFactory, LazyThreadSafetyMode.ExecutionAndPublication);

		private static HttpClient HttpClientFactory()
		{
			var cookieContainer = new CookieContainer();
			var handler = new HttpClientHandler {CookieContainer = cookieContainer};
			{
				var session = Environment.GetEnvironmentVariable("SESSION");
				if (session != null)
					cookieContainer.Add(new Cookie("session", session, "/", ".adventofcode.com"));
				return new HttpClient(handler)
				{
					BaseAddress = new Uri("https://www.adventofcode.com/")
				};
			}
		}

		public static string GetChallengeUrl(int year, int day)
		{
			return $"{year}/day/{day}";
		}
	}
}