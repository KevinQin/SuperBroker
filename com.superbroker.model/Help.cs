using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.superbroker.model
{
    public class Help:ModelBase
    {
        public new const String TABLENAME = "s_help";

        public string Title { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
    }
}
