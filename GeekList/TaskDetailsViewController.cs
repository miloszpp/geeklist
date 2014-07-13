using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Drawing;

namespace GeekList
{
	partial class TaskDetailsViewController : UITableViewController
	{
		private Task task;

		public TaskDetailsViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetTask(Task task) {
			this.task = task;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (task != null) {
				DescriptionTextField.Text = task.Description;
				CompletedSwitch.SetState(task.Completed, false);
				if (task.Due.HasValue)
					DueDateTextField.Text = task.Due.FormatDate ();
				if (task.Priority.HasValue)
					PriorityLabel.Text = task.Priority.FormatPriority ();
			}

			ControlView.SetBottomBorder (2.0f);

			CompletedSwitch.ValueChanged += HandleCompletedSwitchValueChanged;
			DescriptionTextField.ShouldReturn += HandleDescriptionShouldReturn;

			var datePicker = new UIDatePicker ();
			datePicker.Mode = UIDatePickerMode.Date;
			datePicker.ValueChanged += (object sender, EventArgs e) => { 
				DueDateTextField.Text = datePicker.Date.FormatDate(); 
				task.Due = datePicker.Date.ToDateTime ();
			};
			DueDateTextField.InputView = datePicker;

			View.SetupKeyboardDismissal ();
		}


		bool HandleDescriptionShouldReturn (UITextField textField)
		{
			if (!string.IsNullOrEmpty (DescriptionTextField.Text)) {
				task.Description = DescriptionTextField.Text;
				textField.ResignFirstResponder ();
				return true;
			}
			return false;
		}

		void HandleCompletedSwitchValueChanged (object sender, EventArgs e)
		{
			task.Completed = CompletedSwitch.On;
		}
	}
}
