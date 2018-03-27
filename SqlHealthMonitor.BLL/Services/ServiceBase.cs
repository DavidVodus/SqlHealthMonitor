using System.Collections.Generic;
using System.Linq;
using Common;
using Kendo.Mvc;
using SqlHealthMonitor.BLL.Models.WebPages.Components;
using System.Data.Entity;
using SqlHealthMonitor.DAL.Helpers;
using System.Web;
using Castle.Windsor;

namespace SqlHealthMonitor.BLL.Services
{
    public abstract class ServiceBase :LoggerBase,IService
    {

        public virtual void ModifyFilters(IEnumerable<IFilterDescriptor> filters, EntityModelProperty _entityModelPropertyHelper)
        {
            if (filters.Any())
            {
                foreach (var filter in filters)
                {
                    var descriptor = filter as FilterDescriptor;
                    if (descriptor != null && _entityModelPropertyHelper.GetPropertiesFromEntityModel
                        (x => x.TypeUsage.EdmType.Name.Contains("Byte[]")
                    && descriptor.Member.Contains(x.Name)).Any())
                    {
                        descriptor.Value = MacHelper.ConvertMacToBytes(descriptor.Value.ToString().TrimEnd('/'));
                    }
                    else if (filter is CompositeFilterDescriptor)
                    {
                        ModifyFilters(((CompositeFilterDescriptor) filter).FilterDescriptors,_entityModelPropertyHelper);
                    }
                }
            }
        }
      
    }
}
