using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace hzwu.Framework
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {

        }
        /// <summary>
        /// 提供事务
        /// </summary>
        /// <param name="action">就是多个数据库操作</param>
        public void Invoke(Action action)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                action.Invoke();
                trans.Complete();
            }
        }
    }
}
