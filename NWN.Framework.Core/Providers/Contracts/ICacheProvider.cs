using System;

namespace NWN.Framework.Core.Providers.Contracts
{
    public interface ICacheProvider
    {
        void Initialize();
        T Get<T>(Guid key);
        void Set<T>(Guid key, T obj);
    }
}
