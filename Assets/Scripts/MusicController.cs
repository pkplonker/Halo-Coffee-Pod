using UnityEngine;

public class MusicController : MonoBehaviour
{
	public static MusicController instance;

	private void Awake()
	{
		if (instance == null) instance = this;
		else if (instance != this) Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
	
}