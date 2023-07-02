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

    int LastKey = 1;
    int NextKey = 0;
    Player Player;

    bool Moved = false;

    Vector3 LastSegPosition = Vector3.Zero;

    Gameplay GameRef;
    public override void _Ready()
    {
        GameRef = GetParent<Gameplay>();

        GD.Randomize();
        rng = new RandomNumberGenerator();
        rng.Randomize();
        CircleWithObstacle = ResourceLoader.Load<PackedScene>("res://CirclesSegmentWithObstacles.tscn");
        LineSegment = ResourceLoader.Load<PackedScene>("res://LinesSegment.tscn");
        LargeObstacleSegment = ResourceLoader.Load<PackedScene>("res://ExpandedSegmentWithObstacles.tscn");
        SegmentDict = new Dictionary();

        SegmentDict.Add(1, (PackedScene)CircleWithObstacle);
        SegmentDict.Add(2, (PackedScene)LineSegment);
        SegmentDict.Add(3, (PackedScene)LargeObstacleSegment);
        keyArray = new Godot.Collections.Array(SegmentDict.Keys);

        //Spawn first 8 segments
        Restart();
        Player = GetNode<Player>("%Player");

    }
    public override void _Process(double delta)
    {
        if (GetChildCount() < 8 && GameRef.Playing)
        {
            SpawnSegment();
            // SpawnSegment();
            Moved = false;
        }
        //Not a perfect solution, but players hopefully should never run into the issues it causes (Segments not generating fast enough and having gaps in the walls.)
        if (Player.Position.Z < -25000)
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                var child = GetChildren()[i] as Node3D;
                if (child is not null)
                {
                    child.Position += new Vector3(0, 0, 25000);
                    GD.Print(child.Position);
                }
            }

            Player.Position += new Vector3(0, 0, 25000);
            GD.Print(Player.Position);
            LastSegPosition = LastKey == 2 ? new Vector3(0, 0, -220) : new Vector3(0, 0, -100);
            if (LastKey >= 2 && NextKey >= 2)
            {
                // GD.Print("big into big: " + LastKey + ", " + NextKey);
                LastSegPosition = new Vector3(0, 0, -220);
            }
            else if (LastKey >= 2 && NextKey <= 1)
            {

                // GD.Print("big into small: " + LastKey + ", " + NextKey);
                LastSegPosition = new Vector3(0, 0, -160);
            }
            else if (LastKey <= 1 && NextKey <= 1)
            {
                // GD.Print("small into small: " + LastKey + ", " + NextKey);
                LastSegPosition = new Vector3(0, 0, -100);
            }
            else if (LastKey <= 1 && NextKey >= 2)
            {
                // GD.Print("small into big: " + LastKey + ", " + NextKey);
                LastSegPosition = new Vector3(0, 0, -160);
            }
            else
            {
                // GD.Print("unknown segments: " + LastKey + ", " + NextKey);
                LastSegPosition = new Vector3(0, 0, -100);
            }
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
                // GD.Print("big into big: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 220);
            }
            else if (LastKey >= 2 && NextKey <= 1)
            {

                // GD.Print("big into small: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 160);
            }
            else if (LastKey <= 1 && NextKey <= 1)
            {
                // GD.Print("small into small: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 100);
            }
            else if (LastKey <= 1 && NextKey >= 2)
            {
                // GD.Print("small into big: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 160);
            }
            else
            {
                // GD.Print("unknown segments: " + LastKey + ", " + NextKey);
                SegmentInstance.Position += new Vector3(LastSegPosition.X, LastSegPosition.Y, LastSegPosition.Z - 100);
            }
            Moved = true;
        }

        AddChild(SegmentInstance);

        LastKey = NextKey;
        LastSegPosition = SegmentInstance.Position;

    }
    public void Restart()
    {
        LastSegPosition = Vector3.Zero;
        LastKey = 1;
        NextKey = 0;
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
        for (int i = 0; i < 7; i++)
        {
            SpawnSegment();
            Moved = false;
        }
    }
}
