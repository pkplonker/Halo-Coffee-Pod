using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		private Settings settings;

		private void Awake()
		{
			settings = GetComponentInChildren<Settings>();
		}

		public void NewGame()
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

		public void ShowSettings()
		{
			SFXPlayer.instance.ClickSound();
			settings.Show();
		}
	}
}