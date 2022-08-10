using System;
using UnityEngine;

namespace UI
{
	public class HUDController : MonoBehaviour
	{
		private CanvasGroup canvasGroup;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			HideUI();
		} 


		private void OnEnable()
		{
			GameManager.instance.OnNewGame += OnNewGame;
			GameManager.instance.OnGameOver += OnGameOver;
		}

		private void OnGameOver() => HideUI();

		private void HideUI()
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}


		private void OnNewGame(PlayerMovement arg1, QuestionController arg2) => ShowUI();

		private void ShowUI()
		{
			canvasGroup.alpha = 1;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		private void OnDisable()
		{
			GameManager.instance.OnNewGame -= OnNewGame;
			GameManager.instance.OnGameOver -= OnGameOver;

		}
	}
}