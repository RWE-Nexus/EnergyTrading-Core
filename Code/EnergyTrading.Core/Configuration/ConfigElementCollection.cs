namespace EnergyTrading.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    /// <summary>
    /// Provides a generic collection of <see cref="ConfigurationElement" />, implementing most necessary behaviour for client types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigElementCollection<T> : ConfigurationElementCollection, IList<T>
        where T : ConfigurationElement, new()
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public T this[int index]
        {
            get
            {
                return (T)this.BaseGet(index);
            }

            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        public new T this[string key]
        {
            get { return (T)this.BaseGet(key); }
        }

        public int IndexOf(T element)
        {
            return this.BaseIndexOf(element);
        }

        public void Add(T element)
        {
            this.BaseAdd(element);
        }

        public void Remove(T element)
        {
            if (this.BaseIndexOf(element) >= 0)
            {
                this.BaseRemove(this.GetElementKey(element));
            }
        }

        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        public void Clear()
        {
            this.BaseClear();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public new bool IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            this.BaseAdd(element, false);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }
    }
}