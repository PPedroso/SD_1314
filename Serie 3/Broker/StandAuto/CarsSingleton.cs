﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StandAuto {

    public class CarsSingleton
    {
        private static readonly CarsSingleton carsSingleton = new CarsSingleton();
        public static CarsSingleton getInstance() { return carsSingleton; }

        private IDictionary<int, Car> mapCars;
        private string filePath;

        public void initiate(String filePath) {
            this.filePath = filePath;
            this.mapCars = new Dictionary<int, Car>();
            foreach (XElement e in XDocument.Load(filePath).Descendants()) {
                Car car = new Car(Int32.Parse(e.Element("id").Value),
                                  Int32.Parse(e.Element("price").Value),
                                  e.Element("brand").Value,
                                  Int32.Parse(e.Element("yearRegistration").Value),
                                  Int32.Parse(e.Element("isAvailable").Value));
                mapCars.Add(car.getId(), car);
            }
        }

        public IEnumerable<Car> getCarsWithBrand(String brand) {
            return mapCars.Values.Where(car => car.getBrand().Equals(brand));
        }

        public IEnumerable<Car> getCarsWithYearRegistrationHigherThen(int year) {
            return mapCars.Values.Where(car => car.getYearRegistration() >= year);
        }

        public IEnumerable<Car> getCarsWithPriceLowerThen(int price) {
            return mapCars.Values.Where(car => car.getPrice() <= price);
        }

        public bool setReservedStatus(int carId, bool isAvailable) {
            if (mapCars[carId].tryReserve()) {
                //in case the reservation is successefull, it's necessary to save
                
                return true;
            } else {
                return false;
            }
        }
    }
}
