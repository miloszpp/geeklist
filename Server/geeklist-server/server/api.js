RESTstop.configure({
  use_auth: true
});

RESTstop.add('get_sync_date', { require_login: true }, function() {
  return { sync_date: this.user.sync_date };
});

RESTstop.add('receive_tasks', { require_login: true }, function() {
  if (!this.params.tasks) {
    throw new Meteor.Error(400, "Tasks not provided")
  }
  var tasks = JSON.parse(this.params.tasks);
  for (var i=0; i < tasks.length; i++) {
    var task = tasks[i];
    if (!task.hasOwnProperty('description')) throw Meteor.Error(400, "Invalid task format");
    if (!task.hasOwnProperty('completed')) throw Meteor.Error(400, "Invalid task format");
    if (!task.hasOwnProperty('due')) throw Meteor.Error(400, "Invalid task format");
    if (!task.hasOwnProperty('priority')) throw Meteor.Error(400, "Invalid task format");
  }
  console.log("Will update tasks for user: " + this.user._id);
  Meteor.users.update({_id : this.user._id}, {$set: {
    sync_date : new Date(),
    tasks : tasks
  }});
  console.log("Successfully updated tasks for user: " + this.user._id);
  return { success: true };
});

RESTstop.add('get_tasks', { require_login: true }, function() {
  return this.user.tasks;
});
