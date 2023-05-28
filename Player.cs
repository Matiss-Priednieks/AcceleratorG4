using Godot;
using System;

public partial class Player : CharacterBody3D
{

    [Export(PropertyHint.Range, "0.01,5,")] float MouseSensitivity = 1.0f;
    public float Speed = 30;
    bool GameOver = false;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GetNode<Panel>("%GameOver").Hide();
    }

    public override void _Process(double delta)
    {
        Math.Round(MouseSensitivity, 2);
        GetNode<Panel>("%Score").GetNode<Label>("Label").Text = Speed.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (!GameOver)
        {
            var collision = MoveAndCollide(Transform.Basis.Z * (Speed * (float)delta) * -1);
            if ((Time.GetTicksUsec() % 1000 > 900))
            {
                Speed += 0.5f;
            }
            if (collision != null)
            {
                GetNode<AudioStreamPlayer3D>("%Crash").Play();
                GameOver = true;
                GetNode<Panel>("%GameOver").Show();
            }
        }

    }
    public override void _Input(InputEvent inputEvent)
    {
        Aim(inputEvent);
    }

    public void Aim(InputEvent inputEvent)
    {
        var mouseMotion = inputEvent as InputEventMouseMotion;
        if (mouseMotion != null)
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {

                Vector3 currentPitch = RotationDegrees;
                currentPitch.Y -= mouseMotion.Relative.X * MouseSensitivity;

                RotationDegrees = currentPitch;

                Vector3 currentTilt = RotationDegrees;

                currentTilt.X -= mouseMotion.Relative.Y * MouseSensitivity;
                currentTilt.X = Mathf.Clamp(currentTilt.X, -90, 90);
                RotationDegrees = currentTilt;

            }
        }
    }
}
