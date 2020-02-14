using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Visualizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdversarialImage
{
    class CEL
    {

       public static double Crossentropy(List<double> p, List<double> q)
        {
          //  double[] pa = { 0.10, 0.40, 0.50 };
       //  double[] qa = { 0.80, 0.15, 0.05 };
          //  p = pa.ToList();
         //   q = qa.ToList();
           double pmax = p.Max();
           double qmax = q.Max();
            double psum = p.Sum();
            double qsum = q.Sum();
           // Console.WriteLine(qmax+"" + qsum + " sd"+ q.Count);
            double sum = 0.0;
            for (int i=0; i < p.Count; i++)
            {
                double pn = (p[i] )/psum;
                double qn = (q[i] )/qsum;
          //      Console.WriteLine(pn + " " + qn + " sd" + p[i] + " "+q[i]+ " "+sum);
                if(q[i]!=0)
                    sum = sum + (pn * Math.Log(qn,2));
              
            }
            return -sum;
        }
        public static double CrossentropyPDE(List<double> p, List<double> q)
        {
            //  double[] pa = { 0.10, 0.40, 0.50 };
            //  double[] qa = { 0.80, 0.15, 0.05 };
            //  p = pa.ToList();
            //   q = qa.ToList();
         //  

            double pmax = p.Max();
            double qmax = q.Max();
            double psum = p.Sum();
            double qsum = q.Sum();
            var pprob = Array.ConvertAll<double, double>(p.ToArray(), x => (double)x / (double)psum);
            var qprob = Array.ConvertAll<double, double>(p.ToArray(), x => (double)x / (double)qsum);

            var pdist = new NormalDistribution(mean: pprob.Average(), stdDev: StandardDeviation(pprob.ToList()));
            var qdist = new NormalDistribution(mean: qprob.Average(), stdDev: StandardDeviation(qprob.ToList()));
            // Console.WriteLine(qmax+"" + qsum + " sd"+ q.Count);
            double sum = 0.0;
            for (int i = 0; i < p.Count; i++)
            {
                double pn = pprob[i] ;
                double qn = qprob[i];
                pn = pdist.ProbabilityDensityFunction(x: pn);
                qn = qdist.ProbabilityDensityFunction(x: qn);
                //      Console.WriteLine(pn + " " + qn + " sd" + p[i] + " "+q[i]+ " "+sum);
                if ( qn!= 0)
                    sum = sum + (pn * Math.Log(qn, 2));

            }
            return -sum;
        }

        public static double Lcalculate(int y, Histogram h)
        {

            var sum = h.ToArray().ToList().Sum();
            var p = Array.ConvertAll<int, double>(h.ToArray(), x => ((double)x / (double)h.Max) / (double)sum);
            var dist = new NormalDistribution(mean: p.Average(), stdDev: StandardDeviation(p.ToList())); ;
            double l = 0.0;
            for (int i = 0; i < p.Length; i++)
            {


                double pn = dist.ProbabilityDensityFunction(x: p[i]);
              //  Console.Write(pn + " l");
                //      Console.WriteLine(pn + " " + qn + " sd" + p[i] + " "+q[i]+ " "+sum);
                if (y == 1)
                {
                    if ((pn != 0))
                        l = l + (Math.Log(pn, 2)); 
                
                }
                else if (y == 0)
                {
                    if ((1 - pn != 0))
                        l = l + (Math.Log((Math.Abs(1 - pn)), 2));
                }
              //  Console.Write(l + " ");

            }

          //  Console.Write(l + " ");
            return -l;
        }
        private static double StandardDeviation(List<double> values)
        {
            double mean = values.Average();// (start, end);
            double variance = Variance(values, mean, 0, values.Count);

            return Math.Sqrt(variance);
        }
        private static double Variance(List<double> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }

    }
}
