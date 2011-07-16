using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Security.Principal;

namespace GeCoSurvey.Web.Helpers
{
    public static class AuthorizationExtensions
    {
        private static Dictionary<Expression, AuthorizeAttribute[]> expressionAuthorizers = new Dictionary<Expression, AuthorizeAttribute[]>();

        /// <summary>
        /// Determines whether the specified action is authorized.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="actionMethod">The action method.</param>
        /// <returns>
        ///     <c>true</c> if the specified helper is authorized; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAuthorized<TController>(this HtmlHelper helper, Expression<Action<TController>> action)
        {
            var call = action.Body as MethodCallExpression;

            if (call == null) return false;

            var authorizers = expressionAuthorizers.ContainsKey(action)
                ? expressionAuthorizers[action]
                : expressionAuthorizers[action] = GetAttributes<AuthorizeAttribute>(call);

            return (authorizers.Length > 0)
                ? authorizers.All(a => a.IsAuthorized(helper.ViewContext.HttpContext.User))
                : true;
        }

        /// <summary>
        /// Gets the specified attributes for an action method.
        /// </summary>
        /// <param name="call">The call.</param>
        /// <returns></returns>
        private static TAttribute[] GetAttributes<TAttribute>(MethodCallExpression call) where TAttribute : Attribute
        {
            return call.Object.Type.GetCustomAttributes(typeof(TAttribute), true)
                .Union(call.Method.GetCustomAttributes(typeof(TAttribute), true))
                .Cast<TAttribute>()
                .ToArray();
        }

        /// <summary>
        /// Determines whether the specified <see cref="AuthorizeAttribute"/> authorizes the specified user.
        /// </summary>
        /// <param name="authorize">The <see cref="AuthorizeAttribute"/>.</param>
        /// <param name="user">The user.</param>
        /// <returns>
        ///     <c>true</c> if the specified user is authorized; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAuthorized(this AuthorizeAttribute authorize, IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return false;

            var users = authorize.Users.SplitString();
            if (users.Length > 0 && !users.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase)) return false;

            var roles = authorize.Roles.SplitString();
            if (roles.Length > 0 && !roles.Any(user.IsInRole)) return false;

            return true;
        }

        /// <summary>
        /// Splits and trims the specified string.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        public static string[] SplitString(this string original)
        {
            return string.IsNullOrEmpty(original)
                ? new string[0]
                : original.Split('.').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }
    }
}