using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace SqlHealthMonitor.DAL.Helpers
{
    public static class EntityFrameworkExtension
    {
        public static string GetEntitySetName<T>(this ObjectContext context)
        {
            string className = typeof(T).Name;

            var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
            string entitySetName = (from meta in container.BaseEntitySets
                                    where meta.ElementType.Name == className
                                    select meta.Name).First();

            return entitySetName;
        }

        public static string GetOriginalVsCurrentValues<T>(this DbEntityEntry<T> entry)where T:class
        {
            StringBuilder sb = new StringBuilder();
            var originalObject = entry.OriginalValues.ToObject();
            var currentObject = entry.CurrentValues.ToObject();
            var properties = originalObject.GetType().GetProperties();
            for (int i = 0; i < properties.Count(); i++)
            {

                var originalValue = properties[i].GetValue(originalObject);
                var currentValue = properties[i].GetValue(currentObject);
                sb.AppendLine(string.Format("Property Name : {0}", properties[i].Name));
                sb.AppendLine(string.Format("Original Value : {0}", originalValue));
                sb.AppendLine(string.Format("Current Value : {0}", currentValue));

            }
            return sb.ToString();

        }
        public static List<EdmProperty> GetPropertiesFromEntityModel(this DbContext context,Func<EdmProperty,bool> predicate) 
        {
            MetadataWorkspace workspace = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;
            ObjectItemCollection itemCollection = (ObjectItemCollection) (workspace.GetItemCollection(DataSpace.OSpace));
            var varchars = itemCollection
    .Where(gi => gi.BuiltInTypeKind == BuiltInTypeKind.EntityType)
    .Cast<EntityType>()
    .SelectMany(entityTypes => entityTypes.Properties
        .Where(predicate))
    .ToList();
            return varchars;
        }
        public static List<NavigationProperty> GetNavigationProperties<T>(this DbEntityEntry<T> entity,DbContext context) where T : class
        {
            List<System.Reflection.PropertyInfo> properties = new List<System.Reflection.PropertyInfo>();
            //Get the entity type
            Type entityType = entity.GetType();
            //Get the System.Data.Entity.Core.Metadata.Edm.EntityType
            //associated with the entity.
            MetadataWorkspace workspace = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;
            ObjectItemCollection itemCollection = (ObjectItemCollection) (workspace.GetItemCollection(DataSpace.OSpace));
            EntityType etype = itemCollection.OfType<EntityType>().Single(e => itemCollection.GetClrType(e) == typeof(T));
            return etype.NavigationProperties.ToList();
          
        }
        public static bool CheckIfDifferent<T>(this DbEntityEntry<T> entry) where T:class
        {
            if (entry.OriginalValues.PropertyNames.Any(propertyName => !entry.OriginalValues[propertyName].Equals(entry.CurrentValues[propertyName])))
                return true;
            return false;
        }
    }
}
