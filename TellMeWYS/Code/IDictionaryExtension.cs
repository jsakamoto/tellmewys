using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellMeWYS
{
    public static class IDictionaryExtension
    {
        public static TValue GetAsCache<TValue>(this IDictionary self, object key, Func<TValue> getValue)
        {
            lock (self)
            {
                var value = default(TValue);
                if (self.Contains(key))
                {
                    value = (TValue)self[key];
                }
                else 
                {
                    value = getValue();
                    self.Add(key, value);
                }
                return value;
            }
        }
    }
}