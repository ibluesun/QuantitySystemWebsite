using Qs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuantitySystem.org
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static QsWebStorageProvider WebStorage = new QsWebStorageProvider();
        public static Dictionary<string, QsScopeStorage> BotUsersSpaces = new Dictionary<string, QsScopeStorage>( StringComparer.OrdinalIgnoreCase);


        public static void SetBotUserPrimaryStorage(string userId)
        {
            QsScopeStorage st;
            if(!BotUsersSpaces.TryGetValue(userId, out st))
            {
                st = new QsScopeStorage();
                BotUsersSpaces.Add(userId, st);
            }
            Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.ReplacePrimaryScopeStorage(st);
        }

        public static void ResetPrimaryStorage()
        {
            Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.ReplacePrimaryScopeStorage(WebStorage);

        }
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            ResetPrimaryStorage();

            // point to the currency converter
            QuantitySystem.DynamicQuantitySystem.AddDynamicUnitConverterFunction("Currency", QsRoot.Currency.CurrencyConverter);

        }
    }
}
