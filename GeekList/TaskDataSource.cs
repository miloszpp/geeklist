﻿using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Linq;

namespace GeekList
{
	enum SortMode {
		DueDate, Priority, Completed
	}

	class TaskDataSource : UITableViewSource
	{
		private static readonly NSString CellIdentifier = new NSString ("taskcell");
		private readonly IList<Task> objects;
		private IEnumerable<IGrouping<DateTime?, Task>> dueSections;
		private IEnumerable<IGrouping<int?, Task>> prioritySections;
		private IEnumerable<Task> completed;

		private static DateTime Today = DateTime.Today.Date;

		public TaskDataSource(IList<Task> tasks) {
			objects = tasks;
			Mode = SortMode.DueDate;
			dueSections = objects
				.Where (t => t.Due.HasValue && !t.Completed)
				.OrderBy (t => t.Due)
				.Union (objects.Where (t => !t.Due.HasValue && !t.Completed))
				.GroupBy (t => {
					if (!t.Due.HasValue)
						return null;
					if (DateTime.Compare(t.Due.Value.Date, Today) < 0)
						return OutputFormatter.Overdue;
					return new DateTime? (t.Due.Value.Date);
					}
			);
					
			prioritySections = objects
				. Where(t => !t.Completed)
				. OrderByDescending (t => t.Priority)
				. GroupBy (t => t.Priority);
			completed = objects.Where (t => t.Completed);
		}

		public SortMode Mode {
			get;
			set;
		}

		public IEnumerable<Task> GetTasksFromSection(int section) {
			try {
				switch (Mode) {
				case SortMode.DueDate:
					return dueSections.ElementAt (section);
				case SortMode.Priority:
					return prioritySections.ElementAt (section);
				case SortMode.Completed:
					return null;
				}
			} catch (ArgumentOutOfRangeException) {
				return null;
			}
			return null;
		}

		public Task GetTaskAtSectionAndRow(int section, int row) {
			try {
				switch (Mode) {
				case SortMode.DueDate:
					return dueSections.ElementAt (section).ElementAt (row);
				case SortMode.Priority:
					return prioritySections.ElementAt (section).ElementAt (row);
				case SortMode.Completed:
					return completed.ElementAt (row);
				}
			} catch (ArgumentOutOfRangeException) {
				return null;
			}
			return null;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			switch (Mode) {
			case SortMode.DueDate:
				return dueSections.Count ();
			case SortMode.Priority:
				return prioritySections.Count ();
			case SortMode.Completed:
				return 1;
			}
			return 0;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			switch (Mode) {
			case SortMode.DueDate:
				return dueSections.ElementAt (section).Count ();
			case SortMode.Priority:
				return prioritySections.ElementAt (section).Count ();
			case SortMode.Completed:
				return completed.Count ();
			}
			return 0;
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			switch (Mode) {
			case SortMode.DueDate:
				return dueSections.ElementAt (section).Key.FormatDate();
			case SortMode.Priority:
				return prioritySections.ElementAt (section).Key.FormatPriority ();
			case SortMode.Completed:
				return null;
			}
			return null;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (UITableViewCell)tableView.DequeueReusableCell (CellIdentifier, indexPath);

			Task task = null;
			switch (Mode) {
			case SortMode.DueDate:
				task = dueSections.ElementAt (indexPath.Section).ElementAt (indexPath.Row);
				break;
			case SortMode.Priority:
				task = prioritySections.ElementAt (indexPath.Section).ElementAt (indexPath.Row);
				break;
			case SortMode.Completed:
				task = completed.ElementAt (indexPath.Row);
				break;
			}

			cell.TextLabel.Text = string.Format ("[{0}.{1}] {2}", indexPath.Section.FormatId(), indexPath.Row.FormatId(), task.Description);

			return cell;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle) {
			case UITableViewCellEditingStyle.Delete:
				var task = GetTaskAtSectionAndRow (indexPath.Section, indexPath.Row);
				objects.Remove (task);
				tableView.ReloadData ();
				break;
			case UITableViewCellEditingStyle.None:
				break;
			}
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}
	}

}

