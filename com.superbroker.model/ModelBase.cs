using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    public class ModelBase
    {
        public const String TABLENAME = "h_base";
        public int Id { get; set; }
        public DateTime AddOn { get; set; }
    }
}
