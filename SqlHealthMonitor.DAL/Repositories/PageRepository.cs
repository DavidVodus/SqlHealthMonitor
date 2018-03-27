using System;
using System.Data.Entity;
using SqlHealthMonitor.DAL.Models.WebPages;

namespace SqlHealthMonitor.DAL.Repositories
{
    public class PageRepository : RepositoryBase<PageBase>, IPageRepository
    {
        public PageRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override Type LogPrefix => GetType();
    }
}
    
