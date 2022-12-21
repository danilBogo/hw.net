namespace TimMovie.WebApi

#nowarn "20"

open System
open Autofac
open Autofac.Extensions.DependencyInjection
open Microsoft.Extensions.Options
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open OpenIddict.Validation.AspNetCore
open TimMovie.Core
open TimMovie.Core.Classes
open TimMovie.Infrastructure
open TimMovie.WebApi.Configuration
open TimMovie.WebApi.Configuration.AppMappingProfile

type public Program() =

    [<EntryPoint>]
    static let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Host.UseServiceProviderFactory(AutofacServiceProviderFactory())
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)
        builder.Services.Configure(builder.Configuration)

        builder.Host.ConfigureContainer<ContainerBuilder>
            (fun (containerBuilder: ContainerBuilder) ->
                containerBuilder.RegisterModule(CoreModule())

                containerBuilder.RegisterModule(InfrastructureModule(builder.Configuration))
                |> ignore)

        let services = builder.Services
        let configuration = builder.Configuration

        services.AddControllers()

        services.AddCors
            (fun options ->
                options.AddDefaultPolicy
                    (fun builder ->
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                        |> ignore))

        services.AddDbContext(configuration["ConnectionStrings:DefaultConnection"])

        services.AddAuthentication
            (fun options -> options.DefaultScheme <- OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)

        services.AddAuthorization()

        let type1 = typeof<AppMappingProfile>
        let type2 = typeof<CoreMappingProfile>
        let type3 = typeof<InfrastructureMappingProfile>

        services.AddAutoMapper(type1, type2, type3)

        services.AddIdentity()

        services.Configure<MailSetup>(configuration.GetSection("MailSetup"))
        services.AddScoped<MailSetup>(fun (x: IServiceProvider) -> x.GetService<IOptions<MailSetup>>().Value)

        services
            .AddOpenIddict()
            .AddValidation(fun options ->
                options.SetIssuer(configuration["IdentityUrl"])
                |> ignore

                options.UseSystemNetHttp() |> ignore
                options.UseAspNetCore() |> ignore)

        services.AddSwaggerGen
            (fun config ->
                config.AddSecurityDefinition("Bearer", SwaggerSettings.GetScheme())
                config.AddSecurityRequirement(SwaggerSettings.GetSecurityRequirement())
                config.SchemaFilter<EnumSchemaFilter>())
        |> ignore

        let app = builder.Build()

        app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseCors()
            .UseSwagger()
            .UseSwaggerUI(fun config -> config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"))

        app.MapControllers()

        app.Run()

        0
