namespace Calculate_Pi;

using System.Text;

/*
 * Computation of the n'th decimal digit of \pi with very little memory.
 * Written by Fabrice Bellard on January 8, 1997.
 *
 * We use a slightly modified version of the method described by Simon
 * Plouffe in "On the Computation of the n'th decimal digit of various
 * transcendental numbers" (November 1996). We have modified the algorithm
 * to get a running time of O(n^2) instead of O(n^3log(n)^3).
 *
 * This program uses mostly integer arithmetic. It may be slow on some
 * hardwares where integer multiplications and divisons must be done
 * by software. We have supposed that 'int' has a size of 32 bits. If
 * your compiler supports 'long long' integers of 64 bits, you may use
 * the integer version of 'mul_mod' (see HAS_LONG_LONG).
 *
 * https://bellard.org/pi/pi.c
 */

public class CalculatePiPlouffeBellard : CalculatePi
{
    public override AlgorithmInfo AlgorithmInfo => new AlgorithmInfo("Plouffe / Bellard",
        "https://bellard.org/pi/pi.c");

    protected override string CalculatePiDigits(int digits, CancellationTokenSource? cancellationTokenSource = null,
        IProgress<string>? progress = null)
    {
	    var pi = new StringBuilder("3");

	    for (var n = 1; n <= digits; n += DigitsReturned)
	    {
		    if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
		    {
			    throw new CancelException();
		    }

		    var currentDigits = CalculatePiDigits(n);

		    pi = pi.Append(currentDigits);
		    
		    progress?.Report($"Digit: {n + DigitsReturned - 1:N0}");
	    }
	    
	    return pi.ToString()[..(digits + 1)];
    }

    private const int DigitsReturned = 9;

    private static int mul_mod(int a, int b, int m) 
	{
		return (int) (((long) a * (long) b) % m);
	}

	/* return the inverse of x mod y */
	private static int inv_mod(int x, int y) 
	{
		int q,u,v,a,c,t;

		u = x;
		v = y;
		c = 1;
		a = 0;

		do 
		{
			q = v / u;

			t = c;
			c = a - q * c;
			a = t;

			t = u;
			u = v - q * u;
			v = t;
		} while (u != 0);

		a %= y;

		if (a < 0) 
		{
			a = y + a;
		}

		return a;
	}

	/* return (a^b) mod m */
	private static int pow_mod(int a, int b, int m)
	{
		var r = 1;
		var aa = a;

		while (true) 
		{
			if ((b & 1) != 0)
			{
				r = mul_mod(r, aa, m);
			}

			b >>= 1;

			if (b == 0)
			{
				break;
			}

			aa = mul_mod(aa, aa, m);
		}

		return r;
	}

	/* return true if n is prime */
	private static bool is_prime(int n)
	{
		if ((n % 2) == 0) 
		{
			return false;
		}

		var r = (int) Math.Sqrt(n);

		for (var i = 3; i <= r; i += 2)
		{
			if ((n % i) == 0) 
			{
				return false;
			}
		}

		return true;
	}

	/* return the prime number immediately after n */
	private static int next_prime(int n)
	{
		do 
		{
			n++;
		} while (!is_prime(n));

		return n;
	}

	private static string CalculatePiDigits(int n)
	{
		int av, vmax, num, den, s, t;

		var N = (int) ((n + 20) * Math.Log(10) / Math.Log(2));

		double sum = 0;

		for (var a = 3; a <= (2 * N); a = next_prime(a)) 
		{
			vmax = (int) (Math.Log(2 * N) / Math.Log(a));

			av = 1;

			for (var i = 0; i < vmax; i++) 
			{
				av *= a;
			}

			s = 0;
			num = 1;
			den = 1;
			var v = 0;
			var kq = 1;
			var kq2 = 1;

			for (var k = 1; k <= N; k++) 
			{

				t = k;

				if (kq >= a) 
				{
					do 
					{
						t /= a;
						v--;
					} while ((t % a) == 0);

					kq = 0;
				}
				kq++;
				num = mul_mod(num, t, av);

				t = 2 * k - 1;

				if (kq2 >= a) 
				{
					if (kq2 == a) 
					{
						do 
						{
							t /= a;
							v++;
						} while ((t % a) == 0);
					}
					kq2 -= a;
				}
				den = mul_mod(den, t, av);
				kq2 += 2;
  
				if (v > 0) 
				{
					t = inv_mod(den, av);
					t = mul_mod(t, num, av);
					t = mul_mod(t, k, av);

					for (int i = v; i < vmax; i++)
					{
						t = mul_mod(t, a, av);
					}

					s += t;

					if (s >= av)
					{
						s -= av;
					}
				}
  
			}

			t = pow_mod(10, n - 1, av);
			s = mul_mod(s, t, av);
			sum = (sum + (double) s / (double) av) % 1.0;
		}
		
		var result = (int) (sum * 1e9);

		return $"{result:D9}";
	}
}