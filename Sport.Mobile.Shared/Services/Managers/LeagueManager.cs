using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sport.Mobile.Shared.Services;

namespace Sport.Mobile.Shared
{
	public class LeagueManager : BaseManager<League>
	{
		public override string Identifier => "League";

		public Task<DateTime?> StartLeague(string id)
		{
			return new Task<DateTime?>(() => {
				var qs = new Dictionary<string, string>();
				qs.Add("id", id);
				//var dateTime = AzureService.Instance.Client.InvokeApiAsync("startLeague", null, HttpMethod.Post, qs).Result;
				return DateTime.Now;
			});
		}
	}
}