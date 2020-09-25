using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PablaAccountingAndTaxServices
{
    public static class ExpandoClass
    {
        //converting the anonymous object into an ExpandoObject
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            //IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
            //IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            //IDictionary<string, object> expando = new ExpandoObject();
            //foreach (var item in anonymousDictionary)
            //    expando.Add(item);
            //return (ExpandoObject)expando;
        }
    }
}