using QuantitySystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantitySystem.org
{

    public class QuantityInformation
    {
        public QuantityInformation(string name, string dimension, string unit)
        {
            // TODO: Complete member initialization
            this.Name = name;
            this.Dimension = dimension;
            this.Unit = unit;
        }
        public string Name { get; set; }
        public string Dimension { get; set; }
        public string Unit { get; set; }
    }

    public static  class QsWeb
    {
        public static IEnumerable<string> GetVariablesKeys()
        {
            return Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.GetKeys().AsEnumerable();
        }

        public static object GetVariable(string varName)
        {
            object q;
            Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.TryGetValue(varName, out q);
            return q;
        }



        static QuantityInformation[] CachedQuantities;

        public static QuantityInformation[] Quantities()
        {
            if (CachedQuantities == null)
            {
                List<QuantityInformation> quats = new List<QuantityInformation>();

                foreach (Type QType in QuantitySystem.QuantityDimension.AllQuantitiesTypes)
                {
                    quats.Add(
                        new QuantityInformation(
                            QType.Name.Substring(0, QType.Name.Length - 2)
                            , QuantitySystem.QuantityDimension.DimensionFrom(QType).ToString()

                            ,
                            new Units.Unit(QType).Symbol
                        //Units.Unit.DiscoverUnit(QuantitySystem.QuantityDimension.DimensionFrom(QType)).Symbol
                            )
                            );
                }

                var qss = from q in quats orderby q.Name select q;
                CachedQuantities = qss.ToArray();
            }
            return CachedQuantities;
        }

        public struct UnitInfo
        {
            public string Name;
            public string Symbol;
            public string System;
            public string QuantityType;
        }

        public static UnitInfo[] AvailableUnits(string quantity="")
        {
            var units = new List<UnitInfo>();

            foreach (Type utype in Unit.UnitTypes)
            {
                QuantitySystem.Attributes.UnitAttribute ua = Unit.GetUnitAttribute(utype);
                if (ua != null)
                {
                    string uname = utype.Name.PadRight(16);

                    string symbol = "<" + ua.Symbol + ">";
                    symbol = symbol.PadRight(10);

                    string system = utype.Namespace.Substring("QuantitySystem.Units".Length + 1).PadRight(18);

                    string qtype = ua.QuantityType.ToString().Substring(ua.QuantityType.Namespace.Length + 1).TrimEnd("`1[T]".ToCharArray());

                    UnitInfo ui = new UnitInfo
                    {
                        Name = uname,
                        Symbol = symbol,
                        System = system,
                        QuantityType = qtype
                    };

                    if (string.IsNullOrEmpty(quantity))
                    {
                        units.Add(ui);
                    }
                    else
                    {
                        //print if only the unit is for this quantity
                        if (qtype.Equals(quantity, StringComparison.OrdinalIgnoreCase))
                        {
                            //Console.WriteLine("    " + uname + " " + symbol + " " + system + qtype);
                            units.Add(ui);
                        }
                    }
                }
            }

            return units.OrderBy(x => x.QuantityType).ToArray();
        }
    }
}