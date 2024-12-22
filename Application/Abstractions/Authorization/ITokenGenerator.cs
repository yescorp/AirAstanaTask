using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Authorization
{
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(string Username, string Role);
    }
}
