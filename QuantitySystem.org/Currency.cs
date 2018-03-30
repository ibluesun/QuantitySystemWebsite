using ParticleLexer.StandardTokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace QsRoot
{

    static class QsWebData
    {
        public static DirectoryInfo Folder
        {
            get
            {
                string data_folder = HttpContext.Current.Server.MapPath("~/QsWebData");
                DirectoryInfo di = new DirectoryInfo(data_folder);
                if (!di.Exists) di.Create();
                return di;
            }

        }

    }

    public static class Currency
    {

        static ParticleLexer.Token CurrenciesJson;
        static Dictionary<string, double> CurrentCurrencies;

        static Currency()
        {
            ReadCurrenciesJson();
        }



        /// <summary>
        /// Getting the exchange rate file each day so we don't hit the service too much
        /// </summary>
        static string TodayChangeRatesFile
        {
            get
            {

                //string file = string.Format(QsWebData.Folder.FullName + "\\XChangeRates-{0}-{1}-{2}.json", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                string file = HttpContext.Current.Server.MapPath($"~/QsWebData/XChangeRates-{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}.json"); 
                return file;
            }
        }


        static void DownloadExchangeFile()
        {
            string web = "http://openexchangerates.org/api/latest.json?app_id=25e93e39fd2b43939b2b62939479609d";
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] xch = wc.DownloadData(web);
            File.WriteAllBytes(TodayChangeRatesFile, xch);
        }

        static void ReadCurrenciesJson()
        {
            if (!File.Exists(TodayChangeRatesFile))
            {
                // get the file from http://openexchangerates.org
                DownloadExchangeFile();
            }

            using (var rr = new System.IO.StreamReader(TodayChangeRatesFile))
            {
                CurrenciesJson = ParticleLexer.Token.ParseText(rr.ReadToEnd());
            }

            /*
             * json format is 
             * 
             * { "key": value,  "key": "other value", "key":{ "key":value }
             * 
             * value is either string or number
             * 
             */
            CurrenciesJson = CurrenciesJson.TokenizeTextStrings();
            CurrenciesJson = CurrenciesJson.MergeTokens<WordToken>();
            CurrenciesJson = CurrenciesJson.RemoveAnySpaceTokens();
            CurrenciesJson = CurrenciesJson.RemoveNewLineTokens();
            CurrenciesJson = CurrenciesJson.MergeTokens<NumberToken>();
            CurrenciesJson = CurrenciesJson.MergeSequenceTokens<MergedToken>(
                typeof(ParticleLexer.CommonTokens.TextStringToken),
                typeof(ColonToken),
                typeof(NumberToken)
                );


            CurrentCurrencies = new Dictionary<string, double>();

            // find rates key
            foreach (var tok in CurrenciesJson)
            {
                if (tok.TokenClassType == typeof(MergedToken))
                {
                    CurrentCurrencies.Add(tok[0].TrimTokens(1, 1).TokenValue, double.Parse(tok[2].TokenValue));
                }
            }
        }

        public static double CurrencyConverter(string currency)
        {
            if (CurrentCurrencies == null) ReadCurrenciesJson();

            return 1.0 / CurrentCurrencies[currency];
        }

        /// <summary>
        /// updates the currency file and the conversion factors
        /// </summary>
        public static void Update()
        {
            DownloadExchangeFile();

            ReadCurrenciesJson();
        }
    }
}
