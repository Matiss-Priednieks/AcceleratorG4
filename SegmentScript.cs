using Godot;
using System;

public partial class SegmentScript : Node3D
{
    string ChosenBeam;
    public override void _Ready()
    {
        GD.Randomize();
        switch (Mathf.RoundToInt(GD.RandRange(0, 6)))
        {
            case 0:
                ChosenBeam = "MiddleBeam";
                break;
            case 1:
                ChosenBeam = "DoubleBeam";
                break;
            case 2:
                ChosenBeam = "QuadBeam";
                break;
            case 3:
                // ChosenBeam = "HoleBeam";
                ChosenBeam = "QuadBeam";
                // ChosenBeam = "None";
                break;
            case 4:
                ChosenBeam = "None";
                break;
            case 5:
                ChosenBeam = "None";
                break;
            case 6:
                ChosenBeam = "None";
                break;
        }
        GD.Print(ChosenBeam);
        if (ChosenBeam != "None")
        {
            Beam beam = GetNode<Beam>(ChosenBeam);
            if (beam != null)
            {
                beam.Enabled = true;
            }
        }
    }
    public async void _on_Area_body_exited(CharacterBody3D body)
    {
        if (body is Player pBody)
        {
            if (pBody.Speed > 100)
            {
                SegmentSpawner parent = (SegmentSpawner)GetParent();
                parent.InstanceList.Remove((Node3D)parent.GetChild(0));
                QueueFree();
                // GD.Print("killed");
            }
            else
            {
                // GD.Print("start");
                await ToSignal(GetTree().CreateTimer(pBody.Speed / (1.5f * pBody.Speed)), SceneTreeTimer.SignalName.Timeout);
                SegmentSpawner parent = (SegmentSpawner)GetParent();
                parent.InstanceList.Remove((Node3D)parent.GetChild(0));
                QueueFree();
                // GD.Print("end");
            }
        }
    }
}
