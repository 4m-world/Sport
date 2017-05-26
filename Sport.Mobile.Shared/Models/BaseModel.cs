using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SQLite;

namespace Sport.Mobile.Shared
{
	public class BaseModel : BaseNotify, IDirty
	{
		public BaseModel ()
		{

		}
		string _id;

		[JsonProperty("id")]
		[PrimaryKey]
		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				SetPropertyChanged(ref _id, value);
			}
		}

		DateTime? _updatedAt;

		public DateTime? UpdatedAt
		{
			get
			{
				return _updatedAt;
			}
			set
			{
				SetPropertyChanged(ref _updatedAt, value);
			}
		}

		DateTime? _createdAt;

		public DateTime? CreatedAt
		{
			get
			{
				return _createdAt;
			}
			set
			{
				SetPropertyChanged(ref _createdAt, value);
			}
		}

		string _version;

		public string Version
		{
			get
			{
				return _version;
			}
			set
			{
				SetPropertyChanged(ref _version, value);	
			}
		}

		[JsonIgnore]
		[Ignore]
		public bool IsDirty
		{
			get;
			set;
		}

		public virtual void LocalRefresh()
		{
		}

		public virtual void NotifyPropertiesChanged([CallerMemberName] string caller = "")
		{
			Debug.WriteLine($"NotifyPropertiesChanged called for {GetType().Name} by {caller}");
		}
	}
}