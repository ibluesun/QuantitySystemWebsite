using Qs.Types;
using QsRoot;
using QuantitySystem.org.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuantitySystem.org.Controllers
{
    public class FBBotController : Controller
    {
        // GET: FBBot
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Receive()
        {
            var query = Request.QueryString;

            //_logWriter.WriteLine(Request.RawUrl);

            if (query["hub.mode"] == "subscribe" &&
                query["hub.verify_token"] == "QuantitySystem")
            {
                //string type = Request.QueryString["type"];
                var retVal = query["hub.challenge"];
                return Json(int.Parse(retVal), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

 
        string JsonRegularMessageResponse(string senderId, string text)
        {
            dynamic botres = new ExpandoObject();

            botres.recipient = new ExpandoObject();
            botres.recipient.id = senderId;
            botres.message = new ExpandoObject();
            botres.message.text = text;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(botres, Newtonsoft.Json.Formatting.None);

            return json;
        }

        /// <summary>
        /// tuple first string  is the title
        /// tuple second string is the postback
        /// facebook is only taking up to 3 buttons .. so be ware.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="listTitle"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        string JsonButtonsPostBackListResponse(string senderId, string listTitle, params Tuple<string,string>[] buttons)
        {
            dynamic botres = new ExpandoObject();
            botres.recipient = new ExpandoObject();
            botres.recipient.id = senderId;

            botres.message = new ExpandoObject();
            botres.message.attachment = new System.Dynamic.ExpandoObject();

            botres.message.attachment.type = "template";
            botres.message.attachment.payload = new System.Dynamic.ExpandoObject();

            botres.message.attachment.payload.template_type = "button";
            botres.message.attachment.payload.text = listTitle;

            var buttonsList = new List<ResponseButtons>();

            int i = 0;
            foreach (var b in buttons)
            {
                if (i > 2) break;
                buttonsList.Add(new ResponseButtons()
                {
                    type = "postback",
                    title = b.Item1,
                    payload = b.Item2
                });
                i++;
            }

            botres.message.attachment.payload.buttons = buttonsList;


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(botres, Newtonsoft.Json.Formatting.None);
            return json;
        }




        void ListVariables(string senderId)
        {
            var variables = QsWeb.GetVariablesKeys().ToArray();

            var vars = from v in variables
                       select new Tuple<string, string>(v + ": " + QsWeb.GetVariable(v), v);
            var json = JsonButtonsPostBackListResponse(senderId, "Variables", vars.ToArray());
            FBPost(json);

        }

        void ListQuantities(string senderId)
        {
            var quantities = from q in QsWeb.Quantities()
                             select new Tuple<string, string>(q.Name + " " + q.Unit, "list units " + q.Name);

            var json = JsonButtonsPostBackListResponse(senderId, "Quantities", quantities.ToArray());

            FBPost(json);
        }

        void ListUnits(string senderId, string quantitiy)
        {


            var units = from unit in QsWeb.AvailableUnits(quantitiy)
                        select new Tuple<string, string>(unit.Name + $" {unit.Symbol}", $"{unit.Symbol}");

            var json = JsonButtonsPostBackListResponse(senderId, "Units", units.ToArray());

            FBPost(json);

                
        }

        void PrintCopyright(string senderId)
        {
            

            //var lib_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(QuantitySystem.QuantityDimension)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
            //var qsc_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(Qs.Qs)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
            //var calc_ver = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
            //var symb_ver = (AssemblyFileVersionAttribute)Assembly.GetAssembly(typeof(SymbolicAlgebra.SymbolicVariable)).GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];


            var sb = new System.Text.StringBuilder();
            
            sb.AppendLine("Quantity System Framework  ver 1.4");
            sb.AppendLine("Quantity System DLR        ver 1.4");
            sb.AppendLine("Symbolic Algebra Library   ver 0.8.99");



            //var qsc_cwr = (AssemblyCopyrightAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
            sb.AppendLine("Copyright (c) 2008 - " + DateTime.Now.Year.ToString() + " at Lost Particles Network by Ahmed Sadek");
            sb.AppendLine();
            sb.AppendLine("Project Source: https://github.com/ibluesun/QuantitySystem");
            sb.AppendLine("Project Blog:   http://QuantitySystem.WordPress.com");
            sb.AppendLine();
            sb.AppendLine("-------------------------------------------------------------------");
            sb.AppendLine("--                Ahmed Sadek Mohamed Tawfik                     --");
            sb.AppendLine("--              Ahmed.Sadek@LostParticles.net                    --");
            sb.AppendLine("-------------------------------------------------------------------");

            var json = JsonRegularMessageResponse(senderId, sb.ToString());
            FBPost(json);

        }


        public bool CommandProcessed;


        public bool CheckCommand(string senderId, string command)
        {
            string[] commands = command.ToLower().Split(' ');

            //remove unnessacary spaces.
            for (int i = 0; i < commands.Length; i++)
                commands[i] = commands[i].Trim();

            if (commands[0] == "quit") return false;

            if (commands[0] == "exit")
            {
                System.Environment.Exit(0);
                //Console.WriteLine("Press CTRL+Z to exit");
                CommandProcessed = true;
            }

            if (commands[0] == "help")
            {
                //PrintHelp();
                CommandProcessed = true;
            }

            if (commands[0] == "list")
            {
                string param = string.Empty;

                if (commands.Length < 2)
                {
                    ListVariables(senderId);
                }
                else
                {
                    if (commands[1] == "quantities")
                        ListQuantities(senderId);

                    if (commands[1] == "units")
                    {
                        if (commands.Length < 3)
                        {
                            ListUnits(senderId, string.Empty);
                        }
                        else
                        {
                            ListUnits(senderId, commands[2]);
                        }
                    }

                    if (commands[1] == "prefixes")
                    {
                        //ListMetricPrefixes();
                    }
                }

                CommandProcessed = true;
            }

            if (commands[0] == "new")
            {
                Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.Clear();
                FBPost(JsonRegularMessageResponse(senderId, "All Variables Cleared"));
                GC.Collect();

                CommandProcessed = true;
            }

            if (commands[0] == "cls")
            {
                Console.Clear();
                CommandProcessed = true;
            }

            if (commands[0] == "copyright")
            {
                PrintCopyright(senderId);
                CommandProcessed = true;
            }

            if (commands[0] == "run")
            {
                //then we want to load a text file for evaluating its contents
                // and adding its content to this session.

                if (commands.Length > 1)
                {
                    string file = commands[1];
                    //if (File.Exists(file))
                    {
                        CommandProcessed = true;
                    }
                }
            }
            return true;
        }



        void ExecuteMessage(string senderId, string text)
        {
            string msg = "";
            try
            {
                Object result;
                lock (Qs.Runtime.QsEvaluator.CurrentEvaluator)
                {
                    WebApiApplication.SetBotUserPrimaryStorage(senderId);

                    result = null;

                    CheckCommand(senderId, text);

                    if (CommandProcessed) return;
                    
                    result = Qs.Runtime.QsEvaluator.CurrentEvaluator.Evaluate(text);

                    WebApiApplication.ResetPrimaryStorage();
                }

                if (result is QsObject)
                {
                    var bitmap = ((QsObject)result).ThisObject as Bitmap;

                    if (bitmap != null)
                    {
                        
                        string path = HttpContext.Server.MapPath($"~/QsWebData/Plots/{senderId}_plot.png");
                        bitmap.Save(path);

                        var domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);


                        dynamic botres = new System.Dynamic.ExpandoObject();
                        botres.recipient = new System.Dynamic.ExpandoObject();
                        botres.message = new System.Dynamic.ExpandoObject();
                        botres.message.attachment = new System.Dynamic.ExpandoObject();
                        botres.message.attachment.payload = new System.Dynamic.ExpandoObject();
                        botres.recipient.id = senderId;
                        
                        botres.message.attachment.type = "image";

                        string url = domain + "/QsWebData/Plots/" + senderId + "_plot.png";
                        botres.message.attachment.payload.url = url;

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(botres, Newtonsoft.Json.Formatting.None);


                        FBPost(json);

                    }
                    else
                    {
                        msg = ((QsObject)result).ThisObject.GetType().Name + " Response";

                        var json = JsonRegularMessageResponse(senderId, msg);
                        FBPost(json);
                    }
                }
                else if (result is Units.Unit)
                {
                    var unit = result as Units.Unit;

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine();
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

                    var json = JsonRegularMessageResponse(senderId, sb.ToString());
                    FBPost(json);
                }
                else
                {
                    if (result != null)
                    {
                        msg = result.ToString();

                        var json = JsonRegularMessageResponse(senderId, msg);
                        FBPost(json);
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                var json = JsonRegularMessageResponse(senderId, msg);
                FBPost(json);
            }
        }

        [ActionName("Receive")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReceivePost(BotRequest data)
        {
            Task.Factory.StartNew(() =>
            {
            foreach (var entry in data.entry)
            {
                    foreach (var message in entry.messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.message?.text))
                        {
                            // check if it is post back
                            if (string.IsNullOrWhiteSpace(message?.postback?.payload))
                                continue;
                            else
                                ExecuteMessage(message.sender.id, message.postback.payload);
                        }
                        else
                        {
                            ExecuteMessage(message.sender.id, message.message.text);
                        }
                    }

                    
                }
            });

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        private string FBPost(string json)
        {
           return PostRaw("https://graph.facebook.com/v2.6/me/messages?access_token=EAAZAa76aLiW8BAJdeTxxTTGbkcTXvPpgXdZAYuZBF2s6jQjAMgwhXFquahR8AepLQYXZCfZCP0KZCnLGlROLu4ZBHAmdPrPeRHtIKuF3lHhiIUBoz75fy5BZCtmx03GMMfuZBh522yd03KovPPrYeEuTXvhmHr3IBOJJF0TLCQWAv3gZDZD", json);
        }

        private string PostRaw(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
    }
}