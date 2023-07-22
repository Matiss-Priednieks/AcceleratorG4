using Godot;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
public partial class LoginScreen : Panel
{
    // Called when the node enters the scene tree for the first time.
    private string LoginEmail, LoginPassword;
    const string APIKey = "AIzaSyBhb4xMuQsCdPLAmxzDeYCumFfdVfMVwqQ";
    string SignInURL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + APIKey;
    LineEdit EmailInput, PasswordInput;
    HttpRequest HTTPRequest;
    public override void _Ready()
    {
        EmailInput = GetNode<LineEdit>("%EmailInput");
        PasswordInput = GetNode<LineEdit>("%PasswordInput");
        HTTPRequest = GetNode<HttpRequest>("%HTTPRequest");
    }

    public void _on_login_pressed()
    {
        Login();
    }

    public void _on_email_text_changed(string newText)
    {
        LoginEmail = newText;
    }
    public void _on_email_text_submitted(string newText)
    {
        LoginEmail = newText;
        PasswordInput.GrabFocus();
    }

    public void _on_password_text_changed(string newText)
    {
        LoginPassword = newText;
    }
    public void _on_password_text_submitted(string newText)
    {
        LoginPassword = newText;
        Login();
    }

    private void Login()
    {
        var JSONObject = new Json();
        UserRegCreditentials newReg = new UserRegCreditentials(LoginEmail, LoginPassword, true);
        string JsonString = JsonSerializer.Serialize(newReg);
        string[] headers = new string[] { "Content-Type: application/json" };
        var error = HTTPRequest.Request(SignInURL, headers, HttpClient.Method.Post, JsonString);
        GD.Print(JsonString);
    }
    public void _on_http_request_request_completed(long result, long responseCode, string[] headers, byte[] body)
    {
        var response = Json.ParseString(body.GetStringFromUtf8());
        if (responseCode == 200)
        {
            GD.Print(response);
        }
        else
        {
            GD.Print(response);
        }
    }
}
