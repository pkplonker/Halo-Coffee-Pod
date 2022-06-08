using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardCreator : MonoBehaviour
{
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private int boardSize;
	[SerializeField] private Color primaryColor;
	[SerializeField] private Color secondaryColor;
	public static Dictionary<int, Tile> tiles { get; private set; } = new Dictionary<int, Tile>();
	private Camera cam;

	private void Awake()
	{
		cam = Camera.main;
	}


	public void BuildBoard()
	{
		Debug.LogWarning("starting build");
		int offsetY = boardSize / 2;
		int offsetX = Mathf.CeilToInt(boardSize / 1.5f);

		for (int x = 0; x < boardSize; x++)
		{
			for (int y = 0; y < boardSize; y++)
			{
				Tile tile = Instantiate(tilePrefab, new Vector3(x - offsetX, y - offsetY, 0), Quaternion.identity,
					transform).GetComponent<Tile>();
				int id;
				if (y % 2 != 0)
				{
					id = ((y * boardSize) + (boardSize - x));
					Debug.Log("x= "+x+ " ,y= "+ y+ ", -1- calculated id = " + id);
				}
				else
				{
					id = (y * boardSize + (x+1));
					Debug.Log("x= "+x+ " ,y= "+ y+ ", -2- calculated id = " + id);
				}
				tile.Setup(cam, id, id % 2 == 0 ? primaryColor : secondaryColor, id % 2 != 0 ? primaryColor : secondaryColor);
				tiles.Add(id, tile);
			}
		}
	}
}