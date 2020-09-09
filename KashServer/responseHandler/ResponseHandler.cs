using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace KashServer.responseHandler
{
    class ResponseHandler : IResponseAction
    {
        public Response CreateResponse(ResponseType responseType, object content)
        {
            Response response = new Response
            {
                ResponseType = responseType,
                Content = content
            };
            return response;
        }
        

    }
}
