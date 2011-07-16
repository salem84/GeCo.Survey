using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;

namespace GeCoSurvey.Web.Helpers
{

    public static class LinkExtensions
    {
        /// <summary>
        /// Render an HTML action link if authorized by the target action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="action">The action.</param>
        /// <param name="linkText">The link text.</param>
        /// <returns></returns>
        public static string AuthorizedActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText)
            where TController : Controller
        {
            return AuthorizedActionLink<TController>(helper, action, linkText, null);
        }

        /// <summary>
        /// Render an HTML action link if authorized by the target action.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="action">The action.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static string AuthorizedActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText, object htmlAttributes)
            where TController : Controller
        {
            var routeValuesFromExpression = ExpressionHelper.GetRouteValuesFromExpression<TController>(action);

            return null;
            /*return helper.IsAuthorized(action)
                ? null //helper.RouteLink(linkText, routeValuesFromExpression, new RouteValueDictionary(htmlAttributes))
                : null;*/
        }
    }



}