namespace TimMovie.WebApi.Configuration

open Microsoft.OpenApi.Models

type SwaggerSettings() =
    static member GetScheme() =
        OpenApiSecurityScheme(
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme.
            \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.
            \r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
        )

    static member GetSecurityRequirement() =
        let securityRequirement = OpenApiSecurityRequirement()

        securityRequirement.Add(
            OpenApiSecurityScheme(Reference = OpenApiReference(Type = ReferenceType.SecurityScheme, Id = "Bearer")),
            Array.empty
        )

        securityRequirement
