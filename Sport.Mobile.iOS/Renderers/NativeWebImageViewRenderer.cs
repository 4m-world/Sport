using System;
using System.ComponentModel;
using Xamarin.Forms;
using SDWebImage;
using UIKit;
using System.Drawing;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(NativeWebImageView),typeof(NativeWebImageViewRenderer))]
namespace Xamarin.Forms.Platform.iOS
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
			if (Control == null) {
				var imageView = new UIImageView ();
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				imageView.ClipsToBounds = true;
				SetNativeControl (imageView);
			}

			if (e.NewElement != null) {
				Control.ContentMode = Element.Aspect.ToUIViewContentMode ();
				SetImage (e.OldElement);
				Control.Opaque = Element.IsOpaque;
			}
		}

		void SetImage (Image oldElement = null)
		{
			var source = Element.Source as UriImageSource;

			if (oldElement != null) {
				var oldSource = oldElement.Source;
				if (Equals (oldSource, source))
					return;
				Control.Image = null;
			}
			if (source == null)
				return;

			var placeholerSource = ImageView.PlaceHolder as FileImageSource;
			var placeholderImage = placeholerSource == null ? null : new UIImage (placeholerSource);
			if (placeholderImage != null) {
				Control.SetImage (new Foundation.NSUrl (source.Uri.AbsolutePath), placeholderImage);
			} else{
				((IImageController)Element).SetIsLoading (true);
				Control.SetImage (new Foundation.NSUrl (source.Uri.AbsoluteUri));
			}
		}
	}
}
