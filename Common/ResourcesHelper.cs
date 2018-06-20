using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ResourcesHelper
    {
        public static string GetValue(this ResourceManager resourceManager, string keyName)
        {

            ResourceSet resourceSet = resourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            IDictionaryEnumerator dictNumerator = resourceSet.GetEnumerator();

            // Get all string resources
            while (dictNumerator.MoveNext())
            {
                // Only string resources
                if (dictNumerator.Value is string)
                {
                    var key = (string)dictNumerator.Key;
                    var value = (string)dictNumerator.Value;
                    if (key == keyName)
                        return value;
                }
            }
            return null;
        }
    }
}
