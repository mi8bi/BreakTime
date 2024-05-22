using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BreakTimeApp.Models
{
    public class TimeStoreItem
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Span { get; set; }
    }
}
