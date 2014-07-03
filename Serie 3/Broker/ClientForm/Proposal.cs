using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientForm
{
    public class Proposal
    {
        public int id;
        string brand;
        public string endpoint;
        int year;
        int price;

        public Proposal(int id, string brand, string endpoint, int year, int price) {
            this.id = id;
            this.brand = brand;
            this.endpoint = endpoint;
            this.year = year;
            this.price = price;
        }

        public override string ToString() {
            return String.Format("Brand: {0}, Price: {1}, Year of Registration: {2}",brand,price,year);
        }

    }
}
