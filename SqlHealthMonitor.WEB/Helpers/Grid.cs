using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Data.Entity;
using Castle.Components.DictionaryAdapter;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SqlHealthMonitor.BLL.Models.WebPages.Components;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.WebPages;

namespace SqlHealthMonitor.Helpers
{
    public static class Grid
    {
        /// <summary>
        /// Create ActionLinks on pages,based on database data
        /// Tuples: 1.PageName,2.StartActionName,3.ControllerName,4.TypeOfPage(example GridPage)
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="startTag">html begin tag example //<li/></param>
        /// <param name="endTag">html end tag</param>
        /// <param name="predicate">choose appropriate set of links,for example "GridPage", Tuples: 1.PageName,2.StartActionName,3.ControllerName,4.TypeOfPage(example GridPage)</param>
        /// <returns></returns>
        public static IHtmlString ActionLinksBuilder(this AjaxHelper ajax, string startTag, string endTag,
        Func<Tuple<string, string, string, Type>, bool> predicate = null)
        {
            var currentUserId = ajax.ViewContext.HttpContext.User.Identity.GetUserId();
           var admin= ajax.ViewContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .Users.SingleOrDefault(x => x.UserName == "Admin");
            var views = GetDbContext().Set<PageBase>();
          IEnumerable<dynamic> actionLinks = null;
            //if admin isnt created,initialize empty collection of pages
            if (admin!=null)
            {
               actionLinks = from view in views.ToList()
                    where view.ApplicationUserId == admin.Id
                    select new {Name = view.PageName, view.StartActionName, view.ControllerName, Type = view.GetType()};
            }
            else
            {
                actionLinks= new List<dynamic>();      
            }
            //PageName,StartActionName,ControllerName,TypeOfPage(example GridPage)
            List<Tuple<string, string, string, Type>> references = new List<Tuple<string, string, string, Type>>();
            foreach (var item in actionLinks)
            {
                references.Add(new Tuple<string, string, string,Type>
                (item.Name, item.StartActionName, item.ControllerName, item.Type));
            }
            StringBuilder sb = new StringBuilder();
            foreach (var tuple in predicate == null ? references : references.Where(predicate))
            {
                sb.Append(startTag);
                //var link = ajax.ActionLink((string)tuple.Item1, (string)tuple.Item2, (string)tuple.Item3, new AjaxOptions
                //{
                //    OnBegin = "onBegin",
                //    OnComplete = "onComplete('" + tuple.Item1 + "');",
                //    UpdateTargetId = "jumbo",
                //    InsertionMode = InsertionMode.Replace
                //});
                var link = GetHelpers.Html.ActionLink((string)tuple.Item1, (string)tuple.Item2, (string)tuple.Item3);
                sb.Append(link);
                sb.Append(endTag);
            }
            return MvcHtmlString.Create(sb.ToString());
        }


