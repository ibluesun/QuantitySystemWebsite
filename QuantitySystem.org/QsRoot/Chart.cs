using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace QsRoot
{
    public class Chart
    {

        public static string About()
        {
            return "Charting Module V0.1";
        }


        public static Bitmap Line(QsFunction function, QsVector vector)
        {
            var vParam = QsParameter.MakeParameter(vector, "0");
            var result = function.InvokeByQsParameters(vParam) as QsVector;

            if(result !=null)
            {
                var myModel = new PlotModel { Title = "Quantity System" };

                LineSeries ls = new LineSeries();

                for (int ix = 0; ix < vector.Count; ix++)
                {
                    ls.Points.Add(new DataPoint(vector[ix].NumericalQuantity.Value, result[ix].NumericalQuantity.Value));
                }


                ls.Title = function.FunctionBody;
                myModel.Series.Add(ls);



                var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
                var bitmap = pngExporter.ExportToBitmap(myModel);



                //return result.ToString();
                return bitmap;
            }

            return null;
        }

    }
}