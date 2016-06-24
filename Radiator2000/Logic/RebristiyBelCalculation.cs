using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class RebristiyBelCalculation
    {
        public double L { get; set; }
        public double l { get; set; }
        public int Count { get; set; }
        public double sp { get; set; }
        public double b5 { get; set; }

        //коэфициенты/приближения
        public RebristiyBelCoefficients BelCoefficients { get; set; }


        public void Calculate(double tc, double rpk, double rkr, double P, double tmax, double E, RebristiyBelCoefficients belCoefficients)
        {
            BelCoefficients = belCoefficients;
            double tp, rrc, so, n, dt, alfaKgl, F, alfaL, tm, Tc, A1, alfaGLAD, Pgl, F1, alfaLoreb, t1, H6, ni, K, M, C1, A4, B, s1, s2, s3, s4, s5, Ptoreb, Pteor;//объявляем выходные переменные 
            //вычисление
            tp = tmax - P * (rpk + rkr);
            if (tp <= tc)
            {
                throw new Exception("FATAL ERROR: Недопустимые значения");
            }
            rrc = (0.9 * ((tmax - tc) - P * (rpk + rkr))) / P;
            Tc = P * rrc + tc;
            dt = tp - tc;
            L = (244.52 * Math.Pow(2.71828182846, (-0.123 * rrc))) / 2000;
            l = BelCoefficients.k4 * L;
            n = (L + BelCoefficients.b) / (BelCoefficients.b + BelCoefficients.d);
            Count = Convert.ToInt32(Math.Round(n, MidpointRounding.AwayFromZero));

            b5 = (L - BelCoefficients.delt * Count) / (Count-1);

            sp = (n - 1) * L * BelCoefficients.b + (BelCoefficients.delt + 2 * BelCoefficients.h) * L * n + 2 * l * BelCoefficients.delt + 2 * n * BelCoefficients.delt * BelCoefficients.h + L * l;
            B = Math.Pow((P*rrc/ L), 0.25);
            tm = 0.5 * (Tc + tc);
            A1 = -0.0012 * tm + 1.493;
            alfaKgl = 5.62*A1 * B;
            F = 0.3278 * Math.Pow((BelCoefficients.h / BelCoefficients.b), (-0.558));
            alfaL = E * 1 * F;
            alfaGLAD = alfaKgl + alfaL;
            Pgl = alfaGLAD * L * l * (Tc - tc);

            so = (n - 1) * L * BelCoefficients.b + (BelCoefficients.delt + 2 * BelCoefficients.h) * L * n + 2 * l * BelCoefficients.delt + 2 * n * BelCoefficients.delt * BelCoefficients.h;
            F1 = (BelCoefficients.b / (2 * BelCoefficients.h + BelCoefficients.b));
            alfaLoreb = E * F1 * F;
            A4= 0.388 * Math.Pow((2.718), -0.003*(0.5*(Tc-tc)));
            K = Math.Pow((Tc - tc), 0.25);
            M = Math.Pow(L, 0.25);
            C1 = K / M;
            ni = A4 * C1 * b5;
            H6 = 0.0629 * ni + 0.0243;
            t1 = (Tc - (Tc - tc) * H6);
            s1 = ((Count - 1) * (b5 * L));
            s2 = 2 * BelCoefficients.h * l * (n - 1);
            s3 = 2 * (BelCoefficients.h + BelCoefficients.d) * L;
            s4 = (Count * (2 * BelCoefficients.h * BelCoefficients.delt + BelCoefficients.delt * L));
            s5 = L * l;

            
            Ptoreb = (s1*(alfaKgl*(Tc-t1)+alfaLoreb*(Tc-tc)))+ (s2 * (alfaKgl * (Tc - t1) + alfaLoreb * (Tc - tc)))+ (s3 * (alfaKgl * (Tc - t1) + alfaLoreb * (Tc - tc)))+ (s4 * (alfaKgl * (Tc - t1) + alfaLoreb * (Tc - tc)));

            Pteor = Pgl + Ptoreb;

           // if (Pteor <= P)
            //{
          //      throw new Exception("FATAL ERROR: Недопустимые значения.  \n Измените значения теплового сопротивления корпус-тепоотвод \n или p-n переход-корпус");
         //   }

        }
    }
}