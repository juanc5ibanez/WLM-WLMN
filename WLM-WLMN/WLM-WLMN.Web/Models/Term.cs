using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLM_WLMN.Web.Models
{
    public class Term
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

    }
}