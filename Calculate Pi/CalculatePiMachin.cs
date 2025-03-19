namespace Calculate_Pi;

using ExtendedNumerics;

// Based on https://www.cygnus-software.com/misc/pidigits.htm

public class CalculatePiMachin : CalculatePi
{
    public override AlgorithmInfo AlgorithmInfo => 
        new("Machin", "https://www.cygnus-software.com/misc/pidigits.htm");

    private static int LeadingZeros(BigDecimal x)
    {
        var str = x.ToString();

        for (var i = 2; i < str.Length; i++)
        {
            if (str[i] != '0')
            {
                return i - 2;
            }
        }

        return str.Length;
    }
    
    private BigDecimal ATanInvInt(int x, IProgress<string>? progress)
    {
        var result = BigDecimal.Divide(new BigDecimal(1), new BigDecimal(x));
        var xSquared = new BigDecimal(x * x);

        var term = result;
        var divisor = new BigDecimal(1);

        var two = new BigDecimal(2);
        
        while (LeadingZeros(term) < BigDecimal.Precision)
        {
            divisor = BigDecimal.Add(divisor, two);

            term = BigDecimal.Divide(term,xSquared);
            
            result = BigDecimal.Subtract(result, BigDecimal.Divide(term, divisor));
            
            divisor = BigDecimal.Add(divisor, two);

            term = BigDecimal.Divide(term, xSquared);
            
            result = BigDecimal.Add(result, BigDecimal.Divide(term, divisor));
            
            progress?.Report($"{LeadingZeros(term):N0} digits");
        }

        return result;
    }
    
    protected override string CalculatePiDigits(int numberOfDigits, 
        CancellationTokenSource? cancellationTokenSource = null,
        IProgress<string>? progress = null)
    {
        BigDecimal.Precision = numberOfDigits + 10;

        numberOfDigits += 2; // want to calculate digits to the right of "3.".

        var digits = BigDecimal.Multiply(
            BigDecimal.Subtract(
            BigDecimal.Multiply(ATanInvInt(5, progress), new BigDecimal(4)),
            ATanInvInt(239, progress)),
            new BigDecimal(4));
                
        return $"3{digits.ToString().Substring(2, numberOfDigits - 2)}";
    }
}