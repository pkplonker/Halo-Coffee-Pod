using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private List<Sprite> diceSides;
	private SpriteRenderer spriteRenderer;
	[SerializeField] private float rollRefreshSpeed = 0.2f;
	[SerializeField] private float totalRollTime = 1f;
	private int currentRoll;
	public static event Action<int> onRollComplete;
	public static event Action onRollStarted;
	private Sprite defaultSprite;
	private PlayerMovement player;

	private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

	private void Start()
	{
		defaultSprite = spriteRenderer.sprite;
	}

	private void OnEnable()
	{
		QuestionController.OnCorrectAnswer += ShowDefaultSprite;
		GameManager.instance.OnNewGame += RegisterPlayer;

	} 

	private void OnDisable()
	{
		GameManager.instance.OnNewGame -= RegisterPlayer;
		QuestionController.OnCorrectAnswer -= ShowDefaultSprite;
	}

	private void RegisterPlayer(PlayerMovement player, QuestionController questionController)
	{
		this.player = player;
	}


	private void ShowDefaultSprite(PlayerMovement obj)
	{
		spriteRenderer.sprite = defaultSprite;
	}



	private int GenerateRandomNumber() => Mathf.CeilToInt(Random.Range(1, 7));

	private void Roll()
	{
		if (!GameManager.canInteract) return;
		SFXPlayer.instance.PlayDiceRoll();
		StartCoroutine(RollCoroutine());
	}

	private void ShowNewNumber(bool adjust=false)
	{
		currentRoll = GenerateRandomNumber() - 1;
		if(adjust) AdjustRollIfCanReachMaxCell();
		spriteRenderer.sprite = diceSides[currentRoll];
	}

	private void AdjustRollIfCanReachMaxCell()
	{
		if (player.GetCurrentCell() <= BoardCreator.tiles.Count - 6) return;
		currentRoll = BoardCreator.tiles.Count - player.GetCurrentCell() - 1;
		Debug.Log("Overriding roll to " + currentRoll);
	}

	private IEnumerator RollCoroutine()
	{
		var duration = 3.7f;
		//var x = transform.DOShakeRotation(duration,40,4);
		var sequence = DOTween.Sequence();
		sequence.Append(transform.DOJump(transform.position, 1, 6, duration))
			.Join(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 360), duration));
			
		GameManager.canInteract = false;
		onRollStarted?.Invoke();
		float totalRollTimer = 0;
		float changeTimer = 0;
		while (totalRollTimer < totalRollTime)
		{
			totalRollTimer += Time.deltaTime;
			changeTimer += Time.deltaTime;
			if (changeTimer > rollRefreshSpeed)
			{
				changeTimer = 0;
				
				ShowNewNumber();
			}

			yield return null;
		}
		ShowNewNumber(true);
		GameManager.canInteract = true;
		sequence.Complete();
		onRollComplete?.Invoke(currentRoll + 1);
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left) return;
		Roll();
	}

	public static void DebugRoll(int x)
	{
		onRollStarted?.Invoke();
		if (!GameManager.canInteract) return;
		onRollComplete?.Invoke(x);
	}
}