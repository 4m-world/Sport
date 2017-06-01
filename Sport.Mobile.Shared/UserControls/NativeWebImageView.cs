using System;
using Xamarin.Forms;
namespace Xamarin.Forms
{
	public partial class NativeWebImageView : Image
	{
		public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create (nameof (PlaceHolder), typeof (ImageSource), typeof (Image), default (ImageSource));

		[TypeConverter (typeof (ImageSourceConverter))]
		public ImageSource PlaceHolder {
			get { return (ImageSource)GetValue (PlaceHolderProperty); }
			set { SetValue (PlaceHolderProperty, value); }
		}
	}
}
