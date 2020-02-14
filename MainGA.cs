
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdversarialImage
{
    class MainGA
    {
      //  public static ObservableCollection<string> recodedfilters = new ObservableCollection<string>();
        public static ObservableCollection<Fitnessvalues> fitness = new ObservableCollection<Fitnessvalues>();
        public static int genelength = Constant.genesize;
       public static int indidvlength = Constant.genesize*Constant.genelength;
        public static int generationcount = 0;
        public static int inidvidualcount = 0;
        public static void ga()
        { genelength = Constant.genesize;
        indidvlength = Constant.genesize * Constant.genelength;
      
          

            var ga = new CustomizGeneticAlgorithm(3, 12, Constant.population);
            ga.MutationProbability = 0.1f;
            ga.Termination = Constant.generation;
        
            Console.WriteLine("GA running...");
          
        
       //    ga.TaskExecutor = taskExecutor;
            Task.Run(() => {
               
                ga.Start();
             
                Form1.textlist.Add("Best solution found has fitness " +ga.Population.populationset[0].Gene + " " + ga.Population.populationset[0].Fitnessvalue);


                    Form1.textlist.Add(ga.Population.populationset[0].Fitnessvalue+"");
            });
          //  t.Wait();

           // ga.Start();

         
        }

   
    }
}
