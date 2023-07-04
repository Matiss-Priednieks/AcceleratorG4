using Godot;
using System;
using System.Threading;


public partial class Gameplay : Node3D
{
    // Called when the node enters the scene tree for the first time.
    public bool Menu = true;
    public bool Settings = false;
    public bool Playing = false;
    public bool GameOver = false;
    public bool Login = false;
    public bool Register = false;
    private string RegistrationEmail;
    private string RegistrationPassword;
    private string RegistrationPasswordConfirmation;

    Player PlayerObj;
    SegmentSpawner SegmentSpawnerObj;
    float Sensitivity = 0.01f;


    Panel GameMenu, SettingsMenu, LoginScreen, Registration, GameOverPanel;
    TextureRect Crosshair;
    public override void _Ready()
    {
        PlayerObj = GetNode<Player>("%Player");
        SegmentSpawnerObj = GetNode<SegmentSpawner>("%SegmentSpawner");
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GameMenu = GetNode<Panel>("%Menu");
        SettingsMenu = GetNode<Panel>("%SettingsMenu");
        LoginScreen = GetNode<Panel>("%LoginScreen");
        Registration = GetNode<Panel>("%Registration");
        Crosshair = GetNode<TextureRect>("%Crosshair");
        GameOverPanel = GetNode<Panel>("%GameOver");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //Menu
        if (Menu)
        {
            GameMenu.Show();
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            GameMenu.Hide();
        }
        //Settings
        if (Settings)
        {
            SettingsMenu.Show();
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            SettingsMenu.Hide();
        }
        //Login
        if (Login)
        {
            LoginScreen.Show();
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            LoginScreen.Hide();
        }
        //Registration
        if (Register)
        {
            Registration.Show();
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            Registration.Hide();
        }
        //Playing
        if (Playing)
        {
            Crosshair.Show();
            Input.MouseMode = Input.MouseModeEnum.Captured;
            GameMenu.Hide();
            SettingsMenu.Hide();
            LoginScreen.Hide();
            Registration.Hide();
            GameOverPanel.Hide();
            if (Input.IsActionJustReleased("Restart"))
            {
                Restart();
            }
        }
        else
        {
            Crosshair.Hide();
            if (GameOver)
            {
                GameOverPanel.Show();
                if (Input.IsActionJustReleased("Restart"))
                {
                    Playing = true;
                    Menu = false;
                    Login = false;
                    Register = false;
                    Settings = false;
                    GameOverPanel.Hide();
                    Restart();
                }
            }
            if (GameOver && Input.IsActionJustReleased("MainMenu"))
            {
                Menu = true;
                Playing = false;
                GameMenu.Show();
                GameOverPanel.Hide();
            }
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
        if (GameOver)
        {
            Restart();
        }
        else
        {
            GameOver = false;
        }
    }
    public void Restart()
    {
        //TODO: fix restart issue, restart after exiting first segment causes next segments to disappear.
        SegmentSpawnerObj.Restart();
        PlayerObj.Restart();
        GameOver = false;
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
        GetTree().Quit();
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

    public void _on_settingsback_pressed()
    {
        Playing = false;
        Menu = true;
        Login = false;
        Register = false;
        Settings = false;
    }
    public void _on_sensitivity_button_pressed()
    {
        GetNode<VBoxContainer>("%SensitivityMenu").Show();
        GetNode<VBoxContainer>("%ButtonBox").Hide();
    }
    public void _on_sens_slider_value_changed(float newSens)
    {
        Sensitivity = newSens;
        GetNode<SpinBox>("%SensInput").Value = newSens;
        PlayerObj.MouseSensitivity = Sensitivity;
    }

    public void _on_sens_text_input_focus_exited(float newSens)
    {
        Sensitivity = newSens;
        GetNode<Slider>("%SensSlider").Value = newSens;
        PlayerObj.MouseSensitivity = Sensitivity;
    }
    public void _on_sens_back_pressed()
    {
        PlayerObj.MouseSensitivity = Sensitivity;
        Playing = false;
        Menu = false;
        Login = false;
        Register = false;
        Settings = true;
        GetNode<VBoxContainer>("%ButtonBox").Show();
        GetNode<VBoxContainer>("%SensitivityMenu").Hide();


    }
}
