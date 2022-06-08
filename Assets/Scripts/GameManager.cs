using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;
	private GameObject player;
	[SerializeField] private BoardCreator boardCreator;

	private void Start()
	{
		boardCreator.BuildBoard();
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		player = Instantiate(playerPrefab, BoardCreator.tiles[1].transform.position, Quaternion.identity);
	}
}