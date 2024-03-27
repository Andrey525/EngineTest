namespace Engines
{
	public struct EngineData
	{
		public double CriticalTemperature { get; init; }
		public double I { get; init; }
		public double Hm { get; init; }
		public double Hv { get; init; }
		public double C { get; init; }
		public IReadOnlyList<double> Moments { get; init; }
		public IReadOnlyList<double> СrankshaftSpeeds { get; init; }
	}


	public class Engine : AbstractEngine, IDisposable
	{
		private readonly EngineData _engineData;

		public override event Action? CriticalTemperatureCallback;
		public override event Action? MaxCrankshaftRotationSpeedCallback;

		public override double Vh
		{
			get
			{
				return CurrentMoment * _engineData.Hm + CurrentСrankshaftSpeed * CurrentСrankshaftSpeed * _engineData.Hv;
			}
		}

		public override double Vc
		{
			get
			{
				return _engineData.C * (AmbientTemperature - CurrentTemperature);
			}
		}

		private double SpeedUp
		{
			get
			{
				return CurrentMoment / _engineData.I;
			}
		}

		public Engine(EngineData engineData)
		{
			if (engineData.Moments.Count != engineData.СrankshaftSpeeds.Count)
			{
				throw new ArgumentException("Moments Count must be equal СrankshaftSpeeds Count");
			}
			_engineData = engineData;
			CurrentMoment = _engineData.Moments[0];
			CurrentСrankshaftSpeed = _engineData.СrankshaftSpeeds[0];
			CriticalTemperature = _engineData.CriticalTemperature;
		}

		public override async Task Run(CancellationToken token)
		{
			CurrentTemperature = AmbientTemperature;

			await Task.Run(() =>
			{
				int index = 0;
				while (true)
				{
					CurrentСrankshaftSpeed += SpeedUp;

					if (index < _engineData.Moments.Count - 2 &&
						CurrentСrankshaftSpeed >= _engineData.СrankshaftSpeeds[index + 1])
					{
						index++;
					}

					double delta1 = CurrentСrankshaftSpeed - _engineData.СrankshaftSpeeds[index];
					double delta2 = _engineData.СrankshaftSpeeds[index + 1] - _engineData.СrankshaftSpeeds[index];
					double delta3 = _engineData.Moments[index + 1] - _engineData.Moments[index];
					CurrentMoment = delta1 / delta2 * delta3 + _engineData.Moments[index];

					CurrentTemperature += Vc + Vh;

					if (IsOverheated)
					{
						/* Критическая температура */
						CriticalTemperatureCallback?.Invoke();
					}

					if (CurrentСrankshaftSpeed >= _engineData.СrankshaftSpeeds.Last() - 1)
					{
						/* Двигатель дошел до максимальных оборотов, погрешность 1 */
						MaxCrankshaftRotationSpeedCallback?.Invoke();
					}

					if (token.IsCancellationRequested)
						return;

					/* Условная задержка (каждая секунда реального времени равна 0.05 секунды виртуального) */
					Thread.Sleep(50);
				}
			});
		}

		public void Dispose()
		{
			CriticalTemperatureCallback = null;
			MaxCrankshaftRotationSpeedCallback = null;
		}
	}
}
