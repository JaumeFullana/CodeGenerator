using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Net;
using System.Threading.Tasks;

namespace CodeGenerator.Authentication
{
    //This middleware is used to block the acces to the token generation operation through a GET request
    public class OnlyPostTokenMiddleware : OwinMiddleware
    {
        private readonly OAuthAuthorizationServerOptions opts;

        public OnlyPostTokenMiddleware(OwinMiddleware next, OAuthAuthorizationServerOptions opts) : base(next)
        {
            this.opts = opts;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (opts.TokenEndpointPath.HasValue && opts.TokenEndpointPath == context.Request.Path && context.Request.Method == "GET")
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ReasonPhrase = "Not Found";
                return context.Response.WriteAsync("Not Found");
            }

            return Next.Invoke(context);

        }
    }
}