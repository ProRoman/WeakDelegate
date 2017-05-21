using System;
using System.Collections.Generic;

namespace WeakDelegate
{
    public class WeakEvent<TDelegate> where TDelegate : class
    {
        private readonly WeakDelegate<TDelegate> _weakDelegate = new WeakDelegate<TDelegate>();

        public virtual void AddHandler(TDelegate handler)
        {
            _weakDelegate.Combine(handler);
        }

        public virtual void RemoveHandler(TDelegate handler)
        {
            _weakDelegate.Remove(handler);
        }

        public virtual TDelegate Target
        {
            get { return _weakDelegate.Target; }
        }
    }
}