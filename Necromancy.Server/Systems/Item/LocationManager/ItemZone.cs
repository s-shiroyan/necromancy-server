using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item.LocationManager
{
    class ItemZone
    {
        public Container[] _containers { get; }
        public int MaxContainers { get; }
        public int MaxContainerSize { get; }
        
        public ItemZone(int maxContainers, int maxContainerSize)
        {
            MaxContainers = maxContainers;
            MaxContainerSize = maxContainerSize;
        }

        public bool IsFull
        {
            get
            {
                foreach(Container container in _containers)
                {
                    if (container != null && !container.IsFull) return false;
                }
                return true;
            }
        }

        public void PutContainer(int index, int size)
        {
            _containers[index] = new Container(size);
        }

        public Container GetContainer(int index)
        {
            return _containers[index];
        }

        public void RemoveContainer(int index)
        {
            _containers[index] = null;
        }

        public bool HasContainer(int index)
        {
            return _containers[index] != null;
        }       
    }
}
