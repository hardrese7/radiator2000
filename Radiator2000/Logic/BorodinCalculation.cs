using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class BorodinCalculation
    {
        public double H { get; set; }
        public double D { get; set; }
        public int Count { get; set; }
        public double b { get; set; }

        //коэфициенты/приближения
        public double q { get { return 0.0015; } }
        public double delt { get { return 0.004; } }
        public double k3 { get { return 0.8; } }
        public double ks { get { return 7; } }
        public double k4 { get { return 1; } }
        public double h { get { return 0.03; } }
        public double alfa { get { return 7; } }


        public void Calculate(double ts, double rpk, double rkr, double p, double tmax)
        {
            double tp, rrc, dts, sp, so, n, dt;//объявляем выходные переменные 
            //вычисление
            tp = tmax - p * (rpk + rkr);
            if (tp <= ts)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения");
            }
            dt = tp - ts;
            dts = k3 * dt;
            rrc = dt / p;
            sp = 1 / (alfa * rrc);
            so = sp / (ks + 1);
            D = Math.Sqrt(so / k4);
            H = k4 * D;
            n = ((ks - 1) * D) / (2 * h);
            Count = Convert.ToInt32(Math.Round(n, MidpointRounding.AwayFromZero));
            b = (H - (Count * q)) / (Count - 1);
        }
    }
}
