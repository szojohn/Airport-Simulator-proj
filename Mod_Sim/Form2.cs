using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace Mod_Sim
{
    public partial class Form2 : Form
    {
        private long moveT = 24;
        private Random rand = new Random();
        //private int passId = 100;
        List<int> passIds = new List<int>();
        private Airport airport = new Airport(20);
        private List<Label> afterSim = new List<Label>();

        public Form2()
        {
            InitializeComponent();
            get__data();
        }

        private void set_terminal_data(int batch, string dest, long dep, int maxA, int maxB)
        {
            switch (batch)
            {
                case 1:
                    firstDestLabel.Text = dest;
                    firstDeptLabel.Text = dep.ToString();
                    firstMaxALabel.Text = maxA.ToString();
                    firstMaxBLabel.Text = maxB.ToString();
                    break;
                case 2:
                    secDestLabel.Text = dest;
                    secDeptLabel.Text = dep.ToString();
                    secMaxALabel.Text = maxA.ToString();
                    secMaxBLabel.Text = maxB.ToString();
                    break;
                case 3:
                    thirdDestLabel.Text = dest;
                    thirdDeptLabel.Text = dep.ToString();
                    thirdMaxALabel.Text = maxA.ToString();
                    thirdMaxBLabel.Text = maxB.ToString();
                    break;
                case 4:
                    fourthDestLabel.Text = dest;
                    fourthDeptLabel.Text = dep.ToString();
                    fourthMaxALabel.Text = maxA.ToString();
                    fourthMaxBLabel.Text = maxB.ToString();
                    break;
                case 5:
                    fifthDestLabel.Text = dest;
                    fifthDeptLabel.Text = dep.ToString();
                    fifthMaxALabel.Text = maxA.ToString();
                    fifthMaxBLabel.Text = maxB.ToString();
                    break;
                default:
                    Console.WriteLine("Something is wrong.");
                    break;
            }
        }

        private void get__data()
        {
            int maxA = 0, maxB = 0, counter = 0, batch = 1;
            int dep = 0, dur = 0;
            string line, dest = "";

            // Create airport
            //Airport airport = new Airport(20);
            //Airport airport = new Airport(26);

            //Get flights
            StreamReader file = new StreamReader(@"D:\Visual Studio Projects\Mod_Sim\flight.txt");
            while ((line = file.ReadLine()) != null)
            {
                switch (counter)
                {
                    case 0:
                        dest = line;
                        counter++;
                        break;
                    case 1:
                        dep = int.Parse(line);
                        counter++;
                        break;
                    case 2:
                        dur = int.Parse(line);
                        counter++;
                        break;
                    case 3:
                        maxA = int.Parse(line);
                        counter++;
                        break;
                    case 4:
                        maxB = int.Parse(line);
                        airport.set(dest, dep, dur, maxA, maxB);
                        set_terminal_data(batch, dest, dep, maxA, maxB);
                        batch++;
                        counter = 0;
                        break;
                    default:
                        Console.WriteLine("Something went wrong");
                        break;
                }
            }
        }

        private void display_in_grid(int id, string dest)
        {
            dataGridView1.Rows.Add(id, dest);
        }

        private void sim()
        {
            int i, j, totalPass;
            double passC, waitC, failC;
            long now = airport.get_time();
            int noofterm = airport.get_numOfTerm();
            List<Flight> term = new List<Flight>();
            List<int> timeOr = new List<int>();
            List<ApplicationCl> pass = new List<ApplicationCl>();
            List<ApplicationCl> waitCopy = new List<ApplicationCl>();
            List<ApplicationCl> fail = new List<ApplicationCl>();

            term.AddRange(airport.get_terminals());
            waitCopy.AddRange(airport.get_waiting_list());
            fail.AddRange(airport.get_failedApps());
            airport.sort_time();
            timeOr.AddRange(airport.get_timeOrder());
            
            pieLabel.Visible = true;

            totalPass = 0;

            for (i = 0; i < noofterm; i++)
            {
                if ((now <= timeOr[i]) && (timeOr[i] <= (now + moveT)))
                {
                    for (j = 0; j < noofterm; j++)
                    {
                        // && (timeOr[i + 1] != timeOr[j])
                        if ((timeOr[i] == term[j].departs_at()))
                        {
                            pass.Clear();
                            pass.AddRange(term[i].get_bookings());

                            foreach (ApplicationCl app in pass)
                            {
                                //dataGridView2.Rows.Add(app.get_id());
                                totalPass++;
                            }

                            foreach (ApplicationCl appp in waitCopy)
                            {
                                if (appp.matches(term[j]) == true)
                                {
                                    fail.Add(appp);
                                    waitCopy.Remove(appp);
                                }
                            }
                        }
                    }
                }
            }

            passC = Convert.ToDouble(totalPass);
            waitC = Convert.ToDouble(waitCopy.Count);
            //failC = Convert.ToDouble(fail.Count);
            pie_content(passC, waitC);
            //set_waiting_label(waitCopy);
        }

        private int gen_passId()
        {
            int passId;
            do
            {
                passId = rand.Next(100, 201);
            }
            while (passIds.Contains(passId));
            
            passIds.Add(passId);
            return passId;
        }

        private void random_pass(Object myObject, EventArgs myEventArgs)
        {
            string location = "";
            bool classtyp = false;
            int locateRand, arrivTime, classTy;

            timer1.Stop();

            locateRand = rand.Next(0, 5);
            switch (locateRand)
            {
                case 0:
                    location = "Clark";
                    break;
                case 1:
                    location = "Manila";
                    break;
                case 2:
                    location = "Cebu";
                    break;
                case 3:
                    location = "Davao";
                    break;
                case 4:
                    location = "Legazpi";
                    break;
                default:
                    Console.WriteLine("Tangina mali");
                    break;
            }

            arrivTime = rand.Next(0, 51);
            classTy = rand.Next(0, 2);
            switch (classTy)
            {
                case 0:
                    classtyp = true;
                    break;
                case 1:
                    classtyp = false;
                    break;
                default:
                    Console.WriteLine("may mali");
                    break;
            }
            //passId = rand.Next(100, 201);
            int curId = gen_passId();
            airport.add_application(gen_passId(), location, arrivTime, classtyp);
            display_in_grid(curId, location);
            sim();

            timer1.Enabled = true;
        }

        private void pie_content(double passCount, double waitCount)
        {
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            pieChart.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Boarded",
                    Values = new ChartValues<double> {passCount},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Waiting",
                    Values = new ChartValues<double> {waitCount},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };
            pieChart.LegendLocation = LegendLocation.Bottom;
            pieChart.Visible = true;
        }

        private void set_waiting_label(List<ApplicationCl> wait)
        {
            foreach (ApplicationCl w in wait.ToList())
            {
                //dataGridView3.Rows.Add(w.get_id());
            }
        }

        private void simulateBtn_Click(object sender, EventArgs e)
        {
            
            dataGridView1.Visible = true;
            //dataGridView2.Visible = true;
            //dataGridView3.Visible = true;

            timer1.Tick += new EventHandler(random_pass);
            timer1.Interval = 5000;
            timer1.Start();

            //timer1.Enabled = true;
            
        }
    }
}
