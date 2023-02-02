using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlBot
{
    public static class ListExtensions
    {
        public static List<T> Copy<T>(this List<T> list)
        {
            return new List<T>(list);
        }
    }
}
