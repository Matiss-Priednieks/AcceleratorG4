using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
// using FirebaseAdmin;
// using Google.Cloud.Firestore;
// using Google.Apis.Auth.OAuth2;

public partial class RegistrationScreen : Panel
{
    // Called when the node enters the scene tree for the first time.
    private string Username, RegistrationEmail, RegistrationPassword, RegistrationPasswordConfirmation;
    LineEdit NameInput, EmailInput, PasswordInput, ConfirmPasswordInput, LoginEmailInput, LoginPass;
    HttpRequest HTTPRequest, HTTPLoginRequest;

    Panel ErrorPanel, LoginScreen;
    Label ErrorMessage;
    Button Logout, Login, RegisterConfirm;
    Label UserLabel;
    LoggedInUser User;

    Gameplay GameplayNode; //Refactor this so parent node isn't necessary!!!

    public override void _Ready()
    {
        NameInput = GetNode<LineEdit>("%NameInput");
        EmailInput = GetNode<LineEdit>("%EmailInput");
        PasswordInput = GetNode<LineEdit>("%PasswordInput");
        ConfirmPasswordInput = GetNode<LineEdit>("%ConfirmPassword");
        HTTPRequest = GetNode<HttpRequest>("%RegRequest");
        HTTPLoginRequest = GetNode<HttpRequest>("%LoginRequest");
        LoginScreen = GetNode<Panel>("%LoginScreen");
        ErrorPanel = GetNode<Panel>("%ErrorPanel");
        ErrorMessage = GetNode<Label>("%ErrorMessage");

        User = GetNode<LoggedInUser>("/root/LoggedInUser");

        LoginEmailInput = GetNode<LineEdit>("%Email");
        LoginPass = GetNode<LineEdit>("%Password");

        Login = GetNode<Button>("%LoginConfirm");
        RegisterConfirm = GetNode<Button>("%RegisterConfirm");
        GameplayNode = (Gameplay)GetParent().GetParent().GetParent();
    }


    public void _on_name_input_text_changed(string newText)
    {
        if (IsValidUsername(newText))
        {
            Username = newText;
            ErrorPanel.Hide();
        }
        else
        {
            ErrorMessage.Text = "Username cannot containt special characters or be empty";
            ErrorPanel.Show();
        }
    }
    public void _on_name_input_text_submitted(string newText)
    {
        if (IsValidUsername(newText))
        {
            Username = newText;
            ErrorPanel.Hide();
        }
        else
        {
            ErrorMessage.Text = "Username cannot containt special characters or be empty";
            ErrorPanel.Show();
        }
    }


    public void _on_register_confirm_pressed()
    {
        //Confirm registration button
        if (IsValidRegistration())
        {
            //create registration
            RegisterConfirm.Disabled = true;
            CreateRegistration();
        }
    }
    public void _on_password_text_submitted(string newText)
    {
        RegistrationPassword = newText;
        if (IsValidRegistration())
        {
            //create registration
            CreateRegistration();
        }
        else
        {
            ConfirmPasswordInput.GrabFocus();
        }
    }
    public void _on_confirm_password_text_submitted(string newText)
    {
        RegistrationPasswordConfirmation = newText;
        //Confirm registration button
        if (IsValidRegistration())
        {
            CreateRegistration();
            //create registration
        }
        ConfirmPasswordInput.ReleaseFocus();
    }


    public void _on_email_text_changed(string newText)
    {
        RegistrationEmail = newText;
        ErrorPanel.Hide();
    }
    public void _on_email_text_submitted(string newText)
    {
        RegistrationEmail = newText;
        if (IsValidRegistration())
        {
            CreateRegistration();
            //create registration
        }
        ConfirmPasswordInput.GrabFocus();
    }

    public void _on_password_text_changed(string newText)
    {
        RegistrationPassword = newText;
        ErrorPanel.Hide();
    }
    public void _on_confirm_password_text_changed(string newText)
    {
        RegistrationPasswordConfirmation = newText;
        ErrorPanel.Hide();
    }
    static bool IsValidEmail(string email)
    {
        // Regular expression pattern for email validation
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }
    static bool IsStrongPassword(string password)
    {
        // Minimum 6 characters, at least one uppercase letter, one lowercase letter, and one digit
        string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$";

        return Regex.IsMatch(password, pattern);
    }

