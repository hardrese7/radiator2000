using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class RebristiyCalculation
    {
        public double H { get; set; }
        public double D { get; set; }
        public int Count { get; set; }
        public double b { get; set; }
        public double sp { get; set; }

                //коэфициенты/приближения
        public RebristiyBorodinCoefficients BorodinCoefficients { get; set; }


        public void Calculate(double ts, double rpk, double rkr, double p, double tmax, RebristiyBorodinCoefficients borodinCoefficients)
        {
            BorodinCoefficients = borodinCoefficients;
            double tp, rrc, dts, so, n, dt;//объявляем выходные переменные 
            //вычисление
            tp = tmax - p * (rpk + rkr);
            if (tp <= ts)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения");
            }
            dt = tp - ts;
            dts = BorodinCoefficients.k3 * dt;
            rrc = dt / p;
            sp = 1 / (BorodinCoefficients.alfa * rrc);
            so = sp / (BorodinCoefficients.ks + 1);
            D = Math.Sqrt(so / BorodinCoefficients.k4);
            H = BorodinCoefficients.k4 * D;
            n = ((BorodinCoefficients.ks - 1) * D) / (2 * BorodinCoefficients.h);
            Count = Convert.ToInt32(Math.Round(n, MidpointRounding.AwayFromZero));
            b = (H - (Count * BorodinCoefficients.q)) / (Count - 1);
        }


    }
}
