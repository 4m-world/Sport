using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Forms
{
	public class NativeWebImageViewRenderer : ImageRenderer
	{
		public NativeWebImageViewRenderer ()
		{
		}
		NativeWebImageView ImageView => Element as NativeWebImageView;
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Image.SourceProperty.PropertyName) {
				SetImage ();
			} else {
				base.OnElementPropertyChanged (sender, e);
			}
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			if (e.OldElement == null) {
				var view = CreateNativeControl ();
				SetNativeControl (view);
			}
		}
		void SetImage (Image oldElement = null)
		{
			var source = Element.Source as UriImageSource;

			if (oldElement != null) {
				var oldSource = oldElement.Source;
				if (Equals (oldSource, source))
					return;
				Control.SetImageDrawable (null);
			}
			if (source == null)
				return;

			var placeholerSource = ImageView.PlaceHolder as FileImageSource;
			var placeholderImage = placeholerSource == null ? null : global::Android.Graphics.Drawables.Drawable.CreateFromPath (placeholerSource);
			if (placeholderImage != null) {
				// Not sure if this will actually work, migjht need to just:
				Control.SetImageDrawable (placeholderImage);
			} else
				((IImageController)Element).SetIsLoading (true);
			Bumptech.Glide.With (Forms.Context).Load (source.Uri).Into (Control);
		}
	}
}
