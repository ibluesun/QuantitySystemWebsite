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
        
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.ReplacePrimaryScopeStorage(new QsWebStorageProvider());

            // point to the currency converter
            QuantitySystem.DynamicQuantitySystem.AddDynamicUnitConverterFunction("Currency", QsRoot.Currency.CurrencyConverter);

        }
    }
}
