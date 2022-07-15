using UnityEngine;

namespace System.Collections.Generic
{
    /// <summary>
    /// Works like a List, but cannot contain more than the specified number of elements.
    /// </summary>
    [Serializable]
    public class LimitList<T> : List<T>
    {
        [SerializeField] private readonly int _limit;
        public int Limit => _limit;

        public bool isFull => Count == _limit;

        public new void Add(T item)
        {
            if (isFull)
            {
                Debug.LogWarning($"You can't add more elements in list, since all cells are filled. Current limit: {_limit}.");
                return;
            }
            base.Add(item);
        }

        public LimitList(int limit)
        {
            _limit = limit;
        }
    }
}
