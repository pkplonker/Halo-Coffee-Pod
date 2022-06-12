using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class GameOver : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] WinLoseContainer winLoseContainer;
		[SerializeField] private string winMessage;
		[SerializeField] private string loseMessage;
		[SerializeField] private string timeMessage = "Your time was: ";
		[SerializeField] private TextMeshProUGUI timeText;

		private void Awake()
		{
			scoreText.text = winLoseContainer.isWin ? winMessage : loseMessage;
			if (winLoseContainer.isWin)
			{
				timeText.enabled=true;
				timeText.text = timeMessage + TimerUI.FormatSeconds(winLoseContainer.time);
			}
			else
			{
				timeText.enabled = false;
			}
		}

		public void Menu()
		{
			Debug.Log("Menu pressed");
			SceneManager.LoadScene($"MainMenu");
		}

		public void Restart()
		{
			Debug.Log("Restart pressed");

			SceneManager.LoadScene($"Game");
		}

		public void Exit()
		{
			Debug.Log("Quitting...");
			Application.Quit();
		}
	}
}