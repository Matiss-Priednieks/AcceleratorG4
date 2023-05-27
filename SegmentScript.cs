using Godot;
using System;

public partial class SegmentScript : Node3D
{
    public async void _on_Area_body_exited(CharacterBody3D body)
    {
        if (body is Player pBody)
        {
            if (pBody.Speed > 100)
            {
                QueueFree();
                GD.Print("killed");
            }
            else
            {
                GD.Print("start");
                await ToSignal(GetTree().CreateTimer(pBody.Speed / (1.5f * pBody.Speed)), SceneTreeTimer.SignalName.Timeout);
                QueueFree();
                GD.Print("end");
            }
        }
    }
}
