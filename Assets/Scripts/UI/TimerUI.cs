using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI timerText;
	[SerializeField] Timer timer;
	private void OnEnable() => timer.OnTimeUpdated += UpdateTimer;


	private void OnDisable() => timer.OnTimeUpdated -= UpdateTimer;


	public void UpdateTimer(float time) => timerText.text = FormatSeconds(time);


	public static string FormatSeconds(float elapsed)
	{
		var d = (int) (elapsed * 100.0f);
		var minutes = d / (60 * 100);
		var seconds = (d % (60 * 100)) / 100;
		return $"{minutes:00}:{seconds:00}";
	}
}