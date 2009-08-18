using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Specs.Mock
{
    public class SimplePhoneNumber
    {
        private string phoneNumber;

        public SimplePhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return phoneNumber;
        }
    }
}
