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
    [Route("petStore/[controller]")]
    public class PetsController : ControllerBase
    {
        private IList<Pet> pets = new List<Pet>();
        [HttpPost]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }
    }
}
