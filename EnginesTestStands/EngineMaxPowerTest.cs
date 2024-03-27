using Engines;
namespace EnginesTestStands
{
	public class EngineMaxPowerTest : EngineTestBase
	{
		public EngineMaxPowerTest(AbstractEngine engine, int maxRunTime) : base(engine, maxRunTime) { }

		public override async Task<object> Run(double ambientTemperature)
		{
			TestEngine.AmbientTemperature = ambientTemperature;
			TestEngine.CriticalTemperatureCallback += TaskCancelHandler;
			TestEngine.MaxCrankshaftRotationSpeedCallback += TaskCancelHandler;

			var task = TestEngine.Run(_token);


			double maxPower = 0;
			double crankshaftSpeed = 0;
			while (!_isCancelled)
			{
				var power = СalculatePower();
				if (power > maxPower)
				{
					maxPower = power;
					crankshaftSpeed = TestEngine.CurrentСrankshaftSpeed;
				}

				/* Будем проверять раз в t времени */
				Thread.Sleep(50);
			}

			await task;

			TestEngine.CriticalTemperatureCallback -= TaskCancelHandler;
			TestEngine.MaxCrankshaftRotationSpeedCallback -= TaskCancelHandler;
			_cancelTokenSource.Dispose();

			return new EngineMaxPowerTestResult
			{
				IsEngineOverheated = TestEngine.IsOverheated,
				MaxPower = maxPower,
				СrankshaftSpeed = crankshaftSpeed
			};
		}

		private double СalculatePower()
		{
			return TestEngine.CurrentMoment * TestEngine.CurrentСrankshaftSpeed / 1000;
		}

		private void TaskCancelHandler()
		{
			_cancelTokenSource.Cancel();
			_isCancelled = true;
		}
	}

	public record EngineMaxPowerTestResult
	{
		public bool IsEngineOverheated { get; init; }
		public double MaxPower { get; init; }
		public double СrankshaftSpeed { get; init; }
	}
}
