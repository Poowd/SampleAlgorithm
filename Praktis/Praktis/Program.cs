using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace HelloWorld
{
    class Program
    {
        public static NameValueCollection Route_Collection = new NameValueCollection(); // STORES ALL THE ROUTE VALUES
        public static NameValueCollection Path_Collection = new NameValueCollection();

        public static double Route_Value = 0; // DURATION * FARE
        public static double Path_Value = 0;  // (ROUTE_1 + ROUTE_2 + ROUTE_N) / N

        public static double Average_Speed = 0; // KILOMETER PER HOUR
        public static double Distance = 0; // KILOMETER
        public static double Duration = 0; // HOURS; MINUTES ARE DECIMAL FORM OF HOURS
        public static int Base_Fare_Distance = 0; // MINIMUM JEEPNEY DISTANCE BEFORE INCREASING PRICE PER KILOMETER
        public static int Base_Fare = 0; // MINIMUM JEEPNEY PRICE 
        public static int Fare_Increase = 0; // FARE INCREASE PER KILOMETER
        public static int Fare = 0; // TOTAL FARE PRICE INCLUDING PRICE PER KILOMETER

        // GIVEN THAT YOU WANT TO TRAVEL FROM MUNOZ TO MONUMENTO VIA LONG ROUTE :

        // SAMPLE_PATH[0,-,-] = PATH

        //      SAMPLE_PATH[-,0,-] = ROUTES IN A PATH ;
        //        SAMPLE_PATH[0,0,-] = MUNOZ TO SIENA COLLEGE QC.
        //        SAMPLE_PATH[0,1,-] = SIENA COLLEGE QC. TO AYALA MALL
        //        SAMPLE_PATH[0,2,-] = AYALA MALL TO MONUMENTO

        //          SAMPLE_PATH[-,-,0] = TYPE OF PUBLIC UTILITY VEHICLE
        //          SAMPLE_PATH[-,-,1] = APPROXIMATE ROUTE DISTANCE PER RIDE

        public static double[,,] Sample_Path =
        {
            { { 1, 3.1 },  { 2, 2.2 }, { 3, 2.5 } }, // FROM MUNOZ TO MONUMENTO
            { { 3, 5.4 },  { 1, 0.1 }, { 1, 0.1 } }, // FROM MUNOZ TO MONUMENTO 2
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Calculating all Paths for [Munoz] to [Monumento]");
            for (int i = 0; i < Sample_Path.GetLength(0); i++)
            {
                Console.WriteLine("Path No.{0}", i + 1);
                Function(i);
                Route_Collection.Clear();
                Route_Value = 0; // DURATION * FARE
                Path_Value = 0;  // (ROUTE_1 + ROUTE_2 + ROUTE_N) / N

                Average_Speed = 0; // KILOMETER PER HOUR
                Distance = 0; // KILOMETER
                Duration = 0; // HOURS; MINUTES ARE DECIMAL FORM OF HOURS
                Base_Fare_Distance = 0; // MINIMUM JEEPNEY DISTANCE BEFORE INCREASING PRICE PER KILOMETER
                Base_Fare = 0; // MINIMUM JEEPNEY PRICE 
                Fare_Increase = 0; // FARE INCREASE PER KILOMETER
                Fare = 0; // TOTAL FARE PRICE INCLUDING PRICE PER KILOMETER
            }

            Console.WriteLine("Path Values for [Munoz] to [Monumento]");
            for (int i = 0; i < Path_Collection.Count; i++)
            {
                Console.WriteLine("{0} : {1}", Path_Collection.GetKey(i), Path_Collection[i]);
            }
        }

        static void Function(int Path_Index)
        {
            for (int i = 0; i < Sample_Path.GetLength(1); i++)
            {
                double Route = Get_Route_Value(Sample_Path[Path_Index, i, 0], Sample_Path[Path_Index, i, 1]);
                string Route_Key = "Route_" + (i + 1);
                Route_Collection.Add(Route_Key, Route.ToString("#.##"));
            }

            foreach (string i in Route_Collection)
            {
                Console.WriteLine("{0} : {1}", i, Route_Collection[i]);
            }

            for (int i = 0; i < Route_Collection.Count; i++)
            {
                Get_Path_Value(i);
            }

            Path_Value = Path_Value / Route_Collection.Count;
            Console.WriteLine("Path Value : {0}\n", Path_Value.ToString("#.##"));

            string Path_Key = "Path_" + (Path_Index + 1);
            Path_Collection.Add(Path_Key, Path_Value.ToString("#.##"));
        }

        static double Get_Route_Value(double Vehicle, double Route_Distance)
        {
            // DISTANCE / AVERAGE SPEED = TIME
            if (Vehicle == 1) { Average_Speed = 15; Base_Fare = 12; Fare_Increase = 2; Base_Fare_Distance = 4; }
            if (Vehicle == 2) { Average_Speed = 5; Base_Fare = 30; Fare_Increase = 0; Base_Fare_Distance = 0; }
            if (Vehicle == 3) { Average_Speed = 25; Base_Fare = 15; Fare_Increase = 5; Base_Fare_Distance = 2; }

            Distance = Route_Distance;
            Duration = Distance / Average_Speed;
            Fare = (int)(Base_Fare + ((Distance - Base_Fare_Distance) * Fare_Increase));
            Route_Value = Duration * Fare;

            return Route_Value;
        }

        static double Get_Path_Value(int Counter)
        {
            Path_Value = Path_Value + Convert.ToDouble(Route_Collection[Counter]);
            return Path_Value;
        }
    }
}
