using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace ZeroConsole
{
    public class ObjectId
    {
        private static readonly ConcurrentDictionary<string, int> dict = new ConcurrentDictionary<string, int>();

        public static int Next(string key)
        {
            dict.AddOrUpdate(key, 1, (k, v) => v + 1);

            return dict[key];
        }
    }
}
