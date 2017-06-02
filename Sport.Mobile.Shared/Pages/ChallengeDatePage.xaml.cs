using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Sport.Mobile.Shared
{
	public partial class ChallengeDatePage : ChallengeDateXaml
	{
		public Action<Challenge> OnChallengeSent
		{
			get;
			set;
		}

		public Athlete Challengee
		{
			get;
			private set;
		}

		public League League
		{
			get;
			private set;
		}

		public ChallengeDatePage ()
		{
			Initialize ();
		}

		public ChallengeDatePage(Athlete challengee, League league)
		{
			SetTheme(league);
			ViewModel.CreateChallenge(App.Instance.CurrentAthlete, challengee, league);
			Challengee = challengee;
			League = league;

			Initialize();
		}
		ToolbarItem btnCancel;
		protected override void Initialize()
		{
			InitializeComponent();
			Title = "Date and Time";


			btnCancel = new ToolbarItem {
				Text = "Cancel"
			};


			ToolbarItems.Add(btnCancel);
		}
		public async void Challenge (object sender, EventArgs e)
		{
			var errors = ViewModel.Validate ();

			if (errors != null) {
				errors.ToToast (ToastNotificationType.Error);
				return;
			}

			Challenge challenge;
			using (new HUD ("Sending challenge...")) {
				challenge = await ViewModel.PostChallenge ();
			}

			if (OnChallengeSent != null && challenge != null && challenge.Id != null)
				OnChallengeSent (challenge);
		}
		public async void Canceled (object sender, EventArgs e)
		{
			try {
				await Navigation.PopModalAsync ();
			} catch (Exception ex) {

			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			btnChallenge.Clicked += Challenge;
			btnCancel.Clicked += Canceled;
		}
		protected override void OnDisappearing()
		{
			btnChallenge.Clicked -= Challenge;
			btnCancel.Clicked -= Canceled;
			ViewModel.CancelTasks();
			base.OnDisappearing();
		}

		protected override void TrackPage(Dictionary<string, string> metadata)
		{
			if(ViewModel?.Challenge != null)
			{
				metadata.Add("challengeId", ViewModel.Challenge.Id);
				metadata.Add("challengerAthleteId", ViewModel.Challenge.ChallengerAthleteId);
				metadata.Add("challengeeAthleteId", ViewModel.Challenge.ChallengeeAthleteId);
			}

			base.TrackPage(metadata);
		}
	}

	public partial class ChallengeDateXaml : BaseContentPage<ChallengeDateViewModel>
	{
	}
}