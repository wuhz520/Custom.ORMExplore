using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework.SqlDataValidate
{
    public class IntAttribute : AbstractValidateAttribute
    {
        private int _Min = 0;
        private int _Max = 0;
        public IntAttribute(int min, int max)
        {
            this._Min = min;
            this._Max = max;
        }

        public override bool Validate(object oValue)
        {
            return oValue != null
                && oValue.ToString().Length >= this._Min
                && oValue.ToString().Length <= this._Max;
        }
    }
}
