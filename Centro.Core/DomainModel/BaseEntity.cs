using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Centro.Validation;
using StructureMap;

namespace Centro.DomainModel
{
    public class BaseEntity : IValidatable
    {
        [ThreadStatic]
        private static Dictionary<Type, IEnumerable<PropertyInfo>> signaturePropertiesDictionary;
        private const int HASH_MULTIPLIER = 42;

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity;

            if (ReferenceEquals(this, compareTo))
                return true;

            return compareTo != null &&
                   GetType().Equals(compareTo.GetTypeUnproxied()) &&
                   HasSameObjectSignatureAs(compareTo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var signatureProperties = GetSignatureProperties();
                if (!signatureProperties.Any())
                    return base.GetHashCode();

                int hashCode = GetType().GetHashCode();
                foreach (PropertyInfo property in signatureProperties)
                {
                    var value = property.GetValue(this, null);
                    if (value != null)
                        hashCode = (hashCode * HASH_MULTIPLIER) ^ value.GetHashCode();
                }
                return base.GetHashCode();
            }
        }

        public virtual bool HasSameObjectSignatureAs(BaseEntity compareTo)
        {
            var signatureProperties = GetSignatureProperties();

            foreach (PropertyInfo property in signatureProperties)
            {
                object valueOfThisObject = property.GetValue(this, null);
                object valueToCompareTo = property.GetValue(compareTo, null);

                if (valueOfThisObject == null && valueToCompareTo == null)
                    continue;

                if ((valueOfThisObject == null ^ valueToCompareTo == null) ||
                    (!valueOfThisObject.Equals(valueToCompareTo)))
                {
                    return false;
                }
            }

            return signatureProperties.Any() || base.Equals(compareTo);
        }

        public virtual IEnumerable<PropertyInfo> GetSignatureProperties()
        {
            if (signaturePropertiesDictionary == null)
                signaturePropertiesDictionary = new Dictionary<Type, IEnumerable<PropertyInfo>>();
            IEnumerable<PropertyInfo> properties;
            if (signaturePropertiesDictionary.TryGetValue(GetType(), out properties))
                return properties;
            return (signaturePropertiesDictionary[GetType()] = GetTypeSpecificSignatureProperties());
        }

        protected virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        protected virtual IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
        {
            return GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true));
        }

        public bool IsValid()
        {
            return Validator.IsValid(this);
        }

        public IEnumerable<ValidationError> ValidationErrors()
        {
            return Validator.ValidationErrorsFor(this);
        }

        /// <summary>
        /// Throws an InvalidModelException if the domain object is not valid.
        /// </summary>
        /// <exception cref="InvalidModelException">If the domain object is not valid.</exception>
        public void Validate()
        {
            var validationErrors = ValidationErrors();
            if (validationErrors.Any())
                throw new InvalidModelException(validationErrors);
        }

        private IValidator Validator { get { return ObjectFactory.GetInstance<IValidator>(); } }
    }
}
