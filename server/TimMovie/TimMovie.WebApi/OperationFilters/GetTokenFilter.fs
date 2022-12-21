module TimMovie.WebApi.OperationFilters.GetTokenFilter
//
//open Microsoft.AspNetCore.Mvc.ApiExplorer
//open Microsoft.OpenApi.Models
//open Swashbuckle.AspNetCore.SwaggerGen
//open System.Collections.Generic
//
//type GetTokenFilter() =
//    interface IOperationFilter with
//        member this.Apply(operation, _) =
//            let parameter = OpenApiParameter()
//            
//            parameter.Name <- "username"
//            parameter.Content
//            parameter.In <- ParameterLocation.Header
//            parameter.Required <- true
//            operation.Parameters.Add(parameter)
//        
//////        if operation.Parameters = null then operation.Parameters <- new ResizeArray<IParameterFilter>()
////        let parameter = OpenApiParameter()
////        parameter.Name = "MY-HEADER"
////        parameter.In = "header"
////        parameter
////        operation.Parameters.Add()
////    public void Apply(Operation operation, OperationFilterContext context)
////    {
////        if (operation.Parameters == null)
////            operation.Parameters = new List<IParameter>();
//// 
////        operation.Parameters.Add(new NonBodyParameter
////        {
////            Name = "MY-HEADER",
////            In = "header",
////            Type = "string",
////            Required = true // set to false if this is optional
////        });
////    }