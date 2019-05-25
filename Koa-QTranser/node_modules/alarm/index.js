'use strict';

/**
 * Set an alarm that will execute the provided function at the specified time.
 *
 * ```js
 * // 2 seconds from now
 * var now = new Date();
 * var date = new Date(+now + 2000);
 * alarm(date, function() {
 *   console.log('Hello, world!');
 * });
 * ```
 * @name alarm
 * @param  {Date} `date` a Date object or a string that can be passed to the Date constructor.
 * @param  {Function} `fn` Function to execute at the specified time.
 * @return {Function} Returns a function that can be called to cancel the alarm.
 * @api public
 */

var alarm = function(date, fn) {
  if (typeof fn !== 'function') {
    throw new Error('expected second argument to be a function');
  }

  date = new Date(date);
  var now = new Date();
  var ms = date - now;
  var timer = setTimeout(fn, ms);
  return function cancel() {
    clearTimeout(timer);
  };
};

/**
 * Set a recurring alarm that will execute the provide function even X milliseconds.
 *
 * ```js
 * var count = 0;
 * var cancel = alarm.recurring(1000, function() {
 *   console.log((count++) + ' BEEP BEEP BEEP!');
 *   if (count >= 5) {
 *     cancel();
 *   }
 * });
 * ```
 * @param  {Number} `ms` Number of milliseconds to specify how often to execute the function.
 * @param  {Function} `fn` Function to execute every X milliseconds.
 * @return {Function} Returns a function that may be called to cancel the recurring alarm.
 * @api public
 */

alarm.recurring = function(ms, fn) {
  var timer = setInterval(fn, ms);
  return function cancel() {
    clearTimeout(timer);
  };
};

/**
 * Expose `alarm`
 */

module.exports = alarm;
