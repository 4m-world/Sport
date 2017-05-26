using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sport.Mobile.Shared
{
	public class AthleteProfileViewModel : AthleteViewModel
	{
		public ICommand SaveCommand
		{
			get
			{
				return new Command(async(param) =>
					await SaveAthlete());
			}
		}

		async public Task<bool> SaveAthlete()
		{
			using(new Busy(this))
			{
				var success = await AzureService.Instance.AthleteManager.UpsertAsync(Athlete);
				NotifyPropertiesChanged();
				return success;
			}
		}

		bool _enablePushNotifications;
		public bool EnablePushNotifications
		{
			get
			{
				return Settings.EnablePushNotifications;
			}
			set
			{
				SetPropertyChanged(ref _enablePushNotifications, value);
				SetPropertyChanged ("EnablePushNotifications");
				Settings.EnablePushNotifications = _enablePushNotifications;
			}
		}

		public Task<bool> RegisterForPushNotifications()
		{

			return Task.FromResult (true);
		}
	}
}