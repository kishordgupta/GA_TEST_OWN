using System;
using Accord.Statistics.Visualizations;

namespace AdversarialImage
{
    public class Fitnessvalues
    {
        private string individual = "";
        private double histogramdistance = 0.0;
        private double cleanupperrange = 0.0;
        private double lowerupperrange = 0.0;

        private double lbp_distance = 0.0;
        private double lbp_cleanupperrange = 0.0;
        private double lbp_lowerupperrange = 0.0;

        private double ecu_distance = 0.0;
        private Accord.Statistics.Visualizations.Histogram ecu_histogram_clean = null;
        private Accord.Statistics.Visualizations.Histogram ecu_histogram_adv = null;

        private double lvalue = 0.0;

        private double crossentropy = 0.0;
        private double crossentropy_clean = 0.0;
        private double crossentropy_adv = 0.0;

        private double crossentropy_PDE = 0.0;
        private double crossentropy_PDE_clean = 0.0;
        private double crossentropy_PDE_adv = 0.0;

        public string Individual { get => individual; set => individual = value; }

        internal string data()
        {
            return Histogramdistance + " " + Lbp_distance + " " + ecu_distance + " " + crossentropy + " " + crossentropy_PDE+ " "+Lvalue;
        }

        public double Histogramdistance { get => histogramdistance; set => histogramdistance = value; }
        public double Cleanupperrange { get => cleanupperrange; set => cleanupperrange = value; }
        public double Lowerupperrange { get => lowerupperrange; set => lowerupperrange = value; }
        public double Lbp_distance { get => lbp_distance; set => lbp_distance = value; }
        public double Lbp_cleanupperrange { get => lbp_cleanupperrange; set => lbp_cleanupperrange = value; }
        public double Lbp_lowerupperrange { get => lbp_lowerupperrange; set => lbp_lowerupperrange = value; }
        public double Ecu_distance { get => ecu_distance; set => ecu_distance = value; }
        public Histogram Ecu_histogram_clean { get => ecu_histogram_clean; set => ecu_histogram_clean = value; }
        public Histogram Ecu_histogram_adv { get => ecu_histogram_adv; set => ecu_histogram_adv = value; }
        public double Lvalue { get => lvalue; set => lvalue = value; }
        public double Crossentropy { get => crossentropy; set => crossentropy = value; }
        public double Crossentropy_clean { get => crossentropy_clean; set => crossentropy_clean = value; }
        public double Crossentropy_adv { get => crossentropy_adv; set => crossentropy_adv = value; }
        public double Crossentropy_PDE { get => crossentropy_PDE; set => crossentropy_PDE = value; }
        public double Crossentropy_PDE_clean { get => crossentropy_PDE_clean; set => crossentropy_PDE_clean = value; }
        public double Crossentropy_PDE_adv { get => crossentropy_PDE_adv; set => crossentropy_PDE_adv = value; }
    }
}