using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging;
using Accord.Statistics.Distributions.Univariate;


namespace AdversarialImage
{
    class RunAlgorithm
    {

        private static bool greyscaleenable = true;
        private static int threadcount = 10;
        private static int imagetotal = 10;
        private static double count = 0.0;

        private  double adversupperrange = 0.0;
        private  double adverselowerrange = 0.0;

        private  double cleanupperrange = 0.0;
        private  double cleanlowerrange = 0.0;

        private double lbp_adversupperrange = 0.0;
        private double lbp_adverselowerrange = 0.0;

        private double lbp_cleanupperrange = 0.0;
        private double lbp_cleanlowerrange = 0.0;

        private double euc_adversupperrange = 0.0;
        private double euc_adverselowerrange = 0.0;

        private double euc_cleanupperrange = 0.0;
        private double euc_cleanlowerrange = 0.0;

        private double Lvaluesum = 0.0;
        private double Lvalueavg = 0.0;
        private double Lcleanvalueavg = 0.0;
        private double Ladvvalueavg = 0.0;


        private Accord.Statistics.Visualizations.Histogram adv_histogram = null;
        private Accord.Statistics.Visualizations.Histogram clean_histogram = null;

        public  double diff = 0.0;
        private static bool theadon = true;
        List<string> algorithm = new List<string>();
        string sta = "";
        private void fitnesstcheck(MyChromosome chromosome)
        {
           
           checkstring(chromosome.GetGenes());

        }
        public List<string> checkstring(string value)
        {
            int i = 0;
            string s = "";
            foreach (char c in value)
            {
                s = s + c;
                i++;
                if (i == MainGA.genelength)
                {

                    algorithm.Add(s);
                    s = "";
                    i = 0;
                }

            }
            return algorithm;
        }
        public  Fitnessvalues RunAlgorithmchromosomes(MyChromosome chromosome)
        {
            fitnesstcheck(chromosome);
             imagetotal = Constant.samplesize;
            threadcount = Constant.ThreadCount;
            greyscaleenable = Constant.greyscale;
       
                return runonthread();


        }

