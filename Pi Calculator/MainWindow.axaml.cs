using System.Linq;

namespace Pi_Calculator;

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Calculate_Pi;

public partial class MainWindow : Window
{
    private CancellationTokenSource? _cancellationSource;

    private readonly CalculatePiFactory _calculatePiFactory;
    
    private DateTime _cancellationStartTime;

    public MainWindow()
    {
        InitializeComponent();

        _calculatePiFactory = new CalculatePiFactory();
        
        Algorithms.ItemsSource = _calculatePiFactory.Algorithms.Select(x => x.Name).ToList();
        
        Algorithms.SelectedIndex = 0;
    }

    private async void CalculateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Result.Content = string.Empty;
            
            CalculateButton.IsEnabled = false;
            Digits.IsEnabled = false;
            Algorithms.IsEnabled = false;

            CancelButton.IsEnabled = true;

            Status.Content = "Calculating...";

            var digits = (int)Digits.Value.GetValueOrDefault();

            _cancellationSource = new CancellationTokenSource();

            var progress = new Progress<string>(progressMessage => { Status.Content = progressMessage; });

            var result = await Task.Run(() => _calculatePiFactory
                .CreatePiCalculator(Algorithms.SelectedItem.ToString())
                .Pi(digits, _cancellationSource, progress));

            Result.Content = $"π =\n\n{result.digits}";
            Status.Content = $"Elapsed Time: {result.runtime}";
        }
        catch (CancelException)
        {
            Status.Content = $"Cancelled. Elapsed Time: {DateTime.Now - _cancellationStartTime}";
        }
        catch (Exception ex)
        {
            Status.Content = ex.Message;
        }
        finally
        {
            _cancellationSource?.Dispose();
            
            CalculateButton.IsEnabled = true;
            Digits.IsEnabled = true;
            CopyButton.IsEnabled = true;
            Algorithms.IsEnabled = true;
            
            CancelButton.IsEnabled = false;
        }
    }

    private void Digits_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        Result.Content = string.Empty;
        Status.Content = string.Empty;
        
        CopyButton.IsEnabled = false;
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CancelButton.IsEnabled = false;
        Status.Content = "Cancelling...";
        
        _cancellationStartTime = DateTime.Now;
        
        // Cancel the cancellation token
        _cancellationSource?.Cancel();
    }

    private void CopyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Clipboard.SetTextAsync(Result.Content.ToString());
    }

    private void Algorithms_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var url = _calculatePiFactory.Algorithms.Find(x => 
            x.Name == Algorithms?.SelectedValue?.ToString()).Url;
        
        AlgorithmWebPage.NavigateUri = new Uri(url);
    }
}