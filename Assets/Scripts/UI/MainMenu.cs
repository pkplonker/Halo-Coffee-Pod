using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		public void NewGame()
		{
			SceneManager.LoadScene($"Game");
		}


		public void Exit()
		{
			Debug.Log("Quitting...");
			Application.Quit();
		}
	}
}