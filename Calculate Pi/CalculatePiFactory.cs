namespace Calculate_Pi;

public class CalculatePiFactory
{
    private readonly List<CalculatePi> _piCalculators = 
        [new CalculatePiAndrewJennings(), new CalculatePiMachin(), new CalculatePiPlouffeBellard()];

    public List<AlgorithmInfo> AlgorithmInfos =>
        _piCalculators.Select(piCalculator => piCalculator.AlgorithmInfo).ToList();

    public CalculatePi CreatePiCalculator(string algorithm)
    {
        var piCalculator = _piCalculators.Find(piCalculator => piCalculator.AlgorithmInfo.Name == algorithm);

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