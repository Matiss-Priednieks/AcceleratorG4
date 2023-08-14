using Godot;
using System;

public partial class LoggedInUser : Node
{
    // Called when the node enters the scene tree for the first time.
    Label UsernameLabel;

    public bool LoggedIn { get; set; }

    public float UserHighScore { get; private set; }
    public override void _Ready()
    {
        UsernameLabel = GetNode<Label>("../Node3D/CanvasLayer/UIContainer/UsernamePanel/MarginContainer/Username");
        UsernameLabel.Text = "Guest";
        LoggedIn = false;
    }

    public void SetUsername(string username)
    {
        UsernameLabel.Text = username;
    }
    public string GetUsername()
    {
        return UsernameLabel.Text;
    }

    public void Logout()
    {
        UsernameLabel.Text = "Guest";
        LoggedIn = false;
    }

    public void Login(string username)
    {
        SetUsername(username);
        LoggedIn = true;
    }

    public void SetHighscore(float value)
    {
        if (value > UserHighScore) UserHighScore = value;
    }
    public float GetHighscore()
    {
        return UserHighScore;
    }

    //TODO
    // public Error HighscoreUpdateRequest()
    // {
    //     UserCreditentials userData = new(User.GetHighscore());
    //     string userDataJson = JsonSerializer.Serialize(userData);
    //     string[] newRegHeaders = new string[] { "Content-Type: application/json" };
    //     var error = HTTPRequest.Request("https://forwardvector.uksouth.cloudapp.azure.com/save-user", newRegHeaders, HttpClient.Method.Post, userDataJson);
    // }
}