    public bool IsValidRegistration()
    {
        GD.Print(IsValidPassword());
        if (IsValidEmail() && IsValidPassword() && IsValidUsername(Username))
        {
            GD.Print("Valid registration!");
            return true;
        }
        else
        {
            GD.Print("Invalid registration!");
            //do code for error message
            return false;
        }
    }

    public bool IsValidPassword()
    {
        if (RegistrationPasswordConfirmation.Equals(RegistrationPassword))
        {
            GD.Print("Passwords match");
            if (IsStrongPassword(RegistrationPasswordConfirmation))
            {
                return true;
            }
            else
            {
                ErrorMessage.Text = "Password min length is 6 and must contain a digit";
                ErrorPanel.Show();
                GD.Print("Password is too weak!");
                return false;
            }
        }
        else
        {
            ErrorMessage.Text = "Passwords Must Match";
            ErrorPanel.Show();
            GD.Print("Passwords do not match!");
            return false;
        }
    }
    private bool IsValidEmail()
    {
        if (IsValidEmail(EmailInput.Text))
        {
            GD.Print("Valid Email!");
            return true;
        }
        else
        {
            ErrorMessage.Text = "Invalid Email";
            ErrorPanel.Show();
            GD.Print("Not Valid Email!");
            return false;
        }
    }
    static bool IsValidUsername(string username)
    {
        // Alphanumeric characters, underscores, hyphens, and letters from any language, max length 256
        string pattern = "^[\\p{L}a-zA-Z0-9_-]{1,256}$";

        return Regex.IsMatch(username, pattern);
    }
    private void CreateRegistration()
    {
        CallDeferred("NewRegRequest");
    }
    public async void _on_reg_request_request_completed(long result, long responseCode, string[] headers, byte[] body)
    {
        var response = Json.ParseString(body.GetStringFromUtf8());
        var dict = (Godot.Collections.Dictionary)response;

        if (responseCode == 200 && (int)dict[key: "status_code"] != 400)
        {
            ErrorPanel.Hide();
            RegisterConfirm.Disabled = false;
            GameplayNode.Register = false;
            GameplayNode.Login = true;
            await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
            CallDeferred("UserDataRequest");
            await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
            CallDeferred("LoginRequest");
        }
        else
        {
            // if (responseCode == 201)
            // {
            //     ErrorPanel.Show();
            //     ErrorMessage.Text = "User already exists!";
            // }
            if (dict.Count != 0)
            {
                if ((int)dict[key: "status_code"] == 400)
                {
                    ErrorPanel.Show();
                    ErrorMessage.Text = "User already exists!";
                }
            }
            RegisterConfirm.Disabled = false;
            GameplayNode.Login = false;
            GameplayNode.Register = true;
        }
    }

    public void UserDataRequest()
    {
        UserCreditentials userData = new(Username, RegistrationEmail, User.GetHighscore());
        string userDataJson = JsonSerializer.Serialize(userData);
        string[] newRegHeaders = new string[] { "Content-Type: application/json" };
        var error = HTTPRequest.Request("https://forwardvector.uksouth.cloudapp.azure.com/save-user", newRegHeaders, HttpClient.Method.Post, userDataJson);
    }

    public void NewRegRequest()
    {
        UserRegCreditentials newReg = new(RegistrationEmail, RegistrationPasswordConfirmation, true);
        string newRegBody = JsonSerializer.Serialize(newReg);
        string[] newRegHeaders = new string[] { "Content-Type: application/json" };
        var error = HTTPRequest.Request("https://forwardvector.uksouth.cloudapp.azure.com/create-user", newRegHeaders, HttpClient.Method.Post, newRegBody);
    }
    public Error LoginRequest()
    {
        Login.Disabled = true;
        LoginEmailInput.Editable = false;
        LoginPass.Editable = false;
        if (User.GetUsername() == "Guest")
        {
            string[] newRegHeaders = new string[] { "Content-Type: application/json" };
            UserRegCreditentials LoginCredentials = new(RegistrationEmail, RegistrationPasswordConfirmation, true);
            string JsonString = JsonSerializer.Serialize(LoginCredentials);
            var error = HTTPLoginRequest.Request("https://forwardvector.uksouth.cloudapp.azure.com/get-user/login", newRegHeaders, HttpClient.Method.Post, JsonString);
            return error;
        }
        else
        {
            GD.Print("Already Logged in");
            UserLabel.Text = "Guest";
            User.Logout();
            return Error.Ok;
        }
    }
}
