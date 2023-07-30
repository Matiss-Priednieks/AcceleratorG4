using Godot;
using System;

public partial class Menu : Panel
{
    LoggedInUser User;
    Button RegisterButton;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        User = GetNode<LoggedInUser>("/root/LoggedInUser");
        RegisterButton = GetNode<Button>("%Signup");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (User.LoggedIn)
        {
            RegisterButton.Disabled = true;
        }
        else
        {
            RegisterButton.Disabled = false;
        }
    }
}
