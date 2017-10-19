using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWinSubLog
{
    /// <summary>
    /// Class to do the <see cref="Action&lt;"/> after batching the collection to array (with given size) collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BatchAction<T> where T : new()
    {
        private readonly Action<T[]> _action;
        private readonly int _batchSize;

        /// <summary>	
        /// Initializes a new <see cref="BatchAction&lt;"/> with the specified action and batch size.
        /// </summary>
        /// <param name="action">Action on T[]</param>
        /// <param name="batchSize">Batch (array) size</param>
        public BatchAction(Action<T[]> action, int batchSize)
        {
            _action = action;
            _batchSize = batchSize > 0 ? batchSize : 1;
        }

        /// <summary>
        /// Batch the given collection to array and then do the specified action.
        /// </summary>
        /// <param name="collection">Input collection</param>
        public void Batch(IEnumerable<T> collection)
        {
            foreach (var item in GetBatchArrayFrom(collection))
            {
                _action.Invoke(item);
            }
        }

        /// <summary>
        /// Parallel looping the batched collection from the given collection to do the specified action.
        /// </summary>
        /// <param name="collection">Input collection</param>
        public void ParallelBatch(IEnumerable<T> collection)
        {
            Parallel.ForEach(GetBatchArrayFrom(collection), _action);
        }

        /// <summary>
        /// Batch the given collection to array collection.
        /// </summary>
        /// <param name="collection">Input colleciton</param>
        /// <returns>Array collection</returns>
        private IEnumerable<T[]> GetBatchArrayFrom(IEnumerable<T> collection)
        {
            int i = 0;
            List<T> list = new List<T>(_batchSize);
            foreach (var item in collection)
            {
                list.Add(item);
                i++;
                if (_batchSize == i)
                {
                    i = 0;
                    yield return list.ToArray();
                    list = new List<T>(_batchSize);
                }
            }
            yield return list.ToArray();
        }
    }
}
