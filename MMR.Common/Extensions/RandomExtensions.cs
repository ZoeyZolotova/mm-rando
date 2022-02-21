using System;

namespace MMR.Common.Extensions
{
    public static class RandomExtensions
    {
        private static readonly double LOG4 = Math.Log(4);
        private static readonly double SG_MAGICCONST = 1.0 + Math.Log(4.5);

        // Translated from python from https://github.com/python/cpython/blob/3352834f59531dfa42dbef00ada4fb95ded2ae3a/Lib/random.py#L640-L730
        public static double GammaVariate(this Random random, double alpha, double beta)
        {
            if (alpha <= 0 || beta <= 0)
            {
                throw new ArgumentException($"{nameof(GammaVariate)}: {nameof(alpha)} and {nameof(beta)} must be > 0.0");
            }

            if (alpha > 1.0)
            {
                var ainv = Math.Sqrt(2.0 * alpha - 1.0);
                var bbb = alpha - LOG4;
                var ccc = alpha + ainv;

                while (true)
                {
                    var u1 = random.NextDouble();
                    if (!(0.0000001 < u1 && u1 < 0.9999999))
                    {
                        continue;
                    }
                    var u2 = 1.0 - random.NextDouble();
                    var v = Math.Log(u1 / (1.0 - u1)) / ainv;
                    var x = alpha * Math.Exp(v);
                    var z = u1 * u1 * u2;
                    var r = bbb + ccc * v - x;
                    if (r + SG_MAGICCONST - 4.5 * z >= 0.0 || r > Math.Log(z))
                    {
                        return x * beta;
                    }
                }
            }
            else if (alpha == 1.0)
            {
                return -Math.Log(1.0 - random.NextDouble()) * beta;
            }
            else
            {
                double x;
                while (true)
                {
                    var u = random.NextDouble();
                    var b = (Math.E + alpha) / Math.E;
                    var p = b * u;
                    if (p <= 1.0)
                        x = Math.Pow(p, (1.0 / alpha));
                    else
                        x = -Math.Log((b - p) / alpha);
                    var u1 = random.NextDouble();
                    if (p > 1.0)
                        if (u1 <= Math.Pow(x, (alpha - 1.0)))
                            break;
                        else if (u1 <= Math.Exp(-x))
                            break;
                }
                return x * beta;
            }
        }

        public static double BetaVariate(this Random random, double alpha, double beta)
        {
            var y = random.GammaVariate(alpha, 1.0);
            if (y != 0)
            {
                return y / (y + random.GammaVariate(beta, 1.0));
            }
            return 0;
        }
    }
}
