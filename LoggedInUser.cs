using Godot;
using System;

public partial class LoggedInUser : Node
{
    // Called when the node enters the scene tree for the first time.
    Label UsernameLabel;
    public override void _Ready()
    {
        UsernameLabel = GetNode<Label>("../Node3D/CanvasLayer/UIContainer/UsernamePanel/MarginContainer/Username");
        UsernameLabel.Text = "Guest";
    }

    public void SetUsername(string username)
    {
        UsernameLabel.Text = username;
    }
}
