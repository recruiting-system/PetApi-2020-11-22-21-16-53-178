using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi.Model
{
    public class PetNamePrice : IEquatable<PetNamePrice> 
    {
        public PetNamePrice(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public PetNamePrice()
        { 
        }

        public string Name { get; set; }
        public double Price { get; set; }

        public bool Equals(PetNamePrice pet)
        {
            if (pet == null || this == null)
            {
                return false;
            }

            if (pet.GetType() != this.GetType())
            {
                return false;
            }

            return Name == pet.Name && Price == pet.Price;
        }
    }
}
