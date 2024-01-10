using UnityEngine;

namespace At.Ac.FhStp.CanvasLineChart {

	public class ChartArea {

#region Properties

		public RectTransform Container { get; }
		public float ChartWidth => Container.rect.width;
		public float ChartHeight => Container.rect.height;
		public Vector2 ChartPosition => Container.anchoredPosition;

#endregion

#region Constructor

		public ChartArea(RectTransform chartArea) {
			Container = chartArea;
		}

#endregion

	}

}