using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Model.Enums;

namespace Web.Model.Entities
{
    public class Message
    {
        public Int64 MasterId { get; set; }
        public MessageType Type { get; set; }
        public Int64 Author_Id { get; set; }
        public Int64 Topic_Id { get; set; }
        public Int64 Reply_Id { get; set; }
        public bool Has_Read { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
