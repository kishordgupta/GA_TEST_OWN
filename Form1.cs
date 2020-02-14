using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdversarialImage
{
    public partial class Form1 : Form
    {

        public static System.Collections.ObjectModel.ObservableCollection<string> textlist = new System.Collections.ObjectModel.ObservableCollection<string>();
        public static System.Collections.ObjectModel.ObservableCollection<string> accuracytextlist = new System.Collections.ObjectModel.ObservableCollection<string>();
        string mnist_test = @"C:\Users\Cliff\Desktop\test\mnist\test"; //0
        string mnist_cw2 = @"C:\Users\Cliff\Desktop\test\mnist\cw2";//1
        string mnist_deepfool = @"C:\Cliff\kishor\Desktop\test\mnist\df";//2
        string mnist_fsgm = @"C:\Users\Cliff\Desktop\test\mnist\fsgm";//3
        string mnist_jsma = @"C:\Users\Cliff\Desktop\test\mnist\jsma";//4
        string mnist_adv = @"C:\Users\Cliff\Desktop\test\mnist\advgan";//5
        bool test = true;

        string cfiar_test = @"C:\Users\kishor\Desktop\icsw\test\cfiar\test";
        string cfiar_cw2 = @"C:\Users\kishor\Desktop\icsw\test\cfiar\cw2";
        string cfiar_deepfool = @"C:\Users\kishor\Desktop\icsw\test\cfiar\df";
        string cfiar_fsgm = @"C:\Users\kishor\Desktop\icsw\test\cfiar\fsgm";
        string cfiar_jsma = @"C:\Users\kishor\Desktop\icsw\test\cfiar\jsma";
        public Fitnessvalues testfitness = null;
        public static string clean = "";
        public static string adverse = "";

        public Form1()
        { 
            InitializeComponent();
            initiatedefault();
            textlist.CollectionChanged += Textlist_CollectionChanged;
            accuracytextlist.CollectionChanged += Accuracytextlist_CollectionChanged;
        }

        private void Accuracytextlist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {

                richTextBox1.Text = richTextBox1.Text+"\n"+e.NewItems[0].ToString();

            }));
        }

        private void Textlist_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                string newtxt = e.NewItems[0].ToString();
                listBox1.Items.Add(e.NewItems[0]);
                if (newtxt.Contains("Iteration"))
                {
                    label7.Text = e.NewItems[0].ToString();
                }
                if (newtxt.Contains("Sequence "))
                {
                
               
                   // var lista = MainGA.fitness.Where(x => x.Individual.Contains(textBox1.Text)).ToList();
                    var lista = MainGA.fitness.Where(x => x.Histogramdistance== MainGA.fitness.Max(r => r.Histogramdistance)).ToList(); 
                    if (lista.Count > 0)
                    {

                        //  string s=textlist.Where(l => l.Contains(textBox1.Text+"x")).FirstOrDefault();
                        testfitness = lista[0];
                        textBox1.Text = lista[0].Individual;
                        textBox2.Text = lista[0].Histogramdistance+"";
                        textBox3.Text = lista[0].Cleanupperrange + "";
                        textBox5.Text = lista[0].Lowerupperrange + "";
                        textBox4.Text = lista[0].Crossentropy + "";
                        textBox7.Text = lista[0].Crossentropy_PDE + "";
                        textBox6.Text = lista[0].Lvalue + "";
                        test = true;
                        testac();
                    }
                }

            }));
        
      
        }

        void initiatedefault()
        {
        clean = mnist_test;
        adverse = mnist_fsgm;
        }


        void initiatesettings()
        {
            Constant.population =(int)populationsize.Value;
            Constant.samplesize = (int)imagetotal.Value;
            Constant.genelength = (int)algorithmnumber.Value;
            Constant.genesize = (int)genesize.Value;
            Constant.generation =(int)iterationnumber.Value;
            Constant.ThreadCount = (int)ThreadCount.Value;
            Constant.greyscale = checkBox1.Checked;
            textlist.Add("population:"+  Constant.population);
            textlist.Add("samplesize:" + Constant.samplesize);
            textlist.Add("genelength:" + Constant.genelength);
            textlist.Add("genesize:" + Constant.genesize);
            textlist.Add("generation:" + Constant.generation);
            textlist.Add("ThreadCount:" + Constant.ThreadCount);
            textlist.Add("greyscale:" + Constant.greyscale);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            initiatesettings();
            MainGA.ga();
            textBox1.Text = "Searching..";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                clean = folderBrowserDialog1.SelectedPath;
                // listBox1.Items.Add("Clean Sample path: " + clean);
                textlist.Add("Clean Sample path: " + clean);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                adverse = folderBrowserDialog1.SelectedPath;
                // listBox1.Items.Add("Adversarial Sample path: "+adverse);
                textlist.Add("Adversarial Sample path: " + adverse);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        void testac()
        {
            string algorithmseq = testfitness.Individual;// textBox1.Text;
            double upperrange = testfitness.Cleanupperrange;// Double.Parse(textBox2.Text);
            double lowerrane = testfitness.Lowerupperrange;// Doulowble.Parse(textBox3.Text);
            Accuracy ac = new Accuracy();

            Task.Run(() => {

                ac.test(algorithmseq, upperrange, lowerrane);
                saveinfile();
            });
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            initiatesettings();
            if (test)
            {
                testac();


            }
        }
        void saveinfile()
        {
            string[] lines = textlist.ToArray();
            this.Invoke(new MethodInvoker(delegate ()
            {
                lines[0] = lines[0]+"\n"+richTextBox1.Text.ToString() + "\n";

            }));
            string n = string.Format("text-{0:yyyy-MM-dd_hh-mm-ss-tt}.txt",
          DateTime.Now);
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(n, lines);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            HeuristicPopulation heuristicPopulation = new HeuristicPopulation();
            heuristicPopulation.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
