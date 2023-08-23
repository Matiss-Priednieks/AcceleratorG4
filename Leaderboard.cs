using Godot;
using Godot.Collections;
using System;
using System.Linq;


public partial class Leaderboard : PanelContainer
{
	public HttpRequest HTTPRequest { get; private set; }
	PackedScene LeaderboardItem;
	VBoxContainer ScoreList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HTTPRequest = GetNode<HttpRequest>("%LeaderboardRequest");

		LeaderboardItem = ResourceLoader.Load<PackedScene>("res://leaderboard_score_object.tscn");

		ScoreList = GetNode<VBoxContainer>("%ScoreList");

		PopulateLeaderboard();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public Error PopulateLeaderboard()
	{
		string[] newRegHeaders = new string[] { "Content-Type: application/json" };
		var error = HTTPRequest.Request("https://forwardvector.uksouth.cloudapp.azure.com/get-leaderboard", newRegHeaders, HttpClient.Method.Get, "{}");
		return error;
	}

	public void _on_leaderboard_request_request_completed(long result, long responseCode, string[] headers, byte[] body)
	{
		Json json = new();
		json.Parse(body.GetStringFromUtf8());
		var response = json.Data;
		var dict = (Godot.Collections.Array)response;

		if (responseCode == 200 || responseCode == 201)
		{
			GD.Print(dict);
			foreach (var child in ScoreList.GetChildren())
			{
				child.QueueFree();
			}
			foreach (Godot.Collections.Dictionary item in dict.Select(v => (Dictionary)v))
			{
				var lbItemInst = LeaderboardItem.Instantiate<Panel>();
				lbItemInst.GetNode<Label>("LBScoreLabel").Text = item[key: "username"].ToString() + ": " + item[key: "highscore"].ToString();
				ScoreList.AddChild(lbItemInst);
			}
		}
		else
		{
			var lbItemInst = LeaderboardItem.Instantiate<Panel>();
			lbItemInst.GetNode<Label>("LBScoreLabel").Text = "Error loading leaderboard";
			ScoreList.AddChild(lbItemInst);
		}
	}
	public void _on_update_leaderboard_timeout()
	{
		if (ScoreList.GetParent<VBoxContainer>().GetParent<MarginContainer>().GetParent<PanelContainer>().Visible) //lazy way of getting if menu is on or not
		{
			PopulateLeaderboard();
		}
	}
}
