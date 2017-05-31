using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Sport.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.IO;
using System.Reflection;
using System.Linq;

[assembly: ExportImageSourceHandler (typeof (UriImageSource), typeof (ImageLoader))]
namespace Sport.Mobile.Droid.Renderers
{
	public class ImageLoader : IImageSourceHandler
	{
		public static int ScreenWidth { get; set; }
		public async Task<Bitmap> LoadImageAsync (ImageSource imagesource, Context context, CancellationToken cancelationToken = default (CancellationToken))
		{
			var imageLoader = imagesource as UriImageSource;
			if (imageLoader != null && imageLoader.Uri != null) {
				imageLoader.CachingEnabled = true;
				using (Stream imageStream = await GetStream (imageLoader,cancelationToken).ConfigureAwait (false))
					return await DecodeBitmap (imageStream, ScreenWidth);
			}
			return null;
		}

		static async Task<Stream> GetStream (UriImageSource imageSource,CancellationToken cancelationToken = default (CancellationToken))
		{
			try {
				var getStream = imageSource.GetType ().GetMethods (BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault (x => x.Name == "GetStreamAsync");
				var task = getStream.Invoke (imageSource, parameters: new object [] { cancelationToken }) as Task<Stream>;
				var stream = await task;
				return stream;
			} catch (Exception ex) {
				return null;
			}

		}

		public static async Task<Bitmap> DecodeBitmap (Stream stream, int width)
		{
			var options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			await BitmapFactory.DecodeStreamAsync (stream, null, options);
			stream.Seek (0,SeekOrigin.Begin);
			int imageHeight = options.OutHeight;
			int imageWidth = options.OutWidth;
			if (imageWidth < width)
				return await BitmapFactory.DecodeStreamAsync (stream);
			var height = imageHeight * (width/imageWidth);

			String imageType = options.OutMimeType;

			options.InSampleSize = CalculateInSampleSize (options, width, height);

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;
			return await BitmapFactory.DecodeStreamAsync (stream, null, options);
		}

		public static int CalculateInSampleSize (
			BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			// Raw height and width of image
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {

				int halfHeight = height / 2;
				int halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) >= reqHeight
						&& (halfWidth / inSampleSize) >= reqWidth) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}
	}
}
