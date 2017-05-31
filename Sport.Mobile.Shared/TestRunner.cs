using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
namespace Sport.Mobile.Shared
{
	public class TestOptions
	{
		public int IntervalSeconds { get; set; } = 85;

		public int MinRunTime { get; set; } = 65;

		public int HoldTime { get; set; } = 5;

		public int Timeout { get; set; } = 0;

		public double CrashPercent { get; set; } = .12;

		public int Intro { get; set; } = 1;
		//
		//		public bool HasStarted { get; set; }

		static readonly Random random = new Random ();

		public bool ShouldCrash ()
		{
			return random.NextDouble () < CrashPercent;
		}

		public int EndDuration ()
		{
			return random.Next (MinRunTime * 1000, IntervalSeconds * 1000) + (Intro * 1000);
		}

		public int TotalDuration ()
		{
			return (IntervalSeconds + HoldTime + Intro + Timeout) * 1000;
		}
	}

	public class TestRunner
	{
		public static TestRunner Shared { get; set; } = new TestRunner ();
		public TestRunner ()
		{
		}
		TestOptions options = new TestOptions {

		};

		public Action OnReset { get; set; }
		public Action OnStart { get; set; }
		public Action OnFail { get; set; }
		public Action OnSuccess{ get; set; }
		static Random random = new Random ();
		const int minDelay = 3000;
		const int maxDelay = 5000;
		public Task Delay (int min = minDelay, int max = maxDelay)
		{
			return Task.Delay (random.Next (min, max));
		}

		public async Task Delay (CancellationToken cancellationToken,CancellationToken cancellationToken2, int min = minDelay, int max = maxDelay)
		{
			cancellationToken.ThrowIfCancellationRequested ();
			cancellationToken2.ThrowIfCancellationRequested ();
			await Delay (min,max);
			cancellationToken.ThrowIfCancellationRequested ();
			cancellationToken2.ThrowIfCancellationRequested ();
		}
		static object locker = new object ();
		CancellationTokenSource currentTestRun;
		static string _currentTestRunId;
		static string CurrentTestRunId {
			get {
				lock (locker)
					return _currentTestRunId;
			}
			set {
				lock (locker)
					_currentTestRunId = value;
			}
		}
		public async void StartTests ()
		{
			lock (locker) {
				currentTestRun?.Cancel ();
				currentTestRun = new CancellationTokenSource ();
			}
			while (true) {
				try {
					var testRunID = CurrentTestRunId = Guid.NewGuid ().ToString ();
					var totalDuration = options.TotalDuration ();
					await Task.WhenAll (Task.Delay (totalDuration), DoTestRun (currentTestRun.Token,testRunID));
					OnReset ();
					await Task.Delay (options.Intro * 1000);
				} catch (Exception ex) {
					//Debug.WriteLine (ex);
				}
			}
		}


		async Task DoTestRun (CancellationToken cancellationToken, string testRunId)
		{

			Debug.WriteLine ($"Starting: {testRunId}");
			var duration = options.EndDuration ();
			var timeoutTask = Task.Delay (duration );
			var success = options.ShouldCrash ();

			Debug.WriteLine ("Crash: {0} Duration: {1} TotalDuration: {2}", success, duration, options.TotalDuration ());

			var secondToken = new CancellationTokenSource ();
			try {
				var runTestTask = RunTest (cancellationToken, secondToken.Token);
				var finishedTask = await Task.WhenAny (runTestTask, timeoutTask);
				if (success && finishedTask == runTestTask)
					success = runTestTask.Result;
				Debug.WriteLine ("Finished");
				cancellationToken.ThrowIfCancellationRequested ();
				secondToken.Token.ThrowIfCancellationRequested ();
				secondToken.Cancel ();
				if (testRunId != CurrentTestRunId) {
					Debug.WriteLine ($"Test should have canceled: {testRunId}");
					return;
				}
				if (success)
					OnSuccess ();
				else
					OnFail ();

			} catch (Exception ex) {
				//Debug.WriteLine (ex);
				Debug.WriteLine ($"Cancled: {testRunId}");
			} finally {
				secondToken.Cancel ();
				Debug.WriteLine ($"Completed: {testRunId}");
			}
		}


		public async Task Reset ()
		{
			currentTestRun?.Cancel ();
			OnReset ();
			await Task.Delay (options.Intro * 1000);
		}

