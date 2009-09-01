using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Centro.Core.Extensions;

namespace Centro.OpenEntity.Entities
{
    internal class EntityFieldsCollection : IEntityFields, IEnumerable<IEntityField>, IEnumerable
    {
        /// <summary>
        /// the basic store for the entity fields
        /// </summary>
        private IEntityField[] fields;
        /// <summary>
        /// the list of primary key entity field references.
        /// </summary>
        private List<IEntityField> primaryKeyFields = new List<IEntityField>();
        /// <summary>
        /// A lookup dictionary to get the field index from a name. Column name is to be all upper.
        /// </summary>
        //private Dictionary<string, int> fieldIndexFromName = new Dictionary<string, int>();

        internal EntityFieldsCollection(int amount)
        {
            this.fields = new IEntityField[amount];
        }

        /// <summary>
        /// Create an EntityFieldsCollection using the given fieldsArray as the underlying field array.
        /// </summary>
        /// <param name="fieldsArray"></param>
        internal EntityFieldsCollection(IEntityField[] fieldsArray)
        {
            this.fields = fieldsArray;

            for (int i = 0; i < fieldsArray.Length; i++)
            {
                if (this.fields[i].IsPrimaryKey)
                {
                    // add it to the primary key set too
                    this.primaryKeyFields.Add(this.fields[i]);
                }
            }
        }

        #region IEntityFields Members

        public bool IsDirty
        {
            get;
            set;
        }

        public IEntityField this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.fields.Length))
                {
                    throw new IndexOutOfRangeException("The specified index is not in the range of known indexes");
                }

                return this.fields[index];
            }

            set
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException("The index on which the object should be placed cannot be smaller than 0.");
                }

                if (index >= this.fields.Length)
                {
                    throw new IndexOutOfRangeException("The index on which the object should be placed is outside the specified range of indexes.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value", "Item cannot be null");
                }
                
                //this.fieldIndexFromName.Add(value.Name.ToUpper(), index);
                
                this.fields[index] = value;
                
                if (value.IsPrimaryKey)
                {
                    // add it to the primary key set too
                    this.primaryKeyFields.Add(value);
                }
            }
        }

        public IEntityField this[string name]
        {
            get
            {
                if (name.Length <= 0)
                {
                    // Names of zero length are rejected
                    throw new ArgumentException("name cannot be of zero length.");
                }
                if (name.Trim().Length <= 0)
                {
                    throw new ArgumentException("name has to contain other characters than just spaces.");
                }
                //int index = 0;
                return this.FirstOrDefault(f => f.Name.Matches(name));
                //if (!this.fieldIndexFromName.TryGetValue(name.ToUpper(), out index))
                //{
                //    return null;
                //}
                //return this[index];
            }
        }

        public IList<IEntityField> PrimaryKeyFields
        {
            get
            {
                return this.primaryKeyFields;
            }
        }

        public EntityState State
        {
            get;
            set;
        }


        public IEntityField IdentityField
        {
            get
            {
                return this.fields.Where(f => f.IsIdentity).DefaultIfEmpty(null).First();
            }
        }


        #endregion

        #region IEnumerable<IEntityField> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IEntityField> GetEnumerator()
        {
            foreach (IEntityField field in this.fields)
            {
                yield return field;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.fields.GetEnumerator();
        }

        #endregion

        #region IEntityFields Members

        public int Count
        {
            get
            {
                return this.fields.Length;
            }
        }

        #endregion

        #region IEditableObject Members

        internal bool IsChangedInEdit { get; set; }

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            foreach (IEntityField field in this.fields)
                field.BeginEdit();
            this.IsChangedInEdit = false;
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            // we only care about updating the field if changes were made
            if (this.IsChangedInEdit)
            {
                foreach (IEntityField field in this.fields)
                    field.CancelEdit();
                this.IsChangedInEdit = false;
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            // we only care about updating the field if changes were made
            if (this.IsChangedInEdit)
            {
                foreach (IEntityField field in this.fields)
                    field.EndEdit();
                this.IsChangedInEdit = false;
            }
        }

        /// <summary>
        /// All changes to all <see cref="IEntityField"/> objects in this collection are accepted. 
        /// </summary>
        public void AcceptChanges()
        {            
            foreach (EntityField field in this.fields)
            {
                field.AcceptChange();
            }
            this.IsDirty = false;
            this.IsChangedInEdit = false;
        }

        #endregion

    }
}
