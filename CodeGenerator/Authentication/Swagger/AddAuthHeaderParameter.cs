using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace CodeGenerator.Authentication
{
    /// <summary>
    /// The class to add the authorization header in the swagger.
    /// </summary>
    public class AddAuthHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// Applies the operation filter.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null && string.Equals(apiDescription.RelativePath, "api/CodeGenerator/Auth/{userMail}/{user}?platform={platform}"))
            {
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "access token",
                    required = true,
                    type = "string"
                });
            }
        }
    }
}