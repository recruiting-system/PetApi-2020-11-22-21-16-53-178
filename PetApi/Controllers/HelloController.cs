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
        public IList<Pet> GetAllPets()
        { 
            return pets;
        }

        [HttpDelete]
        [Route("pets")]
        public void ClearPets()
        {
            pets.Clear();
        }
    }
}
