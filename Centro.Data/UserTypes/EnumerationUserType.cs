using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Centro.Data.UserTypes
{
    public class EnumerationUserType<TEnum> : IUserType
    {
        object IUserType.NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var val = NHibernateUtil.Byte.NullSafeGet(rs, names);
            if (val == null)
                return null;
            return (TEnum)Enum.ToObject(typeof(TEnum), val);
        }

        void IUserType.NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value != null)
            {
                var val = Convert.ToInt32((TEnum)value);
                NHibernateUtil.Int32.NullSafeSet(cmd, val, index);
            }
            else
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
        }

        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { SqlTypeFactory.Int32 }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(TEnum); }
        }

        bool IUserType.IsMutable
        {
            get { return false; }
        }

        object IUserType.DeepCopy(object value)
        {
            return value;
        }

        object IUserType.Replace(object original, object target, object owner)
        {
            return original;
        }

        object IUserType.Assemble(object cached, object owner)
        {
            return cached;
        }

        object IUserType.Disassemble(object value)
        {
            return value;
        }

        bool IUserType.Equals(object x, object y)
        {
            if ((x != null) && (y != null))
                return x.Equals(y);
            return false;
        }

        int IUserType.GetHashCode(object x)
        {
            return x.GetHashCode();
        }
    }
}
