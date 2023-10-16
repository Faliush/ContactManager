using Faliush.ContactManager.Core.Common.ValidationBehavior;
using Faliush.ContactManager.Core.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Faliush.ContactManager.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<ApiBehaviorOptions>(option =>
        {
            option.SuppressModelStateInvalidFilter = true;
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        services.AddScoped<IDateCalcualtorService, DateCalculatorService>();
        

        return services;
    }
}
