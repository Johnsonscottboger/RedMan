using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model.Entities
{
    public class TopicCollect
    {
        public Int64 UserId { get; set; }
        public Int64 TopicId { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
