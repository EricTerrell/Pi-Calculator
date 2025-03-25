namespace Calculate_Pi;

using ExtendedNumerics;

// Based on https://www.cygnus-software.com/misc/pidigits.htm
// Uses this BigDecimal implementation: https://www.nuget.org/packages/ExtendedNumerics.BigDecimal/

public class CalculatePiMachin : CalculatePi
{
    public override AlgorithmInfo AlgorithmInfo => 
        new("Machin", "https://www.cygnus-software.com/misc/pidigits.htm");

    private static BigDecimal ATanInvInt(int x, CancellationTokenSource? cancellationTokenSource, 
        IProgress<string>? progress, ref int iterations)
    {
        var result = BigDecimal.Divide(new BigDecimal(1), new BigDecimal(x));
        var xSquared = new BigDecimal(x * x);

        var term = result;
        var divisor = new BigDecimal(1);

        var two = new BigDecimal(2);

        var maxValue = BigDecimal.Pow(10, -BigDecimal.Precision);
        
        while (term > maxValue)
        {
            if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
            {
                throw new CancelException();
            }

            divisor = BigDecimal.Add(divisor, two);

            term = BigDecimal.Divide(term,xSquared);
            
            result = BigDecimal.Subtract(result, BigDecimal.Divide(term, divisor));
            
            divisor = BigDecimal.Add(divisor, two);

            term = BigDecimal.Divide(term, xSquared);
            
            result = BigDecimal.Add(result, BigDecimal.Divide(term, divisor));
            
            progress?.Report($"{iterations++:N0} iterations");
        }

        return result;
    }
    
    protected override string CalculatePiDigits(int numberOfDigits, 
        CancellationTokenSource? cancellationTokenSource = null,
        IProgress<string>? progress = null)
    {
        BigDecimal.Precision = numberOfDigits + 10;

        numberOfDigits += 2; // want to calculate digits to the right of "3.".

        var iterations = 0;

        var ataninvint5 = ATanInvInt(5, cancellationTokenSource, progress, ref iterations);
        var ataninvint239 = ATanInvInt(239, cancellationTokenSource, progress, ref iterations);
        
        var digits = BigDecimal.Multiply(
            BigDecimal.Subtract(
            BigDecimal.Multiply(ataninvint5, new BigDecimal(4)),
            ataninvint239),
            new BigDecimal(4));
                
        return $"3{digits.ToString().Substring(2, numberOfDigits - 2)}";
    }
}