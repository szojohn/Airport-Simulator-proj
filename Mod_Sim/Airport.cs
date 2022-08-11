using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Sim
{
    class Airport
    {
        // attributes
        private long currTime;
        private List<Flight> terminals;
        private List<int> timeOrder;
        private int numberOfTerminals = 5;
        private int terminalCounter = 0;
        private List<ApplicationCl> waitingList;
        private List<ApplicationCl> Passengers;
        private List<ApplicationCl> failedApps;

        // constructor
        public Airport(long currT = 0)
        {
            currTime = currT;
            terminals = new List<Flight>(5);
            timeOrder = new List<int>(5);
            waitingList = new List<ApplicationCl>();
            Passengers = new List<ApplicationCl>();
            failedApps = new List<ApplicationCl>();

            Console.WriteLine("An airport was created");
        }

        // Function that sets a flight in a terminal
        public void set(string dest, int deptT, int dur, int maxA, int maxB)
        {
            if(terminalCounter < numberOfTerminals)
            {
                terminals.Add(new Flight(dest, deptT, dur, maxA, maxB));
                terminalCounter++;
            }
        }

        // Function that returns the current time
        public long get_time()
        {
            return currTime;
        }

        //Function that cancels reserved seats in any flight of the airport 
        //and removes applications from airport's waiting list, given a passenger's id
        public void add_application(int iD, string dest, int airT, bool classT)
        {
            int i = 0;
            int flag = 0;
            int seatA = terminals[i].get_seatsReservedA();
            int seatB = terminals[i].get_seatsReservedB();

            ApplicationCl app = new ApplicationCl(iD, dest, airT, classT);

            for(i = 0; i < numberOfTerminals; i++)
            {
                if((app.matches(terminals[i]) == true))
                {
                    if (app.is_luxury_class() == true)
                    {
                        if (seatA < terminals[i].get_maxA())
                        {
                            terminals[i].add_passenger(app);
                            flag++;
                            break;
                        }
                        else
                        {
                            failedApps.Add(app);
                        }
                    }
                    else if(app.is_luxury_class() == false)
                    {
                        if (seatB < terminals[i].get_maxB())
                        {
                            terminals[i].add_passenger(app);
                            flag++;
                            break;
                        }
                        else
                        {
                            failedApps.Add(app);
                        }
                    }
                }
            }
            if(flag < 1)
            {
                waitingList.Add(app);
            }
        }

        //Function that cancels reserved seats in any flight of the airport 
        //and removes applications from airport's waiting list, given a passenger's id
        public void cancel_applications(int cancelA)
        {
            foreach(ApplicationCl app in waitingList.ToList())
            {
                if((app.get_id()) == cancelA)
                {
                    waitingList.Remove(app);
                }
            }
            for(int i = 0; i < numberOfTerminals; i++)
            {
                if(terminals[i] != null)
                {
                    terminals[i].cancel_reservations(cancelA);
                }
            }
        }

        //Function that takes the information of a flight and returns  the number that represents the flight's termimal
        public int add_flight(string dest, int deptT, int dur, int maxA, int maxB)
        {
            Flight f = new Flight(dest, deptT, dur, maxA, maxB);
            int i, tMaxA, fMaxA, tMaxB, fMaxB;
            int terminal = 0;
            long tDeptT, fDeptT, tDur, fDur;
            string tDest, fDest;

            for(i = 0; i < numberOfTerminals; i++)
            {
                tDest = terminals[i].get_destination();
                fDest = f.get_destination();
                tDeptT = terminals[i].departs_at();
                fDeptT = f.departs_at();
                tDur = terminals[i].get_duration();
                fDur = f.get_duration();
                tMaxA = terminals[i].get_maxA();
                fMaxA = f.get_maxA();
                tMaxB = terminals[i].get_maxB();
                fMaxB = f.get_maxB();

                if((fDest == tDest) && (tDeptT == fDeptT) && (tDur == fDur) && (tMaxA == fMaxA) && (tMaxB == fMaxB))
                {
                    terminal = i + 1;
                    break;
                }
                else
                {
                    terminal = 0;
                }
            }
            return terminal;
        }

        //Function that takes a terminal's number and cancels the flight that departs through the terminal
        public void cancel_flight(int term)
        {
            List<ApplicationCl> cancelledApps = new List<ApplicationCl>();
            for(int i = 0; i < numberOfTerminals; i++)
            {
                if(i == term - 1)
                {
                    cancelledApps.AddRange(terminals[i].get_bookings());
                    //terminals[i] = null;
                    terminals[i] = new Flight();
                    break;
                }
            }
            foreach(var app in cancelledApps)
            {
                waitingList.Insert(0, app);
            }
        }

        public void sort_time()
        {
            int i = 0;
            for (i = 0; i < numberOfTerminals; i++)
            {
                timeOrder.Add(terminals[i].departs_at());
            }

            timeOrder.Sort();
        }

        //Function that prints an airport's timetable
        public void show_timetable()
        {
            int i, j;

            sort_time();

            for(i = 0; i < numberOfTerminals; i++)
            {
                for(j = 0; j < numberOfTerminals; j++)
                {
                    // && (timeOrder[i + 1] != timeOrder[j])
                    if (timeOrder[i] == terminals[j].departs_at())
                    {
                        Console.WriteLine("   * Flight No: " + (i + 1) + " *");
                        Console.WriteLine("Destination: " + terminals[j].get_destination());
                        Console.WriteLine("Departure Time: " + terminals[j].departs_at());
                        Console.WriteLine("Duration: " + terminals[j].get_duration());
                        Console.WriteLine("Max Capacity for class A: " + terminals[j].get_maxA());
                        Console.WriteLine("Max Capacity for class B: " + terminals[j].get_maxB());
                        Console.WriteLine();
                        break;
                    }
                }
            }
        }

        // Function that prints the names of people whose applications are still in the waiting list
        public void show_people_waiting()
        {
            Console.WriteLine("People's names in the waiting list are: ");
            Console.WriteLine();

            foreach(var app in waitingList)
            {
                Console.WriteLine(app.get_name());
            }
            Console.WriteLine();
        }

        public List<ApplicationCl> get_waiting_list()
        {
            return waitingList.ToList();
        }

        //Function that return the number of applications of the waiting list that got deleted
        public int failed_applications()
        {
            int failed = failedApps.Count;
            return failed;
        }

        //Function that takes a time period and moves the time instantly
        public void flow_time(long moveT)
        {
            int i, j;
            long now = get_time();
            //List<Flight> sortedTerm = new List<Flight>();
            //sortedTerm = terminals.OrderBy(o => o.departs_at()).ToList();
            for(i = 0; i < numberOfTerminals; i++)
            {
                if((now <= timeOrder[i]) && (timeOrder[i] <= (now + moveT)))
                {
                    for(j = 0; j < numberOfTerminals; j++)
                    {
                        // && (timeOrder[i + 1] != timeOrder[j])
                        //Console.WriteLine("index i: " + i.ToString() + "  index j: " + j.ToString() + "  timeorder: " + timeOrder[i].ToString() + "  terminals:  " + terminals[i].departs_at().ToString() + "  " + terminals[i].get_destination());
                        if (timeOrder[i] == terminals[j].departs_at() && (timeOrder[i + 1] != timeOrder[j]))
                        {
                            Console.WriteLine("*** Flight to " + terminals[j].get_destination() + " departed at " + terminals[j].departs_at() + " units of time***");
                            Console.WriteLine();
                            Passengers.Clear();
                            Passengers.AddRange(terminals[j].get_bookings());
                            Console.WriteLine("People who boarded in the flight are:");
                            Console.WriteLine();

                            foreach(ApplicationCl app in Passengers)
                            {
                                Console.WriteLine(app.get_name());
                            }
                            foreach (ApplicationCl appp in waitingList.ToList())
                            {
                                if ((appp.matches(terminals[j])) == true)
                                {
                                    Console.WriteLine("Failed: " + appp.get_name());
                                    failedApps.Add(appp);
                                    waitingList.Remove(appp);
                                }
                            }
                            Console.WriteLine("i: " + i.ToString() + "  j: " + j.ToString());
                            //terminals[j] = null;
                            terminals[j] = new Flight();
                            //break;
                        }
                        //Console.WriteLine(j.ToString());
                    }
                    Console.WriteLine();
                }
            }
        }

        public int get_numOfTerm()
        {
            return numberOfTerminals;
        }

        public List<int> get_timeOrder()
        {
            return timeOrder.ToList();
        }

        public List<Flight> get_terminals()
        {
            return terminals.ToList();
        }

        public List<ApplicationCl> get_failedApps()
        {
            return failedApps.ToList();
        }

        public void form_flow_time(long moveT)
        {
            int i, j;
            long now = get_time();

            for (i = 0; i < numberOfTerminals; i++)
            {
                if ((now <= timeOrder[i]) && (timeOrder[i] <= (now + moveT)))
                {
                    for (j = 0; j < numberOfTerminals; j++)
                    {
                        if (timeOrder[i] == terminals[i].departs_at() && (timeOrder[i + 1] != timeOrder[j]))
                        {
                            Passengers.Clear();
                            Passengers.AddRange(terminals[j].get_bookings());

                            foreach (ApplicationCl app in Passengers)
                            {
                                Console.WriteLine(app.get_name());
                            }
                            foreach (ApplicationCl appp in waitingList)
                            {
                                if ((appp.matches(terminals[j])) == true)
                                {
                                    failedApps.Add(appp);
                                    waitingList.Remove(appp);
                                }
                            }
                            Console.WriteLine("i: " + i.ToString() + "  j: " + j.ToString());
                            //terminals[j] = null;
                            terminals[j] = new Flight();
                            //break;
                        }
                        //Console.WriteLine(j.ToString());
                    }
                    Console.WriteLine();
                }
            }
        }

        // destructor
        ~Airport()
        {
            terminals.Clear();

            Console.WriteLine("An airport was destroyed");
        }
    }
}
