using System.Collections;

namespace Centro.OpenEntity.Query
{
    public interface IConstraint
    {
        IPredicateExpression IsLike(string value);
        IPredicateExpression IsNotLike(string value);
        IPredicateExpression IsGreaterThan(object value);
        IPredicateExpression IsGreaterThanOrEqualTo(object value);
        //IPredicateExpression In(Select selectQuery);
        IPredicateExpression IsIn(IEnumerable values);
        //IPredicateExpression NotIn(Select selectQuery);
        IPredicateExpression IsNotIn(IEnumerable values);
        IPredicateExpression IsLessThan(object value);
        IPredicateExpression IsLessThanOrEqualTo(object value);
        IPredicateExpression IsNotNull();
        IPredicateExpression IsNull();
        IPredicateExpression IsBetweenAnd(object value1, object value2);
        IPredicateExpression IsEqualTo(object value);
        IPredicateExpression IsNotEqualTo(object value);
    }
}
