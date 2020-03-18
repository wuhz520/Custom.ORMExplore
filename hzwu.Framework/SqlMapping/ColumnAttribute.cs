using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework.SqlMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : AbstractMappingAttribute
    {
        //private string _Name = null;
        public ColumnAttribute(string columnName):base(columnName)
        {
            //this._Name = columnName;
        }

        //public string GetName()
        //{
        //    return this._Name;
        //}
    }
}
