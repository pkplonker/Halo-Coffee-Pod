using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
	private float moveSpeed = .2f;
	private int startCell = 1;
	private int currentCell;
	public event Action OnWin;
	public event Action OnMoveComplete;

	private void Start()
	{
		currentCell = startCell;
	}

	private void OnEnable()
	{
		Dice.onRollComplete += Move;
	}

	private void OnDisable()
	{
		Dice.onRollComplete -= Move;
	}


	private void MoveOneCell()
	{
		currentCell++;
		transform.position = BoardCreator.tiles[currentCell].transform.position;
	}

	private void Move(int moveAmount)
	{
		if (currentCell + moveAmount <= BoardCreator.tiles.Count)
		{
			StartCoroutine(MoveCoroutine(moveAmount));
		}
		else
		{
			Debug.Log("Overshot");
		}
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

		GameManager.canInteract = true;
		CheckWin();
	}

	private void CheckWin()
	{
		if (currentCell == BoardCreator.tiles.Count)
		{
			OnWin?.Invoke();
		}
		else
		{
			OnMoveComplete?.Invoke();
		}
	}

	private int IncrementTarget(int moveAmount)
	{
		if (moveAmount + currentCell >= BoardCreator.tiles.Count)
		{
			return BoardCreator.tiles.Count;
		}

		return currentCell + moveAmount;
	}
}