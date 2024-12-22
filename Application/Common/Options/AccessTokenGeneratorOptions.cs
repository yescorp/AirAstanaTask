using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Options
{
    public class AccessTokenGeneratorOptions
    {
        public const string ConfigurationSection = nameof(AccessTokenGeneratorOptions);
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }

    }
}
