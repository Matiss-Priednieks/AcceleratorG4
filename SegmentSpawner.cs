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
    public List<Node3D> InstanceList;

    public override void _Ready()
    {
        InstanceList = new List<Node3D>();
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
        if (InstanceList.Count < 4)
        {
            GD.Print(InstanceList.Count);
            SpawnSegment();
        }
        if (Player.Position.Z < -10000)
        {
            foreach (Node3D child in InstanceList)
            {
                child.Position += new Vector3(0, 0, 10000);
            }
            Player.Position += new Vector3(0, 0, 10000);
            addedDistance = lastKey == 2 ? new Vector3(0, 0, -160) : new Vector3(0, 0, -100);
        }
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
        GD.Print(addedDistance);

        if (SegmentInstance.Position.DistanceTo(Player.Position) < 500)
        {
            InstanceList.Add(SegmentInstance);
            AddChild(SegmentInstance);
            lastKey = randomKey;
        }

    }


}
