using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("petStore")]
    public class PetsController : ControllerBase
    {
        private static IList<Pet> pets = new List<Pet>();

        [HttpPost]
        [Route("pet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet]
        [Route("pets")]
        public ActionResult<IList<Pet>> GetAllPets([FromQuery] string type, [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            var petsCollection = pets;
            if (!string.IsNullOrEmpty(type))
            {
                petsCollection = petsCollection.Where(pet => pet.Type.ToString() == type).ToList();
            }

            if (minPrice != null && maxPrice != null)
            {
                petsCollection = petsCollection.Where(pet => pet.Price >= minPrice && pet.Price <= maxPrice).ToList();
            }

            return Ok(petsCollection);
        }

        [HttpDelete]
        [Route("pets")]
        public void ClearPets()
        {
            pets.Clear();
        }

        [HttpGet("petName/{name}")]
        public ActionResult<Pet> GetPetByName(string name)
        {
            var pet = pets.FirstOrDefault(pet => pet.Name == name);
            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }

        [HttpDelete("petName/{name}")]
        public ActionResult DeletePetByName(string name)
        {
            var pet = pets.FirstOrDefault(pet => pet.Name == name);
            if (pet == null)
            {
                return NotFound();
            }

            pets.Remove(pet);
            return NoContent();
        }

        [HttpPatch]
        [Route("pets")]
        public ActionResult<Pet> UpdatePriceOfPet(PetNamePrice petNamePriceModel)
        {
            var pet = pets.FirstOrDefault(pet => pet.Name == petNamePriceModel.Name);
            if (pet == null)
            {
                return NotFound();
            }

            pet.Price = petNamePriceModel.Price;
            return Ok(pet);
        }
    }
}
