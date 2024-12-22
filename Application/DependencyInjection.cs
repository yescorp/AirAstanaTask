using Application.Abstractions.Authorization;
using Application.Behaviour;
using Application.Common.Options;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.Configure<AccessTokenGeneratorOptions>(configuration.GetSection(AccessTokenGeneratorOptions.ConfigurationSection));

            AccessTokenGeneratorOptions accessTokenGeneratorOptions = configuration.GetSection(AccessTokenGeneratorOptions.ConfigurationSection).Get<AccessTokenGeneratorOptions>();

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

            services.AddTransient<IPasswordHashEvaluator, PasswordHashEvaluator>();
            services.AddTransient<ITokenGenerator, AccessTokenGenerator>();
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = accessTokenGeneratorOptions.Issuer,
                        ValidAudience = accessTokenGeneratorOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessTokenGeneratorOptions.SecretKey))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
