using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework
{
    public class SqlConnectionPool
    {
        public static string GetConnectionString(SqlConnectionType sqlConnectionType)
        {
            string conn = null;
            switch (sqlConnectionType)
            {
                case SqlConnectionType.Read:
                    conn = Dispatcher(ConfigrationManager.SqlConnectionStringRead);
                    break;
                case SqlConnectionType.Write:
                    conn = ConfigrationManager.SqlConnectionStringWrite;
                    break;
                default:
                    throw new Exception("wrong sqlConnectionType");
            }
            return conn;
        }
        private static int _Seed = 0;
        /// <summary>
        /// 调度分配--随机就是平均
        /// </summary>
        /// <param name="connectionStrings"></param>
        /// <returns></returns>
        private static string Dispatcher(string[] connectionStrings)
        {
            //string conn = connectionStrings[new Random(_Seed++).Next(0, connectionStrings.Length)];//平均策略
            string conn = connectionStrings[_Seed++ % connectionStrings.Length];//轮询--seed需要线程安全
            //能不能根据数据库的状况
            //权重--就是配置文件加点料  
            //2  3  4     9次请求  [1,1,2,2,2,3,3,3,3] 去平均
            return conn;
        }

        public enum SqlConnectionType
        {
            Read,
            Write
        }
    }
}