        private Fitnessvalues DifferenceMethod()
        {
            Lvalueavg = Math.Abs(Lcleanvalueavg - Ladvvalueavg);
         //   Lvalueavg = 1 / Lvalueavg;
          

            double distance = 0.0;
            double distancepercentage = 0.0;
            if (cleanupperrange > adversupperrange && cleanlowerrange > adversupperrange)
            {
                distance = cleanlowerrange - adversupperrange;
                distancepercentage = distance / cleanlowerrange;
                distance = distance * distance;
              

            }
            else if (adversupperrange > cleanupperrange && adverselowerrange > cleanupperrange)
            {
                distance = adverselowerrange - cleanupperrange;
                distancepercentage = distance / adverselowerrange;
                distance = distance * distance;
            }
            else 
            {

                distance = Math.Abs(adversupperrange + adverselowerrange - cleanupperrange - cleanlowerrange);
                distancepercentage = distance / adversupperrange;
            }
           // diff = distance;


            double lbp_distance = 0.0;
            if (lbp_cleanupperrange > lbp_adversupperrange && lbp_cleanlowerrange > lbp_adversupperrange)
            {
                lbp_distance = lbp_cleanlowerrange - lbp_adversupperrange;
                lbp_distance = lbp_distance * lbp_distance;

            }
            else if (lbp_adversupperrange > lbp_cleanupperrange && lbp_adverselowerrange > lbp_cleanupperrange)
            {
                lbp_distance = lbp_adverselowerrange - lbp_cleanupperrange;
                lbp_distance = lbp_distance * lbp_distance;
            }
            else
            {

                lbp_distance = Math.Abs(lbp_adversupperrange + lbp_adverselowerrange - lbp_cleanupperrange - lbp_cleanlowerrange);
            }


            Accord.Math.Distances.Euclidean euclidean = new Accord.Math.Distances.Euclidean();

            var p = Array.ConvertAll<int, double>(adv_histogram.ToArray(), x => (double)x/ (double)adv_histogram.Max);
            var q = Array.ConvertAll<int, double>(clean_histogram.ToArray(), x => (double)x / (double)clean_histogram.Max);

            //   foreach(var b in q)
            //      Console.Write(b + "");
            var plist = p.ToList();
            var qlist = q.ToList();

            double ecu_distance= euclidean.Distance(p,q);
            double crossentropyp = CEL.Crossentropy(plist, plist);
            double crossentropyq = CEL.Crossentropy(qlist, qlist);
            double crossentropy = CEL.Crossentropy(plist, qlist);
         //   Console.WriteLine(sta + " crossentropy " + crossentropyp + " " + crossentropyq + " " + crossentropy);
           // var adv_normal = new NormalDistribution(mean: plist.Average(), stdDev: plist.StdDev);
         //   var clean_normal = new NormalDistribution(mean: clean_histogram.Mean, stdDev: clean_histogram.StdDev);
            double crossentropyppde = CEL.CrossentropyPDE(plist, plist);
            double crossentropyqpde = CEL.CrossentropyPDE(qlist, qlist);
            double crossentropypde = CEL.CrossentropyPDE(qlist, plist); ;
       //     Console.WriteLine(sta + " crossentropypde " + crossentropyppde + " " + crossentropyqpde + " " + crossentropypde);
            double deltax = 0 - lbp_distance;// point1[0];
            double deltay = 0 - distance;
            double deltaz = 0 - ecu_distance;
             diff = (double)Math.Sqrt(
                (deltax * deltax) +
                (deltay * deltay) +
                (deltaz * deltaz));
          //  Console.WriteLine(sta+ " fitness " + diff+"-"+ ecu_distance + "-"+ lbp_distance);
          

            euc_cleanupperrange = clean_histogram.Mean+clean_histogram.StdDev;
         euc_cleanlowerrange = clean_histogram.Mean -clean_histogram.StdDev;

           string sa = sta + ", LVALUE, " + Lvalueavg + " fitness ," + diff + "," + ecu_distance + "," + lbp_distance + " ," + crossentropy + " ," + crossentropypde;
            //  if (!MainGA.recodedfilters.Contains(sta + "," + diff + "," + cleanupperrange + "," + cleanlowerrange + "," + lbp_cleanupperrange + "," + lbp_cleanlowerrange + "," + euc_cleanupperrange + "," + euc_cleanlowerrange));
            //   MainGA.recodedfilters.Add(sta + "," + diff + "," + cleanupperrange + "," + cleanlowerrange + "," + lbp_cleanupperrange + "," + lbp_cleanlowerrange + "," + euc_cleanupperrange + "," + euc_cleanlowerrange);


            Fitnessvalues fitnessvalues = new Fitnessvalues();

            fitnessvalues.Individual = sta;
            fitnessvalues.Histogramdistance = diff;
            fitnessvalues.Cleanupperrange = cleanupperrange;
            fitnessvalues.Lowerupperrange = cleanlowerrange;

            fitnessvalues.Lbp_distance = lbp_distance;
            fitnessvalues.Lbp_cleanupperrange = lbp_cleanupperrange;
            fitnessvalues.Lbp_lowerupperrange = lbp_cleanlowerrange;

            fitnessvalues.Ecu_distance = Math.Abs(ecu_distance);
            fitnessvalues.Ecu_histogram_clean = clean_histogram;
            fitnessvalues.Ecu_histogram_adv = adv_histogram;

            fitnessvalues.Lvalue = Lvalueavg;

            fitnessvalues.Crossentropy = crossentropy;
            fitnessvalues.Crossentropy_clean = crossentropyq;
            fitnessvalues.Crossentropy_adv = crossentropyp;

            fitnessvalues.Crossentropy_PDE = crossentropypde;
            fitnessvalues.Crossentropy_PDE_clean = crossentropyqpde;
            fitnessvalues.Crossentropy_PDE_adv = Math.Abs(crossentropyppde);

            MainGA.fitness.Add(fitnessvalues);

            return fitnessvalues;
        }

