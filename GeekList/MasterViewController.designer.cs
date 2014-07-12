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
	[Register ("MasterViewController")]
	partial class MasterViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MonoTouch.UIKit.UITextField CommandTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MonoTouch.UIKit.UIView ControlView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MonoTouch.UIKit.UIBarButtonItem HelpButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		MonoTouch.UIKit.UISegmentedControl SortSwitch { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CommandTextField != null) {
				CommandTextField.Dispose ();
				CommandTextField = null;
			}
			if (ControlView != null) {
				ControlView.Dispose ();
				ControlView = null;
			}
			if (HelpButton != null) {
				HelpButton.Dispose ();
				HelpButton = null;
			}
			if (SortSwitch != null) {
				SortSwitch.Dispose ();
				SortSwitch = null;
			}
		}
	}
}
