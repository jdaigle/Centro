using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Validation
{
    public interface IRule
    {
        string Message { get; set; }
    }
}
