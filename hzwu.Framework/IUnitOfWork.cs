using System;
using System.Collections.Generic;
using System.Text;

namespace hzwu.Framework
{
    public interface IUnitOfWork : IDisposable
    {
        void Invoke(Action action);
    }
}
