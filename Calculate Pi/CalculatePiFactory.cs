namespace Calculate_Pi;

public class CalculatePiFactory
{
    private readonly List<CalculatePi> piCalculators = [new CalculatePiAndrewJennings(), new CalculatePiMachin()];

    public List<AlgorithmInfo> AlgorithmInfos =>
        piCalculators.Select(piCalculator => piCalculator.AlgorithmInfo).ToList();

    public CalculatePi CreatePiCalculator(string algorithm)
    {
        var piCalculator = piCalculators.Find(piCalculator => piCalculator.AlgorithmInfo.Name == algorithm);

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