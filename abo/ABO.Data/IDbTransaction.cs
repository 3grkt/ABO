using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
