using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class IgolShtirCalculation
    {
        public double l { get; set; }
        public int Count { get; set; }
        public double b { get; set; }
        public double sp { get; set; }

        //коэфициенты/приближения
        public IgolShtirCoefficients ISCoefficients { get; set; }


        public void Calculate(double ts, double rpk, double rkr, double p, double tmax, double aP, IgolShtirCoefficients isCoefficients)
        {
            ISCoefficients = isCoefficients;
            double tp, deq, Q, n, dt, a, Nu, B, U, f, th, X, Qsh, z;                                                                                                  //объявляем выходные переменные 
            //вычисление
            tp = tmax - p * (rpk + rkr);
            if (tp <= ts)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения");
            }
            dt = tp - ts;
            Q = ts - ((ts - (ts - 3)) / 2);
            deq = (ISCoefficients.d1 + ISCoefficients.d2) / 2;
            B = 1 / ts;
            Nu = 0.47 * Math.Pow(((10 * (deq * deq * deq) * B * (tp - ts)) / 15), 0.25);
            a = (Nu * 2.5) / deq;
            U = 3.1415 * deq;
            f = (3.1415 * deq * deq) / 4;
            X = Math.Sqrt((4 * a) / (aP * deq));
            z = X * ISCoefficients.h;
            th = Math.Tan((Math.Exp(z) - Math.Exp(-z)) / (Math.Exp(z) + Math.Exp(-z)));
            Qsh = 1.14 * Q * th * Math.Sqrt(a * U * f * aP);
            n = (p / Qsh) * 0.6;
            Count = Convert.ToInt32(Math.Round(n, MidpointRounding.AwayFromZero));
            sp = ISCoefficients.s * ISCoefficients.s * Count;
            l = (Math.Sqrt(sp))+ISCoefficients.s;
        }


    }
}
