using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using OpenEntity.DataProviders;

namespace OpenEntity.Query
{
    /// <summary>
    /// Implementation of the IPredicateExpression interface.
    /// Predicates are expressions which result in true or false, and which are used in WHERE clauses.
    /// </summary>
    public class PredicateExpression : IPredicateExpression
    {
        private List<PredicateExpressionElement> elements;
        private List<IDataParameter> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateExpression"/> class.
        /// </summary>
        public PredicateExpression()
        {
            this.parameters = new List<IDataParameter>();
            this.elements = new List<PredicateExpressionElement>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateExpression"/> class.
        /// </summary>
        /// <param name="predicateToAdd">The predicate to add.</param>
        public PredicateExpression(IPredicate predicateToAdd)
            : this()
        {
            this.AddWithAnd(predicateToAdd);
        }

        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="dataProvider">The data provider containing schema information (used to build the query).</param>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker)
        {
            return this.ToQueryText(dataProvider, ref uniqueMarker, false);
        }

        /// <summary>
        /// Retrieves a ready to use text representation of the contained Predicate.
        /// </summary>
        /// <param name="dataProvider">The data provider containing schema information (used to build the query).</param>
        /// <param name="uniqueMarker">int counter which is appended to every parameter. The refcounter is increased by every parameter creation,
        /// making sure the parameter is unique in the predicate and also in the predicate expression(s) containing the predicate.</param>
        /// <param name="inHavingClause">if set to true, it will allow aggregate functions to be applied to columns.</param>
        /// <returns>
        /// The contained Predicate in textual format.
        /// </returns>
        public string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker, bool inHavingClause)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider", "Cannot create query part without a schema from the data repository.");
            }

            StringBuilder queryText = new StringBuilder(128);
            this.Parameters.Clear();

            // start with the opening paren
            if (this.Negate)
            {
                queryText.Append("NOT (");
            }
            else
            {
                queryText.Append("(");
            }

            for (int i = 0; i < this.elements.Count; i++)
            {
                PredicateExpressionElement element = this.elements[i];

                switch (element.Type)
                {
                    case PredicateExpressionElementType.Operator:
                        queryText.AppendFormat(null, " {0}", ((PredicateExpressionOperator)element.Contents).ToString().ToUpper(CultureInfo.InvariantCulture));
                        break;
                    case PredicateExpressionElementType.Predicate:
                        IPredicate predicateToAdd = (IPredicate)element.Contents;
                        // get the text of the subquery
                        queryText.AppendFormat(null, " {0}", predicateToAdd.ToQueryText(dataProvider, ref uniqueMarker, inHavingClause));
                        this.parameters.AddRange(predicateToAdd.Parameters);
                        break;
                }
            }

            // add closing paren
            queryText.Append(" )");

            return queryText.ToString();
        }

        /// <summary>
        /// The list of parameters created when the Predicate was translated to text usable in a query. Only valid after a succesful call to ToQueryText
        /// </summary>
        public IList<IDataParameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Flag for setting the Predicate to negate itself, i.e. to add 'NOT' to its result.
        /// </summary>
        public bool Negate
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the constraints and predicate expression within.
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
        }
        /// <summary>
        /// Gets the amount of predicate expression elements in this predicate expression. This is including all operators and constraints.
        /// </summary>
        public int Count
        {
            get { return this.elements.Count; }
        }

        /// <summary>
        /// Adds an IPredicate implementing object to the Predicate Expression with an 'And'-operator.
        /// The object added can be a Predicate derived class or a PredicateExpression. If no objects are present yet in the Predicate Expression ,
        /// the operator is ignored.
        /// </summary>
        /// <param name="predicateToAdd">The IPredicate implementing object to add</param>
        /// <returns>
        /// the Predicate on which this method is called, for command chaining
        /// </returns>
        /// <exception cref="ArgumentNullException">When predicateToAdd is null</exception>
        public IPredicate AddWithAnd(IPredicate predicateToAdd)
        {
            if (predicateToAdd == null)
                throw new ArgumentNullException("predicateToAdd", "Predicate To Add is null");
            return this.AddPredicate(predicateToAdd, PredicateExpressionOperator.And);
        }

        /// <summary>
        /// Adds an IPredicate implementing object to the Predicate Expression with an 'Or'-operator.
        /// The object added can be a Predicate derived class or a Predicate Expression . If no objects are present yet in the Predicate Expression,
        /// the operator is ignored.
        /// </summary>
        /// <param name="predicateToAdd">The IPredicate implementing object to add</param>
        /// <returns>
        /// the Predicate on which this method is called, for command chaining
        /// </returns>
        /// <exception cref="ArgumentNullException">When predicateToAdd is null</exception>
        public IPredicate AddWithOr(IPredicate predicateToAdd)
        {
            if (predicateToAdd == null)
                throw new ArgumentNullException("predicateToAdd", "Predicate To Add is null");
            return this.AddPredicate(predicateToAdd, PredicateExpressionOperator.Or);
        }

        /// <summary>
        /// Adds an element.
        /// </summary>
        /// <param name="elementToAdd">The element to add.</param>
        /// <param name="operatorToUse">The operator to use.</param>
        private IPredicate AddPredicate(object elementToAdd, PredicateExpressionOperator operatorToUse)
        {
            if (elementToAdd == null)
            {
                return this;
            }

            IPredicateExpression predicate = elementToAdd as IPredicateExpression;
            if ((predicate != null) && (predicate.Count <= 0))
            {
                // empty, skip
                return this;
            }

            // the first element should not be an operator
            if (this.Count > 0)
            {
                this.elements.Add(new PredicateExpressionElement() { Type = PredicateExpressionElementType.Operator, Contents = operatorToUse });
            }

            this.elements.Add(new PredicateExpressionElement() { Type = PredicateExpressionElementType.Predicate, Contents = elementToAdd });

            return this;
        }

        private enum PredicateExpressionElementType
        {
            Predicate,
            Operator,
        }

        private class PredicateExpressionElement
        {
            public PredicateExpressionElementType Type { get; set; }
            public object Contents { get; set; }
        }
    }
}
