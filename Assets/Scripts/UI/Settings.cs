using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	[SerializeField] private AudioMixer audioMixer;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		Close();
	}

	public void Close()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	public void Show()
	{
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public void OpenSHLinkedIn()
	{
		Application.OpenURL("https://www.linkedin.com/in/stuartheath1/");
	}

	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
	}

	public void SetSFXVolume(float volume)
	{
		audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
	}

	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
	}
}