using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashServer.responseHandler
{
    public interface IResponseAction
    {
        Response CreateResponse(ResponseType responseType, object content);
    }
}
