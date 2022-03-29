using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagementService.Web.Api.Models
{
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}