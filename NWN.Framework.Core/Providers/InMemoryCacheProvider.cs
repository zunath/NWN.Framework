using System;
using System.Collections.Generic;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Providers
{
    internal class InMemoryCacheProvider: MarshalByRefObject, ICacheProvider
    {
        private readonly Dictionary<Guid, object> _data = new Dictionary<Guid, object>();

        public void Initialize()
        {
        }

        public T Get<T>(Guid key)
        {
            return (T)_data[key];
        }

        public void Set<T>(Guid key, T obj)
        {
            _data[key] = obj;
        }

        public IEnumerable<KeyValuePair<Guid, object>> GetAllKeys()
        {
            foreach (var record in _data)
            {
                yield return record;
            }
        }
    }
}
