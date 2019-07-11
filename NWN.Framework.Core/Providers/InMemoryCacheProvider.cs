using System;
using System.Collections.Generic;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Providers
{
    internal class InMemoryCacheProvider: MarshalByRefObject, ICacheProvider
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Initialize()
        {
        }

        public T Get<T>(Guid key)
        {
            return (T)_data[key.ToString()];
        }

        public void Set<T>(Guid key, T obj)
        {
            _data[key.ToString()] = obj;
        }
    }
}
