using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Sim
{
    class Flight
    {
        // attributes
        private string destination;
        private int deptTime;
        private int duration;
        private int maxCapA;
        private int maxCapB;
        private int seatsReservedA;
        private int seatsReservedB;
        private List<ApplicationCl> flightApps;
        private List<ApplicationCl> failedApp;

        // constructor
        public Flight(string fDest = "None", int depT = 0, int dur = 0, int maxA = 0, int maxB = 0, int seatsRA = 0, int seatsRB = 0)
        {
            destination = fDest;
            deptTime = depT;
            duration = dur;
            maxCapA = maxA;
            maxCapB = maxB;
            seatsReservedA = seatsRA;
            seatsReservedB = seatsRB;

            flightApps = new List<ApplicationCl>();
            failedApp = new List<ApplicationCl>();

            //Console.WriteLine("A flight was created")
        }

        //Function that sets flight's information
        public void set_flight(string dest, int deptT, int dur, int maxA, int maxB)
        {
            destination = dest;
            deptTime = deptT;
            duration = dur;
            maxCapA = maxA;
            maxCapB = maxB;
        }

        //Function that adds a passenger to a flight	
        public void add_passenger(ApplicationCl app)
        {
            if(app.matches(this) == true)
            {
                if(app.is_luxury_class() == true)
                {
                    if(seatsReservedA < maxCapA)
                    {
                        flightApps.Add(app);
                        seatsReservedA++;
                    }
                }
                else if(app.is_luxury_class() == false)
                {
                    if(seatsReservedB < maxCapB)
                    {
                        flightApps.Add(app);
                        seatsReservedB++;
                    }
                }
            }
        }

        public void clear_fail()
        {
            failedApp.Clear();
        }

        public List<ApplicationCl> get_fail()
        {
            List<ApplicationCl> fail = new List<ApplicationCl>();
            fail.AddRange(failedApp);
            failedApp.Clear();
            return fail.ToList();
        }

        //Function that returns flight's departure time
        public int departs_at()
        {
            return deptTime;
        }

        //Function that returns flight's arrival time
        public int arrives_at()
        {
            return deptTime + duration;
        }

        //Function that returns flight's destination
        public string get_destination()
        {
            return destination;
        }

        //Function that returns flight's duration
        public int get_duration()
        {
            return duration;
        }

        //Function that returns flight's max capacity for class A
        public int get_maxA()
        {
            return maxCapA;
        }

        //Function that returns flight's max capacity for class B
        public int get_maxB()
        {
            return maxCapB;
        }

        //Function that returns flight's seats reserved for class A
        public int get_seatsReservedA()
        {
            return seatsReservedA;
        }

        //Function that returns flight's seats reserved for class B
        public int get_seatsReservedB()
        {
            return seatsReservedB;
        }

        public void set_seatsReservedA(int a)
        {
            seatsReservedA = a;
        }

        public void set_seatsReservedB(int b)
        {
            seatsReservedB = b;
        }

        //Function that returns flight's total seats available
        public int get_available()
        {
            int maxA = this.get_maxA();
            int maxB = this.get_maxB();
            int seatsA = this.get_seatsReservedA();
            int seatsB = this.get_seatsReservedB();
            int freeSeats = (maxA - seatsA) + (maxB - seatsB);

            return freeSeats;
        }

        //Function that erases a passenger from a flight given an id
        public void cancel_reservations(int cancelR)
        {
            foreach(ApplicationCl app in flightApps.ToList())
            {
                if(app.get_id() == cancelR)
                {
                    if((app.is_luxury_class() == true) && (get_seatsReservedA() > 0))
                    {
                        seatsReservedA--;
                    }
                    else if((app.is_luxury_class() == false) && (get_seatsReservedB() > 0))
                    {
                        seatsReservedB--;
                    }
                    flightApps.Remove(app);
                }
            }
        }

        //Function that returns all flight's applications in a vector
        public List<ApplicationCl> get_bookings()
        {
            //List<ApplicationCl> appcl = new List<ApplicationCl>(flightApps);
            return flightApps;
        }
        
        // destructor
        ~Flight()
        {
            // Console.WriteLine("A flight was terminated")
        }
    }
}
