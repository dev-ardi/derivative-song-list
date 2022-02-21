using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MathNet.Numerics.SpecialFunctions;
namespace derivative_song_app
{
    static class Calculus
    {
        private static double DfxElem(float alpha, int[] arr, int index)
        {
            const double EPSILON = 0.001;
            const int ITER_MAX = 80;
            double res = 0;
            int sign = 1;
            for (int i = 0; i < Math.Max(arr.Count(), ITER_MAX); i++)
            {
                if (i > index)
                    return res;
                int numerator = arr[index - i];
                double denominator = Factorial(i) * Gamma(alpha - i + 1);
                double term = numerator / denominator;
                if (Math.Abs(term - res) > EPSILON)
                    break;
                res += (res + term) * sign;
                sign *= -1;
            }
            return res *= Gamma(alpha + 1);
        }
        public static double[] DfxArr(float alpha, int[] arr)
        {
            double[] ret = new double[arr.Length];
            foreach(int i in arr)
            {
                ret.Append(DfxElem(alpha, arr, i));
            }
            return ret;
        }
    }
}
