using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OstLib
{
    public static class Context
    {
        public static ApplicationContext db { get; private set; }
        internal static void AddDb(ApplicationContext application)
        {
            db = application;
        }
    }
}
