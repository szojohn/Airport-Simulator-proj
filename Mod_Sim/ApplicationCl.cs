using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Sim
{
    class ApplicationCl
    {
        // attributes
        private int id;
        private string fName;
        private string lName;
        private string destination;
        private int airportTime;
        private int wantedArrival;
        private bool classType;
        private int arrivalTime;
        private bool classtype;

        // constructor
        public ApplicationCl(int iD, string dest, int airTime, bool classT)
        {
            id = iD;
            destination = dest;
            airportTime = airTime;
            classType = classT;
        }

        //Function to check if an application matches to any available flights
        public bool matches(Flight f)
        {
            string des = f.get_destination();
            int airT = airport_arrival();  //Passenger will be at the airport at this time
            int depT = f.departs_at();  //Flight will depart at this time

            int wantedA = arrived_by();    //Passenger wants to arrive to his destination by this time
            int arrT = f.arrives_at();  //Flight arrives to it's destination at this time

            if (des == destination && airT <= depT)
            {

                return true;   //Function returns 1 if application matches to a flight
            }
            else
            {
                return false;	//Otherwise it returns 0
            }
        }

        //Function that returns passenger's id
        public int get_id()
        {
            return id;
        }

        //Function that returns passenger's full name
        public string get_name()
        {
            string blank = " ";
            return fName + blank + lName;
        }

        //Function that returns passenger's arrival time at the airport
        public int airport_arrival()
        {
            return airportTime;
        }

        //Function that returns passenger's wanted arrival to his destination
        public int arrived_by()
        {
            return wantedArrival;
        }

        //Function that returns passenger's class preference
        public bool is_luxury_class()
        {
            return classType;
        }

        //Function that returns passenger's destination
        public string get_destination()
        {
            return destination;
        }

        // destructor
        ~ApplicationCl()
        {
            //Console.WriteLine("An application was destroyed");
        }
    }
}
