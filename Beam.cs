using Godot;
using Godot.Collections;
using System;

public partial class Beam : StaticBody3D
{
    [Export] public bool Enabled = false;
    [Export] int MoveSpeed = 10;
    Dictionary BeamDict;
    Vector3 CurrentPosition;

    Vector3 OutSidePosition;
    [Export] float Amplitutde = 15, Freq = 1, Omega = 1.0f;
    float Index;
    public bool MiddleBeam, DoubleBeam, QuadBeam, HoleBeam = false;
    PathFollow3D Part1;
    PathFollow3D Part2;


    public override void _Ready()
    {
        Part1 = GetNode<PathFollow3D>("%Part1");
        Part2 = GetNode<PathFollow3D>("%Part2");
        if (Part1 != null || Part2 != null)
        {
            Part1.ProgressRatio = 0;
            Part2.ProgressRatio = 0.5f;
        }
        CurrentPosition = Position;
        OutSidePosition = CurrentPosition + new Vector3(0, 2000, 0);
        var rot = RotationDegrees;
        if (this.Name != "QuadBeam")
        {
            rot.Z += (GD.Randf() * 360);
        }
        RotationDegrees = rot;
    }

    public override void _Process(double delta)
    {
        if (Enabled)
        {
            Show();
            if (this.Name == "DoubleBeam")
            {
                Index += (float)delta;
                float y = Amplitutde * Mathf.Sin(Omega * Index);
                Position = new Vector3(0, y, 0);
            }
            if (this.Name == "MiddleBeam")
            {
                RotateZ(1 * (float)delta);
            }
            if (this.Name == "QuadBeam")
            {
                //todo: use path node to make quads move.
                MoveQuads(delta);
            }
        }
        else
        {
            Position = OutSidePosition;
            if (Part1 != null && Part2 != null)
            {
                Part1.Position = new Vector3(0, 1000, 0);
                Part2.Position = new Vector3(0, 1000, 0);
            }
            Hide();
        }
    }

    public void MoveQuads(double delta)
    {
        if (Part1 != null || Part2 != null)
        {
            Part1.Progress += MoveSpeed * (float)delta;
            Part2.Progress += MoveSpeed * (float)delta;
        }
    }
}
