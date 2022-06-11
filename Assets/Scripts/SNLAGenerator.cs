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
		if (tile.GetIsSnakeOrLadder() || !tile.GetCanBeLadder()) return;
		var go = new GameObject("Ladder");
		var lr = go.AddComponent<LineRenderer>();
		lr.startWidth = 0.5f;
		lr.endWidth = 0.5f;
		lr.positionCount = 2;
		lr.material = ladderMaterial;
		lr.startColor = Color.green;
		lr.endColor = Color.green;
		lr.SetPosition(0, tile.transform.position);
		Tile destinationTile = CalculateLadderEnd(tile);
		lr.SetPosition(1, destinationTile.transform.position);
		player.MoveAlongLadder(lr, destinationTile);
	}

	private Tile CalculateLadderEnd(Tile tile)
	{
		return BoardCreator.tiles[player.GetCurrentCell() + 22];
	}
}