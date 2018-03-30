using Qs.Runtime;
using Qs.Types;
using QuantitySystem.org.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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

                if (result is QsObject)
                {
                    var bitmap = ((QsObject)result).ThisObject as Bitmap;

                    if (bitmap != null)
                    {
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();

                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        byte[] imageBytes = stream.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);


                        //msg = "null";




                        dynamic botres = new System.Dynamic.ExpandoObject();
                        botres.recipient = new System.Dynamic.ExpandoObject();
                        botres.message = new System.Dynamic.ExpandoObject();
                        botres.message.attachment = new System.Dynamic.ExpandoObject();
                        botres.message.attachment.payload = new System.Dynamic.ExpandoObject();
                        botres.recipient.id = "hohoho";

                        botres.message.text =  "Function Plot";
                        botres.message.attachment.type = "image";
                        botres.message.attachment.payload.url = "https://lh5.googleusercontent.com/-IvKtT_Wrhkc/TYyu07xdqZI/AAAAAAAAAeU/m2IFsY_ELh8/s1600/Function.png";


                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(botres, Newtonsoft.Json.Formatting.None);

                        return Ok("   " + json);

                    }
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