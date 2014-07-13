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
					PriorityTextField.Text = task.Priority.FormatPriority ();
			}

			ControlView.SetBottomBorder (2.0f);

			CompletedSwitch.ValueChanged += HandleCompletedSwitchValueChanged;
			DescriptionTextField.ShouldReturn += HandleDescriptionShouldReturn;

			var datePicker = new UIDatePicker ();
			datePicker.SetDate (task.Due, false);
			datePicker.Mode = UIDatePickerMode.Date;
			datePicker.ValueChanged += (object sender, EventArgs e) => { 
				DueDateTextField.Text = datePicker.Date.FormatDate(); 
				task.Due = datePicker.Date.ToDateTime ();
			};
			DueDateTextField.InputView = datePicker;

			var priorityPicker = new UIPickerView (new RectangleF(0.0f, 0.0f, View.Frame.Width, 100.0f));
			priorityPicker.Model = new PriorityPickerViewModel (task, PriorityTextField);
			PriorityTextField.InputView = priorityPicker;

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

		class PriorityPickerViewModel : UIPickerViewModel
		{
			private string[] priorities = new string[] { "Low", "Medium", "High" };
			private readonly Task task;
			private readonly UITextField textField;

			public PriorityPickerViewModel(Task task, UITextField priorityTextField) {
				this.task = task;
				this.textField = priorityTextField;
			}

			public override void Selected (UIPickerView picker, int row, int component)
			{
				task.Priority = row + 1;
				textField.Text = new int? (row + 1).FormatPriority ();
			}

			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return 3;
			}

			public override string GetTitle (UIPickerView picker, int row, int component)
			{
				return priorities[row];
			}
		}
	}
}
