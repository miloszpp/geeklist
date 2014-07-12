using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.IO;

namespace GeekList
{
	partial class HelpViewContainer : UIViewController
	{
		public HelpViewContainer (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var localHtmlUrl = Path.Combine (NSBundle.MainBundle.BundlePath, "WebContent/Help.html");
			HelpWebView.LoadRequest(new NSUrlRequest(new NSUrl(localHtmlUrl, false)));
			HelpWebView.ScalesPageToFit = false;
		}
	}
}
