using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hzwu.Framework.SqlFilter;
using hzwu.Framework.SqlMapping;
using hzwu.Model;

namespace hzwu.DAL
{
    /// <summary>
    /// 用来完成sql语句的缓存
    /// 每张表都是几个固定sql
    /// 泛型缓存:适合不需要释放的  体积小点  不同类型不同数据  
    /// </summary>
    public class SqlBuilder<T> where T : BaseModel
    {
        private static string _FindSql = null;
        private static string _InsertSql = null;
        static SqlBuilder()
        {
            Type type = typeof(T);
            {
                string columnsString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetMappingName()}]"));
                _FindSql = $"SELECT {columnsString} FROM [{type.GetMappingName()}] WHERE ID= ";
            }
            {
                string columnsString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"[{p.GetMappingName()}]"));
                string valuesString = string.Join(",", type.GetPropertiesWithoutKey().Select(p => $"@{p.GetMappingName()}"));
                _InsertSql = $"INSERT INTO [{type.GetMappingName()}] ({columnsString}) VALUES({valuesString}) SELECT @@Identity;";

            }
        }
        /// <summary>
        /// 以Id= 结尾，可以直接添加参数
        /// </summary>
        /// <returns></returns>
        public static string GetFindSql()
        {
            return _FindSql;
        }
        public static string GetInsertSql()
        {
            return _InsertSql;
        }
    }
}
