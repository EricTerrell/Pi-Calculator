namespace Calculate_Pi;

using System.Text;

public abstract class CalculatePi
{
    private const int SpaceFreq   =  10;
    private const int NewlineFreq = 100;

    public abstract AlgorithmInfo AlgorithmInfo
    {
        get;
    }
    
    public (string digits, TimeSpan runtime) Pi(int digits, CancellationTokenSource? cancellationTokenSource = null, 
        IProgress<string>? progress = null)
    {
        var startTime = DateTime.Now;

        var pi = CalculatePiDigits(digits, cancellationTokenSource, progress);
        
        var result = Format(pi[..(digits + 1)]);

        return (result, DateTime.Now - startTime);
    }

    protected abstract string CalculatePiDigits(int digits, CancellationTokenSource? cancellationTokenSource = null, 
        IProgress<string>? progress = null); 

    private static string Format(string digits)
    {
        var result = new StringBuilder();

        for (var i = 1; i < digits.Length; i++)
        {
            result = result.Append(digits[i]);

            if (i % NewlineFreq == 0)
            {
                result = result.Append('\n');
            }
            else if (i % SpaceFreq == 0 && i < digits.Length - 1)
            {
                result = result.Append(' ');
            }
        }

        return $"3.\n{result.ToString().TrimEnd()}";
    }
}