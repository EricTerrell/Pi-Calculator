namespace Calculate_Pi;

using System.Numerics;

public class CalculatePiAndrewJennings : CalculatePi
{
    // http://ajennings.net/blog/a-million-digits-of-pi-in-9-lines-of-javascript.html
    // https://math.tools/numbers/pi/1000000

    public override AlgorithmInfo AlgorithmInfo => new(
        "Andrew Jennings",
        "http://ajennings.net/blog/a-million-digits-of-pi-in-9-lines-of-javascript.html");

    protected override string CalculatePiDigits(int digits, CancellationToken cancellationToken, 
        IProgress<string>? progress = null)
    {
        var i = BigInteger.One;
        var x = new BigInteger(3) * BigInteger.Pow(10, digits + 20);
        var pi = BigInteger.Parse(x.ToString());
        var bi2 = new BigInteger(2);
        var bi4 = new BigInteger(4);

        var iterations = 0;
        
        while (x > BigInteger.Zero)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            x = (x * i) / ((i + BigInteger.One) * bi4);
            pi += x / (i + bi2);
            i += bi2;
            
            progress?.Report($"{++iterations:N0} iterations");
        }

        return pi.ToString()[..(digits + 1)];
    }
}