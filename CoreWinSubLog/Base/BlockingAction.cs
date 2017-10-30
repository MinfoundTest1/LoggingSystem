using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    /// <summary>
    /// Class to do the <see cref="Action&lt;"/> in another task.
    /// </summary>
    public class BlockingAction<T>
    {
        // Input buffer.
        private readonly BlockingCollection<T> _blockCollection = new BlockingCollection<T>();
        // Actually the action on items.
        private readonly Action<T> _action;
        // Task to process the items in collection.
        private Task _consumeTask;

        public BlockingAction(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            _action = action;
            _consumeTask = ConsumeAsync();
        }

        /// <summary>
        /// Posts an item to the <see cref="BlockingAction&lt;"/>.
        /// </summary>
        /// <param name="item"></param>
        public void Post(T item)
        {
            _blockCollection.Add(item);
        }

        /// <summary>
        ///  Run a task to consume the items in blocking collection.
        /// </summary>
        private Task ConsumeAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var item in _blockCollection.GetConsumingEnumerable())
                {
                    _action.Invoke(item);
                }
            });
        }
    }
}
