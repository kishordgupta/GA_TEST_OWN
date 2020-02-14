
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdversarialImage
{
    class MyProblemFitness
    {
     
        public Fitnessvalues Evaluate(MyChromosome chromosome)
        {
        
           
           
            //  fitnesstcheck(chromosome);
            RunAlgorithm r = new RunAlgorithm();

          
            return r.RunAlgorithmchromosomes(chromosome);
        }

        
    }
}
