using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace GeekList
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private TaskList taskList;
		
		public override UIWindow Window {
			get;
			set;
		}

		public override void FinishedLaunching (UIApplication application)
		{
			taskList = new TaskList ();
			taskList.Load ();
			var masterViewController = (MasterViewController)((UINavigationController)Window.RootViewController).ViewControllers.First ();
			masterViewController.TaskList = taskList;
		}

		//
		// This method is invoked when the application is about to move from active to inactive state.
		//
		// OpenGL applications should use this method to pause.
		//
		public override void OnResignActivation (UIApplication application)
		{
		}
		
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
			taskList.Persist ();
			SetupNotifications (application);
			UpdateBadge (application);
		}
		
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
			taskList.Persist ();
			SetupNotifications (application);
			UpdateBadge (application);
			taskList.Dispose ();
		}


		void SetupNotifications (UIApplication application)
		{
			application.CancelAllLocalNotifications ();
			foreach (var g in taskList.TaskCollection
				.Where (t => t.Due.HasValue && !t.Completed && t.Due.Value.Date >= DateTime.Today)
				.GroupBy(t => t.Due.Value.Date)) 
			{
				var notification = new UILocalNotification ();
				notification.FireDate = new DateTime (g.Key.Year, g.Key.Month, g.Key.Day, 9, 0, 0);	
				notification.AlertAction = "Task reminder";
				notification.AlertBody = string.Format("You have {0} tasks scheduled for today", g.Count());
				notification.SoundName = UILocalNotification.DefaultSoundName;
				notification.ApplicationIconBadgeNumber = g.Count ();
				application.ScheduleLocalNotification (notification);
			}
		}

		void UpdateBadge(UIApplication application)
		{
			application.ApplicationIconBadgeNumber = taskList.TaskCollection
				.Where (t => t.Due.HasValue && !t.Completed && t.Due.Value.Date == DateTime.Today).Count();
		}
	}
}

