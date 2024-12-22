using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Authorization
{
    public interface IPasswordHashEvaluator
    {
        public (string password, string saltBase64) HashPassword(string password);
        public bool PasswordMatch(string password, string salt, string hash);
    }
}
