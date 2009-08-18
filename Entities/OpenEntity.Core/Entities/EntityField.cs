using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using OpenEntity.Schema;

namespace OpenEntity.Entities
{
    internal class EntityField : IEntityField
    {
        public EntityField(IColumn schema)
        {
            this.schema = schema;
            this.IsNull = true;
        }

        #region Class Instance Fields

        private bool wasChanged;
        private object originalValue;
        private object currentValue;
        private IColumn schema;

        #endregion

        #region IEntityField Members

        public object CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                if (value != null)
                {
                    if ((this.DataType != typeof(object)) && (value.GetType() != this.DataType) && !this.DataType.IsInstanceOfType(value))
                    {
                        // see if _dataType has an op_Implicit defined which accepts value.GetType(). if so the type can be converted and the value is
                        // ok, otherwise the value isn't valid and can't be used. 
                        MethodInfo implicitConverterMethod = this.DataType.GetMethod("op_Implicit", new Type[] { value.GetType() });
                        if (implicitConverterMethod == null)
                        {
                            // can't be converted to type of this field, error
                            throw new ValueTypeMismatchException("The value " + value.ToString() + " is of type '" + value.GetType().ToString() + "' while the field is of type '" + this.DataType + "'");
                        }
                    }
                }

                if (this.IsReadOnly && !this.IsPrimaryKey)
                {
                    throw new FieldIsReadOnlyException();
                }

                this.currentValue = value;

                if (value == null || value == DBNull.Value)
                    this.IsNull = true;
                else
                    this.IsNull = false;

                this.IsChanged = true;
            }
        }

        public bool IsChanged
        {
            get;
            set;
        }

        public bool IsNull
        {
            get;
            set;
        }

        #endregion

        #region IFieldSchema Members

        public DbType DbDataType { get { return this.schema.DbDataType; } }

        public Type DataType { get { return this.schema.DataType; } }

        public bool IsIdentity { get { return this.schema.IsIdentity; } }

        public bool IsPrimaryKey { get { return this.schema.IsPrimaryKey; } }

        public bool IsNullable { get { return this.schema.IsNullable; } }

        public int ColumnIndex { get { return this.schema.ColumnIndex; } }

        public bool IsForeignKey { get { return this.schema.IsForeignKey; } }

        public bool IsReadOnly { get { return this.schema.IsReadOnly; } }

        public int MaxLength { get { return this.schema.MaxLength; } }

        public short Scale { get { return this.schema.Scale; } }

        public short Precision { get { return this.schema.Precision; } }

        public ITable Table { get { return this.schema.Table; } }

        #endregion

        #region IDatabaseObject Members

        public string Name { get { return this.schema.Name; } }

        public string SchemaName { get { return this.schema.SchemaName; } }

        #endregion

        #region IEditableObject Members

        /// <summary>
        /// IEditableObject method. Used by databinding.
        /// Original value is overwritten by currentValue, currentValue keeps its value.
        /// </summary>
        public void BeginEdit()
        {
            this.originalValue = this.currentValue;
            this.wasChanged = this.IsChanged;
        }

        /// <summary>
        /// IEditableObject method. Used by databinding.
        /// The currentvalue is reset by the original value.
        /// The field's IsChanged state is rolled back as well. 
        /// </summary>
        public void CancelEdit()
        {
            this.currentValue = this.originalValue;
            this.IsChanged = this.wasChanged;
        }

        /// <summary>
        /// IEditableObject method. Used by databinding.
        /// The field is kept marked changed, in effect, this method is empty.
        /// </summary>
        public void EndEdit()
        {

        }

        /// <summary>
        /// Accepts the value of the current value as the final current value. Original value is discarded
        /// </summary>
        public void AcceptChange()
        {
            if (!this.IsChanged)
            {
                // no change to accept
                return;
            }
            this.originalValue = null;
            this.IsChanged = false;
        }

        #endregion

        /// <summary>
        /// Forces the current value to be some value. There is no original value, and the field is not marked as changed.
        /// </summary>
        internal void ForceSetCurrentValue(object value)
        {
            this.currentValue = value;
            this.originalValue = null;
            this.IsChanged = false;
        }
    }
}
