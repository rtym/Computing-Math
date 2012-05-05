using System;

namespace ComputingMath.NumericalIntergration
{
    public class Integration
    {
        #region SimpsonIntegration

        public static double SimpsonIntegration(Integral.Function f, double a, double b, double eps)
        {
            double integral;
            double h;
            int n = 1;
            bool key = true;
            double result = 0;

            while (key)
            {
                double x = a;
                h = (b - a) / n;
                integral = f(x);

                while (Math.Abs(b - x) > eps)
                {
                    integral += (4 * f(x + (h / 2)) + 2 * f(x + h));
                    x += h;
                }

                integral -= f(b);
                integral *= h / 6;

                key = Math.Abs((integral - result) / integral) > eps;

                if (key)
                {
                    result = integral;
                    n += 2;
                }
            }

            return result;      
        }

        #endregion

        #region TrapezeIntegration

        public static double TrapezeIntegration(Integral.Function f, double a, double b, double eps)
        {
            double integral;
            double h;
            int n = 1;
            bool key = true;
            double result = 0;

            while (key)
            {
                double x = a;
                h = (b - a) / n;
                integral = 0;

                while (Math.Abs(b - x) > eps)
                {
                    integral += (h/2) * (f(x) + f(x + h));
                    x += h;
                }

                key = Math.Abs((integral - result) / integral) > eps;

                if (key)
                {
                    result = integral;
                    n *= 2;
                }
            }

            return result;
        }

        #endregion

        #region RectangleIntegration

        public static double RectangleIntegration(Integral.Function f, double a, double b, double eps)
        {
            double integral;
            double h;
            int n = 1;
            bool key = true;
            double result = 0;

            while (key)
            {
                double x = a;
                h = (b - a) / n;
                integral = 0;

                while (Math.Abs(b - x) > eps)
                {
                    integral += h * f(x + (h / 2));
                    x += h;
                }

                key = Math.Abs((integral - result) / integral) > eps;

                if (key)
                {
                    result = integral;
                    n *= 2;
                }
            }

            return result;
        }

        #endregion
    }
}
