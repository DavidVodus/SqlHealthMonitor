using System;
using System.Data.Entity;
using SqlHealthMonitor.DAL.Models;

namespace SqlHealthMonitor.DAL.Repositories
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override Type LogPrefix => GetType();
    }
}
