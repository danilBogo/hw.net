namespace TimMovie.WebApi.Configuration

open System
open Microsoft.OpenApi.Any
open Swashbuckle.AspNetCore.SwaggerGen

type EnumSchemaFilter() =
    interface ISchemaFilter with
        member this.Apply(schema, context) =
            if context.Type.IsEnum then
                schema.Enum.Clear()
                Enum.GetNames(context.Type) |> Array.iter (fun item -> schema.Enum.Add(OpenApiString(item)))