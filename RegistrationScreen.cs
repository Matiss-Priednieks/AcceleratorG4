using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using FirebaseAdmin;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;

public partial class RegistrationScreen : Panel
{
    // Called when the node enters the scene tree for the first time.
    private string Username, RegistrationEmail, RegistrationPassword, RegistrationPasswordConfirmation;
    const string APIKey = "AIzaSyBhb4xMuQsCdPLAmxzDeYCumFfdVfMVwqQ";
    string SignUpURL = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + APIKey;
    string UserDataBase = "firebaseio.com/users.json";
    const string PROJECT_ID = "accelerator-630fc";
    const string HTTPSExtension = "https://" + PROJECT_ID;
    LineEdit NameInput, EmailInput, PasswordInput, ConfirmPasswordInput;
    HttpRequest HTTPRequest;
    public override void _Ready()
    {
        NameInput = GetNode<LineEdit>("%NameInput");
        EmailInput = GetNode<LineEdit>("%EmailInput");
        PasswordInput = GetNode<LineEdit>("%PasswordInput");
        ConfirmPasswordInput = GetNode<LineEdit>("%ConfirmPassword");
        HTTPRequest = GetNode<HttpRequest>("%HTTPRequest");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    public void _on_name_input_text_changed(string newText)
    {
        Username = newText;
    }
    public void _on_name_input_text_submitted(string newText)
    {
        Username = newText;
    }


    public void _on_register_confirm_pressed()
    {
        //Confirm registration button
        if (ValidRegistration())
        {
            //create registration
            CreateRegistration();
        }
    }
    public void _on_password_text_submitted(string newText)
    {
        RegistrationPassword = newText;
        if (ValidRegistration())
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
        if (ValidRegistration())
        {
            CreateRegistration();
            //create registration
        }
        ConfirmPasswordInput.ReleaseFocus();

    }
    public async void test()
    {
        string path = @"acceleratorcreds.json";


        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
        FirestoreDb db = FirestoreDb.Create(PROJECT_ID);

        CollectionReference usersCollection = db.Collection("users");
        UserCreditentials userData = new UserCreditentials(Username, RegistrationEmail);

        // Serialize the userData object to a JSON string
        string userDataJson = JsonSerializer.Serialize(userData);

        // Deserialize the JSON string to a dictionary
        Dictionary<string, string> userDataDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(userDataJson);

        // Add the dictionary data to Firestore using AddAsync
        DocumentReference addedDocRef = await usersCollection.AddAsync(userDataDictionary);
        Console.WriteLine("Document written with ID: " + addedDocRef.Id);

        // Read data from Firestore
        QuerySnapshot querySnapshot = usersCollection.GetSnapshotAsync().Result;
        foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
        {
            Console.WriteLine(documentSnapshot.Id + " => " + documentSnapshot.ToDictionary());
        }

    }

    public void _on_email_text_changed(string newText)
    {
        RegistrationEmail = newText;
    }
    public void _on_email_text_submitted(string newText)
    {
        RegistrationEmail = newText;
        ValidRegistration();
        ConfirmPasswordInput.GrabFocus();
    }

    public void _on_password_text_changed(string newText)
    {
        RegistrationPassword = newText;
    }
    public void _on_confirm_password_text_changed(string newText)
    {
        RegistrationPasswordConfirmation = newText;
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

    public bool ValidRegistration()
    {
        GD.Print(ValidPassword());
        if (ValidEmail() && ValidPassword())
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

    public bool ValidPassword()
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
                GD.Print("Password is too weak!");
                return false;
            }
        }
        else
        {
            GD.Print("Passwords do not match!");
            return false;
        }
    }
    private bool ValidEmail()
    {
        if (IsValidEmail(EmailInput.Text))
        {
            GD.Print("Valid Email!");
            return true;
        }
        else
        {
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
    private async void CreateRegistration()
    {
        CallDeferred("NewRegRequest");
        await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
        CallDeferred("UserDataRequest");
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
    public void UserDataRequest()
    {
        // UserCreditentials userData = new UserCreditentials(Username, RegistrationEmail);
        // string userDataBody = JsonSerializer.Serialize(userData);
        // string[] userDataHeaders = new string[] { "Content-Type: application/json" };
        // var usersaveerror = HTTPRequest.Request(HTTPSExtension + UserDataBase, userDataHeaders, HttpClient.Method.Post, userDataBody);
        // GD.Print(usersaveerror);

        test();

    }
    public void NewRegRequest()
    {
        UserRegCreditentials newReg = new UserRegCreditentials(RegistrationEmail, RegistrationPasswordConfirmation, true);
        string newRegBody = JsonSerializer.Serialize(newReg);
        string[] newRegHeaders = new string[] { "Content-Type: application/json" };
        var error = HTTPRequest.Request(SignUpURL, newRegHeaders, HttpClient.Method.Post, newRegBody);
    }
}
