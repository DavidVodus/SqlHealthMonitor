using SqlHealthMonitor.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;


namespace SqlHealthMonitor.DAL.Helpers
{
   public class EntityModelProperty:IHelper
    {
        private DbContext _context;
        public EntityModelProperty(DbContext contex)
        {
            _context = contex;
        }
       public List<EdmProperty> GetPropertiesFromEntityModel(Func<EdmProperty, bool> predicate)

        {
          return  _context.GetPropertiesFromEntityModel(predicate);

        }
    }
}
