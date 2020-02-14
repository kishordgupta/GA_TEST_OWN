using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AdversarialImage
{
    public class CustomizGeneticAlgorithm
    {
           CustomPopulation population;
         

        public CustomizGeneticAlgorithm(int min, int max, int size)
        {
            Population = new CustomPopulation(max,min,size);
          
        }

        public float MutationProbability { get; internal set; }
        public int Termination { get; internal set; }
        internal CustomPopulation Population { get => population; set => population = value; }

        internal void Start()
        {
            int i = 0;
            while(i< Termination) {
                MyProblemFitness();
                Selection();
                Crossover();
                Mutation();
                i++;
            }

        }

        private void Mutation()
        {
          //  throw new NotImplementedException();
        }

        private void Crossover()
        {
            int size = Population.populationset.Count();
            int grpnumber = 2;
            if (size / 2 >= 12) grpnumber = 12;
            else if (size / 2 >= 10) grpnumber = 10;
            else if (size / 2 >= 8) grpnumber = 8;
            else if (size / 2 >= 6) grpnumber = 6;
            else if (size / 2 >= 4) grpnumber = 4;
            else if (size / 2 >= 2) grpnumber = 2;

            List<MyChromosome> parents = new List<MyChromosome>();

            foreach (MyChromosome p in Population.populationset)
            {
                parents.Add(p);
                if (parents.Count >= grpnumber) break;

            }
            createoffspring(grpnumber, parents);
        }

        private void Selection()
        {
            Population.populationset = Population.populationset.OrderByDescending(w => w.Fitnessvalue).ToList();
            countnew = Population.populationset.Count;


        }
        static int countnew = 0;
        private void createoffspring(int grpnumber, List<MyChromosome> parents)
        {
            Random r = new Random();
            for (int i=0;i<grpnumber-1;i++)
            {
                string data = parents[i].Gene + parents[i + 1].Gene;
               
                int firstcrosspoint = r.Next(0, parents[i].Gene.Length);
                int secondcrosspoint = r.Next(0, parents[i + 1].Gene.Length);
                string nextS1 = parents[i].Gene.Substring(0, firstcrosspoint) + parents[i + 1].Gene.Substring(secondcrosspoint);
                string nextS2 = parents[i + 1].Gene.Substring(0, secondcrosspoint) + parents[i].Gene.Substring(firstcrosspoint);

                if(nextS1.Length%Constant.genesize!=0)
                {
                    int k = nextS1.Length % Constant.genesize;
                    for(int l=0;l<k; l++)
                    {
                        nextS1 = nextS1 + "0";
                    }
                }

                if (nextS2.Length % Constant.genesize != 0)
                {
                    int k = nextS2.Length % Constant.genesize;
                    for (int l = 0; l < k; l++)
                    {
                        nextS2 = nextS2 + "1";
                    }
                }
                if (!Population.populationset.Exists(x => x.Gene.Contains(nextS1)))
                  { MyChromosome children1 = new MyChromosome(nextS1);
                    Population.populationset.Add(children1);
                }
                if (!Population.populationset.Exists(x => x.Gene.Contains(nextS2))){
                    MyChromosome children2 = new MyChromosome(nextS2);
                    Population.populationset.Add(children2); }
            }
        }

        double minhistogramdistance = 9999999.0;
        double minlbp_distance = 9999999.0;
        double minecu_distance = 9999999.0;
        double minlvalue = 9999999.0;
        double mincrossentropy = 9999999.0;
        double mincrossentropy_PDE = 9999999.0;

        double maxhistogramdistance = 0.0;
        double maxlbp_distance = 0.0;
        double maxecu_distance = 0.0;
        double maxlvalue = 0.0;
        double maxcrossentropy = 0.0;
        double maxcrossentropy_PDE = 0.0;

        double whistogramdistance = 0.0;
        double wlbp_distance = 0.0;
        double wecu_distance = 0.0;
        double wlvalue = 0.0;
        double wcrossentropy = 0.0;
        double wcrossentropy_PDE = 0.0;

      
        public  void MyProblemFitness()
        {
            int threadcount = 2;
            var T1 = Task.Run(() => {
                for (int i = countnew; i< countnew+threadcount * 1|| i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count)break;
               //     System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T2 = Task.Run(() => {
                for (int i = countnew + threadcount * 1; i < countnew + threadcount * 2 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                   // System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T3 = Task.Run(() => {
                for (int i = countnew + threadcount * 2; i < countnew + threadcount * 3 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                  //  System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T4 = Task.Run(() => {
                for (int i = countnew + threadcount * 3; i < countnew + threadcount * 4 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                  //  System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T5 = Task.Run(() => {
                for (int i = countnew + threadcount * 4; i < countnew + threadcount * 5 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                  //  System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T6 = Task.Run(() => {
                for (int i = countnew + threadcount * 5; i < countnew + threadcount * 6 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                 //   System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T7 = Task.Run(() => {
                for (int i = countnew + threadcount * 6; i < countnew + threadcount * 7 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                //    System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
                }

            });
            var T8 = Task.Run(() => {
                for (int i = countnew + threadcount * 7; i < countnew + threadcount * 8 || i < Population.populationset.Count; i++)
                {
                    if (i >= Population.populationset.Count) break;
                 //   System.Console.WriteLine(Population.populationset[i].Gene);
                    Population.populationset[i].Fitnessvalues = Evaluate(Population.populationset[i]);

                    Population.populationset[i].Sequencemeasured = true;
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


                for (int i=0;i<Population.populationset.Count;i++)
            {
             
                if (Population.populationset[i].Fitnessvalues.Histogramdistance <= minhistogramdistance) minhistogramdistance = Population.populationset[i].Fitnessvalues.Histogramdistance;
                if (Population.populationset[i].Fitnessvalues.Lbp_distance <= minlbp_distance) minlbp_distance = Population.populationset[i].Fitnessvalues.Lbp_distance;
                if (Population.populationset[i].Fitnessvalues.Ecu_distance <= minecu_distance) minecu_distance = Population.populationset[i].Fitnessvalues.Ecu_distance;
                if (Population.populationset[i].Fitnessvalues.Lvalue <= minlvalue) minlvalue = Population.populationset[i].Fitnessvalues.Lvalue;
                if (Population.populationset[i].Fitnessvalues.Crossentropy <= mincrossentropy) mincrossentropy = Population.populationset[i].Fitnessvalues.Crossentropy;
                if (Population.populationset[i].Fitnessvalues.Crossentropy_PDE <= mincrossentropy_PDE) mincrossentropy_PDE = Population.populationset[i].Fitnessvalues.Crossentropy_PDE;
                if (Population.populationset[i].Fitnessvalues.Histogramdistance >= maxhistogramdistance) maxhistogramdistance = Population.populationset[i].Fitnessvalues.Histogramdistance;
                if (Population.populationset[i].Fitnessvalues.Lbp_distance >= maxlbp_distance) maxlbp_distance = Population.populationset[i].Fitnessvalues.Lbp_distance;
                if (Population.populationset[i].Fitnessvalues.Ecu_distance >= maxecu_distance) maxecu_distance = Population.populationset[i].Fitnessvalues.Ecu_distance;
                if (Population.populationset[i].Fitnessvalues.Lvalue >= maxlvalue) maxlvalue = Population.populationset[i].Fitnessvalues.Lvalue;
                if (Population.populationset[i].Fitnessvalues.Crossentropy >= maxcrossentropy) maxcrossentropy = Population.populationset[i].Fitnessvalues.Crossentropy;
                if (Population.populationset[i].Fitnessvalues.Crossentropy_PDE >= maxcrossentropy_PDE) maxcrossentropy_PDE = Population.populationset[i].Fitnessvalues.Crossentropy_PDE;

            }
          
            maxhistogramdistance = maxhistogramdistance - minhistogramdistance;
             maxlbp_distance = maxlbp_distance - minlbp_distance;
             maxecu_distance = maxecu_distance - minecu_distance;
             maxlvalue = maxlvalue - minlvalue;
             maxcrossentropy = maxcrossentropy - mincrossentropy;
             maxcrossentropy_PDE = maxcrossentropy_PDE - mincrossentropy_PDE;

            for (int i= 0; i < Population.populationset.Count; i++)
            {

                Population.populationset[i].Normalizehistogramdistance = (Population.populationset[i].Fitnessvalues.Histogramdistance - minhistogramdistance) / maxhistogramdistance;
                Population.populationset[i].Normalizelbp_distance = (Population.populationset[i].Fitnessvalues.Lbp_distance - minlbp_distance ) / maxlbp_distance;
                Population.populationset[i].Normalizeecu_distance = (Population.populationset[i].Fitnessvalues.Ecu_distance - minecu_distance) / maxecu_distance;
                Population.populationset[i].Normalizelvalue = (Population.populationset[i].Fitnessvalues.Lvalue - minlvalue) / maxlvalue;
                Population.populationset[i].Normalizecrossentropy = (Population.populationset[i].Fitnessvalues.Crossentropy - mincrossentropy) / maxcrossentropy;
                Population.populationset[i].Normalizecrossentropy_PDE = (Population.populationset[i].Fitnessvalues.Crossentropy_PDE - mincrossentropy_PDE ) / maxcrossentropy_PDE;

                whistogramdistance = whistogramdistance + Population.populationset[i].Normalizehistogramdistance;
                wlbp_distance = wlbp_distance + Population.populationset[i].Normalizelbp_distance;
                wecu_distance = wecu_distance + Population.populationset[i].Normalizeecu_distance;
                wlvalue = wlvalue + Population.populationset[i].Normalizelvalue;
                wcrossentropy = wcrossentropy + Population.populationset[i].Normalizecrossentropy;
                wcrossentropy_PDE = wcrossentropy_PDE + Population.populationset[i].Normalizecrossentropy_PDE;


       
            }

            int l = Population.populationset.Count();
            whistogramdistance = whistogramdistance / l;
            wlbp_distance = wlbp_distance / l;
            wecu_distance = wecu_distance / l;
            wlvalue = wlvalue / l;
            wcrossentropy = wcrossentropy / l;
            wcrossentropy_PDE = wcrossentropy_PDE / l;

            double total = whistogramdistance + wlbp_distance + wecu_distance + wlvalue + wlvalue + wcrossentropy + wcrossentropy_PDE;
            whistogramdistance = whistogramdistance / total;
            wlbp_distance = wlbp_distance / total;
            wecu_distance = wecu_distance / total;
            wlvalue = wlvalue / total;
            wcrossentropy = wcrossentropy / total;
            wcrossentropy_PDE = wcrossentropy_PDE / total;

            System.Console.WriteLine("Weights: " + whistogramdistance + " " + wlbp_distance + " " + wecu_distance + " " + wlvalue + " " + wcrossentropy + " " + wcrossentropy_PDE);

            for (int i = 0; i < Population.populationset.Count; i++)
            {

                Population.populationset[i].Fitnessvalue = (whistogramdistance * Population.populationset[i].Normalizehistogramdistance) +
                                         (wlbp_distance * Population.populationset[i].Normalizelbp_distance) +
                                         (wecu_distance * Population.populationset[i].Fitnessvalues.Ecu_distance) +
                                         (wlvalue * Population.populationset[i].Normalizelvalue) +
                                         (wcrossentropy * Population.populationset[i].Normalizecrossentropy) +
                                         (wcrossentropy_PDE * Population.populationset[i].Normalizecrossentropy_PDE);
                string s =Population.populationset[i].Gene+" normalize: " + Population.populationset[i].Fitnessvalue +" "+ Population.populationset[i].Normalizehistogramdistance + " " + Population.populationset[i].Normalizelbp_distance
                    + " " + Population.populationset[i].Normalizeecu_distance + " " + Population.populationset[i].Normalizelvalue + " " + Population.populationset[i].Normalizecrossentropy 
                    + " " + Population.populationset[i].Normalizecrossentropy_PDE;
                Form1.textlist.Add(s);

            }


         
         
        }


        Fitnessvalues Evaluate(MyChromosome chromosome)
        {    if (chromosome.Sequencemeasured) return chromosome.Fitnessvalues;
            string a = chromosome.GetGenes();
         

            // Evaluate the fitness of chromosome.

            //  fitnesstcheck(chromosome);
            RunAlgorithm r = new RunAlgorithm();
           return r.RunAlgorithmchromosomes(chromosome);

        }

    }
}