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

        private EntityDataObject entityProxyObject;

        public EntityFieldInterceptor()
        {
            entityProxyObject = new EntityDataObject();
        }

        public IEntity Entity { get { return entityProxyObject; } }

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
