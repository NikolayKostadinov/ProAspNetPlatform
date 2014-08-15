using System.Collections.Generic;

namespace SimpleApp.Models
{
    public class ApplicationModel 
    {
        public IEnumerable<string> Events { get; set; }

        public IEnumerable<string> TimeStamps { get; set; }
    }
}