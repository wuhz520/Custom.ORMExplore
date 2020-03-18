using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework.SqlDataValidate
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class AbstractValidateAttribute : Attribute
    {
        public abstract bool Validate(object oValue);
    }
}
