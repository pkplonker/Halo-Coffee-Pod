using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private float scaleAmount = 1.1f;
		[SerializeField] private float scaleDuration = 0.2f;
		
		public void OnPointerEnter(PointerEventData eventData) => Hover();
		
		private void Hover()
		{
			transform.DOScale(Vector3.one * scaleAmount, 0.2f);
		}

		public void OnPointerExit(PointerEventData eventData) => UnHover();

		private void UnHover()
		{
			transform.DOScale(Vector3.one, 0.2f);
		}
	}
}