using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Common;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.WebPages;
using System.Collections.Specialized;
using System.Web.Routing;
using System.Reflection;
using System.Collections;

namespace SqlHealthMonitor.Helpers
{
    public static class Html
    {

        public class DataForActionlinks
        {
            public string Name { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public Type Type { get; set; }

        }
        /// <summary>
        /// Create ActionLinks on pages,based on database data
        /// Tuples: 1.PageName,2.StartActionName,3.ControllerName,4.TypeOfPage(example GridPage)
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="startTag">html begin tag example //<li/></param>
        /// <param name="endTag">html end tag</param>
        /// <param name="predicate">choose appropriate set of links,for example "GridPage", Tuples: 1.PageName,2.StartActionName,3.ControllerName,4.TypeOfPage(example GridPage)</param>
        /// <returns></returns>
        public static IHtmlString ActionLinksBuilder(this AjaxHelper ajax, string startTag, string endTag, string className,
        Func<DataForActionlinks, bool> predicate = null)
        {
            var currentUserId = ajax.ViewContext.HttpContext.User.Identity.GetUserId();
            var admin = ajax.ViewContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
                 .Users.SingleOrDefault(x => x.UserName == "Admin");
            var views = Database.GetDbContext().Set<PageBase>();
            IEnumerable<DataForActionlinks> dataForActionlinks = new List<DataForActionlinks>();
            //Get pages that belong to Admin,we take these pages as a Template for other Accounts and also as the data for initializing the web Site
            if (admin != null)
            {
                dataForActionlinks = from view in views.ToList()
                                     where view.ApplicationUserId == admin.Id
                                     select new DataForActionlinks { Name = view.PageName, Action = view.StartActionName, Controller = view.ControllerName, Type = view.GetType() };
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in predicate == null ? dataForActionlinks : dataForActionlinks.Where(predicate))
            {
                string nameOfLink = Resources.Global.ResourceManager.GetValue(item.Name) ?? item.Name;

                sb.Append(startTag);
                //var link = ajax.ActionLink((string)tuple.Item1, (string)tuple.Item2, (string)tuple.Item3, new AjaxOptions
                //{
                //    OnBegin = "onBegin",
                //    OnComplete = "onComplete('" + tuple.Item1 + "');",
                //    UpdateTargetId = "jumbo",
                //    InsertionMode = InsertionMode.Replace
                //});
                var link = GetMvcHelpers.Html.ActionLink(nameOfLink, item.Action, item.Controller, null, new { @class = className });
                sb.Append(link);
                sb.Append(endTag);
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Using with bootstrap to determine which of tabs on page is active
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string IsActive(this HtmlHelper htmlHelper, string action, string controller)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController && action == routeAction);

            return returnActive ? "active" : "";
        }


        /// <summary>
        /// it makes up route dictonary from an object that can contain primitive types or IEnumerable objects
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="parameters">object contains primitive date types and IEnumerables</param>
        /// <returns></returns>
        public static RouteValueDictionary RouteValueDictonaryBuilder(this HtmlHelper htmlHelper, object parameters)
        {
            var routeValueDictonary = new RouteValueDictionary();
            var objectType = parameters.GetType();
            string targetTypeName = objectType.Name;

            // Find all properties
            var sourceMembers = from source in objectType.GetMembers().ToList()
                                where source.MemberType == MemberTypes.Property
                                select source;

            foreach (var memberInfo in sourceMembers)
            {

                var property = ((PropertyInfo)memberInfo);
                //for IEnumerables
                if (property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {

                    int counter = 0;
                    foreach (var item in (IEnumerable)property.GetValue(parameters, null))
                    {
                        routeValueDictonary.Add(property.Name + "[" + counter + "]", item);
                        counter++;
                    }
                    if(counter==0)
                        routeValueDictonary.Add(property.Name,property.GetValue(parameters, null));
                }
                //for primitive date types
                else
                {
                    routeValueDictonary.Add(property.Name, property.GetValue(parameters, null));
                }


            }

            return routeValueDictonary;

        }



    }
}