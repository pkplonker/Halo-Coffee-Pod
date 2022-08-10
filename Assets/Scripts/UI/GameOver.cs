using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class GameOver : MonoBehaviour
	{
		public static GameOver instance;
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] WinLoseContainer winLoseContainer;
		[SerializeField] private string winMessage;
		[SerializeField] private string loseMessage;
		[SerializeField] private string timeMessage = "Your time was: ";
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private float showWinScreenFadeTime = 1f;

		private void OnEnable()
		{
			GameManager.instance.OnNewGame += HideUIONG;
			GameManager.instance.OnGameOver += OnWin;
		}

		private void HideUIONG(PlayerMovement arg1, QuestionController arg2) => HideUI();

		private IEnumerator ShowWinCor()
		{
			for (var t = 0f; t < showWinScreenFadeTime; t += Time.deltaTime)
			{
				var normalizedTime = t / showWinScreenFadeTime;
				canvasGroup.alpha = Mathf.Lerp(0, 1, normalizedTime);
				yield return null;
			}

			canvasGroup.alpha = 1;
		}

//ui button
		public void Menu()
		{
			SFXPlayer.instance.ClickSound();
			SceneManager.LoadScene($"MainMenu");
			HideUI();

		}

//ui button
		public async void Restart()
		{
			SFXPlayer.instance.ClickSound();
			var t= SceneManager.LoadSceneAsync($"Game");
			t.allowSceneActivation = false;
		await	Task.Delay(100);
		t.allowSceneActivation = true;

			HideUI();
		}

		public void Exit()
		{
			SFXPlayer.instance.ClickSound();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_WEBGL)
    Application.OpenURL("https://halo.coffee/");
#else
			Application.Quit();
#endif
		}

		private void Awake()
		{
			if (instance == null) instance = this;

			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			HideUI();
		} 


		private void HideUI()
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		public void OnWin()
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			scoreText.text = winLoseContainer.isWin ? winMessage : loseMessage;
			if (winLoseContainer.isWin)
			{
				timeText.enabled = true;
				timeText.text = timeMessage + TimerUI.FormatSeconds(winLoseContainer.time);
			}
			else timeText.enabled = false;

			StartCoroutine(ShowWinCor());
		}
	}
}