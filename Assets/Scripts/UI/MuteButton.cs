using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
	public class MuteButton : MonoBehaviour
	{
		[SerializeField] private Image muteImage;
		[SerializeField] private AudioMixer audioMixer;
		private void Awake() => ToggleMute();

		public void ToggleMute()
		{
			muteImage.enabled = !muteImage.enabled;
			audioMixer.SetFloat("MasterVol", Mathf.Log10(muteImage.enabled?0.0001f:1f) * 20);
		}
	}
}