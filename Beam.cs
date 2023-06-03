using Godot;
using Godot.Collections;
using System;

public partial class Beam : StaticBody3D
{
    [Export] public bool Enabled = false;
    Dictionary BeamDict;
    Vector3 CurrentPosition;

    Vector3 OutSidePosition;
    [Export] float Amplitutde = 15, Freq = 1, Omega = 1.0f;
    float Index;
    public bool MiddleBeam, DoubleBeam, QuadBeam, HoleBeam = false;

    public override void _Ready()
    {
        CurrentPosition = Position;
        OutSidePosition = CurrentPosition + new Vector3(0, 1000, 0);
        var rot = RotationDegrees;
        rot.Z += (GD.Randf() * 360);
        RotationDegrees = rot;
    }

    public override void _Process(double delta)
    {
        if (Enabled)
        {
            Show();
            if (Enabled && this.Name == "DoubleBeam")
            {
                Index += (float)delta;
                float y = Amplitutde * Mathf.Sin(Omega * Index);
                Position = new Vector3(0, y, 0);
            }
            if (Enabled && this.Name == "MiddleBeam")
            {
                RotateZ(1 * (float)delta);
            }
        }
        else
        {
            Position = OutSidePosition;
            Hide();
        }
    }
}
