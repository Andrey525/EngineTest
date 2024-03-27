using Engines;
using EnginesTestStands;
using System.Windows;
using System.Windows.Controls;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		double _ambientTemperature;
		int _maxRunTime = 10;
		EngineData _engineData = new EngineData
		{
			I = 10,
			Hm = 0.01,
			Hv = 0.0001,
			C = 0.1,
			Moments = [20, 75, 100, 105, 75, 0],
			СrankshaftSpeeds = [0, 75, 150, 200, 250, 300],
			CriticalTemperature = 110
		};
		public MainWindow()
		{
			InitializeComponent();
			restartButton.Visibility = Visibility.Collapsed;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_ambientTemperature = Convert.ToDouble(textbox.Text);

				button.Visibility = Visibility.Collapsed;
				textbox.Visibility = Visibility.Collapsed;

				var task = Run();
			}
			catch { }
		}

		private void Restart_Button_Click(object sender, RoutedEventArgs e)
		{
			restartButton.Visibility = Visibility.Collapsed;
			button.Visibility = Visibility.Visible;
			textbox.Visibility = Visibility.Visible;

			textbox.Clear();
			textbox.Text = "Enter ambient temperature";
			label.Content = string.Empty;
		}

		private async Task Run()
		{
			label.Content = $"Max per test run time is {_maxRunTime} seconds. Please wait!";

			/* First test */
			try
			{
				var test = new EngineTestOverheat(new Engine(_engineData), _maxRunTime);


				var result = (EngineTestOverheatResult)await test.Run(_ambientTemperature);
				if (result.IsEngineOverheated)
				{
					label.Content = $"Engine is overheated: ellapsed time {result.EllapsedTimeBeforeOverheat:f}\n";
				}
				else
				{
					label.Content = $"Engine is not overheated: ellapsed time {result.EllapsedTimeBeforeOverheat:f}\n";
				}
			}
			catch (Exception ex)
			{
				label.Content = ex.Message;
			}

			/* Second test */
			try
			{
				var test = new EngineMaxPowerTest(new Engine(_engineData), _maxRunTime);
				var result = (EngineMaxPowerTestResult)await test.Run(_ambientTemperature);
				label.Content += $"Max Power: {result.MaxPower:f}; Сrankshaft Speed: {result.СrankshaftSpeed:f}";
			}
			catch (Exception ex)
			{
				label.Content = "Error: " + ex.Message;
			}

			restartButton.Visibility = Visibility.Visible;
		}
	}
}