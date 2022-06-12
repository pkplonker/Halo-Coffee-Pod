using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		private Settings settings;
		private void Awake() => settings = GetComponentInChildren<Settings>();
		public void NewGame() => SceneManager.LoadScene($"Game");

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

		public void ShowSettings() => settings.Show();
	}
}