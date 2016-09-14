using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLM_WLMN.Common.DTOs
{
    public class UserUpdate
    {
        public Guid InternalID { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Source { get; set; }
        public DateTime ExtractionDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Location { get; set; }
    }
}
