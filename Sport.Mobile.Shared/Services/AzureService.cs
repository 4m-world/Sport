using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using System.Diagnostics;

namespace Sport.Mobile.Shared
{
	public class AzureService
	{
		public AzureService()
		{
			var url = new Uri(Keys.AzureDomain);

			LeagueManager = new LeagueManager();
			MembershipManager = new MembershipManager();
			AthleteManager = new AthleteManager();
			ChallengeManager = new ChallengeManager();
			GameResultManager = new GameResultManager();
		}

		#region Properties

		public GameResultManager GameResultManager
		{
			get;
			private set;
		}

		public AthleteManager AthleteManager
		{
			get;
			private set;
		}

		public MembershipManager MembershipManager
		{
			get;
			private set;
		}

		public ChallengeManager ChallengeManager
		{
			get;
			private set;
		}

		public LeagueManager LeagueManager
		{
			get;
			private set;
		}


		static AzureService _instance;

		public static AzureService Instance
		{
			get
			{
				return _instance ?? (_instance = new AzureService());
			}
		}


		#endregion

		public Task<bool> SyncAllAsync()
		{
			return Task.FromResult (true);
		}

		#region Push Notifications

		/// <summary>
		/// This app uses Azure as the backend which utilizes Notifications hubs
		/// </summary>
		/// <returns>The athlete notification hub registration.</returns>
		public Task UpdateAthleteNotificationHubRegistration(Athlete athlete, bool forceSave = false, bool sendTestPush = true)
		{

			return Task.FromResult (false);

		}

		public Task UnregisterAthleteForPush(Athlete athlete)
		{
			return Task.FromResult (false);
		}

		#endregion
	}
}