using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using OpenEntity.Schema;
using OpenEntity.Query;
using OpenEntity.DataProviders;
using OpenEntity.Model;

namespace OpenEntity.Entities
{
    internal class ProxyEntityObject : IProxyEntity, IEntity
    {
        public ProxyEntityObject()
        {
            this.EntityObjectID = Guid.NewGuid();
            this.Initialized = false;
        }

        public ITable Table { get; internal set; }
        public bool Initialized { get; private set; }

        public void Initialize(ITable table, IEntityFields fields)
        {
            if (this.Initialized)
                throw new NotSupportedException("This method can only be called once.");
            this.Table = table;
            this.Fields = fields;
            this.IsNew = true;
            this.IsDirty = false;
            this.Initialized = true;
        }

        protected bool SetValue(int fieldIndex, object value)
        {
            if (this.Fields == null)
            {
                throw new NullReferenceException("A field's value is tried to be set, but there's no Fields object in this entity!");
            }

            if ((fieldIndex < 0) || (fieldIndex >= this.Fields.Count))
            {
                throw new ArgumentOutOfRangeException("fieldIndex", fieldIndex, "The field index passed is not a valid index in the fields collection of this entity.");
            }

            bool valueIsSet = false;
            object valueToSet = value;
            IEntityField fieldToSet = this.Fields[fieldIndex];
            if (!this.IsNew && fieldToSet.IsPrimaryKey)
            {
                // if it's not null, then we must be updating, which isn't supported.
                throw new FieldIsReadOnlyException("Updating primary keys is not supported by this data provider.");
            }

            if (FieldUtilities.DetermineIfFieldShouldBeSet(fieldToSet, this.IsNew, valueToSet))
            {
                if (this.editCycleInProgress)
                {
                    fieldToSet.CurrentValue = valueToSet;
                    Fields.IsDirty = true;
                    (Fields as EntityFieldsCollection).IsChangedInEdit = true;
                    valueIsSet = true;
                }
                else
                {
                    try
                    {
                        fieldToSet.BeginEdit();
                        fieldToSet.CurrentValue = valueToSet;
                        fieldToSet.EndEdit();
                        Fields.IsDirty = true;
                        valueIsSet = true;
                    }
                    catch
                    {
                        fieldToSet.CancelEdit();
                        valueIsSet = false;
                        throw;
                    }
                }
            }

            return valueIsSet;
        }

        protected object GetValue(int fieldIndex)
        {
            if (this.Fields == null)
            {
                return null;
            }

            if ((fieldIndex < 0) || (fieldIndex >= this.Fields.Count))
            {
                throw new ArgumentOutOfRangeException("fieldIndex", fieldIndex, "The field index passed is not a valid index in the fields collection of this entity.");
            }

            if ((this.Fields.State == EntityState.OutOfSync) && !this.Fields[fieldIndex].IsPrimaryKey)
            {
                throw new EntityOutOfSyncException();
            }

            if (this.Fields.State == EntityState.Deleted)
            {
                throw new EntityIsDeletedException();
            }

            IEntityField field = this.Fields[fieldIndex];

            // check if the field is set to a value, if that's required. 
            if (this.IsNew && !field.IsChanged && field.CurrentValue == null && !field.IsNullable)
            {
                // not set to a value, illegal.
                throw new InvalidFieldReadException(string.Format("The nullable field '{0}' at index {1} isn't set to a value yet, so reading its value leads to invalid results. ", this.Fields[fieldIndex].Name, fieldIndex));
            }

            object valueToReturn = null;
            

            valueToReturn = field.CurrentValue;
            // convert from DBNull to null
            if (valueToReturn == DBNull.Value)
                valueToReturn = null;

            //if ((valueToReturn == null) && returnDefaultIfNull)
            //{
            //    ITypeDefaultValue providerToUse = _typeDefaultValueProvider;
            //    if (providerToUse == null)
            //    {
            //        providerToUse = CreateTypeDefaultValueProvider();
            //    }
            //    if (providerToUse != null)
            //    {
            //        valueToReturn = providerToUse.DefaultValue(field.DataType);
            //    }
            //}

