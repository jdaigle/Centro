using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace OpenEntity.Validation.Rules
{
    public class DigitsValidator : IInitializableValidator<DigitsAttribute>, IValidator
    {
        private int fractionalDigits;
        private int integerDigits;

        public void Initialize(DigitsAttribute parameters)
        {
            this.integerDigits = parameters.IntegerDigits;
            this.fractionalDigits = parameters.FractionalDigits;
        }

        private static bool IsNumeric(object Expression)
        {
            double num;
            return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, (IFormatProvider)NumberFormatInfo.InvariantInfo, out num);
        }

        public bool IsValid(object value, IValidatorContext validatorContext)
        {
            string str;
            if (value == null)
            {
                return true;
            }
            if (value is string)
            {
                try
                {
                    str = Convert.ToDouble(value).ToString();
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            else if (IsNumeric(value))
            {
                str = value.ToString();
            }
            else
            {
                return false;
            }
            string currencyDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            int index = str.IndexOf(currencyDecimalSeparator);
            int num2 = (index == -1) ? str.Length : index;
            int num3 = (index == -1) ? 0 : ((str.Length - index) - 1);
            if ((num2 == 1) && (str[0] == '0'))
            {
                num2--;
            }
            return ((num2 <= this.integerDigits) && (num3 <= this.fractionalDigits));
        }
    }

}