		async Task<bool> RunTest (CancellationToken cancellationToken, CancellationToken testToken)
		{
			var startTime = DateTime.Now;
			try {
				OnStart ();

				await Delay (cancellationToken,testToken, 4000, 6000);
				var login = GetPage<WelcomeStartPage> ();
				login.AuthButtonClicked (this, EventArgs.Empty);

				await Delay (cancellationToken,testToken);
				var aliasPage = GetPage<SetAliasPage> ();
				aliasPage.SaveButtonClicked (this, EventArgs.Empty);

				await Delay (cancellationToken,testToken, 5000, 6000);
				var athleteProfileViewModel = GetViewModel<AthleteProfileViewModel> ();
				athleteProfileViewModel.EnablePushNotifications = !athleteProfileViewModel.EnablePushNotifications;
				await Delay (cancellationToken,testToken, 2000, 3000);

				var pushPage = GetPage<EnablePushPage> ();
				pushPage.ContinueButtonClicked (this, EventArgs.Empty);

				await Delay (cancellationToken, testToken);
				var availableLeaguesPage = GetPage<AthleteLeaguesPage> ();
				var availableLeaguesViewModel = GetViewModel<AthleteLeaguesViewModel> ();
				availableLeaguesPage.List.SelectedItem = availableLeaguesViewModel.Leagues [0];

				await Delay (cancellationToken, testToken);
				await TestLeaderBoard (cancellationToken, testToken);

				await Delay (cancellationToken, testToken);
				availableLeaguesPage = GetPage<AthleteLeaguesPage> ();
				availableLeaguesViewModel = GetViewModel<AthleteLeaguesViewModel> ();
				availableLeaguesPage.List.SelectedItem = availableLeaguesViewModel.Leagues [1];

				await Delay (cancellationToken, testToken);
				await TestLeaderBoard (cancellationToken, testToken);
				await Delay (cancellationToken, testToken);

				return true;


			} catch (TaskCanceledException) {
				return true;

			} catch (Exception ex) {
				//Debug.WriteLine (ex);
			} finally {
				Debug.WriteLine ($"Test run took: {(DateTime.Now - startTime).TotalSeconds}");
			}
			return false;
		}
		async Task TestLeaderBoard (CancellationToken cancellationToken, CancellationToken testToken)
		{
			var leagueDetails = GetPage<LeagueDetailsPage> ();
			leagueDetails.OnRankings ();

			await Delay (cancellationToken, testToken);
			var leaderPage = GetPage<LeaderboardPage> ();
			var leaderBoardViewModel = GetViewModel<LeaderboardViewModel> ();
			leaderPage.List.ScrollTo (leaderBoardViewModel.Memberships.Last (), ScrollToPosition.MakeVisible, true);

			await Delay (cancellationToken, testToken);
			leaderPage.List.SelectedItem = leaderBoardViewModel.Memberships [leaderBoardViewModel.Memberships.Count - 2];

			await Delay (cancellationToken, testToken);
			var membershipDetailsPage = GetPage<MembershipDetailsPage> ();
			membershipDetailsPage.ChallengeClicked (this, EventArgs.Empty);

			await Delay (cancellationToken, testToken);
			membershipDetailsPage.Done (this, EventArgs.Empty);

			await Delay (cancellationToken, testToken);
			await GoBack ();

			await Delay (cancellationToken, testToken);
			await GoBack ();

			await Delay (cancellationToken, testToken);
			await GoBack ();
		}

		public static async Task GoBack ()
		{
			if (App.Current.MainPage.Navigation.ModalStack.Count > 0)
				await App.Current.MainPage.Navigation.PopModalAsync (true);
			else
				await App.Current.MainPage.Navigation.PopAsync (true);
		}

		public static T GetViewModel<T> ()
		{
			return (T)(GetCurrentPage().BindingContext);
		}
		public static T GetPage<T> () where T : Xamarin.Forms.Page
		{
			var page = GetCurrentPage ();
			try {
				return (T)page;
			} catch (Exception ex) {
				Debug.WriteLine ($"Expected: {typeof(T)}, Was: {page.GetType ()}");
				throw ex;
			}
		}

		static Page GetCurrentPage ()
		{
			var page = App.Current.MainPage;
			var navPage = page as NavigationPage;
			return navPage?.CurrentPage ?? page;
		}
	}
}
