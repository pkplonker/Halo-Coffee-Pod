using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;
	private PlayerMovement player;
	[SerializeField] private BoardCreator boardCreator;
	public static bool canInteract = true;
	public static GameManager instance;
	[SerializeField] QuestionController questionController;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		canInteract = true;
		boardCreator.BuildBoard();
		SpawnPlayer();
		questionController.Init(this, player);
	}

	private void OnDisable()
	{
		if (player == null) return;
		player.OnWin -= OnWin;
	}

	private void SpawnPlayer()
	{
		player = Instantiate(playerPrefab, BoardCreator.tiles[1].transform.position, Quaternion.identity).GetComponent<PlayerMovement>();
		player.OnWin += OnWin;
	}

	private void OnWin()
	{
		Debug.LogWarning("WINNERRRR");
	}
}