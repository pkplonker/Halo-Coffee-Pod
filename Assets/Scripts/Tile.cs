using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	private Canvas canvas;
	private TextMeshProUGUI textMeshProUGUI;
	private int id;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		canvas = GetComponentInChildren<Canvas>();
		textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
	}


	public void Setup(Camera cam, int id, Color cellColor, Color textColor)
	{
		SetCanvas(cam);
		SetID(id);
		SetText(id, textColor);
		SetColor(cellColor);
	}

	private void SetColor(Color cellColor)
	{
		spriteRenderer.color = cellColor;
	}

	private void SetText(int id, Color textColor)
	{
		textMeshProUGUI.text = id.ToString();
		textMeshProUGUI.color = textColor;
	}

	private void SetCanvas(Camera cam)
	{
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.worldCamera = cam;
	}

	private void SetID(int id)
	{
		this.id = id;
		gameObject.name = id.ToString();
	}
}