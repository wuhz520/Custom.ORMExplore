using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace hzwu.Framework.SqlFilter
{
    public static class FilterExtend
    {
        /// <summary>
        /// 过滤掉主键 返回全部属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithoutKey(this Type type)
        {
            return type.GetProperties().Where(p => !p.IsDefined(typeof(KeyAttribute), true));
        }

        /// <summary>
        /// 就是通过json字符串找出更新的字段
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesInJson(this Type type, string json)
        {
            return type.GetProperties().Where(p => json.Contains($"'{p.Name}':") || json.Contains($"\"{p.Name}\":"));
        }
    }
}
