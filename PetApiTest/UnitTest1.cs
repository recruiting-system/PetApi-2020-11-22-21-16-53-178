using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using PetApi.Model;
using System;
using Xunit;
using PetApi;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using PetApi.Controllers;
using System.Linq;

namespace PetApiTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Add_Pet_When_Add_PetAsync()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SIHSHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var urI = "petStore/pet";
            var response = await client.PostAsync(urI, requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.True(pet.Equals(actualPet));
        }

        [Fact]
        public async Task Should_Return_All_Pets_When_Get_PetsAsync()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SIHSHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/pet", requestBody);

            // when
            var urI = "petStore/pets";
            var response = await client.GetAsync(urI);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);

            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }
    }
}
