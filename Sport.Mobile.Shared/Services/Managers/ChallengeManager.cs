using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sport.Mobile.Shared
{
	public class ChallengeManager : BaseManager<Challenge>
	{
		public override string Identifier => "Challenge";

		public void PostMatchResults(Challenge challenge)
		{
			//TODO: save
			//return new Task(() => {
			//	var completedChallenge = AzureService.Instance.Client.InvokeApiAsync<List<GameResult>, Challenge>("postMatchResults", challenge.MatchResult).Result;
			//	if(completedChallenge != null)
			//	{
			//		PullLatestAsync().Wait();
			//		AzureService.Instance.GameResultManager.PullLatestAsync().Wait();
			//		AzureService.Instance.MembershipManager.PullLatestAsync().Wait();

			//		challenge.League?.LocalRefresh();
			//		challenge.LocalRefresh();
			//	}
			//});
		}

		public Task NudgeAthlete(string challengeId)
		{
			return Task.FromResult (false);
			//return new Task(() => {
			//	var qs = new Dictionary<string, string>();
			//	qs.Add("challengeId", challengeId);
			//	var g = AzureService.Instance.Client.InvokeApiAsync("nudgeAthlete", null, HttpMethod.Get, qs).Result;
			//});
		}
	}
}