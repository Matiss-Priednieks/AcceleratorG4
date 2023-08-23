using Godot;
using System;

public partial class Player : CharacterBody3D
{

    [Export(PropertyHint.Range, "0.01,5,")] public float MouseSensitivity = 0.1f;
    public float Speed = 10;
    Gameplay GameRef;
    float Highscore;
    LoggedInUser User;

    public bool RequestNotSent { get; private set; }


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GameRef = GetParent<Gameplay>();
        User = GetNode<LoggedInUser>("/root/LoggedInUser");

    }

    public override void _Process(double delta)
    {
        var displaySpeed = (int)Speed;
        if (Speed > Highscore)
        {
            Highscore = Speed;
            User.SetHighscore(Highscore);
            if (GameRef.GameOver && RequestNotSent)
            {
                Error requestResult = User.HighscoreUpdateRequest();
                if (requestResult is Error.Ok)
                {
                    GD.Print("Score sent to server.");
                    RequestNotSent = true;
                }
            }
        }
        GetNode<Panel>("%Score").GetNode<Label>("Label").Text = displaySpeed.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (!GameRef.GameOver && GameRef.Playing)
        {
            var collision = MoveAndCollide(Transform.Basis.Z * (Speed * (float)delta) * -1);
            if (Time.GetTicksUsec() % 1000 > 900)
            {
                Speed += 0.1f;
            }
            if (collision != null)
            {
                GetNode<AudioStreamPlayer3D>("%Crash").Play();
                if (Speed > Highscore)
                {
                    Highscore = Speed;
                    User.SetHighscore(Highscore);
                }
                GameRef.GameOver = true;
                GameRef.Playing = false;
            }
        }



    }
    public override void _Input(InputEvent inputEvent)
    {
        Aim(inputEvent);
    }

    public void Aim(InputEvent inputEvent)
    {
        if (GameRef.Playing)
        {
            InputEventMouseMotion mouseMotion = inputEvent as InputEventMouseMotion;
            if (mouseMotion != null)
            {
                if (Input.MouseMode == Input.MouseModeEnum.Captured)
                {

                    Vector3 currentPitch = RotationDegrees;
                    currentPitch.Y -= (mouseMotion.Relative.X * MouseSensitivity);

                    RotationDegrees = currentPitch;


                    Vector3 currentTilt = RotationDegrees;
                    currentTilt.X -= (mouseMotion.Relative.Y * MouseSensitivity);
                    currentTilt.X = Mathf.Clamp(currentTilt.X, -90, 90);
                    RotationDegrees = currentTilt;

                }
            }
        }
    }
    public void Restart()
    {
        Speed = 10;
        Position = new Vector3(0, 0, -50);
        RotationDegrees = new Vector3(0, 0, 0);
        RequestNotSent = false;
    }
}
