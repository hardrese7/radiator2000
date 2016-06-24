using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class RebristiyChernCalculation
    {
        public double L { get; set; }
        public double l { get; set; }
        public int Count { get; set; }
        public double sp { get; set; }
        public double b5 { get; set; }


        //коэфициенты/приближения
        public RebristiyChernCoefficients ChernCoefficients { get; set; }


        public void Calculate(double tc, double rpk, double rkr, double P, double tmax, double E, RebristiyChernCoefficients chernCoefficients)
        {
            ChernCoefficients = chernCoefficients;
            double b5, tp, rrc, so, n, dt, alfaKgl, F, alfaL, tm, Tc, A1, alfaGLAD, Pgl, F1, alfaOREB, alfaLoreb, K, M, C1, B, alfaKoreb, Ptoreb, Pteor;//объявляем выходные переменные 
            //вычисление
            tp = (tmax - P * (rpk + rkr))*0.96;
            if (tp <= tc)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения");
            }
            rrc = (((tmax - tc) - P * (rpk + rkr))) / P*0.9;
            Tc = P * rrc + tc;
            dt = tp - tc;
            L = (110 * Math.Pow(2.71828182846, (-0.1433 * rrc)))/1000;
            l = ChernCoefficients.k4 * L;
            n = (L + ChernCoefficients.b) / (ChernCoefficients.b + ChernCoefficients.d);
            Count = Convert.ToInt32(Math.Round(n, MidpointRounding.AwayFromZero));
            b5 = (L - ChernCoefficients.delt * Count) / (Count - 1);

            sp = (n - 1) * L * ChernCoefficients.b + (ChernCoefficients.delt + 2 * ChernCoefficients.h) * L * n + 2 * l * ChernCoefficients.delt + 2 * n * ChernCoefficients.delt * ChernCoefficients.h + L * l;
            B = Math.Pow((dt / L), 0.25);
            tm = 0.5 * (Tc + tc);
            A1 = -0.0012 * tm + 1.4093;
            alfaKgl = A1 * B;
            F = 5 * Math.Pow((2.718), (0.00735 * Tc));
            alfaL = E * 1 * F;
            alfaGLAD = alfaKgl + alfaL;
            Pgl = alfaGLAD * L * l * (Tc - tc);

            so = (n - 1) * L * ChernCoefficients.b + (ChernCoefficients.delt + 2 * ChernCoefficients.h) * L * n + 2 * l * ChernCoefficients.delt + 2 * n * ChernCoefficients.delt * ChernCoefficients.h;
            F1 = (ChernCoefficients.b / (2 * ChernCoefficients.h + ChernCoefficients.b));
            alfaLoreb = E * F1 * F;
            K = Math.Pow((Tc - tc), 0.25);
            M = Math.Pow(L, 0.25);
            C1 = K / M;
            alfaKoreb = A1 * C1;
            alfaOREB = alfaLoreb + alfaKoreb;
            Ptoreb = so * (alfaKoreb * (Tc - tc) + alfaLoreb * (Tc - tc));

            Pteor = Pgl + Ptoreb;
           
            if (Pteor <= P)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения.  \n Измените значения теплового сопротивления корпус-тепоотвод \n или p-n переход-корпус");
            }

        }
    }
}