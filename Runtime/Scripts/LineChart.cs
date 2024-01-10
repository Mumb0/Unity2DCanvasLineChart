using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace At.Ac.FhStp.CanvasLineChart {

	public class LineChart : MonoBehaviour {

#region Fields

		[Header("References")]
		[SerializeField] private GameObject datumPrefab;
		[SerializeField] private LineRenderer xAxis;
		[SerializeField] private LineRenderer yAxis;

		[Header("Values")]
		[SerializeField] private Color backgroundColor;
		[SerializeField] private Color dataPointColor;
		[SerializeField] private Color lineColor;
		[Range(0, 1)] [SerializeField] private float lineAlpha;
		[SerializeField] private float lineStrength;
		[SerializeField] private float axisStrength;
		[SerializeField] private float dataPointRadius;

		private float minValue = 0f;
		private float maxValue = 0f;
		private long minDateValue = 0;
		private long maxDateValue = 0;

#endregion

#region Properties

		public ChartArea ChartArea { get; private set; }
		public Dictionary<DateTime, float> ChartData { get; private set; }
		public ICollection<Datum> ChartDataPoints { get; private set; }
		private LineRenderer Line { get; set; }

#endregion

#region Methods

		public void SetChartArea(RectTransform chartRectArea) {
			ChartArea = new ChartArea(chartRectArea);
		}

		public void SetChartData(Dictionary<DateTime, float> chartData) {
			ChartData = chartData;
		}

		public void DrawChart(bool hasLine, bool hasBackground) {

			if (hasBackground) {
				var bg = gameObject.AddComponent<Image>();
				bg.color = backgroundColor;
			}

			if (ChartData != null && ChartData?.Count > 0) {

				ChartDataPoints = new List<Datum>();

				minDateValue = GetDateValues(ChartData.Keys).Min();
				maxDateValue = GetDateValues(ChartData.Keys).Max();

				minValue = ChartData.Values.Min();
				maxValue = ChartData.Values.Max();

				DrawAxis(minValue, maxValue, minDateValue, maxDateValue, 10, 10);

				foreach (KeyValuePair<DateTime, float> datum in ChartData) {

					float mappedDate = ChartMathExtensions.Map(datum.Key.ToUniversalTime().Ticks, minDateValue, maxDateValue, 0,
						(long)ChartArea.ChartWidth);
					float mappedValue = ChartMathExtensions.Map(datum.Value, minValue, maxValue, 0, ChartArea.ChartHeight);
					Vector3 dataPointPosition = new Vector3(mappedDate, mappedValue, 0);
					Datum newDataPoint = Instantiate(datumPrefab, Vector3.zero, Quaternion.identity, ChartArea.Container)
						.GetComponent<Datum>();
					newDataPoint.Time = datum.Key;
					newDataPoint.Value = datum.Value;
					newDataPoint.DataPointTransform = newDataPoint.GetComponent<RectTransform>();
					newDataPoint.DataPointTransform.anchoredPosition3D = dataPointPosition;
					newDataPoint.SetTransformSize(dataPointRadius);
					newDataPoint.GetComponent<Image>().color = dataPointColor;
					ChartDataPoints.Add(newDataPoint);

				}

				if (hasLine) {

					Line = gameObject.GetComponent<LineRenderer>();
					Line.enabled = true;
					Line.positionCount = ChartData.Count;
					Datum[] dataPoints = ChartDataPoints.ToArray();

					for (int i = 0; i <= dataPoints.Length - 1; i++) {
						Line.SetPosition(i, dataPoints[i].DataPointTransform.anchoredPosition3D);
					}

					Line.startWidth = lineStrength;
					Line.endWidth = lineStrength;

					Gradient gradient = new();
					gradient.SetKeys(
						new[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
						new[] { new GradientAlphaKey(lineAlpha, 0.0f), new GradientAlphaKey(lineAlpha, 1.0f) }
					);
					Line.colorGradient = gradient;

				}

			}
			else {
				throw new Exception("No Chart Data!");
			}

		}

		private void DrawAxis(float minValue, float maxValue, long minDateTicks, long maxDateTicks, int xTickAmount, int yTickAmount) {

			xAxis.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ChartArea.ChartWidth);
			xAxis.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ChartArea.ChartHeight);
			yAxis.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ChartArea.ChartWidth);
			yAxis.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ChartArea.ChartHeight);

			xAxis.startWidth = axisStrength;
			xAxis.endWidth = axisStrength;
			yAxis.startWidth = axisStrength;
			yAxis.endWidth = axisStrength;

			xAxis.positionCount = 2;
			yAxis.positionCount = 2;

			xAxis.SetPosition(0, new Vector3(0, 0));
			xAxis.SetPosition(1, new Vector3(ChartArea.ChartWidth, 0));
			yAxis.SetPosition(0, new Vector3(0, 0));
			yAxis.SetPosition(1, new Vector3(0, ChartArea.ChartHeight));

		}

		public void SetDataPointsRadius(float radius) {

			if (ChartDataPoints == null) {
				return;
			}

			foreach (Datum dp in ChartDataPoints) {
				dp.SetTransformSize(radius);
			}

		}

		private static IEnumerable<long> GetDateValues(IEnumerable<DateTime> dates) {
			return dates.Select(d => d.ToUniversalTime().Ticks);
		}

#endregion

	}

}