        private int getfilter(string s)
        {
            int k=1;
            switch (s)
            {
                case "00":
                    return AlgorithmList.G00;
                case "01":
                    return AlgorithmList.G01;
                case "10":
                    return AlgorithmList.G10;
                case "11":
                    return AlgorithmList.G11;
                case "000":
                    return AlgorithmList.G000;
                case "001":
                    return AlgorithmList.G001;
                case "010":
                    return AlgorithmList.G010;
                case "011":
                    return AlgorithmList.G011;
                case "100":
                    return AlgorithmList.G100;
                case "101":
                    return AlgorithmList.G101;
                case "110":
                    return AlgorithmList.G110;
                case "111":
                    return AlgorithmList.G111;
                default:
                    break;
            }

            return k;
        }
        public rettype runalgo(string s)
        {
            string dupImagePath = s;
            Bitmap org0 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
           // Bitmap org1 = org0.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (greyscaleenable) { 
           Accord.Imaging.Filters.GrayscaleBT709 gr = new Accord.Imaging.Filters.GrayscaleBT709();
           org0 = gr.Apply(org0);
            }
            Bitmap org1 = org0.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);
            Bitmap noiserem = org1.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            foreach (string filterid in algorithm)
            {
                int filterno = getfilter(filterid);
                if (filterno == 2)
                {
                    // Console.WriteLine("AdaptiveSmoothing");
                    Accord.Imaging.Filters.AdaptiveSmoothing noisefilter = new Accord.Imaging.Filters.AdaptiveSmoothing();
                   noiserem = noisefilter.Apply(noiserem);

                }
                else if  (filterno == 4)
                    {
                   //  Console.WriteLine("AdditiveNoise");
                   Accord.Imaging.Filters.AdditiveNoise noisefilter = new Accord.Imaging.Filters.AdditiveNoise();
                    noiserem = noisefilter.Apply(noiserem);
                }
                else if (filterno == 3)
                {
                   //  Console.WriteLine("BilateralSmoothing");
                    Accord.Imaging.Filters.BilateralSmoothing noisefilter = new Accord.Imaging.Filters.BilateralSmoothing();
                    noiserem = noisefilter.Apply(noiserem);
                }
                if (filterno == 5)
                {
                    //Bitmap gra = new Bitmap(noiserem.Width, noiserem.Height, PixelFormat.Format8bppIndexed);
                    
                   // Bitmap gra = new bnoiserem.Clone(System.Drawing.Imaging.PixelFormat.form);
                    // Console.WriteLine("AdaptiveSmoothing");
                    Accord.Imaging.Filters.Erosion noisefilter = new Accord.Imaging.Filters.Erosion();
                      noiserem = noisefilter.Apply(noiserem);

                }
                else if (filterno == 6)
                {
                    //  Console.WriteLine("AdditiveNoise");
                    Accord.Imaging.Filters.Pixellate noisefilter = new Accord.Imaging.Filters.Pixellate();
                    noiserem = noisefilter.Apply(noiserem);
                }
                else if (filterno == 7)
                {
                    //  Console.WriteLine("BilateralSmoothing");
                    Accord.Imaging.Filters.GaussianBlur noisefilter = new Accord.Imaging.Filters.GaussianBlur();
                    noiserem = noisefilter.Apply(noiserem);
                }
                else if (filterno == 8)
                {
                    //  Console.WriteLine("BilateralSmoothing");
                    Accord.Imaging.Filters.GaussianSharpen noisefilter = new Accord.Imaging.Filters.GaussianSharpen();
                    noiserem = noisefilter.Apply(noiserem);
                }
                else
                {
                    ///donothing
                }
            }
           
            // apply the filter
        
            Bitmap resultImage = filter.Apply(noiserem);
          

