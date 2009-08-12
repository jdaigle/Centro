using System;
using Castle.Core.Interceptor;
using OpenEntity.Entities;

namespace OpenEntity.Proxy
{
    [Serializable]
    public class EntityFieldInterceptor : 
        IInterceptor
#if DEBUG
        , IHasCount
#endif
    {
        public IEntity Entity { get; private set; }

        public void Intercept(IInvocation invocation)
        {
#if DEBUG
            Count++;
#endif
            invocation.Proceed();
        }

#if DEBUG
        public int Count { get; private set; }
#endif
    }
}
