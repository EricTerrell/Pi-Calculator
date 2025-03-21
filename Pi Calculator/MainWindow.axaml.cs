namespace Pi_Calculator;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Calculate_Pi;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private CancellationTokenSource? _cancellationSource;

    private readonly CalculatePiFactory _calculatePiFactory = new();
    
    private DateTime _cancellationStartTime;

    public List<AlgorithmInfo> AlgorithmInfos => _calculatePiFactory.AlgorithmInfos;

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool _isCalculating;
    
    public bool IsCalculating 
    { 
        get => _isCalculating;
        private set
        {
            _isCalculating = value;
            OnPropertyChanged();
        } 
    }

    private bool _canCancel;
    
    public bool CanCancel 
    { 
        get => _canCancel;
        private set
        {
            _canCancel = value;
            OnPropertyChanged();
        } 
    }
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        AlgorithmComboBox.SelectedIndex = 0;
    }

    private async void CalculateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            IsCalculating = true;
            CanCancel = true;
            
            Result.Content = string.Empty;
            
            Status.Content = "Calculating...";

            var digits = (int)Digits.Value.GetValueOrDefault();

            _cancellationSource = new CancellationTokenSource();

            var progress = new Progress<string>(progressMessage => { Status.Content = progressMessage; });

            var name = ((AlgorithmInfo) AlgorithmComboBox.SelectedItem).Name;
            
            var result = await Task.Run(() => _calculatePiFactory
                .CreatePiCalculator(name)
                .Pi(digits, _cancellationSource, progress));

            Result.Content = $"π ≈\n\n{result.digits}";
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
            
            IsCalculating = false;
            CanCancel = false;
        }
    }

    private void Digits_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        Result.Content = string.Empty;
        Status.Content = string.Empty;
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CanCancel = false;
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
        var name = ((AlgorithmInfo)AlgorithmComboBox.SelectedValue).Name;

        var url = _calculatePiFactory.AlgorithmInfos.Find(
            x => x.Name == name
        ).Url;
        
        AlgorithmWebPage.NavigateUri = new Uri(url);
    }
}