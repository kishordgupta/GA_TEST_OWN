using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Accord.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdversarialImage
{
    class Accuracy
    {
      
        ObservableCollection<string> cleancorrect = new ObservableCollection<string>();
        ObservableCollection<string> cleanincorrect = new ObservableCollection<string>();
        ObservableCollection<string> adversecorrect = new ObservableCollection<string>();
        ObservableCollection<string> adverseincorrect = new ObservableCollection<string>();
        private static bool greyscaleenable = true;
        private static int threadcount = 10;
        private static int imagetotal = 10;
        private static double count = 0.0;
        private double adversupperrange = 0.0;
        private double adverselowerrange = 0.0;
        private double cleanupperrange = 0.0;
        private double cleanlowerrange = 0.0;
        public double diff = 0.0;
        private static bool theadon = true;
        List<string> algorithm = new List<string>();
        string sta = "";
        internal void test(string s, double ur, double lr)
        {
            cleanupperrange = ur;
            adverselowerrange = lr;
            Form1.textlist.Add("clean count  " + s + ur + lr);
            RunAlgorithmchromosomes(s);
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
        public double RunAlgorithmchromosomes(string s)
        {
            cleancorrect = new ObservableCollection<string>();
             cleanincorrect = new ObservableCollection<string>();
         adversecorrect = new ObservableCollection<string>();
            adverseincorrect = new ObservableCollection<string>();
            checkstring(s);
            imagetotal = Constant.samplesize;
            threadcount = Constant.ThreadCount;
            greyscaleenable = Constant.greyscale;
      
                return runonthread();


        }

        private double DifferenceMethod()
        {

            double totalsample = cleancorrect.Count() + adversecorrect.Count() + cleanincorrect.Count() + adverseincorrect.Count();
            double totalaccuracy = cleancorrect.Count() + adversecorrect.Count();
            double totalinaccraucy =  cleanincorrect.Count() + adverseincorrect.Count();
            double falseposetive = cleanincorrect.Count();
            double trueposetive = cleancorrect.Count();
            double falsenegative = adverseincorrect.Count();
            double truenegative = adversecorrect.Count(); ;
            double accuracy = totalaccuracy / totalsample;
            double precision = trueposetive / (trueposetive + falseposetive);
            double recall = trueposetive / (trueposetive + falsenegative);
            double f1 = (2 * precision * recall) / (precision + recall);
            Form1.accuracytextlist.Add("\nrecall: " + recall + "\nprecision: "+ precision + "\nAccuracy: " + accuracy +"\n" + "\nF1: " + f1);

            return 0.0;
        }

        private int getfilter(string s)
        {
            int k = 1;
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
            if (greyscaleenable)
            {
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
                else if (filterno == 4)
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
                av = av + d.Average();
            }
            av = av / descriptors.Count;

            rettype ret = new rettype();
            ret.histogram = histogram;
            ret.lbpavg = av;
            ret.histoavg = mean;

            return ret;
        }
       
        void cleancheck(rettype d)
        {
          //  Form1.textlist.Add("clean count "+cleancorrect.Count()+"");

            if (cleanupperrange >= d.histoavg && d.histoavg>= cleanlowerrange) { cleancorrect.Add("1");  }
            else
            { cleanincorrect.Add("1");  }
        }
        void adversecheck(rettype d)
        {
            //Form1.textlist.Add("adv count  "+ adversecorrect.Count());
            if (cleanupperrange <= d.histoavg || d.histoavg <= cleanlowerrange) adversecorrect.Add("1");
            else
                adversecorrect.Add("1");
        }
        private void runoncleanonthread()
        {
           

            string[] file = Directory.GetFiles(Form1.clean);
            if (imagetotal > file.Length) imagetotal = file.Length;
            var T1 = Task.Run(() =>
            {
                for (int i = 0; i < threadcount * 1; i++)
                {
                      //Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                  
                }
            });
            var T2 = Task.Run(() =>
            {
                for (int i = threadcount * 1; i < threadcount * 2; i++)
                {
                    //Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T3 = Task.Run(() =>
            {
                for (int i = threadcount * 2; i < threadcount * 3; i++)
                {
                    // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T4 = Task.Run(() =>
            {
                for (int i = threadcount * 3; i <= threadcount * 4; i++)
                {
                    // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T5 = Task.Run(() =>
            {
                for (int i = threadcount * 4; i < threadcount * 5; i++)
                {
                    // Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T6 = Task.Run(() =>
            {
                for (int i = threadcount * 5; i < threadcount * 6; i++)
                {
                    //    Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T7 = Task.Run(() =>
            {
                for (int i = threadcount * 6; i < threadcount * 7; i++)
                {
                    //    Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
                }

            });
            var T8 = Task.Run(() =>
            {
                for (int i = threadcount * 7; i < file.Length; i++)
                {
                    //   Form1.textlist.Add("clean count  " + i);
                    if (i >= imagetotal) break;
                    cleancheck(runalgo(file[i]));
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
          
        }

        private void runonadverseonthread()
        {
          

            string[] file = Directory.GetFiles(Form1.adverse);
            if (imagetotal > file.Length) imagetotal = file.Length;
            var T1 = Task.Run(() =>
            {
                for (int i = 0; i < threadcount * 1; i++)
                {
                    //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));

                }
            });
            var T2 = Task.Run(() =>
            {
                for (int i = threadcount * 1; i < threadcount * 2; i++)
                {
                    //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T3 = Task.Run(() =>
            {
                for (int i = threadcount * 2; i < threadcount * 3; i++)
                {
                    // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T4 = Task.Run(() =>
            {
                for (int i = threadcount * 3; i <= threadcount * 4; i++)
                {
                    // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T5 = Task.Run(() =>
            {
                for (int i = threadcount * 4; i < threadcount * 5; i++)
                {
                    //  Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T6 = Task.Run(() =>
            {
                for (int i = threadcount * 5; i < threadcount * 6; i++)
                {
                    // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T7 = Task.Run(() =>
            {
                for (int i = threadcount * 6; i < threadcount * 7; i++)
                {
                    // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
                }

            });
            var T8 = Task.Run(() =>
            {
                for (int i = threadcount * 7; i < file.Length; i++)
                {
                    // Form1.textlist.Add("adverse count  " + i);
                    if (i >= imagetotal) break;
                    adversecheck(runalgo(file[i]));
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
      


        }
        private double StandardDeviation(List<double> values, int start, int end)
        {
            double mean = values.Average();// (start, end);
            double variance = Variance(values, mean, start, end);

            return Math.Sqrt(variance);
        }
        private double Variance(List<double> values, double mean, int start, int end)
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


        public double runonthread()
        {
            var T1 = Task.Run(() =>
            {
                runoncleanonthread();

            });
            var T2 = Task.Run(() =>
            {
                //    runonadverse();
                runonadverseonthread();

            });
            T1.Wait();
            T2.Wait();
            return DifferenceMethod();

        }

   
   
    }
    }
