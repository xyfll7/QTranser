const alarm = require('alarm')

var count = 0;
var cancel = alarm.recurring(1000, function() {
  console.log((count++) + ' BEEP BEEP BEEP!');
  if (count >= 5) {
    cancel();
  }
});