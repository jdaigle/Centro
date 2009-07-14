using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Repository
{
    /// <summary>
    /// Enum definition for the various aggregate functions which can be applied to fields in a scalar fetch.    
    /// Not all functions are legal on all fields. Some functions will produce errors when used with fields of a given type,
    /// like a Sum function with a character field. This is the responsibility of the developer. Aggregate functions are
    /// never applied to *lob fields. 
    /// </summary>
    public enum AggregateFunction : int
    {
        /// <summary>
        /// Default, do not apply an aggregate function to the field.
        /// </summary>
        None,
        /// <summary>
        /// Calculates the amount of rows for the field. Results in COUNT(field) 
        /// </summary>
        Count,
        /// <summary>
        /// Calculates the amount of rows with distinct values for field. Results in COUNT(DISTINCT field).
        /// Access,Excel not supported
        /// </summary>
        CountDistinct,
        /// <summary>
        /// Calculates the amount of rows. Results in a COUNT(*)
        /// </summary>
        CountRow,
        /// <summary>
        /// Calculates the average value for the field. Results in an AVG(field)
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        Avg,
        /// <summary>
        /// Calculates the average value for the distinct values for field. Results in an AVG(DISTINCT field).
        /// Access, Excel: not supported
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        AvgDistinct,
        /// <summary>
        /// Calculates the max value for field. Results in a MAX(field). 
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        Max,
        /// <summary>
        /// Calculates the min value for field. Results in a MIN(field)
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        Min,
        /// <summary>
        /// Calculates the sum of all values of field. Results in a SUM(field)
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        Sum,
        /// <summary>
        /// Calculates the sum of all distinct values of field. Results in a SUM(DISTINCT field). 
        /// Access, Excel: not supported
        /// </summary>
        /// <remarks>works on numeric fields (decimal/int/float/byte/etc.) only</remarks>
        SumDistinct,
    }
}
