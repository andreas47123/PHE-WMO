using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

public static class HtmlHelperExtensions
{
    public static string CreateRouteFromUri(this HtmlHelper htmlHelper, string url)
    {
        ViewContext viewContext = htmlHelper.ViewContext;

        if (viewContext.HttpContext.Request != null)
        {
            string requestUrl = viewContext.HttpContext.Request.ApplicationPath;
            if (requestUrl != "/")
            {
                requestUrl = requestUrl + "/";
            }

            return requestUrl + url;
        }

        return url;
    }
}