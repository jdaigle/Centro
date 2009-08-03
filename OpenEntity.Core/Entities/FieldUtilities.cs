using System;

namespace OpenEntity.Entities
{
    internal class FieldUtilities
    {
        /// <summary>
        /// Determines if field should be set.
        /// </summary>
        /// <param name="fieldToSet">The field to set.</param>
        /// <param name="entityIsNew">if set to <c>true</c> [entity is new].</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool DetermineIfFieldShouldBeSet(IEntityField fieldToSet, bool entityIsNew, object value)
        {
            // field value has to be updated in the following situations:
            // - when the entity is new and:
            //		- the field hasn't been changed
            //		- the field has been changed but the value is different, only if the current value is not null
            // - when the entity is not new and:
            //		- the field is already changed and the value isn't the same value.
            //		- the field's DbValue is null and value is not null
            //		- the field's DbValue is not null and the field's CurrentValue is different than the new value and not null
            //		- the field's CurrentValue is null and value isn't null
            if (entityIsNew)
            {
                if (!fieldToSet.IsChanged)
                    return true;
                if (fieldToSet.CurrentValue != null && fieldToSet.CurrentValue != DBNull.Value)
                    return !ValuesAreEqual(fieldToSet.CurrentValue, value);
            }
            else
            {
                if (fieldToSet.IsChanged)
                    return !ValuesAreEqual(fieldToSet.CurrentValue, value);
                if (fieldToSet.IsNullable)
                    return value == null || value == DBNull.Value || !ValuesAreEqual(fieldToSet.CurrentValue, value);
                if (!fieldToSet.IsNullable)
                    return (value != null) && !ValuesAreEqual(fieldToSet.CurrentValue, value);
                if (fieldToSet.IsNull)
                    return value != null && value != DBNull.Value;
            }
            return false;
        }

        /// <summary>
        /// Compares the two values passed in and checks if they're value-wise the same. This extends 'Equals' in the sense that if the values are
        /// arrays it considers them the same if the values of the arrays are the same as well and the length is the same.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// true if the values should be considered equal. If value1 or value2 are null and the other isn't false is returned. If both are null,
        /// true is returned.
        /// </returns>
        /// <remarks>It assumes the types of value1 and value2 are the same</remarks>
        internal static bool ValuesAreEqual(object value1, object value2)
        {
            if (((value1 == null) && (value2 != null)) || ((value1 != null) && (value2 == null)))
            {
                return false;
            }
            if ((value1 == null) && (value2 == null))
            {
                return true;
            }

            // not null, proceed with checks on values.
            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
            {
                return false;
            }

            if (value1Type.IsArray)
            {
                return CheckArraysAreEqual((Array)value1, (Array)value2);
            }
            else
            {
                return value1.Equals(value2);
            }
        }

        /// <summary>
        /// Performs a per-value compare on the arrays passed in and returns true if the arrays are of the same length and contain the same values.
        /// </summary>
        /// <param name="array1">The array1.</param>
        /// <param name="array2">The array2.</param>
        /// <returns>
        /// true if the arrays contain the same values and are of the same length
        /// </returns>
        public static bool CheckArraysAreEqual(Array array1, Array array2)
        {
            if (((array1 == null) && (array2 != null)) || ((array1 != null) && (array2 == null)))
            {
                return false;
            }

            if ((array1 == null) && (array2 == null))
            {
                return true;
            }

            // non-null arrays.
            if (array1.Length != array2.Length)
            {
                return false;
            }

            bool areEqual = true;
            for (int i = 0; i < array1.Length; i++)
            {
                areEqual &= array1.GetValue(i).Equals(array2.GetValue(i));
                if (!areEqual)
                {
                    break;
                }
            }

            return areEqual;
        }
    }
}