            return valueToReturn;
        }

        #region IEntity Members

        public bool SetNewFieldValue(string fieldName, object value)
        {
            if (!this.Initialized)
                throw new InvalidOperationException("Entity has not been initialized.");
            IEntityField field = this.Fields[fieldName];
            if (field == null)
            {
                return false;
            }
            return this.SetNewFieldValue(field.ColumnIndex, value);
        }

        public bool SetNewFieldValue(int fieldIndex, object value)
        {
            return this.SetValue(fieldIndex, value);
        }

        public object GetCurrentFieldValue(string fieldName)
        {
            if (!this.Initialized)
                throw new InvalidOperationException("Entity has not been initialized.");
            IEntityField field = this.Fields[fieldName];
            if (field == null)
            {
                throw new InvalidFieldReadException("The field {" + fieldName + "} is not known on this entity.");
            }
            return this.GetCurrentFieldValue(field.ColumnIndex);
        }

        public object GetCurrentFieldValue(int fieldIndex)
        {
            return this.GetValue(fieldIndex);
        }

        public bool IsNew
        {
            get
            {
                if (this.Fields == null)
                    return false;
                return this.Fields.State == EntityState.New;
            }
            set
            {
                if (this.Fields == null)
                    this.Fields.State = EntityState.New;
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                if (this.Fields == null)
                    return false;
                return this.Fields.IsDirty;
            }
            set
            {
                if (this.Fields != null)
                    this.Fields.IsDirty = value;
            }
        }

        public Guid EntityObjectID
        {
            get;
            private set;
        }

        public IEntityFields Fields
        {
            get;
            private set;
        }

        public IList<IEntityField> PrimaryKeyFields
        {
            get
            {
                if (this.Fields != null)
                {
                    return this.Fields.PrimaryKeyFields;
                }
                else
                {
                    return new List<IEntityField>();
                }
            }
        }

        public IPredicateExpression GetPrimaryKeyPredicateExpression()
        {
            if (this.PrimaryKeyFields.Count == 0)
                return null;
            IPredicateExpression pkPredicateExpression = new PredicateExpression();

            foreach (IEntityField field in this.PrimaryKeyFields)
            {
                new ColumnConstraint(this.Table.Name, field.Name, pkPredicateExpression, PredicateExpressionOperator.And).IsEqualTo(field.CurrentValue);
            }

            return pkPredicateExpression;
        }

        IDictionary<string, ICustomTypeConverter> customTypeConverters = new Dictionary<string, ICustomTypeConverter>();

        public void AddCustomTypeConverter(ICustomTypeConverter customTypeConverter, string propertyName)
        {
            if (!customTypeConverters.ContainsKey(propertyName.ToUpperInvariant()))
                customTypeConverters.Add(propertyName.ToUpperInvariant(), customTypeConverter);
        }

        public ICustomTypeConverter GetCustomTypeConverter(string propertyName)
        {
            if (customTypeConverters.ContainsKey(propertyName.ToUpperInvariant()))
                return customTypeConverters[propertyName.ToUpperInvariant()];
            return null;
        }

        public void Reload()
        {
            if (Reloaded != null)
                Reloaded(this, EventArgs.Empty);
        }

        public event EventHandler Reloaded;

        #endregion

        #region IEditableObject Members

        private bool editCycleInProgress;

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            if (this.Fields != null && !this.editCycleInProgress)
            {
                this.Fields.BeginEdit();
                this.editCycleInProgress = true;
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            if (this.Fields != null && this.editCycleInProgress)
            {
                this.Fields.CancelEdit();
                this.editCycleInProgress = false;
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            if (this.Fields != null && this.editCycleInProgress)
            {
                this.Fields.EndEdit();
                this.editCycleInProgress = false;
            }
        }

        #endregion
    }
}
