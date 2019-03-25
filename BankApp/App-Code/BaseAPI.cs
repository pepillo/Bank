using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ApiStruct
{
    public class BaseAPI : ApiController
    {
        protected dynamic ApiReturnValue = null;
        protected dynamic ApiValue = null;

        public string ApiVersion = "";

        public string Version()
        {
            return this.ApiVersion;
        }

        public dynamic SetValue(dynamic Value)
        {
            this.ApiValue = Value;

            return this;
        }

        public dynamic Result()
        {
            var Out = (this.ApiReturnValue != null) ? this.ApiReturnValue : this.ApiValue;

            return Out;
        }
    }
}