using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public event Action OnTimeStart;
	public event Action<float> OnTimeUpdated;
	public event Action OnTimeEnd;
	private float currentTime;
	private bool timeRunning;
	public float GetTime() => currentTime;

	private void Start() => timeRunning = true;


	private void Update()
	{
		if (!timeRunning) return;
		currentTime += Time.deltaTime;
		OnTimeUpdated?.Invoke(currentTime);
	}

	public void StartTime()
	{
		currentTime = 0;
		timeRunning = true;
		OnTimeStart?.Invoke();
	}

	public void StopTime()
	{
		timeRunning = false;
		OnTimeEnd?.Invoke();
	}
}