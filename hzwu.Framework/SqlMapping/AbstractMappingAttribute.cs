using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework.SqlMapping
{
    public abstract class AbstractMappingAttribute : Attribute
    {
        private string _Name = null;
        public AbstractMappingAttribute(string name)
        {
            this._Name = name;
        }

        public string GetName()
        {
            return this._Name;
        }
    }
}
