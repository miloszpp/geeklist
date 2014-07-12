using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using System.CodeDom.Compiler;
using System.Collections.Generic;


namespace GeekList
{
	partial class CreateTaskViewController : UIViewController
	{
		public CreateTaskViewController (IntPtr handle) : base (handle)
		{
		}

		public ICreateTaskViewControllerDelegate Delegate {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			CreateTaskButton.TouchUpInside += (object sender, EventArgs e) => CreateClicked ();
		}

		private void CreateClicked() 
		{
			var task = new Task 
			{
				Description = TaskDescriptionField.Text
			};
			Delegate.TaskCreated(task);
			NavigationController.PopViewControllerAnimated(true);
		}
	}

	interface ICreateTaskViewControllerDelegate {
		void TaskCreated(Task task);
	}
}
