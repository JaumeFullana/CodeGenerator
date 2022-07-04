using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace CodeGenerator.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        private string platform = "Your Platform";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.MvLogin.ActiveViewIndex != 2)
            {
                if (!IsPostBack)
                {
                    this.MvLogin.ActiveViewIndex = 0;
                }
            }
        }

        protected async void btn_Login_Click(object sender, EventArgs e)
        {
            if (this.MvLogin.ActiveViewIndex == 0)
            {
                if ((this.txt_Us.Text == string.Empty) || (this.txt_Pass.Text == string.Empty))
                {
                    if (this.txt_Us.Text == string.Empty)
                    {
                        this.txt_Us.BackColor = ColorTranslator.FromHtml("#FFBFBF");
                    }
                    if (this.txt_Pass.Text == string.Empty)
                    {
                        this.txt_Pass.BackColor = ColorTranslator.FromHtml("#FFBFBF");
                    }
                }
                else
                {
                    string mensaje = await this.SignIn();
                    if (!string.IsNullOrEmpty(mensaje))
                    {
                        this.lblIncorrectoLogin.Text = mensaje;
                        this.lblIncorrectoLogin.Visible = true;
                    }
                }
            }
            else
            {
                string mensaje = await this.SignIn();
                if (!string.IsNullOrEmpty(mensaje))
                {
                    this.lblIncorrectoLogin.Text = mensaje;
                    this.lblIncorrectoLogin.Visible = true;
                }
            }
        }

        private async Task<string> SignIn() {
            string mensaje = "";
            //here you have to check the credentials of the user on your own way (usually consulting a DDBB).
            if (this.txt_Us.Text == "user" && this.txt_Pass.Text == "user")
            {
                try
                {
                    this.Session["Email"] = this.txt_correo.Text;
                    this.Session["User"] = this.txt_Us.Text;

                    //Checking if we alredy have a token saved.
                    if (Session["AutToken"] == null)
                    {
                        await this.RequestApiToken(this.txt_Us.Text, this.txt_Pass.Text);
                    }
                    else 
                    {
                        //Checking if the token saved is still usable.
                        TimeSpan tiempoPasado = DateTime.Now - (DateTime)this.Session["TimeAutTokenReceived"];
                        if (tiempoPasado.Seconds > 86400)
                        {
                            await this.RequestApiToken(this.txt_Us.Text, this.txt_Pass.Text);
                        }
                    }

                    mensaje = await this.GenerateCode();

                }
                catch (Exception ex)
                {
                    mensaje = "An error has occurred, try again later";
                    string error = ex.Message;
                }
            }
            else {
                mensaje = "The user or password are wrong";
            }

            return mensaje;
        }

        //Connecting the API with your credentials to genrate an authentication token.
        private async Task RequestApiToken(string user, string password)
        {
            List<KeyValuePair<string, string>> credenciales = new List<KeyValuePair<string, string>>();
            credenciales.Add(new KeyValuePair<string, string>("username", user));
            credenciales.Add(new KeyValuePair<string, string>("password", password));
            credenciales.Add(new KeyValuePair<string, string>("grant_type", "password"));

            using (var client = new HttpClient())
            {

                FormUrlEncodedContent contenido = new FormUrlEncodedContent(credenciales);
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                HttpResponseMessage respuesta = await client.PostAsync(baseUrl + "/token", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = await respuesta.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(resultado);
                    this.Session["AutToken"] = json.access_token;
                    this.Session["TimeAutTokenReceived"] = DateTime.Now.AddSeconds(-180);
                }
            }
        }

        //Connecting the API to generate and send via mail a new code using the previously generated authentication token.
        private async Task<string> GenerateCode()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(Session["AutToken"].ToString()))
            {
                mensaje = "An error has occurred on the server side, please try again later";
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["AutToken"].ToString());
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    HttpResponseMessage respuesta = await client.PostAsync(baseUrl + "/api/CodeGenerator/Auth/" + this.Session["Email"] + "/" + this.Session["User"] + "?platform=" + platform, null);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        this.MvLogin.ActiveViewIndex = 2;

                        string resultado = await respuesta.Content.ReadAsStringAsync();

                        string code = resultado.Replace("/", "").Replace("\"", "");
                        this.Session["DobleAutCode"] = code;
                        this.Session["TimeCodeReceived"] = DateTime.Now;
                        this.Session["DobleAutAttempts"] = 0;
                        this.btn_Login.Visible = false;
                    }
                    else
                    {
                        mensaje = "An error has occurred on the server side, please try again later";
                    }
                }
            }
            return mensaje;
        }

        //Check if the inserted code and the code recived from the API are the same.
        protected void btn_Verficar_Click(object sender, EventArgs e)
        {
            DateTime horaCodigoRecibido = (DateTime)this.Session["TimeCodeReceived"];
            TimeSpan tiempoPasado = DateTime.Now - horaCodigoRecibido;

            if (tiempoPasado.Minutes < 5 && Convert.ToInt16(this.Session["DobleAutAttempts"]) < 3)
            {
                if (String.Equals(this.txt_Dobl_Code.Text, this.Session["DobleAutCode"]))
                {
                    Response.Redirect("~/Pages/Default.aspx");
                }
                else
                {
                    int intentosActuales = Convert.ToInt16(this.Session["DobleAutAttempts"]) + 1;

                    if (intentosActuales < 3)
                    {
                        this.lblIncorrecto.Text = "Wrong code";
                        this.lblIncorrecto.Visible = true;
                    }
                    else
                    {
                        this.lblIncorrecto.Text = "The code has expired, request a new code";
                        this.lblIncorrecto.Visible = true;
                    }

                    this.Session["DobleAutAttempts"] = intentosActuales;
                }
            }
            else
            {
                this.lblIncorrecto.Text = "The code has expired, request a new code";
                this.lblIncorrecto.Visible = true;
            }

            this.txt_Dobl_Code.Text = "";
        }

        //Ask for a new code.
        protected async void btn_Reenviar_Click(object sender, EventArgs e)
        {
            string mensaje;
            try
            {
                if (Session["AutToken"] == null)
                {
                    await this.RequestApiToken(this.txt_Us.Text, this.txt_Pass.Text);
                }
                else
                {
                    TimeSpan tiempoPasado = DateTime.Now - (DateTime)this.Session["TimeAutTokenReceived"];
                    if (tiempoPasado.Seconds > 86400)
                    {
                        await this.RequestApiToken(this.txt_Us.Text, this.txt_Pass.Text);
                    }
                }

                mensaje = await this.GenerateCode();

            }
            catch (Exception ex)
            {
                mensaje = "An error has occurred, try again later";
                string error = ex.Message;
            }

            if (!string.IsNullOrEmpty(mensaje))
            {
                this.MvLogin.ActiveViewIndex = 0;
                this.lblIncorrectoLogin.Text = mensaje;
                this.lblIncorrectoLogin.Visible = true;
            }
            else 
            {
                this.lblIncorrecto.Visible = false;
            }
        }

        protected void btn_Otra_Click(object sender, EventArgs e)
        {
            this.MvLogin.ActiveViewIndex = 0;
        }

        protected void txt_Pass_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txt_Us_TextChanged(object sender, EventArgs e)
        {

        }
    }
}