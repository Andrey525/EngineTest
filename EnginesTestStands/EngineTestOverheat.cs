using Engines;

namespace EnginesTestStands
{
	public class EngineTestOverheat : EngineTestBase
	{
		public EngineTestOverheat(AbstractEngine engine, int maxRunTime) : base(engine, maxRunTime) { }
		public override async Task<object> Run(double ambientTemperature)
		{
			TestEngine.AmbientTemperature = ambientTemperature;
			TestEngine.CriticalTemperatureCallback += _cancelTokenSource.Cancel;

			var watch = System.Diagnostics.Stopwatch.StartNew();
			await TestEngine.Run(_token);

			watch.Stop();

			TestEngine.CriticalTemperatureCallback -= _cancelTokenSource.Cancel;
			_cancelTokenSource.Dispose();

			return new EngineTestOverheatResult
			{
				IsEngineOverheated = TestEngine.IsOverheated,
				EllapsedTimeBeforeOverheat = watch.Elapsed.TotalSeconds
			};
		}
	}

	public record EngineTestOverheatResult
	{
		public bool IsEngineOverheated { get; init; }
		public double EllapsedTimeBeforeOverheat { get; init; }
	}
}
