Deps.autorun(function(){
  Meteor.subscribe('userData');
});

Template.taskList.tasks = function () {
  var user = Meteor.user();
  if (user) {
    return user['tasks'];
  }
  return [];
}

Template.taskList.currentUser = function () {
  var user = Meteor.user();
  if (user) {
    return JSON.stringify(user['tasks'][0]);
  }
  return "";
}
