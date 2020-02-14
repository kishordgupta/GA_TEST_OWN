
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdversarialImage
{
    class MyChromosome 
    {
        // Change the argument value passed to base construtor to change the length 
        // of your chromosome.
        string gene = "";
        int length = 0;
        double normalizehistogramdistance = 0.0;
        double normalizelbp_distance = 0.0;
        double normalizeecu_distance = 0.0;
        double normalizelvalue = 0.0;
        double normalizecrossentropy = 0.0;
        double normalizecrossentropy_PDE = 0.0;
        double fitnessvalue = 0.0;
        bool sequencemeasured = false;
        Random random = new Random();
        public MyChromosome(int max,int min,Random r)
        {
          
             Length= r.Next(min, max);
            for (int i = 0; i <= Length; i++)
            { 
            Gene = Gene+ ((int)r.Next(1, 10)%2);
            }

        }
        public MyChromosome(string data)
        {
                Gene = data;
            Length = data.Length;

        }
        public string Gene { get => gene; set => gene = value; }
        public int Length { get => length; set => length = value; }
        public Fitnessvalues Fitnessvalues { get; set; }
        public double Fitnessvalue { get => fitnessvalue; set => fitnessvalue = value; }
        public double Normalizehistogramdistance { get => normalizehistogramdistance; set => normalizehistogramdistance = value; }
        public double Normalizelbp_distance { get => normalizelbp_distance; set => normalizelbp_distance = value; }
        public double Normalizeecu_distance { get => normalizeecu_distance; set => normalizeecu_distance = value; }
        public double Normalizelvalue { get => normalizelvalue; set => normalizelvalue = value; }
        public double Normalizecrossentropy { get => normalizecrossentropy; set => normalizecrossentropy = value; }
        public double Normalizecrossentropy_PDE { get => normalizecrossentropy_PDE; set => normalizecrossentropy_PDE = value; }
        public bool Sequencemeasured { get => sequencemeasured; set => sequencemeasured = value; }

        internal string GetGenes()
        {
            return Gene;
        }
    }
}
