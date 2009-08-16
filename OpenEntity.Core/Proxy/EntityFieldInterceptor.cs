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
        private IDataProvider dataProvider;
        private Dictionary<IReferenceMapping, object> cachedReferenceObjects = new Dictionary<IReferenceMapping, object>();

        public EntityFieldInterceptor(IClassMapping classMapping, IDataProvider dataProvider)
        {
            this.classMapping = classMapping;
            this.dataProvider = dataProvider;
            this.entityProxyObject = new ProxyEntityObject();
            this.entityProxyObject.Reloaded += new EventHandler(HandleEntityProxyObjectReloaded);
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
                        invocation.ReturnValue = HandleReferencePropertyGet(property);
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
                        HandleReferencePropertySet(property, value);
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

        private object HandleReferencePropertyGet(IPropertyMapping property)
        {
            if (entityProxyObject.Fields[property.Column].IsNull)
                return null;
            if (cachedReferenceObjects.ContainsKey(property.Reference))
                return cachedReferenceObjects[property.Reference];
            var foriegnKeyValue = entityProxyObject.GetCurrentFieldValue(property.Column);
            var repository = RepositoryFactory.GetRepositoryFactoryFor(dataProvider).GetRepository(property.Reference.ReferenceModelType);
            var entity = repository.CreateEmptyEntity();
            var foreignKeyColumn = entity.PrimaryKeyFields[0].Name;
            if (property.Reference.HasSpecifiedForeignKey)
                foreignKeyColumn = property.Reference.ForeignKey;
            var predicate = new PredicateExpression()
                            .Where(entity.Table.Name, foreignKeyColumn).IsEqualTo(foriegnKeyValue);
            var referenceObject = repository.FetchAll(predicate, null, null, 1).FirstOrDefault();
            if (referenceObject != null)
                cachedReferenceObjects.Add(property.Reference, referenceObject);
            return referenceObject;
        }

        private void HandleReferencePropertySet(IPropertyMapping property, object value)
        {
            if (value == null)
            {
                entityProxyObject.SetNewFieldValue(property.Column, value);
                return;
            }
            var entity = EntityProxyFactory.AsEntity(value);
            if (entity == null)            
                throw new InvalidOperationException(string.Format("Supplied value of type [{0}] is not an entity and cannot be used for this property.", value.GetType().FullName));           
            var fieldValue = entity.PrimaryKeyFields[0].CurrentValue;
            if (property.Reference.HasSpecifiedForeignKey)
                fieldValue = entity.Fields[property.Reference.ForeignKey].CurrentValue;
            if (fieldValue == null || fieldValue == DBNull.Value)
                throw new InvalidOperationException("Foreign key value for the supplied reference is null, the entity is invalid for this property.");
            entityProxyObject.SetNewFieldValue(property.Column, fieldValue);
        }

        private void HandleEntityProxyObjectReloaded(object sender, EventArgs e)
        {
            cachedReferenceObjects.Clear();
        }

#if DEBUG
        public int Count { get; private set; }
#endif
    }
}
