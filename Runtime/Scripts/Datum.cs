using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace At.Ac.FhStp.CanvasLineChart {

	public class Datum : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {

#region Events

		public event Action<DateTime, float> datumHoverEvent;
		public event Action<DateTime, float> datumClickEvent;
		public event Action datumExitEvent;

#endregion

#region Properties

		public float Value { get; set; }
		public DateTime Time { get; set; }
		public RectTransform DataPointTransform { get; set; }

#endregion

#region Methods

		public void OnPointerEnter(PointerEventData eventData) {
			SetTransformSize(DataPointTransform.rect.height * 1.5f);
			datumHoverEvent?.Invoke(Time, Value);
		}

		public void OnPointerClick(PointerEventData eventData) {
			datumClickEvent?.Invoke(Time, Value);
		}

		public void OnPointerExit(PointerEventData eventData) {
			SetTransformSize(DataPointTransform.rect.height / 1.5f);
			datumExitEvent?.Invoke();
		}

		public void SetTransformSize(float radius) {

			DataPointTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius);
			DataPointTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius);

		}

#endregion

	}

}