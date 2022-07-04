using System;
using System.Net.Mail;

namespace CodeGenerator.Logic
{
    public class Utils
    {
        /// <summary>
        /// Method MailMessage sends a email to the the user email address with a 6 numbers code in the emails body.
        /// </summary>
        /// <param name="userEmailAddress">the user email address</param>
        /// <param name="code">the 6 numbers code</param>
        /// <param name="user">The user name</param>
        /// <param name="platform">The name of the web calling the api (optional)</param>
        public static void MailMessage(string userEmailAddress, string code, string user, string platform)
        {
            //Here you have to use your email and your email's password
            string senderEmailAddress = "yourEmail@emailProvider.com";
            string senderEmailPassword = "yourPassword";

            MailMessage mailMessage = new MailMessage(senderEmailAddress, userEmailAddress);
            mailMessage.Subject = "Login Code";

            string cuerpo = "Hello " + user + ", this is your login code: " + code;
            if (!string.IsNullOrEmpty(platform)) {
                cuerpo += "\n\nGreetings from " + platform;
            }
            mailMessage.Body = cuerpo;

            //587 is the port used by the google's smtp to send emails, don't change it.
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential(senderEmailAddress, senderEmailPassword);
            smtpClient.EnableSsl = true;

            //If you use a google account you have to create a password for this application.
            smtpClient.Send(mailMessage);
        }

        /// <summary>
        /// Method GenerateNewCode generates a 6 numbers code in string format and returns it.
        /// </summary>
        /// <returns>The generated 6 numbers code in string format</returns>
        public static string GenerateNewCode()
        {
            string code = "";
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                code += random.Next(0, 10);
            }

            return code;
        }

    }
}