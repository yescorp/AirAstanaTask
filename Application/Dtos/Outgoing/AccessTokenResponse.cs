using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Outgoing
{
    public class AccessTokenResponse
    {
        public AccessTokenResponse(string accessToken)
        {
            this.AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
    }
}
