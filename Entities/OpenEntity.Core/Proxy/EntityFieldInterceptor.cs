using System;
using Castle.Core.Interceptor;
using OpenEntity.Entities;
using OpenEntity.Mapping;
using System.Linq;
using OpenEntity.Extensions;
using OpenEntity.DataProviders;
using System.Collections.Generic;
using OpenEntity.Repository;
using OpenEntity.Query;

namespace OpenEntity.Proxy
{
    [Serializable]
    public class EntityFieldInterceptor :
        IInterceptor
#if DEBUG
, IHasCount
#endif
    {

        private ProxyEntityObject entityProxyObject;
        private IClassMapping classMapping;        

        public EntityFieldInterceptor(IClassMapping classMapping)
        {
            this.classMapping = classMapping;
            this.entityProxyObject = new ProxyEntityObject();
        }

        public IEntity Entity { get { return entityProxyObject; } }

        public void Intercept(IInvocation invocation)
        {
#if DEBUG
            Count++;
#endif
            if (invocation.Method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase))
            {
                string propertyName = invocation.Method.Name.Substring(4);
                var property = classMapping.Properties.FirstOrDefault(p => p.Name.Matches(propertyName));
                if (property != null)
                {
                    if (property.HasReference)
                    {
                        invocation.ReturnValue = entityProxyObject.HandleReferencePropertyGet(property);
                    }
                    else
                    {
                        var value = entityProxyObject.GetCurrentFieldValue(property.Column);
                        if (property.CustomTypeConverter != null)
                            invocation.ReturnValue = property.CustomTypeConverter.ConvertTo(value);
                        else
                            invocation.ReturnValue = value;
                    }
                    return;
                }
            }
            else if (invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
            {
                string propertyName = invocation.Method.Name.Substring(4);
                var property = classMapping.Properties.FirstOrDefault(p => p.Name.Matches(propertyName));
                if (property != null)
                {
                    var value = invocation.GetArgumentValue(0);
                    if (property.HasReference)
                    {
                        entityProxyObject.HandleReferencePropertySet(property, value);
                    }
                    else
                    {
                        if (property.CustomTypeConverter != null)
                            entityProxyObject.SetNewFieldValue(property.Column, property.CustomTypeConverter.ConvertFrom(value));
                        else
                            entityProxyObject.SetNewFieldValue(property.Column, value);
                    }
                    return;
                }
            }
            invocation.Proceed();
        }

#if DEBUG
        public int Count { get; private set; }
#endif
    }
}
