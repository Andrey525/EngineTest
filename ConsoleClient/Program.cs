using Engines;
using EnginesTestStands;

namespace EngineTest
{
	internal class Program
	{
		public static void Main()
		{
			var task = Run();
			task.Wait();
		}

		private static async Task Run()
		{
			double ambientTemperature = 0;

			while (true)
			{
				try
				{
					Console.Write("Enter ambient temperature: ");
					ambientTemperature = Convert.ToDouble(Console.ReadLine());
				}
				catch
				{
					continue;
				}
				break;
			}

			var engineData = new EngineData
			{
				I = 10,
				Hm = 0.01,
				Hv = 0.0001,
				C = 0.1,
				Moments = [20, 75, 100, 105, 75, 0],
				СrankshaftSpeeds = [0, 75, 150, 200, 250, 300],
				CriticalTemperature = 110
			};

			int maxRunTime = 10;

			/* First test */
			try
			{
				var test = new EngineTestOverheat(new Engine(engineData), maxRunTime);

				Console.WriteLine($"Max test run time is {maxRunTime} seconds. Please wait!");

				var result = (EngineTestOverheatResult)await test.Run(ambientTemperature);

				if (result.IsEngineOverheated)
				{
					Console.WriteLine($"Engine is overheated: ellapsed time {result.EllapsedTimeBeforeOverheat:f}");
				}
				else
				{
					Console.WriteLine($"Engine is not overheated: ellapsed time {result.EllapsedTimeBeforeOverheat:f}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}

			/* Second test */
			try
			{
				var test = new EngineMaxPowerTest(new Engine(engineData), maxRunTime);

				Console.WriteLine($"Max test run time is {maxRunTime} seconds. Please wait!");

				var result = (EngineMaxPowerTestResult)await test.Run(ambientTemperature);

				Console.WriteLine($"Max Power: {result.MaxPower:f}; Сrankshaft Speed: {result.СrankshaftSpeed:f}");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}
}
