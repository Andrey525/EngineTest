namespace Engines
{
	public class Engine : AbstractEngine, IDisposable
	{
		private const double I = 10;
		private const double Hm = 0.01;
		private const double Hv = 0.0001;
		private const double C = 0.1;
		private IReadOnlyList<double> Moments = [20, 75, 100, 105, 75, 0];
		private IReadOnlyList<double> СrankshaftSpeeds = [0, 75, 150, 200, 250, 300];

		public override event Action? CriticalTemperatureCallback;
		public override event Action? MaxCrankshaftRotationSpeedCallback;

		public override double Vh
		{
			get
			{
				return CurrentMoment * Hm + CurrentСrankshaftSpeed * CurrentСrankshaftSpeed * Hv;
			}
		}

		public override double Vc
		{
			get
			{
				return C * (AmbientTemperature - CurrentTemperature);
			}
		}

		public Engine()
		{
			CurrentMoment = Moments[0];
			CurrentСrankshaftSpeed = СrankshaftSpeeds[0];
			CriticalTemperature = 110;
		}

		public override async Task Run(CancellationToken token)
		{
			CurrentTemperature = AmbientTemperature;

			await Task.Run(() =>
			{
				int index = 0;
				while (true)
				{
					double a = CurrentMoment / I;

					CurrentСrankshaftSpeed += a;

					if (index < Moments.Count - 2)
						index += CurrentСrankshaftSpeed < СrankshaftSpeeds[index + 1] ? 0 : 1;

					double delta1 = CurrentСrankshaftSpeed - СrankshaftSpeeds[index];
					double delta2 = СrankshaftSpeeds[index + 1] - СrankshaftSpeeds[index];
					double delta3 = Moments[index + 1] - Moments[index];
					CurrentMoment = delta1 / delta2 * delta3 + Moments[index];

					CurrentTemperature += Vc + Vh;


					if (CurrentTemperature > CriticalTemperature)
					{
						/* Критическая температура */
						/*Console.WriteLine($"Критическая температура");*/
						CriticalTemperatureCallback?.Invoke();
					}

					if (CurrentСrankshaftSpeed >= СrankshaftSpeeds.Last() - 1)
					{
						/* Двигатель дошел до максимальных оборотов, погрешность 1 */
						/*Console.WriteLine($"Двигатель дошел до максимальных оборотов, погрешность ");*/
						MaxCrankshaftRotationSpeedCallback?.Invoke();
					}

					/*Console.WriteLine($"temp {CurrentTemperature}; speed {CurrentСrankshaftSpeed}; moment {CurrentMoment}");*/

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
