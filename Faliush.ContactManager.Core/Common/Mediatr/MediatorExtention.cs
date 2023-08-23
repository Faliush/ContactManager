using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Faliush.ContactManager.Core.Common.Mediatr;
public static class MediatorExtention
{
    public static void AddMeditorCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
