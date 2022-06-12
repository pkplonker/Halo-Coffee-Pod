using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private float moveSpeed = .2f;
	private float snakeLadderSpeed = 2f;

	private int startCell = 1;
	private int currentCell;
	public event Action OnWin;
	public event Action OnMoveComplete;
	public event Action OnMove;
	public event Action OnSlide;

	public int GetCurrentCell() => currentCell;

	private void Start() => currentCell = startCell;


	private void OnEnable() => Dice.onRollComplete += Move;


	private void OnDisable() => Dice.onRollComplete -= Move;


	private void MoveOneCell()
	{
		currentCell++;
		transform.position = BoardCreator.tiles[currentCell].transform.position;
		OnMove?.Invoke();
	}

	private void Move(int moveAmount)
	{
		if (currentCell + moveAmount <= BoardCreator.tiles.Count) StartCoroutine(MoveCoroutine(moveAmount));
		else Debug.Log("Overshot");
	}

	IEnumerator MoveCoroutine(int moveAmount)
	{
		GameManager.canInteract = false;
		int target = IncrementTarget(moveAmount);
		float timer = 0;
		while (currentCell != target)
		{
			timer += Time.deltaTime;
			if (timer > moveSpeed)
			{
				timer = 0;
				MoveOneCell();
			}

			yield return null;
		}

		var x = SNLAGenerator.snakeLadderDatas.Find(x => x.startCell == BoardCreator.tiles[currentCell]);
		if (x != null) StartCoroutine(MoveAlongLineRendererCoroutine(x.lineRenderer, x.endCell));
		else CheckWin();
	}

	private void CheckWin()
	{
		GameManager.canInteract = true;
		if (currentCell == BoardCreator.tiles.Count) OnWin?.Invoke();
		else OnMoveComplete?.Invoke();
	}

	private int IncrementTarget(int moveAmount)
	{
		if (moveAmount + currentCell >= BoardCreator.tiles.Count) return BoardCreator.tiles.Count;
		return currentCell + moveAmount;
	}

	public void MoveAlongSnakeLadder(LineRenderer lr, Tile destinationTile)
	{
		GameManager.canInteract = false;
		StartCoroutine(MoveAlongLineRendererCoroutine(lr, destinationTile));
	}

	private IEnumerator MoveAlongLineRendererCoroutine(LineRenderer lr, Tile destinationTile)
	{
		OnSlide?.Invoke();
		var currentTarget = 1;
		while (transform.position != lr.GetPosition(currentTarget))
		{
			GameManager.canInteract = false;

			transform.position = Vector3.MoveTowards(transform.position, lr.GetPosition(currentTarget),
				snakeLadderSpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, lr.GetPosition(currentTarget)) < 0.1f)
			{
				currentTarget++;
				if (currentTarget == lr.positionCount) break;
			}

			yield return null;
		}

		currentCell = destinationTile.GetId();
		GameManager.canInteract = true;
	}
}