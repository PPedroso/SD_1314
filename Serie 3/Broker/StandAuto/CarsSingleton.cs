using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StandAuto
{
    public class CarsSingleton
    {
        private static readonly CarsSingleton carsSingleton = new CarsSingleton();
        public static CarsSingleton getInstance() { return carsSingleton; }

        private IEnumerable<Car> listCars;
        
        public void initiate(String filePath) {
            XDocument docx = XDocument.Load(filePath);
            listCars = docx.Descendants("car").Select(car =>
                new Car(Int32.Parse(car.Element("id").Value),
                        Int32.Parse(car.Element("price").Value),
                        car.Element("brand").Value,
                        Int32.Parse(car.Element("yearRegistration").Value))
            ).ToList<Car>();
        }

        public IEnumerable<Car> getCarsWithBrand(String brand) {
            return listCars.Where(car => car.getBrand().Equals(brand));
        }

        public IEnumerable<Car> getCarsWithYearRegistrationHigherThen(int year) {
            return listCars.Where(car => car.getYearRegistration() >= year);
        }

        public IEnumerable<Car> getCarsWithPriceLowerThen(int price) {
            return listCars.Where(car => car.getPrice() <= price);
        }
    }
}
