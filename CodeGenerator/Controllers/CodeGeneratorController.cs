using CodeGenerator.Logic;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodeGenerator.Controllers
{
    public class CodeGeneratorController : ApiController
    {
        // http://<servidor>/api/CodeGenerator/mail
        /// <summary>
        /// Generates a random code in string format.
        /// </summary>
        /// <remarks>
        /// Generates a random code in string format, send it to the mail address passed by paramether and returns the code. 
        /// You need to request an acces token to use this operation.
        /// </remarks>
        /// <param name="userMail">The user mail address</param>
        /// <param name="user">The user name</param>
        /// <param name="platform">The name of the web calling the api (optional)</param>
        /// <response code="500">Internal Server Error. Something went wrong in the web service. Returns an exception describing the error.</response>  
        /// <response code="401">Unauthorized. Wrong token or the token has expired.</response> 
        /// <response code="200">OK. Returns the code.</response>  
        [Authorize]
        [HttpPost]
        [Route("api/CodeGenerator/Auth/{userMail}/{user}")]
        [ResponseType(typeof(string))]
        public System.Web.Http.IHttpActionResult PostNewCodeAuth(string userMail, string user, string platform = null)
        {
            string code;
            IHttpActionResult httpResult = null;
            try
            {
                code = Utils.GenerateNewCode();
                Utils.MailMessage(userMail, code, user, platform);
                httpResult = Ok(code);
            }
            catch (Exception ex) { 
                httpResult = InternalServerError(ex);
            };

            return httpResult;
        }

        // http://<servidor>/api/CodeGenerator/mail
        /// <summary>
        /// Generates a random code in string format.
        /// </summary>
        /// <remarks>
        /// Generates a random code in string format, send it to the mail address passed by paramether and returns the code. 
        /// You don't need to request an acces token to use this operation.
        /// </remarks>
        /// <param name="userMail">The user mail address</param>
        /// <param name="user">The user name</param>
        /// <param name="platform">The name of the web calling the api (optional)</param>
        /// <response code="500">Internal Server Error. Something went wrong in the web service. Returns an exception describing the error.</response>  
        /// <response code="200">OK. Returns the code.</response>  
        [HttpPost]
        [AllowAnonymous]
        [Route("api/CodeGenerator/NoAuth/{userMail}/{user}")]
        [ResponseType(typeof(string))]
        public System.Web.Http.IHttpActionResult PostNewCodeNoAuth(string userMail, string user, string platform = null)
        {
            string code;
            IHttpActionResult httpResult = null;
            try
            {
                code = Utils.GenerateNewCode();
                Utils.MailMessage(userMail, code, user, platform);
                httpResult = Ok(code);
            }
            catch (Exception ex)
            {
                httpResult = InternalServerError(ex);
            };

            return httpResult;
        }
    }
    
}
