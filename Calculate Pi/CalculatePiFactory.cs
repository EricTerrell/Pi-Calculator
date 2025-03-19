namespace Calculate_Pi;

public class CalculatePiFactory
{
    private readonly List<CalculatePi> piCalculators = [new CalculatePiAndrewJennings(), new CalculatePiMachin()];

    public List<(string Name, string Url)> Algorithms =>
        piCalculators.Select(piCalculator => piCalculator.Algorithm).ToList();

    public CalculatePi CreatePiCalculator(string algorithm)
    {
        var piCalculator = piCalculators.Find(piCalculator => piCalculator.Algorithm.Name == algorithm);

        if (piCalculator == null)
        {
            throw new NotImplementedException($"Algorithm {algorithm} not implemented");
        }
        else
        {
            return piCalculator;
        }
    }
}