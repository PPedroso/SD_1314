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
        private readonly int id;

        public Car(int id, int price, String brand, int yearRegistration) {
            this.id = id;
            this.price = price;
            this.brand = brand;
            this.yearRegistration = yearRegistration;
        }

        public int getId() { return id; }
        
        public int getPrice() { return price; }

        public String getBrand() { return brand; }

        public int getYearRegistration() { return yearRegistration; }
    }
}
