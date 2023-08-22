namespace Faliush.ContactManager.Api.Definitions.Base;

public static class AppDefinitionExtention
{
    public static void AddDefinition(this WebApplicationBuilder builder, params Type[] entryPointsAssembly)
    {
        var definitions = new List<IAppDefinition>();

        foreach (var entryPoint in entryPointsAssembly)
        {
            // get the types you want 
            var types = entryPoint.Assembly.ExportedTypes.Where(
                x => !x.IsAbstract 
                && typeof(IAppDefinition).IsAssignableFrom(x));

            // create instances and casts to IAppDefinition
            var instances = types.Select(Activator.CreateInstance).Cast<IAppDefinition>().ToList();

            // added instances to definition
            definitions.AddRange(instances);
        }

        definitions.ForEach(app => app.ConfigureServices(builder));
        builder.Services.AddSingleton(definitions as IReadOnlyCollection<IAppDefinition>);
    }

    public static void UseDefinition(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IAppDefinition>>();

        foreach (var endpoint in definitions)
            endpoint.ConfigureApplication(app);
    }
}
