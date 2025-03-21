using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.API.Models
{
    public enum MessageType
    {
        Calculation,
        Response,
        VariableRequest,
        VariableCompletion
    }

    public class Msg
    {
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }
}
