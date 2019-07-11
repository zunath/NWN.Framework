using System;
using NWN.Framework.Core.Messaging;

namespace NWN.Framework.Core
{
    public class Application: MarshalByRefObject
    {
        private readonly AppCache _cache;

        public Application()
        {
            _cache = new AppCache();
        }

        public void Initialize()
        {
            _cache.Initialize();
            MessageHub.Instance.RegisterGlobalErrorHandler(OnMessageHubError);
        }

        private void OnMessageHubError(Guid id, Exception ex)
        {
            Console.WriteLine("MessageHub error: " + ex);
        }

    }
}
