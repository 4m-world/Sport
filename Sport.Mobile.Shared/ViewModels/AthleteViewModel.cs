using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sport.Mobile.Shared
{
	public class AthleteViewModel : BaseViewModel
	{
		string _athleteId;

		public string AthleteId
		{
			get
			{
				return _athleteId;
			}
			set
			{
				_athleteId = value;
				_athlete = null;
				SetPropertyChanged("Athlete");
			}
		}

		Athlete _athlete;

		public Athlete Athlete
		{
			get
			{
				if(_athlete == null && AthleteId != null)
				{
					_athlete = AzureService.Instance.AthleteManager.GetItem(AthleteId);
				}
				return _athlete;
			}
		}

		public override void NotifyPropertiesChanged([CallerMemberName] string caller = "")
		{
			_athlete = null;
			SetPropertyChanged("Athlete");
			base.NotifyPropertiesChanged(caller);
		}
	}
}

