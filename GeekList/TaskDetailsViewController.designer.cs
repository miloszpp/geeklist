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
	[Register ("TaskDetailsViewController")]
	partial class TaskDetailsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch CompletedSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ControlView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField DescriptionTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField DueDateTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel PriorityLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CompletedSwitch != null) {
				CompletedSwitch.Dispose ();
				CompletedSwitch = null;
			}
			if (ControlView != null) {
				ControlView.Dispose ();
				ControlView = null;
			}
			if (DescriptionTextField != null) {
				DescriptionTextField.Dispose ();
				DescriptionTextField = null;
			}
			if (DueDateTextField != null) {
				DueDateTextField.Dispose ();
				DueDateTextField = null;
			}
			if (PriorityLabel != null) {
				PriorityLabel.Dispose ();
				PriorityLabel = null;
			}
		}
	}
}
