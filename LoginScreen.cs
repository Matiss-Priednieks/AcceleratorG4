using Godot;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
public partial class LoginScreen : Panel
{
    // Called when the node enters the scene tree for the first time.
    private string LoginEmail, LoginPassword;
    LineEdit EmailInput, PasswordInput;
    HttpRequest HTTPRequest;

    LoggedInUser User;
    Panel ErrorPanel;
    Label ErrorMessage;

    public override void _Ready()
    {
        EmailInput = GetNode<LineEdit>("%EmailInput");
        PasswordInput = GetNode<LineEdit>("%PasswordInput");
        HTTPRequest = GetNode<HttpRequest>("%LoginRequest");
        ErrorPanel = GetNode<Panel>("%ErrorPanel");
        ErrorMessage = GetNode<Label>("%ErrorMessage");
        User = GetNode<LoggedInUser>("/root/LoggedInUser");

        ErrorPanel.Hide();
    }

    // public void _on_login_pressed()
    // {
    //     MoveThisElseWhere();
    // }

    public void _on_email_text_changed(string newText)
    {
        LoginEmail = newText;
        ErrorPanel.Hide();

    }
    public void _on_email_text_submitted(string newText)
    {
        LoginEmail = newText;
        PasswordInput.GrabFocus();
    }

    public void _on_password_text_changed(string newText)
    {
        LoginPassword = newText;
        ErrorPanel.Hide();
    }
    public void _on_password_text_submitted(string newText)
    {
        LoginPassword = newText;
        LoginRequest();
    }

    // private Error Login()
    // {
    //     var JSONObject = new Json();
    //     UserRegCreditentials newReg = new UserRegCreditentials(LoginEmail, LoginPassword, true);
    //     string JsonString = JsonSerializer.Serialize(newReg);
    //     string[] headers = new string[] { "Content-Type: application/json" };
    //     var error = HTTPRequest.Request(SignInURL, headers, HttpClient.Method.Post, JsonString);
    //     return error;
    // }

    public void _on_login_confirm_pressed()
    {
        LoginRequest();
    }
    public void _on_login_request_request_completed(long result, long responseCode, string[] headers, byte[] body)
    {
        var response = Json.ParseString(body.GetStringFromUtf8());

        GD.Print(responseCode);
        if (responseCode == 200)
        {
            ErrorPanel.Hide();
            // GD.Print(body);
            var dict = (Godot.Collections.Dictionary)response;
            User.SetUsername(dict[key: "username"].ToString());

        }
        else
        {
            ErrorMessage.Text = "Invalid Login";
            ErrorPanel.Show();
            // GD.Print(response);
        }
    }
    public Error LoginRequest() //this is just to save the function so I don't forget. This should return whatever username is associated with the email. 
    {
        string[] newRegHeaders = new string[] { "Content-Type: application/json" };
        UserRegCreditentials LoginCredentials = new UserRegCreditentials(LoginEmail, LoginPassword, true);
        string JsonString = JsonSerializer.Serialize(LoginCredentials);
        var error = HTTPRequest.Request("http://20.58.57.165/get-user/login", newRegHeaders, HttpClient.Method.Post, JsonString);
        GD.Print(error);
        return error;
    }
}
