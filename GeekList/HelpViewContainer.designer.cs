// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace GeekList
{
	[Register ("HelpViewContainer")]
	partial class HelpViewContainer
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIWebView HelpWebView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (HelpWebView != null) {
				HelpWebView.Dispose ();
				HelpWebView = null;
			}
		}
	}
}
