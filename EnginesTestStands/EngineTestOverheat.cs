using Engines;

namespace EnginesTestStands
{
	public class EngineTestOverheat : EngineTestBase
	{
		public EngineTestOverheat(AbstractEngine engine) : base(engine) { }
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
				IsEngineOverheated = TestEngine.CurrentTemperature >= TestEngine.CriticalTemperature,
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
