using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace hzwu.Framework.SqlMapping
{
    public static class MappingExtend
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">可以是type  也可以是property</param>
        /// <returns></returns>
        public static string GetMappingName(this MemberInfo type)
        {
            if (type.IsDefined(typeof(AbstractMappingAttribute), true))
            {
                var attribute = type.GetCustomAttribute<AbstractMappingAttribute>();
                return attribute.GetName();
            }
            else
            {
                return type.Name;
            }
        }


        public static string GetTableName(this Type type)
        {
            if (type.IsDefined(typeof(TableAttribute), true))
            {
                TableAttribute attribute = type.GetCustomAttribute<TableAttribute>();
                return attribute.GetName();
            }
            else
            {
                return type.Name;
            }
        }
        public static string GetColumnName(this PropertyInfo prop)
        {
            if (prop.IsDefined(typeof(ColumnAttribute), true))
            {
                ColumnAttribute attribute = prop.GetCustomAttribute<ColumnAttribute>();
                return attribute.GetName();
            }
            else
            {
                return prop.Name;
            }
        }
    }
}
