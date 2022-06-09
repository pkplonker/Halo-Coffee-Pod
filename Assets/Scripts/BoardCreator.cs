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
	[SerializeField] private Color startColor;
	[SerializeField] private Color endColor;

	public static Dictionary<int, Tile> tiles { get; private set; } = new Dictionary<int, Tile>();
	private Camera cam;

	private void Awake()
	{
		cam = Camera.main;
	}


	public void BuildBoard()
	{
		tiles.Clear();
		var offsetY = boardSize / 2;
		var offsetX = Mathf.CeilToInt(boardSize / 1.5f);

		for (var x = 0; x < boardSize; x++)
		{
			for (var y = 0; y < boardSize; y++)
			{
				Tile tile = Instantiate(tilePrefab, new Vector3(x - offsetX, y - offsetY, 0), Quaternion.identity,
					transform).GetComponent<Tile>();
				int id;
				if (y % 2 != 0)
				{
					id = ((y * boardSize) + (boardSize - x));
				}
				else
				{
					id = (y * boardSize + (x + 1));
				}

				tile.Setup(cam, id, id % 2 == 0 ? primaryColor : secondaryColor,
					id % 2 != 0 ? primaryColor : secondaryColor, y != 0 && id!=(boardSize*boardSize), 
					y != boardSize - 1 &&id!=(boardSize*boardSize));
				SetStartAndEndColors(id, tile);
				tiles.Add(id, tile);
			}
		}
	}

	private void SetStartAndEndColors(int id, Tile tile)
	{
		if (id == boardSize * boardSize)
		{
			tile.SetColor(startColor);
		}
		else if (id == 1)
		{
			tile.SetColor(endColor);
		}
	}
}