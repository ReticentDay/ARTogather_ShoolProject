using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace client
{
    public class NetException : Exception, ISerializable
    {
        public NetException() 
            : base("網路連線錯誤") { }

        public NetException(string message)
            : base(message) { }

        public NetException(string message, Exception inner)
        : base(message, inner) { }

        protected NetException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
