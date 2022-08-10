using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestionController : MonoBehaviour
{
	[SerializeField] private List<QuestionData> questions;
	[SerializeField] private TextMeshProUGUI questionText;
	[SerializeField] private Transform answerContainer;
	[SerializeField] private GameObject answerPrefab;
	[SerializeField] private Color correctColor;
	[SerializeField] private Color wrongColor;
	[SerializeField] private float closeDelay;
	[SerializeField] private Image closeButton;
	private PlayerMovement player;
	private QuestionData currentQuestion;
	private List<QuestionData> unusedQuestions = new List<QuestionData>();
	private CanvasGroup canvasGroup;
	private List<Button> buttons = new List<Button>();
	private bool optionChosenThisQuestion = false;

	public static event Action<PlayerMovement> OnCorrectAnswer;
	public static event Action<PlayerMovement> OnWrongAnswer;
	public PlayerMovement GetPlayer() => player;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		unusedQuestions = new List<QuestionData>(questions);
		CloseUI();
	}

	private void OnDisable()
	{
		if (player == null) return;
		player.OnMoveComplete -= OnMoveComplete;
		Dice.onRollStarted -= OnRollStarted;
	}

	private void OnMoveComplete()
	{
		GameManager.canInteract = false;
		currentQuestion = ChooseRandomQuestion();
		if (currentQuestion == null)
		{
			CloseUI();
			return;
		}

		ShowQuestion(currentQuestion);
		ShowUI();
	}

	public void Init(GameManager gameManager, PlayerMovement player)
	{
		this.player = player;
		player.OnMoveComplete += OnMoveComplete;
		Dice.onRollStarted += OnRollStarted;

	}

	private void OnRollStarted()=>CloseUI();
	
	//ui button
	public void CloseButton()=> CloseUI();

	private QuestionData ChooseRandomQuestion()
	{
		return unusedQuestions.Count == 0 ? null : unusedQuestions[Random.Range(0, unusedQuestions.Count)];
	}

	private void ShowQuestion(QuestionData currentQuestion)
	{
		//populate question
		for (var i = 0; i < currentQuestion.answers.Length; i++)
		{
			var button = Instantiate(answerPrefab, answerContainer).GetComponent<Button>();
			var i1 = i;
			button.onClick.AddListener(delegate { ClickAnswer(i1); });
			button.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
			buttons.Add(button);
		}

		questionText.text = currentQuestion.question;
	}

	private void CloseUI()
	{
		GameManager.canInteract = true;
		buttons.ForEach(b => Destroy(b.gameObject));
		buttons.Clear();
		if (canvasGroup == null)
		{
			Debug.Log("err");
			throw new Exception("canvasGroup is null");
		}
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void ShowUI()
	{
		closeButton.enabled = false;
		GameManager.canInteract = false;
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		optionChosenThisQuestion = false;
	}

	private void ClickAnswer(int selectedAnswer)
	{
		if (optionChosenThisQuestion) return;
		optionChosenThisQuestion = true;
		if (selectedAnswer + 1 == currentQuestion.correctAnswer) OnCorrectAnswer?.Invoke(player);
		else OnWrongAnswer?.Invoke(player);

		DisplayResult(selectedAnswer);
		RemoveQuestion();
	}

	private void DisplayResult(int selectedAnswerIndex)
	{
		
		closeButton.enabled = true;
		if (currentQuestion.correctAnswer == selectedAnswerIndex + 1) ShowCorrectAnswer();
		else
		{
			ShowCorrectAnswer();
			ShowWrongAnswer(selectedAnswerIndex);
			ShowAdditionalInformation();
		}

		GameManager.canInteract = true;
	}

	private void ShowAdditionalInformation()
	{
//todo show additional information

}


	private void ShowWrongAnswer(int selectedAnswerIndex)
	{
		buttons[selectedAnswerIndex].colors = new ColorBlock
		{
			normalColor = wrongColor,
			highlightedColor = wrongColor,
			pressedColor = wrongColor,
			disabledColor = wrongColor,
			colorMultiplier = 1
		};
	}

	private void ShowCorrectAnswer()
	{
		buttons[currentQuestion.correctAnswer - 1].colors = new ColorBlock
		{
			normalColor = correctColor,
			highlightedColor = correctColor,
			pressedColor = correctColor,
			disabledColor = correctColor,
			colorMultiplier = 1
		};
	}

	private void RemoveQuestion() => unusedQuestions.Remove(currentQuestion);
}