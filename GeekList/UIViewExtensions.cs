using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;

namespace GeekList
{
	public static class UIViewExtensions
	{
		public static void SetBottomBorder (this UIView view, float thickness)
		{
			var borderLayer = new CALayer ();
			borderLayer.Frame = new RectangleF (0.0f, view.Frame.Height - thickness, view.Frame.Width, thickness);
			borderLayer.BackgroundColor = new CGColor(68.0f/255.0f, 76.0f/255.0f, 120.0f/255.0f, 0.0f);
			view.Layer.AddSublayer (borderLayer);
		}

		public static void SetupKeyboardDismissal(this UIView view) {
			var gestureRecognizer = new UITapGestureRecognizer((UITapGestureRecognizer obj) => {
				view.EndEditing(true);
			});
			gestureRecognizer.CancelsTouchesInView = false;
			view.AddGestureRecognizer (gestureRecognizer);
		}
	}
}

