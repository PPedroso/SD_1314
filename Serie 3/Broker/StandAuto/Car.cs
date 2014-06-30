using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandAuto
{
    public class Car
    {
        private readonly int price;
        private readonly String brand;
        private readonly int yearRegistration;

        public Car(int price, String brand, int yearRegistration) {
            this.price = price;
            this.brand = brand;
            this.yearRegistration = yearRegistration;
        }

        public int getPrice() { return price; }

        public String getBrand() { return brand; }

        public int getYearRegistration() { return yearRegistration; }
    }
}
