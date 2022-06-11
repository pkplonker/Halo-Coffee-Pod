using System;
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
			SceneManager.LoadScene($"Game");
		}


		public void Exit()
		{
			Debug.Log("Quitting...");
			Application.Quit();
		}

		public void ShowSettings()
		{
			Debug.Log("Show settings");
			settings.Show();
		}
	}
}