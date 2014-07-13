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
			var command = commandParser.Parse (CommandTextField.Text);
			if (command != null) {
				if (command is AddCommand) {
					var addCommand = command as AddCommand;
					var task = new Task { 
						Description = addCommand.Description,
						Priority = addCommand.Priority,
						Due = addCommand.Due,
						Created = DateTime.Now,
						Completed = false
					};
					TaskList.TaskCollection.Add (task);
				}
				if (command is TaskParameterisedCommand) {
					var parameterisedCommand = command as TaskParameterisedCommand;
					IList<Task> tasks = new List<Task> ();

					if (parameterisedCommand.RowId == null) {
						var sectionTasks = dataSource.GetTasksFromSection(parameterisedCommand.SectionId);
						if (sectionTasks != null)
							tasks = sectionTasks.ToList();
					} else {
						var task = dataSource.GetTaskAtSectionAndRow (parameterisedCommand.SectionId, parameterisedCommand.RowId.Value);
						if (task != null)
							tasks.Add (task);
					}
					if (tasks.Count == 0)
						return false;

					foreach (var task in tasks) {
						if (parameterisedCommand is RemoveCommand)
							TaskList.TaskCollection.Remove (task);
						else if (parameterisedCommand is CompleteCommand)
							task.Completed = true;
						else if (parameterisedCommand is PostponeCommand && task.Due.HasValue) {
							if (task.Due < DateTime.Today) {
								task.Due = DateTime.Today;
							} else {
								task.Due = task.Due.Value.AddDays (1);
							}
						}
					}
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

