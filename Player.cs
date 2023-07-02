using Godot;
using System;

public partial class Player : CharacterBody3D
{

    [Export(PropertyHint.Range, "0.01,5,")] public float MouseSensitivity = 0.1f;
    public float Speed = 10;
    Gameplay GameRef;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GameRef = GetParent<Gameplay>();
    }

    public override void _Process(double delta)
    {
        var displaySpeed = (int)Speed;
        GetNode<Panel>("%Score").GetNode<Label>("Label").Text = displaySpeed.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (!GameRef.GameOver && GameRef.Playing)
        {
            var collision = MoveAndCollide(Transform.Basis.Z * (Speed * (float)delta) * -1);
            if ((Time.GetTicksUsec() % 1000 > 900))
            {
                Speed += 0.1f;
            }
            if (collision != null)
            {
                GetNode<AudioStreamPlayer3D>("%Crash").Play();
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
            var mouseMotion = inputEvent as InputEventMouseMotion;
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
        Position = new Vector3(0, 0, 0);
        RotationDegrees = new Vector3(0, 0, 0);
    }
}
