using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNLAGenerator : MonoBehaviour
{
	[SerializeField] private QuestionController questionController;
	[SerializeField] private BoardCreator boardCreator;
	private PlayerMovement player;
	[SerializeField] private Material ladderMaterial;
	[SerializeField] private Material snakeMaterial;
	[SerializeField] private int defaultLadderJump = 22;
	[SerializeField] private int minLadderJump = 10;

	List<SnakeLadderData> snakeLadderDatas = new List<SnakeLadderData>();

	private void Start()
	{
		player = questionController.GetPlayer();
	}

	private void OnEnable()
	{
		questionController.OnCorrectAnswer += GenerateLadder;
		questionController.OnWrongAnswer += GenerateSnake;
	}

	private void OnDisable()
	{
		questionController.OnCorrectAnswer -= GenerateLadder;
		questionController.OnWrongAnswer -= GenerateSnake;
	}

	private void GenerateSnake(PlayerMovement player)
	{
		this.player = player;
	}

	private void GenerateLadder(PlayerMovement player)
	{
		this.player = player;

		var tile = BoardCreator.tiles[player.GetCurrentCell()];
		if (!tile.GetCanBeLadder()) return;

		var destinationTile = CalculateLadderEnd(tile);
		if (destinationTile == null) return;

		var start = snakeLadderDatas.Find(s => s.startCell == tile || s.startCell == destinationTile);
		if (start != null) return;

		var lr = BuildLadder(player, destinationTile, tile);
		snakeLadderDatas.Add(new SnakeLadderData(tile, destinationTile, lr));
		player.MoveAlongLadder(lr, destinationTile);
	}

	private LineRenderer BuildLadder(PlayerMovement player, Tile destinationTile, Tile tile)
	{
		var go = new GameObject("Ladder");
		var lr = go.AddComponent<LineRenderer>();

		lr.startWidth = 0.5f;
		lr.endWidth = 0.5f;
		lr.positionCount = 2;
		lr.material = ladderMaterial;
		lr.startColor = Color.green;
		lr.endColor = Color.green;
		lr.SetPosition(0, tile.transform.position);
		lr.SetPosition(1, destinationTile.transform.position);
		return lr;
	}

	private Tile CalculateLadderEnd(Tile tile)
	{
		var ladderJump = defaultLadderJump;
		var loopAmount = ladderJump - minLadderJump;
		for (int i = 0; i < loopAmount; i++)
		{
			Debug.Log("Testing cell " + (tile.GetId() + ladderJump));
			if (player.GetCurrentCell() + ladderJump > BoardCreator.tiles.Count)
			{
				Debug.Log((tile.GetId() + ladderJump) + " Cell too high");
			}
			else if (snakeLadderDatas.Find(x => x.startCell == tile) != null)
			{
				Debug.Log((tile.GetId() + ladderJump) + " cell already used as start");
			}
			else if (snakeLadderDatas.Find(x => x.endCell == tile) != null)
			{
				Debug.Log((tile.GetId() + ladderJump) + " cell already used as end");
			}
			else if (BoardCreator.tiles[player.GetCurrentCell() + ladderJump].GetCanBeLadder())
			{
				return BoardCreator.tiles[player.GetCurrentCell() + ladderJump];
			}
			ladderJump--;
		}

		Debug.Log("failed to find destination");
		return null;
	}
}

public class SnakeLadderData
{
	public Tile startCell;
	public Tile endCell;
	public LineRenderer lineRenderer;

	public SnakeLadderData(Tile startCell, Tile endCell, LineRenderer lineRenderer)
	{
		this.startCell = startCell;
		this.endCell = endCell;
		this.lineRenderer = lineRenderer;
	}
}