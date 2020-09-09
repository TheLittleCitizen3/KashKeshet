using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class Response
    {
        public ResponseType ResponseType;
        public Object Content;
    }
}
