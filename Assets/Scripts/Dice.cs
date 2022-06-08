using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
	[SerializeField] private List<Sprite> diceSides;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Roll();
		}
	}

	private void Roll()
	{
		spriteRenderer.sprite = diceSides[GenerateRandomNumber()-1];
	}

	private int GenerateRandomNumber()
	{
		return Mathf.CeilToInt(Random.Range(1, 7));
	}
}