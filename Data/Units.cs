namespace follow_the_metric.Data
{
    public class Units
    {
        public Dictionary<string, Dictionary<string, double>> units = new Dictionary<string, Dictionary<string, double>>();

        public Units() 
        {
            units = new Dictionary<string, Dictionary<string, double>>()
            {
                { "length",
                    new Dictionary<string, double>()
                    {
                        { "cm", 30.48 },
                        { "m", 0.3048 },
                        { "in", 12 },
                        { "ft", 1 },
                        { "yd", 0.3333 },
                        { "miles", 0.00019 },
                    }
                },
                { "area", 
                    new Dictionary<string, double>()
                    {
                        { "cm2", 929.03 },
                        { "m2", 0.092903 },
                        { "in2", 144 },
                        { "ft2", 1 },
                        { "h2", 0.000009 },
                        { "평", 0.028109 },
                    }
                },
                { "weight",
                    new Dictionary<string, double>()
                    {
                        { "mg", 1000 },
                        { "g", 1 },
                        { "kg", 0.001 },
                        { "ton", 0.000001 },
                        { "oz", 0.03527 },
                        { "lb", 0.0022 },
                    }
                },
                { "volume",
                    new Dictionary<string, double>()
                    {
                        { "cu.cm", 1000000 },
                        { "cu.m", 1 },
                        { "ℓ", 1000 },
                        { "cu.in", 61027 },
                        { "cu.ft", 35.3165 },
                        { "gal", 264.186 },
                    }
                }
            };
        }

        public double GetPair(string kind, string first, string second)
        {
            if (!units.ContainsKey(kind))
            {
                return 1;
            }
            if (!units[kind].ContainsKey(first) || !units[kind].ContainsKey(second))
            {
                return 1;
            }
            return units[kind][second] / units[kind][first];
        }
    }
}
