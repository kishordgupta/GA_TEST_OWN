using System;
using System.Collections.Generic;

namespace AdversarialImage
{
      class CustomPopulation
    {
        
        public  List<MyChromosome> populationset = new List<MyChromosome>();
        public  CustomPopulation(int max, int min,int populationsize)
        {
            
            populationset.Clear();
            populationset = new List<MyChromosome>();
            Random random = new Random();
            for (int i = 0; i < populationsize; i++)          { MyChromosome chromosome = new MyChromosome(max, min,random);
               // Console.WriteLine(chromosome.Gene);
                populationset.Add(chromosome);
            }


        }
    }   
}