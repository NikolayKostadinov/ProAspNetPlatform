using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModules
{
    public class RequestTimerEventArgs:EventArgs
    {
        public float Duration { get; set; }
    }
}
