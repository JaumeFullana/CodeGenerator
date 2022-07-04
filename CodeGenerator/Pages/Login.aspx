<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CodeGenerator.Pages.Login" EnableEventValidation="false" Async="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TEST LOGIN</title>
    <link href="<%= ResolveUrl("~/css/styles.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <div class="block-login" style="width: 500px;">
                <div class="block-login-content">
                    <div class="login-header">
                        <h1>LOGIN</h1>
                    </div>
                    <div class="login-body">
                        <form id="signinForm" method="post" action="" novalidate="novalidate">
                        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
                        <asp:MultiView ID="MvLogin" runat="server">
                            <asp:View ID="sinUs" runat="server">
                                <div class="form-group">
                                    <label class="login-icones"><i class="fa fa-user"></i></label>
                                    <asp:TextBox ID="txt_correo" runat="server" name="correo" class="form-control" placeholder="Your email" value="" style="width:100%;"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="login-icones"><i class="fa fa-user"></i></label>
                                    <asp:TextBox ID="txt_Us" runat="server" name="login" class="form-control" placeholder="Your user" value="" style="width:100%;" OnTextChanged="txt_Us_TextChanged"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="login-icones"><i class="fa fa-lock"></i></label>
                                    <asp:TextBox ID="txt_Pass" runat="server" type="password" name="password" class="form-control" placeholder="Your password" value="" style="width:100%;" OnTextChanged="txt_Pass_TextChanged"></asp:TextBox>
                                </div>  
                                <div class="form-group" >
                                     <asp:Label ID="lblIncorrectoLogin" runat="server" Visible="false" Text="" style="color:red; font-weight:bold"/>
                                </div>
                            </asp:View>
                            <asp:View ID="conUs" runat="server">
                                <div class="profile" style="border:0px;padding: 20px 0px 20px 25px;">
                                <div class="profile-info" style="width:34.9%;">
                                    <a id="nombre" runat="server" href="#" class="profile-title"></a>
                                </div>
                                <asp:TextBox ID="txt_Pass2" runat="server" type="password" name="password" class="form-control" placeholder="Your password" value="" style="width:65%; background:none;"></asp:TextBox>
                            </div>
                            </asp:View>
                            <asp:View ID="doblAut" runat="server">
                                <div class="form-group">
                                    <h3 style="text-align : center">Insert the code sended to your email</h3>
                                    <br />
                                    <asp:TextBox ID="txt_Dobl_Code" runat="server" type="password" name="txt_Dobl_Code" class="form-control" placeholder="CODE" value="" style="width:100%;" OnTextChanged="txt_Pass_TextChanged"></asp:TextBox>
                                </div>  
                                 <div class="form-group" >
                                     <asp:Label ID="lblIncorrecto" runat="server" Visible="false" Text="" style="color:red; font-weight:bold"/>
                                </div>
                                 <div class="form-group" >
                                     <asp:Button ID="btn_Verficar" runat="server" CssClass="btn btn-primary btn-block" Text="Verify" OnClick="btn_Verficar_Click"/>
                                     <asp:Button ID="btn_Reenviar" runat="server" CssClass="btn btn-success btn-orange1 btn-block" Text="Resend code" OnClick="btn_Reenviar_Click"/>
                                </div>   
                            </asp:View>
                        </asp:MultiView>
                            <div style="clear:both"></div>
                            <asp:Button ID="btn_Login" runat="server" CssClass="btn btn-primary btn-block" Text="Login" onclick="btn_Login_Click"/>
                        </form>                    
                    </div>
                    <div class="login-footer">
                        <p>
                            © Jaume Fullana
                        </p>
                    </div>
                    </div>
                </div>
        </div>
    </form>
</!--body>
</html>