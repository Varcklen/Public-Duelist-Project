using System;
using Project.Utils.Events.Interfaces;

namespace Project.Utils.Events
{
    /// <summary>
    /// Similar to a regular event, but has access to the call through the interface.
    /// </summary>
    //And it can also be changed at any time without additional problems.
    public class ActionEvent : IEventInvoke
    {
        private event Action _action = delegate { };

        void IEventInvoke.Invoke()
        {
            _action?.Invoke();
        }

        public void AddListener(Action listener/*, bool debug = false*/)
        {
            //if (debug) Debug.Log($"_action {_action} add delegate. Count: {_action.GetInvocationList().Length}");
            _action += listener;
        }

        public void RemoveListener(Action listener)
        {
            _action -= listener;
        }
    }

    /// <summary>
    /// Similar to a regular event, but has access to the call through the interface.
    /// </summary>
    //And it can also be changed at any time without additional problems.
    public class ActionEvent<T> : IEventInvoke<T>
    {
        private event Action<T> _action = delegate { };

        void IEventInvoke<T>.Invoke(T param)
        {
            _action?.Invoke(param);
        }

        public bool Contains(Action<T> listener)
        {
            var list = _action.GetInvocationList();
            foreach (var item in list)
            {
                if ((Action<T>)item == listener)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddListener(Action<T> listener)
        {
            if (Contains(listener)) return;
            _action += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            if (!Contains(listener)) return;
            _action -= listener;
        }
    }
}
