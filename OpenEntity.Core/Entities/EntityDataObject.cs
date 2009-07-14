using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using OpenEntity.Schema;
using OpenEntity.Query;
using OpenEntity.DataProviders;

namespace OpenEntity.Entities
{
    public class EntityDataObject : IProxyEntity, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataObject"/> class.
        /// </summary>
        /// <param name="table">The table schema.</param>
        public EntityDataObject(ITable table)
        {
            this.Table = table;
            this.EntityObjectID = Guid.NewGuid();
            this.Initialized = false;
        }

        public ITable Table { get; internal set; }

        public bool Initialized { get; private set; }

        public void Initialize(IEntityFields fields)
        {
            if (this.Initialized)
                throw new NotSupportedException("This method can only be called once.");
            this.Fields = fields;
            this.IsNew = true;
            this.IsDirty = false;
            this.Initialized = true;
        }

        /// <summary>
        /// Sets the value of the field with the index specified to the value specified. 
        /// </summary>
        /// <param name="fieldIndex">The fieldindex of the field which value to set.</param>
        /// <param name="value">The value to set the field's currentvalue to.</param>
        /// <exception cref="ArgumentOutOfRangeException">When fieldIndex is smaller than 0 or bigger than the amount of fields in the fields collection.</exception>
        /// <returns>true if the value is actually set, false otherwise</returns>
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

        /// <summary>
        /// Gets the value of the field with the index specified. 
        /// </summary>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns>the value of the field</returns>
        /// <exception cref="EntityOutOfSyncException">When the entity is out of sync and needs to be refetched first.</exception>
        /// <exception cref="EntityIsDeletedException">When the entity is marked as deleted.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When fieldIndex is smaller than 0 or bigger than the amount of fields in the fields collection.</exception>
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

            // check if the field is set to a value, if that's required. 
            if (this.IsNew && !this.Fields[fieldIndex].IsChanged && this.Fields[fieldIndex].CurrentValue == null)
            {
                // not set to a value, illegal.
                throw new InvalidFieldReadException(string.Format("The field '{0}' at index {1} isn't set to a value yet, so reading its value leads to invalid results. ", this.Fields[fieldIndex].Name, fieldIndex));
            }

            object valueToReturn = null;
            IEntityField field = this.Fields[fieldIndex];

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

        public object GetCurrentFieldValue(int fieldIndex)
        {
            return this.GetValue(fieldIndex);
        }

        public object GetCurrentFieldValue(string fieldName)
        {
            IEntityField field = this.Fields[fieldName];
            if (field == null)
            {
                return null;
            }
            return this.GetCurrentFieldValue(field.ColumnIndex);
        }

        public bool IsNew
        {
            get;
            set;
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

        //public RelationshipGraphNode RelationshipGraph
        //{
        //    get
        //    {                
        //        return this.relationshipGraph;
        //    }
        //    internal set
        //    {
        //        this.relationshipGraph = value;
        //    }
        //}

        //public void FetchRelatedEntities(IDataProvider dataProvider)
        //{
        //    this.FetchRelatedEntities(dataProvider, false);
        //}

        //public void FetchRelatedEntities(IDataProvider dataProvider, bool recursive)
        //{
        //    foreach (var relationship in this.RelationshipGraph)
        //    {
        //        relationship.FetchEntities(this, dataProvider, recursive);
        //    }
        //}

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

        /// <summary>
        /// Gets an IPredicateExpression that represents the primary keys for this entity.
        /// </summary>
        public IPredicateExpression GetPrimaryKeyPredicateExpression()
        {
            if (this.PrimaryKeyFields.Count == 0)
                return null;
            IPredicateExpression pkPredicateExpression = new PredicateExpression();

            foreach (IEntityField field in this.PrimaryKeyFields)
            {
                pkPredicateExpression.AddWithAnd(new ColumnConstraint(this.Table.Name, field.Name).IsEqualTo(field.CurrentValue));
            }

            return pkPredicateExpression;
        }

        
    }
}
