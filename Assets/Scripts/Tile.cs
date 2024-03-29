using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	private Canvas canvas;
	private TextMeshProUGUI textMeshProUGUI;
	private int id;
	private bool canBeSnake;
	private bool canBeLadder;
	public bool GetCanBeSnake() => canBeSnake;
	public bool GetCanBeLadder() => canBeLadder;
	public int GetId() => id;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		canvas = GetComponentInChildren<Canvas>();
		textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
	}


	public void Setup(Camera cam, int id, Color cellColor, Color textColor, bool canBeSnake, bool canBeLadder)
	{
		SetCanvas(cam);
		SetID(id);
		SetText(id, textColor);
		SetColor(cellColor);
		SetCanBeSnakeAndLadder(canBeSnake, canBeLadder);
	}

	private void SetCanBeSnakeAndLadder(bool canBeSnake, bool canBeLadder)
	{
		this.canBeSnake = canBeSnake;
		this.canBeLadder = canBeLadder;
	}

	public void SetColor(Color cellColor) => spriteRenderer.color = cellColor;


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