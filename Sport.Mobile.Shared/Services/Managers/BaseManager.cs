using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Connectivity;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Sport.Mobile.Shared.Services;

namespace Sport.Mobile.Shared
{
	public class BaseManager<T> where T : BaseModel, new ()
	{
		public virtual string Identifier => "Items";

		public void DropTable()
		{
			
		}

		public List<T> Table => LocalDatabase.GetItems<T> ();

		public virtual IList<T> GetItems(bool forceRefresh = false)
		{

			return LocalDatabase.GetItems<T> ();
		}

		public virtual T GetItem(string id, bool forceRefresh = false)
		{

			var item = LocalDatabase.GetItem<T> (id);
			return item;
		}

		public virtual async Task<bool> UpsertAsync(T item)
		{
			if(item.Id == null)
			{
				return await InsertAsync(item);
			}
			else
			{
				return await UpdateAsync(item);
			}
		}

		public virtual async Task<bool> InsertAsync(T item)
		{
			LocalDatabase.Insert (item);
			var success = await SyncAsync ();

			//if(success)
			//{
			//	var updated = GetItem (item.Id, false);
			//	item.Version = updated.Version;
			//	item.UpdatedAt = updated.UpdatedAt;

			//}

			return success;
		}

		public virtual async Task<bool> UpdateAsync(T item)
		{
			try
			{
				LocalDatabase.Update (item);
				var success = await SyncAsync().ConfigureAwait(false);
				var updated = GetItem (item.Id, false);

				//item.Version = updated.Version;
				//item.UpdatedAt = updated.UpdatedAt;;

				return success;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public virtual async Task<bool> RemoveAsync(T item)
		{
			LocalDatabase.Delete (item);
			var success = await SyncAsync().ConfigureAwait(false);
			return success;
		}

		public async Task<bool> PullLatestAsync()
		{
			await Task.Delay (1000);
			return true;
		}


		public async Task<bool> SyncAsync()
		{
			await Task.Delay (1000);
			return true;
		}
	}
}