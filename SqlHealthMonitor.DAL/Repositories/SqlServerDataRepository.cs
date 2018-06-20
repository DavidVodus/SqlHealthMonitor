using SqlHealthMonitor.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Principal;

namespace SqlHealthMonitor.DAL.Repositories
{
    public class SqlServerDataRepository : RepositoryBase<SqlServerData>, ISqlServerDataRepository
    {
        public SqlServerDataRepository(DbContext context) : base(context)
        {
        }

        protected override Type LogPrefix => GetType();
    }
}
