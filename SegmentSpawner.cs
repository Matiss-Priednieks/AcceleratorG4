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
    PackedScene CircleSegment, CircleWithObstacle, LineSegment, ExpandedSegment, LargeObstacleSegment;
    RandomNumberGenerator rng;

    Dictionary SegmentDict;
    Godot.Collections.Array keyArray;
    Vector3 addedDistance = new Vector3(0, 0, 100);
    int LastKey = 1;
    int NextKey = 0;
    Player Player;
    float SpawnTime = 2f;
    public List<Node3D> InstanceList;
    bool FirstRun = true;
    bool Moved = false;

    Vector3 LastSegPosition, NextSegPosition = Vector3.Zero;

    Gameplay GameRef;
    bool GameState;
    public override void _Ready()
    {
        GameRef = GetParent<Gameplay>();
        GameState = GameRef.Playing;
        InstanceList = new List<Node3D>();
        GD.Randomize();
        rng = new RandomNumberGenerator();
        rng.Randomize();
        // CircleSegment = ResourceLoader.Load<PackedScene>("res://CirclesSegmentWithAudio.tscn");
        CircleWithObstacle = ResourceLoader.Load<PackedScene>("res://CirclesSegmentWithObstacles.tscn");
        LineSegment = ResourceLoader.Load<PackedScene>("res://LinesSegment.tscn");
        // ExpandedSegment = ResourceLoader.Load<PackedScene>("res://ExpandedSegmentWithAudio.tscn");
        LargeObstacleSegment = ResourceLoader.Load<PackedScene>("res://ExpandedSegmentWithObstacles.tscn");
        SegmentDict = new Dictionary();

        // SegmentDict.Add(1, (PackedScene)CircleSegment);
        SegmentDict.Add(1, (PackedScene)CircleWithObstacle);
        SegmentDict.Add(2, (PackedScene)LineSegment);
        // SegmentDict.Add(5, (PackedScene)ExpandedSegment);
        SegmentDict.Add(3, (PackedScene)LargeObstacleSegment);
        keyArray = new Godot.Collections.Array(SegmentDict.Keys);
        for (int i = 0; i < 7; i++)
        {
            SpawnSegment();
            Moved = false;
        }
        Player = GetNode<Player>("%Player");

    }
    public override void _Process(double delta)
    {
        if (!GameState)
        {
            GameRef = GetParent<Gameplay>();
            GameState = GameRef.Playing;
        }


        if (InstanceList.Count < 8 && GameState)
        {
            SpawnSegment();
            Moved = false;
        }
        if (Player.Position.Z < -30000)
        {
            foreach (Node3D child in InstanceList)
            {
                if (child != null)
                {
                    child.Position += new Vector3(0, 0, 10160);
                }
            }
            Player.Position += new Vector3(0, 0, 10000);
            LastSegPosition = LastKey == 2 ? new Vector3(0, 0, -220) : new Vector3(0, 0, -100);
        }
    }

    public PackedScene ChooseSegment()
    {
        GD.Randomize();

        NextKey = rng.RandiRange(0, 2);


        if (LastKey != NextKey)
        {
            var nextSeg = SegmentDict[keyArray[NextKey]];
            return (PackedScene)nextSeg;
        }
        else
        {
            do
            {
                NextKey = rng.RandiRange(0, 2);
            } while (LastKey == NextKey);
            var nextSeg = SegmentDict[keyArray[NextKey]];
            return (PackedScene)nextSeg;
        }
    }

    public void SpawnSegment()
    {
        var SegmentInstance = ChooseSegment().Instantiate<Node3D>();

        if (!Moved)
        {
            // SegmentInstance.Position += addedDistance;
            if (LastKey >= 2 && NextKey >= 2)
            {
                GD.Print("big into big: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 220);
            }
            else if (LastKey >= 2 && NextKey <= 1)
            {

                GD.Print("big into small: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 160);
            }
            else if (LastKey <= 1 && NextKey <= 1)
            {
                GD.Print("small into small: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 100);
            }
            else if (LastKey <= 1 && NextKey >= 2)
            {
                GD.Print("small into big: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 160);
            }
            else
            {
                GD.Print("unknown segments: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 100);
            }
            Moved = true;
        }

        InstanceList.Add(SegmentInstance);
        AddChild(SegmentInstance);

        LastKey = NextKey;
        LastSegPosition = SegmentInstance.Position;

    }


}
