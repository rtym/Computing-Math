using System;
using ComputingMath;

namespace ComputingMath.NumericalIntergration
{
    public class Integral:ICloneable
    {
        public delegate double Function(double x);
        private Function f;
        private double a = 0;
        private double b = 0;
        private double eps = 1e-6;

        public Integral.Function integralFunction
        {
            set
            {
                this.f = value;
            }
        }

        public Integral(Function f, double a, double b)
        {
            this.f = f;
            this.SetInterval(a, b);
        }

        public void SetInterval(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public double IntegralValue(IntegrationConstants method, double from, double to, double eps)
        {
            double result;

            if ((from < this.a) || (to > this.b))
            {
                throw new IntegralRangeUndefinedException("Incorrect integration range.");
            }
            else
            {
                switch (method)
                {
                    case IntegrationConstants.Rectangle:
                        result = Integration.RectangleIntegration(this.f, from, to, eps);
                        break;
                    case IntegrationConstants.Trapeze:
                        result = Integration.TrapezeIntegration(this.f, from, to, eps);
                        break;
                    case IntegrationConstants.Simpson:
                        result = Integration.SimpsonIntegration(this.f, from, to, eps);
                        break;
                    default:
                        throw new IncorrectIntegrationMethodException("Error! You're trying to use unknown integration method.");
                }
            }

            return result;
        }

        public double IntegralValue()
        {
            return this.IntegralValue(IntegrationConstants.Simpson, this.a, this.b, this.eps);
        }

        public double IntegralValue(double percision)
        {
            return this.IntegralValue(IntegrationConstants.Simpson, this.a, this.b, percision);
        }

        public double IntegralValue(double from, double to)
        {
            return this.IntegralValue(IntegrationConstants.Simpson, from, to, this.eps);
        }

        public double IntegralValue(double from, double to, double percision)
        {
            return this.IntegralValue(IntegrationConstants.Simpson, from, to, percision);
        }

        public object Clone()
        {
            Integral result = new Integral(this.f, this.a, this.b);
            return result;
        }
    }
}