        public static void BoundColumns<T>(Kendo.Mvc.UI.Fluent.GridColumnFactory<T> columnsInGrid,
        ICollection<GridColumnDefinitionViewModel> columnsBindData) where T : class
        {
            foreach (var columnBindData in columnsBindData)
            {
                Type columnType = Type.GetType(columnBindData.DateType);

                if ((columnType == typeof(DateTime?) || columnType == typeof(DateTime)) &&
                ( !columnBindData.Hiden))
                {
                    columnsInGrid.Bound(columnType, columnBindData.BindName)
                    .Title(columnBindData.DisplayName).ClientTemplate
                    ("#=" + columnBindData.BindName +
                    " ? kendo.toString(kendo.parseDate(" + columnBindData.BindName +
                    "), 'dd/MM/yyyy HH:mm') : '' #").Filterable(f => f.UI("gridApp.DateTimeFilter"));
                    continue;
                }

                if ((columnType == typeof(Byte?[]) || columnType == typeof(Byte[])) &&
                (!columnBindData.Hiden))
                {
                    columnsInGrid.Bound(columnType, columnBindData.BindName)
                    .Title(columnBindData.DisplayName);
                    continue;

                }

                if ( columnBindData.Hiden)
                {
                    columnsInGrid.Bound(columnType, columnBindData.BindName)
                    .Title(columnBindData.DisplayName).Visible(false);
                }
                else
                    columnsInGrid.Bound(columnType, columnBindData.BindName)
                    .Title(columnBindData.DisplayName);

            }
            // foreach (var sw in property == null ? typeof(T).GetProperties() : property.PropertyType.GetProperties())
            // {
            // if (sw.PropertyType.Namespace.StartsWith("SqlHealthMonitor"))
            // {
            // string fullParentClassName;
            // if (parentClassName == null)
            // fullParentClassName = sw.Name + ".";
            // else
            // fullParentClassName = parentClassName + sw.Name + ".";
            // BoundColumns<T>(columns, hidenColumns, sw, fullParentClassName);
            // continue;
            // }
            // if (sw.PropertyType == typeof(DateTime?) || sw.PropertyType == typeof(DateTime))
            // {
            // if (hidenColumns == null ? false : hidenColumns.Exists(x => sw.Name.Contains(x)))
            // {
            // columns.Bound(sw.PropertyType, parentClassName + sw.Name).Title(Grid.GetTitle(sw)).ClientTemplate
            //("#=" + parentClassName + sw.Name + " ? kendo.toString(kendo.parseDate(" + parentClassName + sw.Name + "), 'dd/MM/yyyy HH:mm') : '' #")
            //.Filterable(f => f.UI("DateTimeFilter")).Hidden(true);
            // }
            // else
            // {
            // columns.Bound(sw.PropertyType, parentClassName + sw.Name).Title(Grid.GetTitle(sw)).ClientTemplate
            //("#=" + parentClassName + sw.Name + " ? kendo.toString(kendo.parseDate(" + parentClassName + sw.Name + "), 'dd/MM/yyyy HH:mm') : '' #")
            //.Filterable(f => f.UI("DateTimeFilter"));
            // }
            // continue;
            // }
            // if (hidenColumns == null ? false : hidenColumns.Exists(x => sw.Name.Contains(x)))
            // columns.Bound(sw.PropertyType, parentClassName + sw.Name).Title(Grid.GetTitle(sw)).Hidden(true);
            // else
            // columns.Bound(sw.PropertyType, parentClassName + sw.Name).Title(Grid.GetTitle(sw));
            // }
        }

        public static IHtmlString RowItemsBuilder(this HtmlHelper helper, ICollection<GridColumnDefinitionViewModel> columnsBindData)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<td >#= data.renderRowNumber() #</td>");
            sb.Append("<td>");
            foreach (var column in columnsBindData)
            {
                string bindName = "";
                if (column.Hiden)
                    continue;
                else if (column.DateType.Contains("DateTime"))
                {
                    bindName = "" + column.BindName +
                    " ? kendo.toString(kendo.parseDate(" + column.BindName +
                    "), 'dd/MM/yyyy HH:mm') : ''";
                }
                else
                    bindName = column.BindName;
                string record = string.Format(
                "<div class='panel panel-default' style='display: inline-block;vertical-align:top'>" +
                "<div class='panel-heading'>" +
                "{0}" +
                "</div>" +
                "<div class='panel-body' >" +
                "#:{1}#" +
                "</div>" +
                "</div>"
                , column.DisplayName, bindName);


                sb.AppendLine(record);
            }
            sb.Append("</td>");
            sb.Append("</table>");
            return MvcHtmlString.Create(sb.ToString());

        }

        public static IHtmlString RowItemsBuilder(this HtmlHelper helper, Type source, List<string> hidenColumns, string parentClassName = null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var sw in source.GetProperties())
            {
                if (hidenColumns == null ? false : hidenColumns.Exists(x => sw.Name.Contains(x)))
                    continue;
                string record = "";
                if (sw.PropertyType.Namespace.StartsWith("SqlHealthMonitor"))
                {
                    string fullParentClassName;
                    if (parentClassName == null)
                        fullParentClassName = sw.Name + ".";
                    else
                        fullParentClassName = parentClassName + sw.Name + ".";
                    sb.Append(RowItemsBuilder(helper, sw.PropertyType, hidenColumns, fullParentClassName));
                }
                else


                    record = string.Format("<div class='panel panel-default' style='display: inline-block'>" +
                    "<div class='panel-heading'>" +
                    "{0}" +
                    "</div>" +
                    "<div class='panel-body' >" +
                    "#:{1}#" +
                    "</div>" +
                    "</div>"
                    , sw.Name, parentClassName + sw.Name);


                sb.AppendLine(record);
            }
            return MvcHtmlString.Create(sb.ToString());

        }

        private static DbContext GetDbContext()
        {
            var container = (HttpContext.Current.ApplicationInstance as IContainerAccessor).Container;
            return container.Resolve<DbContext>(); 
        }
    }

}