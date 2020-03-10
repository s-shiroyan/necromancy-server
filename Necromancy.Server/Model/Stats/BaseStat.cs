using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Model.Stats
{
    public class BaseStat
    {
        protected readonly object CurrentLock = new object();
        protected readonly object MaxLock = new object();
        protected int _max;
        protected int _current;
        protected uint _instanceId;
        protected bool _depleted;
        public BaseStat(int currVal, int maxVal)
        {
            _current = currVal;
            _max = maxVal;
            _depleted = false;
        }

        public bool depleted
        {
            get => _depleted;
            set
            {
                _depleted = value;
            }
        }

        public void setCurrent(int value)
        {
            lock (CurrentLock)
            {
                _current = value;
            }
        }

        // Set current to +/- value % of _current/_max
        public int setCurrent(sbyte value, bool useMax = false)
        {
            lock (CurrentLock)
            {
                if (useMax)
                    _current += (_max * (value / 100));
                else
                    _current += (_current * (value / 100));
            }
            return _current;
        }
        public int setMax(int value)
        {
            lock (MaxLock)
            {
                _max = value;
            }
            return _max;
        }
        public void toMax()
        {
            _current = _max;
            _depleted = false;
        }
        public int current
        {
            get => _current;
            protected set
            {
                lock (CurrentLock)
                {
                    _current = value;
                }
            }
        }
        public int max
        {
            get => _max;
            private set
            {
                lock (MaxLock)
                {
                    _current = value;
                }
            }
        }
        public int Modify(int amount, uint instanceId = 0)
        {
            lock (CurrentLock)
            {
                if (_depleted)
                    return _current;
                _current += amount;
                if (_current <= 0)
                {
                    _depleted = true;
                    _instanceId = instanceId;
                }
            }
            return _current;
        }
    }
}
