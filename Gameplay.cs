using Godot;
using System;

public partial class Gameplay : Node3D
{
    // Called when the node enters the scene tree for the first time.
    public bool Menu = true;
    public bool Settings = false;
    public bool Playing = false;
    public bool Login = false;
    public bool Register = false;
    private string RegistrationEmail;
    private string RegistrationPassword;
    private string RegistrationPasswordConfirmation;


    Panel GameMenu;
    Panel SettingsMenu;
    Panel LoginScreen;
    Panel Registration;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Confined;
        GameMenu = GetNode<Panel>("%Menu");
        SettingsMenu = GetNode<Panel>("%SettingsMenu");
        LoginScreen = GetNode<Panel>("%LoginScreen");
        Registration = GetNode<Panel>("%Registration");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //Menu
        if (Menu)
        {
            GameMenu.Show();
            Input.MouseMode = Input.MouseModeEnum.Confined;
        }
        else
        {
            GameMenu.Hide();
        }
        //Settings
        if (Settings)
        {
            SettingsMenu.Show();
            Input.MouseMode = Input.MouseModeEnum.Confined;
        }
        else
        {
            SettingsMenu.Hide();
        }
        //Login
        if (Login)
        {
            LoginScreen.Show();
            Input.MouseMode = Input.MouseModeEnum.Confined;
        }
        else
        {
            LoginScreen.Hide();
        }
        //Registration
        if (Register)
        {
            Registration.Show();
            Input.MouseMode = Input.MouseModeEnum.Confined;
        }
        else
        {
            Registration.Hide();
        }
        //Playing
        if (Playing)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            GameMenu.Hide();
            SettingsMenu.Hide();
            LoginScreen.Hide();
            Registration.Hide();
        }
    }

    //Main Menu buttons start
    public void _on_play_pressed()
    {
        Playing = true;
        Menu = false;
        Login = false;
        Register = false;
        Settings = false;
    }
    public void _on_settings_pressed()
    {
        Playing = false;
        Menu = false;
        Login = false;
        Register = false;
        Settings = true;
    }
    public void _on_login_pressed()
    {
        Playing = false;
        Menu = false;
        Login = true;
        Register = false;
        Settings = false;
    }
    public void _on_signup_pressed()
    {
        Playing = false;
        Menu = false;
        Login = false;
        Register = true;
        Settings = false;
    }
    public void _on_exit_pressed()
    {
        //exit game code here, todo
    }
    //Main Menu buttons end


    //Registration buttons start
    public void _on_existing_pressed()
    {
        Playing = false;
        Menu = false;
        Login = true;
        Register = false;
        Settings = false;
    }
    public void _on_register_confirm_pressed()
    {
        //Confirm registration button
    }
    public void _on_confirm_password_text_submitted()
    {
        //Confirm registration button
    }

    public void _on_email_text_changed()
    {
        RegistrationEmail = GetNode<LineEdit>("%Email").Text;
    }
    public void _on_password_text_changed()
    {
        RegistrationPassword = GetNode<LineEdit>("%Password").Text;
    }
    public void _on_confirm_password_text_changed()
    {
        RegistrationPasswordConfirmation = GetNode<LineEdit>("%ConfirmPassword").Text;
    }
}
