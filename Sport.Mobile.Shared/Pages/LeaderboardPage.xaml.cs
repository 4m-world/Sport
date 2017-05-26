using Xamarin.Forms;
using System;
using System.Collections.Generic;

namespace Sport.Mobile.Shared
{
	public partial class LeaderboardPage : LeaderboardXaml
	{
		public LeaderboardPage ()
		{
			Initialize ();
		}

		public LeaderboardPage(League league)
		{
			ViewModel.League = league;
			SetTheme(league);

			Initialize();
		}

		~LeaderboardPage()
		{
			MessagingCenter.Unsubscribe<App>(this, Messages.ChallengesUpdated);
		}
		public ListView List => list;
		async protected override void Initialize()
		{
			InitializeComponent();
			Title = "Leaderboard";


			SubscribeToChallenges();

			if(ViewModel.League != null)
				await ViewModel.LocalRefresh();
		}

		async void OnItemSelected (object sender, EventArgs e)
		{
			if (list.SelectedItem == null)
				return;

			var vm = list.SelectedItem as MembershipViewModel;
			list.SelectedItem = null;
			var page = new MembershipDetailsPage (vm.Membership);

			await Navigation.PushAsync (page);
		}

		protected override void OnAppearing ()
		{
			list.ItemSelected += OnItemSelected;
			base.OnAppearing ();
		}

		protected override void OnDisappearing ()
		{
			list.ItemSelected -= OnItemSelected;
			base.OnDisappearing ();
		}

		protected override void TrackPage(Dictionary<string, string> metadata)
		{
			if(ViewModel?.League != null)
				metadata.Add("leagueId", ViewModel.League.Id);

			base.TrackPage(metadata);
		}

		void SubscribeToChallenges()
		{
			var self = new WeakReference<LeaderboardPage>(this);
			Action<App> action = async(app) =>
			{
				LeaderboardPage v;
				if(!self.TryGetTarget(out v))
					return;

				await v.ViewModel.LocalRefresh();
			};
			MessagingCenter.Subscribe<App>(this, Messages.ChallengesUpdated, action);
		}

		protected override async void OnIncomingPayload(NotificationPayload payload)
		{
			base.OnIncomingPayload(payload);

			string leagueId;
			if(payload.Payload.TryGetValue("leagueId", out leagueId))
			{
				if(leagueId == ViewModel.League.Id)
				{
					await ViewModel.GetLeaderboard(true);
				}
			}
		}
	}

	public partial class LeaderboardXaml : BaseContentPage<LeaderboardViewModel>
	{
	}
}