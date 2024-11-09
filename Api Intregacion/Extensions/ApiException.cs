﻿using System.Globalization;

namespace Api_Intregacion.Extensions
{
    public class ApiException:Exception
    {
        //excep
        public ApiException () : base() { }
        public ApiException (string message) : base(message) { }
        public ApiException (string message, params object[] args):base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
