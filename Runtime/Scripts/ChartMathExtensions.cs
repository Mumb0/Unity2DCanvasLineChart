namespace At.Ac.FhStp.CanvasLineChart {

	public static class ChartMathExtensions {

		public static float Map(float value, float x1, float x2, float y1, float y2) {
			return (value - x1) / (x2 - x1) * (y2 - y1) + y1;
		}

		public static float Map(long value, long x1, long x2, long y1, long y2) {
			float division = (float)(value - x1) / (x2 - x1);
			return division * (y2 - y1) + y1;
		}

	}

}