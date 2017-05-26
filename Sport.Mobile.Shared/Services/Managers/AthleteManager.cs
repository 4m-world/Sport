
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Sport.Mobile.Shared.Services;

namespace Sport.Mobile.Shared
{
	public class AthleteManager : BaseManager<Athlete>
	{
		public override string Identifier => "Athlete";

		public Task<Athlete> GetAthleteByEmail(string email)
		{
			var a = LocalDatabase.GetItems<Athlete> ().FirstOrDefault (x=> x.Email == email);
			return Task.FromResult (a);
		}

		async public override Task<bool> UpdateAsync(Athlete item)
		{			
			var result = await base.UpdateAsync(item);

			if(item.Id == App.Instance.CurrentAthlete?.Id)
			{
				App.Instance.CurrentAthlete = AzureService.Instance.AthleteManager.GetItem(item.Id);
			}

			return result;
		}
	}
}