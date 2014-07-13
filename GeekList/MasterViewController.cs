using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.CoreAnimation;

namespace GeekList
{
	public partial class MasterViewController : UITableViewController
	{
		private TaskDataSource dataSource;
		private CommandParser commandParser;

		public TaskList TaskList {
			get;
			set;
		}

		public MasterViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			commandParser = new CommandParser ();
			dataSource = new TaskDataSource (TaskList.TaskCollection);
			TableView.Source = dataSource;
			CommandTextField.ShouldReturn += HandleCommandTextFieldShouldReturn;

			var toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, View.Frame.Size.Width, 44.0f));
			var hashButton = new UIBarButtonItem { Title = "Priority" };
			hashButton.Clicked += (object sender, EventArgs e) => { CommandTextField.Text += "#"; };
			var tickButton = new UIBarButtonItem { Title = "Due" };
			tickButton.Clicked += (object sender, EventArgs e) => { CommandTextField.Text += "^"; };
			toolbar.Translucent = false;
			toolbar.Items = new UIBarButtonItem[] { hashButton, tickButton };
			CommandTextField.InputAccessoryView = toolbar;

			ControlView.SetBottomBorder (2.0f);

			SortSwitch.ValueChanged += HandleSortSwitchValueChanged;

			View.SetupKeyboardDismissal ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			TableView.ReloadData ();
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail") 
			{
				var item = dataSource.GetTaskAtSectionAndRow (TableView.IndexPathForSelectedRow.Section, TableView.IndexPathForSelectedRow.Row);
				((TaskDetailsViewController)segue.DestinationViewController).SetTask (item);
			}
		}

		private void HandleSortSwitchValueChanged (object sender, EventArgs e)
		{
			if (SortSwitch.SelectedSegment == 0) {
				dataSource.Mode = SortMode.DueDate;
			}
			if (SortSwitch.SelectedSegment == 1) {
				dataSource.Mode = SortMode.Priority;
			}
			if (SortSwitch.SelectedSegment == 2) {
				dataSource.Mode = SortMode.Completed;
			}
			TableView.ReloadData ();
		}

		private bool HandleCommandTextFieldShouldReturn(UITextField sender) {
			if (string.IsNullOrEmpty (CommandTextField.Text)) {
				CommandTextField.ResignFirstResponder();
				return true;
			}
			var commands = commandParser.Parse (CommandTextField.Text);
			if (commands.Count == 1) {
				if (commands.First () is AddCommand) {
					var addCommand = commands.First () as AddCommand;
					var task = new Task { 
						Description = addCommand.Description,
						Priority = addCommand.Priority,
						Due = addCommand.Due,
						Created = DateTime.Now,
						Completed = false
					};
					TaskList.TaskCollection.Add (task);
				}
				if (commands.First () is RemoveCommand) {
					var removeCommand = commands.First () as RemoveCommand;
					var task = dataSource.GetTaskAtSectionAndRow (removeCommand.SectionId, removeCommand.RowId);
					if (task == null)
						return false;
					TaskList.TaskCollection.Remove (task);
				}
				if (commands.First () is CompleteCommand) {
					var completeCommand = commands.First () as CompleteCommand;
					var task = dataSource.GetTaskAtSectionAndRow (completeCommand.SectionId, completeCommand.RowId);
					if (task == null)
						return false;
					task.Completed = true;
				}

				TableView.ReloadData ();
				CommandTextField.ResignFirstResponder();
				CommandTextField.Text = string.Empty;
				return true; 
			}
			return false;
		}
	}
}

