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
	[SerializeField] private int defaultSnakeJump = 22;
	[SerializeField] private int minSnakeJump = 10;
	public static List<SnakeLadderData> snakeLadderDatas;

	private void Start()
	{
		snakeLadderDatas = new List<SnakeLadderData>();
		player = questionController.GetPlayer();
	}

	private void OnEnable()
	{
		QuestionController.OnCorrectAnswer += GenerateLadder;
		QuestionController.OnWrongAnswer += GenerateSnake;
	}

	private void OnDisable()
	{
		QuestionController.OnCorrectAnswer -= GenerateLadder;
		QuestionController.OnWrongAnswer -= GenerateSnake;
	}

	private void GenerateSnake(PlayerMovement player)
	{
		this.player = player;
		var tile = BoardCreator.tiles[player.GetCurrentCell()];
		if (!tile.GetCanBeSnake()) return;
		var destinationTile = CalculateSnakeEnd(tile);
		if (destinationTile == null) return;
		var start = snakeLadderDatas.Find(s => s.startCell == tile || s.startCell == destinationTile);
		if (start != null) return;
		var lr = BuildLadder(player, destinationTile, tile, true);
		snakeLadderDatas.Add(new SnakeLadderData(tile, destinationTile, lr));
		player.MoveAlongSnakeLadder(lr, destinationTile);
	}

	private Tile CalculateSnakeEnd(Tile tile)
	{
		var snakeJump = defaultLadderJump;
		var loopAmount = defaultSnakeJump - minSnakeJump;
		for (var i = 0; i < loopAmount; i++)
		{
			Debug.Log("Testing cell " + (tile.GetId() + snakeJump));
			if (player.GetCurrentCell() - snakeJump > BoardCreator.tiles.Count)
			{
				Debug.Log((tile.GetId()- snakeJump) + " Cell too high");
			}
			else if (player.GetCurrentCell() - snakeJump <= 0)
			{
				Debug.Log((tile.GetId() - snakeJump) + " Cell too low");
			}
			else if (snakeLadderDatas.Find(x => x.startCell == tile) != null)
			{
				Debug.Log((tile.GetId() - snakeJump) + " cell already used as start");
			}
			else if (snakeLadderDatas.Find(x => x.endCell == tile) != null)
			{
				Debug.Log((tile.GetId() - snakeJump) + " cell already used as end");
			}
			else if (BoardCreator.tiles[player.GetCurrentCell() - snakeJump].GetCanBeSnake())
			{
				return BoardCreator.tiles[player.GetCurrentCell() - snakeJump];
			}

			snakeJump--;
		}

		Debug.Log("failed to find destination");
		return null;
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

		var lr = BuildLadder(player, destinationTile, tile, false);
		snakeLadderDatas.Add(new SnakeLadderData(tile, destinationTile, lr));
		player.MoveAlongSnakeLadder(lr, destinationTile);
	}

	private LineRenderer BuildLadder(PlayerMovement player, Tile destinationTile, Tile tile, bool isSnake)
	{
		var go = new GameObject("Ladder");
		var lr = go.AddComponent<LineRenderer>();

		lr.startWidth = 0.5f;
		lr.endWidth = 0.5f;

		lr.positionCount = isSnake ? 3 : 2;
		if (isSnake)
		{
			lr.material = snakeMaterial;
			lr.startColor = Color.red;
			lr.endColor = Color.red;
		}
		else
		{
			lr.material = ladderMaterial;
			lr.startColor = Color.green;
			lr.endColor = Color.green;
		}

		lr.SetPosition(0, tile.transform.position);
		if (isSnake)
		{
			lr.SetPosition(1,
				tile.transform.position + ((destinationTile.transform.position - tile.transform.position) / 2) +
				new Vector3(0,  UnityEngine.Random.Range(-1.5f,1.5f), 0));

			lr.SetPosition(2, destinationTile.transform.position);
		}
		else
		{
			lr.SetPosition(1, destinationTile.transform.position);
		}

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