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
		public void Menu() => SceneManager.LoadScene($"MainMenu");
		public void Restart() => SceneManager.LoadScene($"Game");

		public void Exit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("https://halo.coffee/");
#endif
		}

		private void Awake()
		{
			scoreText.text = winLoseContainer.isWin ? winMessage : loseMessage;
			if (winLoseContainer.isWin)
			{
				timeText.enabled = true;
				timeText.text = timeMessage + TimerUI.FormatSeconds(winLoseContainer.time);
			}
			else timeText.enabled = false;
		}
	}
}