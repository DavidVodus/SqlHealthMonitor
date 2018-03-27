using AutoMapper;
using Castle.DynamicProxy;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using SqlHealthMonitor.BLL.Attributes.Markers;
using SqlHealthMonitor.BLL.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using SqlHealthMonitor.BLL.Models.WebPages;

namespace SqlHealthMonitor.BLL.Services
{
    /// <summary>
    /// 
    /// if method is decorated by CacheMeAttribute ,Intercept method try to find if result of operation exist in Cache.
    /// if so, return it and store into Session ResultIsCached=true,else invoke method and store result into Cache.
    /// if CacheMe attrib doesnt exist,just call method and store into Session ResultIsCached=false
    /// </summary>
    public class CacheInterceptor : IInterceptor
    {
        public class CachedData
        {
            public object ReturnValue { get; set; }
            public string[] Arguments { get; set; }
            public string UserId { get; set; }
            public DateTime Created { get; set; }
            public string MethodName { get; set; }


        }
        private readonly ICacheService cacheService;

        public CacheInterceptor(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }
        /// <summary>
        /// Get specified custom attribute from method which is specified in IInvocation instance
        /// </summary>
        /// <typeparam name="T">type we want to check  if exist</typeparam>
        /// <param name="invocation">instance from interceptor</param>
        /// <returns></returns>
        private static T getCustomAttribute<T>(IInvocation invocation) where T : class
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }
            return (T)(object)Attribute.GetCustomAttribute(methodInfo, typeof(T));
        }
        /// <summary>
        ///iterate throught arguments of method and try to find out,if it's a kendoArg,
        ///if so and any filter has been set,return true
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static bool IsKendoArgumentEmpty(object[] arguments)
        {
            foreach (var arg in arguments)
            {
                if (arg.GetType() == typeof(Kendo.Mvc.UI.DataSourceRequest))
                {
                    var kendoArg = (Kendo.Mvc.UI.DataSourceRequest)arg;
                    if (kendoArg.Filters.Count <= 0)
                        return true;
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        ///  every called method must proceed throught this method
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
          
            var attribute = getCustomAttribute<CacheMeAttribute>(invocation);
            //If request comes from kendoGrid methods and none filter has been set, no caching will be used
            if (attribute != null && !IsKendoArgumentEmpty(invocation.Arguments))
            {
                //argumentJson contain arguments ,they were passed by calling method
              List<string> argumentJson = new List<string>();
                foreach (var arg in invocation.Arguments)
                {
                    //cancellation token gives a different number for each request
                    if (arg is CancellationToken)
                        continue;
                    argumentJson.Add(JsonConvert.SerializeObject(arg,
                  new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, TypeNameHandling = TypeNameHandling.Auto }));
                }
                //converts search parameters of user queries to string for making hashcode



                //var m = JsonConvert.DeserializeObject<object>(argumentJson);
                int hash = 0;
                foreach (var item in argumentJson)
                {
                  hash+= item.GetHashCode();
                }
               
              
                var key = invocation.TargetType.FullName + "." + invocation.Method.Name+":"+hash;
                //if result is cached,return cached value
                var cachedResult = cacheService.Get(key);
                if (cachedResult != null)
                {
                    HttpContext.Current.Items["ResultIsCached"] = true;
                    invocation.ReturnValue = ((CachedData)cachedResult).ReturnValue;
                }
                //else execute method and save result to cache
                else
                {
                    HttpContext.Current.Items["ResultIsCached"] = false;
                    var userName = HttpContext.Current.User.Identity.GetUserName();
                   invocation.Proceed();
                    var cachedData = new CachedData { ReturnValue = invocation.ReturnValue,
                        Arguments = argumentJson.ToArray(), UserId = userName, Created = DateTime.Now,
                        MethodName =invocation.Method.Name
                    };
                    string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString(); 
                    TimeSpan cacheDuration = new TimeSpan(0, attribute.DurationInMinutes, 0);
                    string applicationId = controller + "_" + "GridPageViewModel";

                    if (HttpContext.Current.Application[applicationId] != null)
                    {
                      //var  gridPageModel = (GridPageViewModel)HttpContext.Current.Application[applicationId];
                      //  cacheDuration = gridPageModel.CacheDuration;
                    }
                    cacheService.Add(key,cachedData, cacheDuration);
                }
            }
            //if method doesnt have cacheMe attribute,just execute it without caching
            else
            {
                HttpContext.Current.Items["ResultIsCached"] = false;
                invocation.Proceed();
            }
        }
    }
}
