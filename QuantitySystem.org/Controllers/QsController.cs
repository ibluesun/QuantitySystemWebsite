using Qs.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace QuantitySystem.org.Controllers
{
    public class QsController : ApiController
    {

        static QsController()
        {

            QuantitySystem.DynamicQuantitySystem.AddDynamicUnitConverterFunction("Currency", QsRoot.Currency.CurrencyConverter);


        }

        public QsController()
        {

        }


        public string Get(string line)
        {
            try
            {
                var result = Qs.Runtime.QsEvaluator.CurrentEvaluator.Evaluate(line);

                return result.ToString();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
            }
        }

        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                if (value.Equals("new", StringComparison.OrdinalIgnoreCase))
                {
                    QsEvaluator.CurrentEvaluator.Scope.Clear();

                    return Ok("All Variables Cleared.");
                }
                
                var result = Qs.Runtime.QsEvaluator.CurrentEvaluator.Evaluate(value);

                if (result is Units.Unit)
                {
                    var unit = result as Units.Unit;

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("    Unit:        {0}", unit.ToString());
                    sb.AppendLine();

                    sb.AppendFormat("    Quantity:    {0}", unit.QuantityType.Name);
                    sb.AppendLine();

                    sb.AppendFormat("    Dimension:   {0}", unit.UnitDimension);
                    sb.AppendLine();
                    
                    sb.AppendFormat("    Unit System: {0}", unit.UnitSystem);

                    if (unit.IsOverflowed)
                    {
                        sb.AppendFormat("    Unit overflow: {0}", unit.GetUnitOverflow());
                        sb.AppendLine();
                    }

                    return Ok(sb.ToString());


                }

                return Ok("    " + result.ToString());
            }
            catch (Exception e)
            {
                
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));

            }
        }

    }
}