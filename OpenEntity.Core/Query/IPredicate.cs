using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OpenEntity.DataProviders;

namespace OpenEntity.Query
{
    /// <summary>
    /// Interface for a predicate. Predicates are expressions which result in true or false, and which are used in WHERE clauses.
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="dataProvider">The data provider containing schema information (used to build the query).</param>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker);
        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="dataProvider">The data provider containing schema information (used to build the query).</param>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <param name="inHavingClause">if set to true, it will allow aggregate functions to be applied to colums.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker, bool inHavingClause);
        /// <summary>
        /// The list of parameters created when the Predicate was translated to text usable in a query. Only valid after a succesful call to ToQueryText
        /// </summary>
        IList<IDataParameter> Parameters { get; }
        /// <summary>
        /// Flag for setting the Predicate to negate itself, i.e. to add 'NOT' to its result.
        /// </summary>
        bool Negate { get; set; }
    }
}
