using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdversarialImage
{
    class ChromosomeDecode
    {
    
         public ChromosomeDecode()
        {
        }

        public List<string>   checkstring(string value)
        {

            List<string> algorithm = new List<string>();

            int i = 0;
            string s = "";
            foreach (char c in value)
            {
              s = s + c;
                i++;
                if(i==MainGA.genelength)
                {

                    algorithm.Add(s);
                    s = "";
                    i = 0;
                }

            }
            return algorithm;
        }

       
    }
}
