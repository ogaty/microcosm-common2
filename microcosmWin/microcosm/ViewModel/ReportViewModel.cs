using microcosm.Planet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microcosm.ViewModel
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string houseDown { get; set; }
        public string houseRight { get; set; }
        public string houseUp { get; set; }
        public string houseLeft { get; set; }

        public string signFire { get; set; }
        public string signEarth { get; set; }
        public string signAir { get; set; }
        public string signWater { get; set; }

        public string signCardinal { get; set; }
        public string signFixed { get; set; }
        public string signMutable { get; set; }

        public string houseAngular { get; set; }
        public string houseCadent { get; set; }
        public string houseSuccedent { get; set; }

        public ReportViewModel(
            Dictionary<int, PlanetData> signList1,
            double[] list1
        )
        {
            ReCalcReport(signList1, list1);
        }

        public void ReCalcReport(
            Dictionary<int, PlanetData> signList1,
            double[] list1
            )
        {
            int down = 0;
            int right = 0;
            int up = 0;
            int left = 0;

            double[] newList = new double[13];

            Enumerable.Range(1, 12).ToList().ForEach(i =>
            {
                newList[i] = list1[i] - list1[1];
                if (newList[i] < 0)
                {
                    newList[i] += 360;
                }
                //                Console.WriteLine(list1[i].ToString());
            });

            double target;
            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                target = signList1[i].absolute_position - list1[1];
                if (target < 0)
                {
                    target += 360;
                }
                if (
                    (newList[1] <= target && target < newList[2])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target +  ":1");
                    down++;
                    left++;
                }
                else if (
                    (newList[2] <= target && target < newList[3])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":2");
                    down++;
                    left++;
                }
                else if (
                    (newList[3] <= target && target < newList[4])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":3");
                    down++;
                    left++;
                }
                else if (
                    (newList[4] <= target && target < newList[5])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":4");
                    down++;
                    right++;
                }
                else if (
                    (newList[5] <= target && target < newList[6])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":5");
                    down++;
                    right++;
                }
                else if (
                    (newList[6] <= target && target < newList[7])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":6");
                    down++;
                    right++;
                }
                else if (
                    (newList[7] <= target && target < newList[8])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":7");
                    up++;
                    right++;
                }
                else if (
                    (newList[8] <= target && target < newList[9])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":8");
                    up++;
                    right++;
                }
                else if (
                    (newList[9] <= target && target < newList[10])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":9");
                    up++;
                    right++;
                }
                else if (
                    (newList[10] <= target && target < newList[11])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":10");
                    up++;
                    left++;
                }
                else if (
                    (newList[11] <= target && target < newList[12])
                )
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":11");
                    up++;
                    left++;
                }
                else
                {
                    //                    Console.WriteLine(i.ToString() + " " + target + ":12");
                    up++;
                    left++;
                }

            });

            houseDown = down.ToString();
            houseRight = right.ToString();
            houseUp = up.ToString();
            houseLeft = left.ToString();

            OnPropertyChanged("houseDown");
            OnPropertyChanged("houseRight");
            OnPropertyChanged("houseUp");
            OnPropertyChanged("houseLeft");

            int fire = 0;
            int earth = 0;
            int air = 0;
            int water = 0;

            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                if (
                    (0.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 30.0) ||
                    (120.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 150.0) ||
                    (240.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 270.0)
                )
                {
                    fire++;
                }
                else if (
                    (30.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 60.0) ||
                    (150.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 180.0) ||
                    (270.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 300.0)
                )
                {
                    earth++;
                }
                else if (
                    (60.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 90.0) ||
                    (180.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 210.0) ||
                    (300.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 330.0)
                )
                {
                    air++;
                }
                else
                {
                    water++;
                }

            });

            signFire = fire.ToString();
            signEarth = earth.ToString();
            signAir = air.ToString();
            signWater = water.ToString();

            OnPropertyChanged("signFire");
            OnPropertyChanged("signEarth");
            OnPropertyChanged("signAir");
            OnPropertyChanged("signWater");

            int cardinalS = 0;
            int fixedS = 0;
            int mutableS = 0;

            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                if (
                    (0.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 30.0) ||
                    (90.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 120.0) ||
                    (180.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 210.0) ||
                    (270.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 300.0)
                )
                {
                    cardinalS++;
                }
                else if (
                    (30.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 60.0) ||
                    (120.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 150.0) ||
                    (210.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 240.0) ||
                    (300.0 <= signList1[i].absolute_position && signList1[i].absolute_position < 330.0)
                )
                {
                    fixedS++;
                }
                else
                {
                    mutableS++;
                }

            });

            signCardinal = cardinalS.ToString();
            signFixed = fixedS.ToString();
            signMutable = mutableS.ToString();

            OnPropertyChanged("signCardinal");
            OnPropertyChanged("signFixed");
            OnPropertyChanged("signMutable");

            int angularH = 0;
            int succedentH = 0;
            int cadentH = 0;

            Enumerable.Range(0, 10).ToList().ForEach(i =>
            {
                target = signList1[i].absolute_position - list1[1];
                if (target < 0)
                {
                    target += 360;
                }
                if (
                    (newList[1] <= target && target < newList[2]) ||
                    (newList[4] <= target && target < newList[5]) ||
                    (newList[7] <= target && target < newList[8]) ||
                    (newList[10] <= target && target < newList[11])
                )
                {
                    angularH++;
                }
                else if (
                    (newList[2] <= target && target < newList[3]) ||
                    (newList[5] <= target && target < newList[6]) ||
                    (newList[8] <= target && target < newList[9]) ||
                    (newList[11] <= target && target < newList[12])
                )
                {
                    cadentH++;
                }
                else
                {
                    succedentH++;
                }
            });

            houseAngular = angularH.ToString();
            houseCadent = cadentH.ToString();
            houseSuccedent = succedentH.ToString();
            OnPropertyChanged("houseAngular");
            OnPropertyChanged("houseCadent");
            OnPropertyChanged("houseSuccedent");
        }

        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }

        }
    }
}
