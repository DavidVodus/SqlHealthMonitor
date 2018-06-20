
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;


namespace SqlHealthMonitor.Helpers
{
    public static class JsonHelper
    {
        /// <summary>
        /// Create json in format { Result = "OK", Records = myObject ,Message=message} with  myObject embedded in Records
        /// This format is consumed by Jtable
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ContentResult JsonOk(object myObject=null,string message=null)
        {
            try
            {
                var objectToParse = new { Result = "OK", Records = myObject, Message = message };
                var ser = JsonConvert.SerializeObject(objectToParse,
                new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat });
                var parsed = JObject.Parse(ser);
                return new ContentResult { Content = parsed.ToString(), ContentType = "application/json" };
            }
            catch (System.Exception e)
            {
                throw e;
            }
          
        }
        /// <summary>
        /// Create json in format { Result = "ERROR" ,Message=message }
        /// This format is consumed by Jtable
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ContentResult JsonError(string message )
        {
            try
            {
                var objectToParse = new { Result = "ERROR",Message = message };
                var ser = JsonConvert.SerializeObject(objectToParse,
                new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat });
                var parsed = JObject.Parse(ser);
                return new ContentResult { Content = parsed.ToString(), ContentType = "application/json" };
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }
    }
}
