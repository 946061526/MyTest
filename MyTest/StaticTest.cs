using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTest
{
    public static class StaticTest
    {
        public static int userid;
        public static string auth;

        public static string GetAuth()
        {
            return auth;
        }

        public static int GetID()
        {
            return userid;
        }
    }
}