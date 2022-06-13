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

		public void Menu()
		{
			SFXPlayer.instance.ClickSound();
			SceneManager.LoadScene($"MainMenu");
		}

		public void Restart()
		{
			SFXPlayer.instance.ClickSound();
			SceneManager.LoadScene($"Game");
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