            double mean = 0.0;// histogram.Mean;     // mean red value
            Accord.Statistics.Visualizations.Histogram histogram = null; ;
            if (greyscaleenable == false)
            {
                Accord.Imaging.ImageStatistics statistics = new Accord.Imaging.ImageStatistics(resultImage);
                 histogram = statistics.Red;
                mean = histogram.Mean;     // mean red value
          
            }
            else
            {
                Accord.Imaging.Filters.GrayscaleBT709 gr = new Accord.Imaging.Filters.GrayscaleBT709();
                resultImage = gr.Apply(resultImage);
                Accord.Imaging.ImageStatistics statistics = new Accord.Imaging.ImageStatistics(resultImage);
               
                 histogram = statistics.Gray;
                mean = histogram.Mean;     // mean red value
            
            }
            var lbp = new LocalBinaryPattern(blockSize: 3, cellSize: 6);

            // Use it to extract descriptors from the Lena image:
            var descriptors = lbp.ProcessImage(resultImage);
            double av = 0.0;
            foreach (var d in descriptors)
            {
               av=av+ d.Average();
            }
            av = av / descriptors.Count;

            

            rettype ret = new rettype();
            ret.histogram = histogram;
            ret.lbpavg = av;
            ret.histoavg = mean;
        
            return ret;
        }
        private  void runoncleanonthread()
        {
            ObservableCollection<double> means = new ObservableCollection<double>();
            ObservableCollection<double> lbpmeans = new ObservableCollection<double>();
            ObservableCollection<double> L = new ObservableCollection<double>();
            Accord.Statistics.Visualizations.Histogram histogram = null ;
            //  List<double> stddeva = new List<double>();
            count = 0;
           
            string[] file = Directory.GetFiles(Form1.clean);
            if (imagetotal > file.Length) imagetotal = file.Length;
            var T1 = Task.Run(() => {
                for (int i = 0; i < threadcount * 1; i++)
                {
                  //  Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                    histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1,rt.histogram));
                }
            });
            var T2 = Task.Run(() => {
                for (int i = threadcount * 1; i < threadcount * 2; i++)
                {
                   // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T3 = Task.Run(() => {
                for (int i = threadcount * 2; i < threadcount * 3; i++)
                {
                  // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T4 = Task.Run(() => {
                for (int i = threadcount * 3; i <= threadcount * 4; i++)
                {
                   // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T5 = Task.Run(() => {
                for (int i = threadcount * 4; i < threadcount * 5; i++)
                {
                   // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T6 = Task.Run(() => {
                for (int i = threadcount * 5; i < threadcount * 6; i++)
                {
                //    Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T7 = Task.Run(() => {
                for (int i = threadcount * 6; i < threadcount * 7; i++)
                {
                //    Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            var T8 = Task.Run(() => {
                for (int i = threadcount * 7; i < file.Length; i++)
                {
                 //   Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(1, rt.histogram));
                }

            });
            T1.Wait();
            T2.Wait();
            T3.Wait();
            T4.Wait();
            T5.Wait();
            T6.Wait();
            T7.Wait();
            T8.Wait();
           // Form1.textlist.Add("total sample processed " + means.Count());
            double std = StandardDeviation(means.ToList(), 0, means.Count());
            double avg = means.Average();
            cleanupperrange =std + avg;
            cleanlowerrange = Math.Abs(avg - std);

             std = StandardDeviation(lbpmeans.ToList(), 0, lbpmeans.Count());
             avg = means.Average();
            lbp_cleanupperrange = std + avg;
            lbp_cleanlowerrange = Math.Abs(avg - std);

            clean_histogram = histogram;
            Lcleanvalueavg = L.Average();
            Lvaluesum = Lvaluesum +L.Sum();
            //   Form1.textlist.Add(sta + " cleanupperrange " + cleanupperrange + " cleanlowerrange " + cleanlowerrange);
            //Console.WriteLine("adversupperrange " + adversupperrange + "adverselowerrange " + adverselowerrange);

        }

        private  void runonadverseonthread()
        {
            ObservableCollection<double> means = new ObservableCollection<double>();
            ObservableCollection<double> lbpmeans = new ObservableCollection<double>();
            ObservableCollection<double> L = new ObservableCollection<double>();
            Accord.Statistics.Visualizations.Histogram histogram = null;
            //List<double> stddeva = new List<double>();
            count = 0;
           
            string[] file = Directory.GetFiles(Form1.adverse);
            if (imagetotal > file.Length) imagetotal = file.Length;
            var T1 = Task.Run(() => {
                for (int i = 0; i < threadcount * 1; i++)
                {
                  //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));

                }
            });
            var T2 = Task.Run(() => {
                for (int i = threadcount*1; i < threadcount*2; i++)
                {
                  //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T3 = Task.Run(() => {
                for (int i = threadcount * 2; i < threadcount * 3; i++)
                {
                  // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T4 = Task.Run(() => {
                for (int i = threadcount * 3; i <= threadcount * 4; i++)
                {
                   // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T5 = Task.Run(() => {
                for (int i = threadcount * 4; i < threadcount *5; i++)
                {
                  //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T6 = Task.Run(() => {
                for (int i = threadcount * 5; i < threadcount * 6; i++)
                {
                   // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T7 = Task.Run(() => {
                for (int i = threadcount * 6; i < threadcount * 7; i++)
                {
                   // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
            var T8 = Task.Run(() => {
                for (int i = threadcount * 7; i < file.Length; i++)
                {
                   // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    rettype rt = runalgo(file[i]);
                    means.Add(rt.histoavg);
                    lbpmeans.Add(rt.lbpavg);
                    if (histogram == null) histogram = rt.histogram;
                    else
                        histogram.Add(rt.histogram);
                    L.Add(CEL.Lcalculate(0, rt.histogram));
                }

            });
           
            T1.Wait();
            T2.Wait();
            T3.Wait();
            T4.Wait();
            T5.Wait();
            T6.Wait();
            T7.Wait();
            T8.Wait();
           // Form1.textlist.Add("total sample processed " + means.Count());
            double std = StandardDeviation(means.ToList(), 0, means.Count());
            double avg = means.Average();
            adversupperrange = std + avg;
            adverselowerrange = Math.Abs(avg - std);

            std = StandardDeviation(lbpmeans.ToList(), 0, lbpmeans.Count());
            avg = means.Average();
            lbp_adversupperrange = std + avg;
            lbp_adverselowerrange = Math.Abs(avg - std);
            //   Form1.textlist.Add(sta+" adversupperrange " + adversupperrange + " adverselowerrange " + adverselowerrange);
            adv_histogram = histogram;
            Ladvvalueavg= L.Average();
            Lvaluesum = Lvaluesum + L.Sum();
        }
         private  double StandardDeviation( List<double> values, int start, int end)
        {
            double mean = values.Average();// (start, end);
            double variance =Variance(values,mean, start, end);

            return Math.Sqrt(variance);
        }
        private  double Variance( List<double> values, double mean, int start, int end)
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


        public Fitnessvalues runonthread()
        {
            var T1=Task.Run(() => {
                runoncleanonthread();

            });
            var T2 = Task.Run(() => {
            //    runonadverse();
                runonadverseonthread();

            });
            T1.Wait();
            T2.Wait();
            return DifferenceMethod();

        }

        private void runonclean()
        {

            List<double> means = new List<double>();
            List<double> stddeva = new List<double>();

            string[] file = Directory.GetFiles(Form1.clean);
            for (int i = 0; i < file.Length; i++)
            {
                Console.WriteLine("clean count " + i);
                if (i == imagetotal) break;
                string dupImagePath = file[i];
                Bitmap org0 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org1 = org0.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap noiserem = org1;
                foreach (string filterid in algorithm)
                {
                    if (filterid.Equals("01"))
                    {
                        // Console.WriteLine("AdaptiveSmoothing");
                        Accord.Imaging.Filters.AdaptiveSmoothing noisefilter = new Accord.Imaging.Filters.AdaptiveSmoothing();
                        noiserem = noisefilter.Apply(noiserem);

                    }
                    else if (filterid.Equals("11"))
                    {
                        //  Console.WriteLine("AdditiveNoise");
                        Accord.Imaging.Filters.AdditiveNoise noisefilter = new Accord.Imaging.Filters.AdditiveNoise();
                        noiserem = noisefilter.Apply(noiserem);
                    }
                    else if (filterid.Equals("10"))
                    {
                        // Console.WriteLine("BilateralSmoothing");
                        Accord.Imaging.Filters.BilateralSmoothing noisefilter = new Accord.Imaging.Filters.BilateralSmoothing();
                        noiserem = noisefilter.Apply(noiserem);
                    }
                    else
                    {
                        ///donothing
                    }
                }
                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);
                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem);
                Accord.Imaging.ImageStatistics statistics = new Accord.Imaging.ImageStatistics(resultImage);

                double mean = 0.0;// histogram.Mean;     // mean red value
                double stddev = 0.0;// histogram.StdDev
                if (MainGA.genelength == 2)
                {
                    var histogram = statistics.Red;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                else
                {
                    var histogram = statistics.Gray;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                means.Add(mean);
                //  stddeva.Add(stddev);


            }

            cleanupperrange = StandardDeviation(means, 0, means.Count()) + means.Average();
            cleanlowerrange = Math.Abs(StandardDeviation(means, 0, means.Count()) - means.Average());
        }

        private void runonadverse()
        {
            List<double> means = new List<double>();
            List<double> stddeva = new List<double>();
            count = 0;
            string[] file = Directory.GetFiles(Form1.adverse);
            for (int i = 0; i < file.Length; i++)
            {
                if (i == imagetotal) break;
                string dupImagePath = file[i];
                Bitmap org0 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org1 = org0.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap noiserem = org1;
                foreach (string filterid in algorithm)
                {
                    if (filterid.Equals("01"))
                    {
                        // Console.WriteLine("AdaptiveSmoothing");
                        Accord.Imaging.Filters.AdaptiveSmoothing noisefilter = new Accord.Imaging.Filters.AdaptiveSmoothing();
                        noiserem = noisefilter.Apply(noiserem);

                    }
                    else if (filterid.Equals("11"))
                    {
                        //  Console.WriteLine("AdditiveNoise");
                        Accord.Imaging.Filters.AdditiveNoise noisefilter = new Accord.Imaging.Filters.AdditiveNoise();
                        noiserem = noisefilter.Apply(noiserem);
                    }
                    else if (filterid.Equals("10"))
                    {
                        // Console.WriteLine("BilateralSmoothing");
                        Accord.Imaging.Filters.BilateralSmoothing noisefilter = new Accord.Imaging.Filters.BilateralSmoothing();
                        noiserem = noisefilter.Apply(noiserem);
                    }
                    else
                    {
                        ///donothing
                    }
                }
                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);
                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem);
                Accord.Imaging.ImageStatistics statistics = new Accord.Imaging.ImageStatistics(resultImage);

                double mean = 0.0;// histogram.Mean;     // mean red value
                double stddev = 0.0;// histogram.StdDev
                if (MainGA.genelength == 2)
                {
                    var histogram = statistics.Red;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                else
                {
                    var histogram = statistics.Gray;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                means.Add(mean);
                //  stddeva.Add(stddev);
                // org0.Dispose();
                //  org1.Dispose();
                //  noiserem.Dispose();
                //  resultImage.Dispose();

                if (!theadon)
                {
                    if (cleanupperrange > mean && cleanlowerrange > mean)
                    {
                        count++;
                    }
                    else if (mean > cleanupperrange && mean > cleanupperrange)
                    {
                        count++;

                    }
                    Console.WriteLine("count " + count + " index " + i);
                }


                //Form1.textlist.Add("count " + count + " index " + i);
            }

            adversupperrange = StandardDeviation(means, 0, means.Count()) + means.Average();
            adverselowerrange = Math.Abs(means.Average() - StandardDeviation(means, 0, means.Count()));
          //  Form1.textlist.Add("adversupperrange " + adversupperrange + "adverselowerrange " + adverselowerrange);
       //     Console.WriteLine("adversupperrange " + adversupperrange + "adverselowerrange " + adverselowerrange);

        }
    }


    class rettype 
    {

       public double histoavg = 0.0;
        public double lbpavg = 0.0;
        public Accord.Statistics.Visualizations.Histogram histogram = null;
    }

}
