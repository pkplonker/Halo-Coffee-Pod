using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestionController : MonoBehaviour
{
	private PlayerMovement player;
	[SerializeField] private List<QuestionData> questions;
	private QuestionData currentQuestion;
	private List<QuestionData> unusedQuestions = new List<QuestionData>();
	private CanvasGroup canvasGroup;
	public event Action OnCorrectAnswer;
	public event Action OnWrongAnswer;
	[SerializeField] private TextMeshProUGUI questionText;
	[SerializeField] private Transform answerContainer;
	[SerializeField] private GameObject answerPrefab;
	private List<Button> buttons = new List<Button>();
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
	}

	private void OnMoveComplete()
	{
		currentQuestion = ChooseRandomQuestion();
		if (currentQuestion == null)
		{
			CloseUI();
			return;
		}

		GameManager.canInteract = false;

		ShowQuestion(currentQuestion);
		ShowUI();
	}

	public void Init(GameManager gameManager, PlayerMovement player)
	{
		this.player = player;
		player.OnMoveComplete += OnMoveComplete;
	}

	private QuestionData ChooseRandomQuestion()
	{
		return unusedQuestions.Count == 0 ? null : unusedQuestions[Random.Range(0, unusedQuestions.Count)];
	}

	private void ShowQuestion(QuestionData currentQuestion)
	{
		//populate question
		for (int i = 0; i < currentQuestion.answers.Length; i++)
		{
			var button = Instantiate(answerPrefab, answerContainer).GetComponent<Button>();
			button.onClick.AddListener(delegate { ClickAnswer(i); });
			button.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
			buttons.Add(button);
		}
	}

	private void CloseUI()
	{
		GameManager.canInteract = true;
		buttons.ForEach(b => Destroy(b.gameObject));
		buttons.Clear();
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void ShowUI()
	{
		GameManager.canInteract = false;
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public void ClickAnswer(int answer)
	{
		if (answer == currentQuestion.correctAnswer)
		{
			Debug.Log("Correct");
			OnCorrectAnswer?.Invoke();
		}
		else
		{
			Debug.Log("Wrong");
			OnWrongAnswer?.Invoke();
		}

		RemoveQuestion();
		CloseUI();
	}

	private void RemoveQuestion()
	{
		unusedQuestions.Remove(currentQuestion);
	}
}