using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework.SqlMapping
{
    /// <summary>
    /// 做表名称的别名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : AbstractMappingAttribute
    {
        //private string _Name = null;
        public TableAttribute(string tableName) : base(tableName)
        {
            //this._Name = tableName;
        }

        //public string GetName()
        //{
        //    return this._Name;
        //}
    }
}
