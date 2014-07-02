using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StandAuto
{
    public class Car
    {
        private static readonly int TRUE = 1;
        private static readonly int FALSE = 0;
        private readonly int price;
        private readonly String brand;
        private readonly int yearRegistration;
        private readonly int id;
        private volatile Int32 isAvailable;

        public Car(int id, int price, String brand, int yearRegistration, Int32 isAvailable) {
            this.id = id;
            this.price = price;
            this.brand = brand;
            this.yearRegistration = yearRegistration;
            this.isAvailable = isAvailable;
        }

        public int getId() { return id; }
        
        public int getPrice() { return price; }

        public String getBrand() { return brand; }

        public int getYearRegistration() { return yearRegistration; }

        public bool getIsAvailable() { return isAvailable == TRUE; }

        public bool tryReserve() { return Interlocked.CompareExchange(ref isAvailable, TRUE, FALSE) == FALSE; }
    }
}
