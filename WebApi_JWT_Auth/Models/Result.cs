using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi_JWT_Auth.Models
{
    public class Result
    {
        public string status { get; set; }

        public List<string> response { get; set; }
    }
}