using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public partial class SegmentSpawner : Node3D
{
    PackedScene CircleSegment;
    PackedScene LineSegment;
    PackedScene ExpandedSegment;
    RandomNumberGenerator rng;

    Dictionary SegmentDict;
    Godot.Collections.Array keyArray;
    Vector3 addedDistance = new Vector3(0, 0, 100);
    int lastKey = 0;
    int randomKey = 0;
    Player Player;
    float SpawnTime = 2f;

    public override void _Ready()
    {
        GD.Randomize();
        rng = new RandomNumberGenerator();
        rng.Randomize();
        CircleSegment = ResourceLoader.Load<PackedScene>("res://CirclesSegmentWithAudio.tscn");
        LineSegment = ResourceLoader.Load<PackedScene>("res://LinesSegment.tscn");
        ExpandedSegment = ResourceLoader.Load<PackedScene>("res://ExpandedSegmentWithAudio.tscn");
        SegmentDict = new Dictionary();

        SegmentDict.Add(1, (PackedScene)CircleSegment);
        SegmentDict.Add(2, (PackedScene)LineSegment);
        SegmentDict.Add(3, (PackedScene)ExpandedSegment);
        keyArray = new Godot.Collections.Array(SegmentDict.Keys);
        CallDeferred("SpawnSegment");
        Player = GetNode<Player>("%Player");

    }
    public override void _Process(double delta)
    {
        // testFunction();
    }

    public PackedScene ChooseSegment()
    {
        GD.Randomize();
        randomKey = rng.RandiRange(0, 2);
        if (lastKey != randomKey)
        {
            var nextSeg = SegmentDict[keyArray[randomKey]];
            return (PackedScene)nextSeg;
        }
        else
        {
            do
            {
                randomKey = rng.RandiRange(0, 2);
            } while (lastKey == randomKey);
            var nextSeg = SegmentDict[keyArray[randomKey]];
            return (PackedScene)nextSeg;
        }
    }

    public void SpawnSegment()
    {
        var SegmentInstance = ChooseSegment().Instantiate<Node3D>();
        if (lastKey == 2)
        {
            addedDistance.Z += -160;
        }
        else
        {
            addedDistance.Z += -100;
        }
        SegmentInstance.Position += addedDistance;


        AddChild(SegmentInstance);

        lastKey = randomKey;
    }

    // implementation that doesn't work, similar idea might work but this just creates a large amount of segments in a very short amount of time and crashes the game
    // public async void testFunction()
    // {
    //     await ToSignal(GetTree().CreateTimer(SpawnTime), SceneTreeTimer.SignalName.Timeout);
    //     SpawnTime = Player.Speed % 25 == 0 ? SpawnTime - 0.1f : SpawnTime;
    //     GD.Print(SpawnTime);
    //     System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(SpawnSegment));
    //     t.Start();
    // }
    public void _on_spawn_timer_timeout()
    {
        // CallDeferred("SpawnSegment");
        System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(SpawnSegment));
        t.Start();
    }

}
