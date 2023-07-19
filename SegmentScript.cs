using Godot;
using System;

public partial class SegmentScript : Node3D
{
    string ChosenBeam;
    public Timer DespawnSafeguard;
    float PlayerSpeed;
    SegmentSpawner SpawnerRef;
    public override void _Ready()
    {
        SpawnerRef = GetNode<SegmentSpawner>("../%SegmentSpawner");
        DespawnSafeguard = GetNode<Timer>("%DespawnSafeguard");
        DespawnSafeguard.Stop();
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
                ChosenBeam = "None";
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
        GD.Print(this.Name);
        if (ChosenBeam != "None" && this.Name != "sectionwithstraightlines") //doesn't entirely fix the errors. errors currently appear because sectionwithstraightlines isn't meant to have any obstacles, but due to how the script has been written it needs them. Flawed.
        {
            Beam beam = GetNode<Beam>(ChosenBeam);
            if (beam != null)
            {
                beam.Enabled = true;
            }
        }
    }
    public void _on_Area_body_exited(Node3D body)
    {
        if (body is Player pBody && !SpawnerRef.Pause)
        {
            DespawnSafeguard.Start(0.15f);
            PlayerSpeed = pBody.Speed;
        }
    }
    public void _on_area_body_entered(Node3D body)
    {
        if (body is Player pBody)
        {
            DespawnSafeguard.Stop();
        }
    }
    public async void _on_despawn_safeguard_timeout()
    {
        if (PlayerSpeed > 100 && !SpawnerRef.Pause)
        {
            RemoveSegment();
            GD.Print("despawned without delay");
        }
        else
        {
            GD.Print("despawn started");
            await ToSignal(GetTree().CreateTimer(PlayerSpeed / (1.5f * PlayerSpeed)), SceneTreeTimer.SignalName.Timeout);
            RemoveSegment();
            GD.Print("despawn finished");
        }
    }

    private void RemoveSegment()
    {
        // SegmentSpawner parent = (SegmentSpawner)GetNode<SegmentSpawner>("../%SegmentSpawner");
        CallDeferred("queue_free");
    }
}
