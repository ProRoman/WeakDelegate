using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WeakDelegate
{
    public class WeakDelegate<TDelegate> where TDelegate : class
    {
        private class MethodTarget
        {
            public readonly WeakReference Reference;
            public readonly MethodInfo Method;

            public MethodTarget(Delegate d)
            {
                Reference = new WeakReference(d.Target);
                Method = d.Method;
            }
        }

        private readonly List<MethodTarget> _targets = new List<MethodTarget>();

        public WeakDelegate()
        {
            if (!typeof (TDelegate).IsSubclassOf(typeof (Delegate)))
                throw new InvalidOperationException("TDelegate must bе а delegate type");
        }

        public void Combine(TDelegate target)
        {
            Delegate targetDelegate = target as Delegate;
            if (targetDelegate != null)
            {
                var invocationList = targetDelegate.GetInvocationList();
                foreach (Delegate d in invocationList)
                {
                    _targets.Add(new MethodTarget(d));
                }
            }
        }

        public void Remove(TDelegate target)
        {
            Delegate targetDelegate = target as Delegate;
            if (targetDelegate != null)
            {
                var invocationList = targetDelegate.GetInvocationList();
                foreach (MethodTarget mt in invocationList.Select(d => _targets.
                    Find(
                        w =>
                            Equals(d.Target, (w.Reference != null ? Target : null)) &&
                            Equals(d.Method.MethodHandle, w.Method.MethodHandle))).Where(mt => mt != null))
                {
                    _targets.Remove(mt);
                }
            }
        }

        public TDelegate Target
        {
            get
            {
                var deadRefs = new List<MethodTarget>();
                Delegate combinedTarget = null;
                foreach (MethodTarget mt in _targets.ToArray())
                {
                    WeakReference wr = mt.Reference;
                    // Static delegate or object delegate
                    if (wr == null || wr.Target != null)
                    {
                        var newDelegate = Delegate.CreateDelegate(typeof (TDelegate), wr != null ? wr.Target : null, mt.Method);
                        combinedTarget = Delegate.Combine(combinedTarget, newDelegate);
                    }
                    else
                    {
                        _targets.Remove(mt);
                    }
                }
                return combinedTarget as TDelegate;
            }
            set
            {
                _targets.Clear();
                Combine(value);
            }
        }
    }
}
