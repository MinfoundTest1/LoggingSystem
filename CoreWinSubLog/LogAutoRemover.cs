using System;
using System.Threading;

namespace CoreWinSubLog
{
    /// <summary>
    /// Class representing to remove the excess logs.
    /// The logs before keep days will be removed, and
    /// the removing process is auto running every day.
    /// </summary>
    public abstract class LogAutoRemover : IDisposable
    {
        // Log keep days
        private readonly int _keepDays;
        // Timer to remove logs
        private Timer _timer;

        /// <summary>
        /// Initialize a <see cref="LogAutoRemover"/> with given keep days.
        /// </summary>
        /// <param name="keepDays">Log keep days. The logs before keep days will be removed</param>
        public LogAutoRemover(int keepDays)
        {
            _keepDays = keepDays > 0 ? keepDays : 0;
            StartCleanTick();
        }

        /// <summary>
        /// Create a timer to do the removing every 24 hours.
        /// Do once immediately.
        /// </summary>
        protected void StartCleanTick()
        {
            // 24 hours
            int periodInMs = 24 * 60 * 60 * 1000;
            // Removing action.
            TimerCallback callback = new TimerCallback(o => RemoveLogsBefore(_keepDays));
            // Create the timer.
            _timer = new Timer(callback, null, 0, periodInMs);
        }

        /// <summary>
        /// Release the removing process.
        /// </summary>
        public virtual void Dispose()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// Remove the logs of the given days before today. 
        /// </summary>
        /// <param name="daysBefore"></param>
        protected abstract void RemoveLogsBefore(int daysBefore);
    }
}
