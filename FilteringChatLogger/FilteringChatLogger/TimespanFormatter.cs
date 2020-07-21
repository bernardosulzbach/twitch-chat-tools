using System.Collections.Generic;
using System.Linq;

namespace FilteringChatLogger
{
    internal class TimespanFormatter
    {
        private static readonly List<Unit> Units = new List<Unit>
        {
            new Unit("ns", 1),
            new Unit("µs", 1000),
            new Unit("ms", 1000 * 1000),
            new Unit("s", 1000 * 1000 * 1000)
        };

        public static string FormatTimespan(ulong nanoseconds)
        {
            foreach (var unit in Units)
                if (nanoseconds / unit.RatioToSecond < 1000)
                    return unit.Format(nanoseconds);

            return Units.Last().Format(nanoseconds);
        }

        private readonly struct Unit
        {
            private readonly string _symbol;
            public readonly ulong RatioToSecond;

            public Unit(string symbol, ulong ratioToSecond)
            {
                _symbol = symbol;
                RatioToSecond = ratioToSecond;
            }

            public string Format(ulong nanoseconds)
            {
                return $"{nanoseconds / RatioToSecond} {_symbol}";
            }
        }
    }
}