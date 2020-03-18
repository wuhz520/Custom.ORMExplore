using System;
using System.Data.SqlClient;
using System.Linq;
using hzwu.Framework;
using hzwu.Framework.SqlFilter;
using hzwu.Framework.SqlMapping;
using hzwu.Model;
using hzwu.Framework.SqlDataValidate;
using System.Collections.Generic;

namespace hzwu.DAL
{
    /// <summary>
    /// 延迟提交式，模拟DBContext
    /// </summary>
    public class SqlHelperDelay : IDisposable
    {
        /// <summary>
        /// 通用主键查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel, new()
        {
            Type type = typeof(T);
            //string columnsString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetMappingName()}]"));
            //string sql = $"SELECT {columnsString} FROM [{type.GetMappingName()}] WHERE ID={id} ";
            string sql = $"{SqlBuilder<T>.GetFindSql()}{id}";
            string connString = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Read);
            Console.WriteLine($"当前查询的字符串为{connString}");
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    T t = new T();
                    foreach (var prop in type.GetProperties())
                    {
                        string propName = prop.GetMappingName();//查询时as一下，可以省下一轮
                        prop.SetValue(t, reader[propName] is DBNull ? null : reader[propName]);//可空类型  设置成null而不是数据库查询的值
                    }
                    return t;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private IList<SqlCommand> _SqlCommandList = new List<SqlCommand>();

        public void Insert<T>(T t) where T : BaseModel, new()
        {
            Type type = t.GetType();
            string sql = SqlBuilder<T>.GetInsertSql();
            var paraArray = type.GetProperties().Select(p => new SqlParameter($"@{p.GetMappingName()}", p.GetValue(t) ?? DBNull.Value)).ToArray();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.AddRange(paraArray);
            this._SqlCommandList.Add(command);
        }
        public void SaveChange()
        {
            string connString = SqlConnectionPool.GetConnectionString(SqlConnectionPool.SqlConnectionType.Write);
            if (this._SqlCommandList.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var command in this._SqlCommandList)
                            {
                                command.Connection = conn;
                                command.Transaction = trans;
                                command.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            this._SqlCommandList?.Clear();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            this._SqlCommandList?.Clear();
        }
    }
}

