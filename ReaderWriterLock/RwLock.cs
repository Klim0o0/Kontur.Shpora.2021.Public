using System;
using System.Collections.Generic;
using System.Threading;

namespace ReaderWriterLock
{
    public class RwLock : IRwLock
    {
        private readonly HashSet<object> locksObjects = new();

        public void ReadLocked(Action action)
        {
            var obj = new object();

            lock (locksObjects)
                locksObjects.Add(obj);

            lock (obj)
                action();

            lock (locksObjects)
                locksObjects.Remove(obj);
        }

        public void WriteLocked(Action action)
        {
            lock (locksObjects)
            {
                foreach (var obj in locksObjects)
                    Monitor.Enter(obj);

                action();

                foreach (var obj in locksObjects)
                    Monitor.Exit(obj);
            }
        }
    }
}