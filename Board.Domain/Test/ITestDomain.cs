using Board.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Domain.Test
{
    public interface ITestDomain : IBaseDomain
    {
        public Task TestDB();
    }
}
