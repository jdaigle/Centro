using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace OpenEntity.Query
{
    public interface IConstraint : IPredicate
    {
        IConstraint IsLike(string value);
        IConstraint IsNotLike(string value);
        IConstraint IsGreaterThan(object value);
        IConstraint IsGreaterThanOrEqualTo(object value);
        //IConstraint In(Select selectQuery);
        IConstraint IsIn(IEnumerable values);
        //IConstraint NotIn(Select selectQuery);
        IConstraint IsNotIn(IEnumerable values);
        IConstraint IsLessThan(object value);
        IConstraint IsLessThanOrEqualTo(object value);
        IConstraint IsNotNull();
        IConstraint IsNull();
        IConstraint IsBetweenAnd(object value1, object value2);
        IConstraint IsEqualTo(object value);
        IConstraint IsNotEqualTo(object value);
    }
}
