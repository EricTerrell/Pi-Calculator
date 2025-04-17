using System.Threading;

namespace Calculate_Pi.Tests;

using Calculate_Pi;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
[TestSubject(typeof(CalculatePi))]
public class CalculatePiTests
{
    private readonly CalculatePiFactory _calculatePiFactory = new();
    
    private void test_calculate_pi(string expectedValue, int digits)
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            _calculatePiFactory.AlgorithmInfos.ForEach(algorithm =>
            {
                var result = _calculatePiFactory
                    .CreatePiCalculator(algorithm.Name)
                    .Pi(digits, cancellationTokenSource.Token).digits;

                Assert.AreEqual(expectedValue, result);
            });
        }
    }

    [TestMethod]
    public void test_calculate_pi_0()
    {
        test_calculate_pi("3.\n", 0);
    }

    [TestMethod]
    public void test_calculate_pi_10()
    {
        test_calculate_pi("3.\n1415926535", 10);
    }

    [TestMethod]
    public void test_calculate_pi_100()
    {
        test_calculate_pi(
            "3.\n1415926535 8979323846 2643383279 5028841971 6939937510 5820974944 5923078164 0628620899 8628034825 3421170679",
                        100);
    }

    private void test_algorithms_give_identical_results(int digits)
    {
        var values = new List<string>();

        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            _calculatePiFactory.AlgorithmInfos.ForEach(algorithm =>
            {
                var result = _calculatePiFactory
                    .CreatePiCalculator(algorithm.Name)
                    .Pi(digits, cancellationTokenSource.Token).digits;

                values.Add(result);
            });
        }

        for (var i = 0; i < values.Count - 1; i++)
        {
            Assert.AreEqual(values[i], values[i + 1]);
        }
    }

    [TestMethod]
    public void test_algorithms_give_identical_results()
    {
        for (var i = 0; i <= 100; i++)
        {
            test_algorithms_give_identical_results(i);
        }
    }
}