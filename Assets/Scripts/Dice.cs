using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private List<Sprite> diceSides;
	private SpriteRenderer spriteRenderer;
	[SerializeField] private float rollRefreshSpeed = 0.2f;
	[SerializeField] private float totalRollTime = 1f;
	private int currentRoll;
	public static event Action<int> onRollComplete;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}


	private void Roll()
	{
		if (!GameManager.canInteract) return;
		StartCoroutine(RollCoroutine());
	}

	private int GenerateRandomNumber()
	{
		return Mathf.CeilToInt(Random.Range(1, 7));
	}

	private void ShowNewNumber()
	{
		currentRoll = GenerateRandomNumber() - 1;
		spriteRenderer.sprite = diceSides[currentRoll];
	}

	private IEnumerator RollCoroutine()
	{
		GameManager.canInteract = false;
		float totalRollTimer = 0;
		float chanageTimer = 0;
		while (totalRollTimer < totalRollTime)
		{
			totalRollTimer += Time.deltaTime;
			chanageTimer += Time.deltaTime;
			if (chanageTimer > rollRefreshSpeed)
			{
				chanageTimer = 0;
				ShowNewNumber();
			}

			yield return null;
		}

		GameManager.canInteract = true;
		onRollComplete?.Invoke(currentRoll+1);
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left) return;

		Roll();
	}

	public static void DebugRoll(int x)
	{
		if (!GameManager.canInteract) return;
		onRollComplete?.Invoke(x);
	}
}