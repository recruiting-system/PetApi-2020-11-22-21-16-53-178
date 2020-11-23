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
using System.Net;
namespace PetApiTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet()
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
        public async Task Should_Return_All_Pets_When_Get_AllPets()
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

        [Fact]
        public async Task Should_Return_The_Pet_With_Given_Name_When_Get_Pet_By_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet1 = new Pet("SHISHI", Animal.Dog, "RED", 12);
            var pet2 = new Pet("Tony", Animal.Dog, "RED", 12);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);

            var requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            var requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/pet", requestBody1);
            await client.PostAsync("petStore/pet", requestBody2);

            // when
            var urI = "petStore/petName/SHISHI";
            var response = await client.GetAsync(urI);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);

            Assert.Equal(pet1, actualPet);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Pet_Not_Existed_When_Get_Pet_By_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");

            // when
            var urI = "petStore/petName/SHISHI";
            var response = await client.GetAsync(urI);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Pet_Not_Existed_When_Delete_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SHISHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/pet", requestBody);

            // when
            var urI = "petStore/petName/Tony";
            var response = await client.DeleteAsync(urI);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_No_Content_Given_Pet_Existed_When_Successfully_Delete_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SHISHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/pet", requestBody);

            // when
            var urI = "petStore/petName/SHISHI";
            var deleteResponse = await client.DeleteAsync(urI);

            // then
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await client.GetAsync("petStore/pets");
            var responseString = await getResponse.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>(), actualPet);
        }

        [Fact]
        public async Task Should_Return_Not_Found_Given_Pet_Not_Existed_When_Update_Pet_Price()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SHISHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/pet", requestBody);

            // when
            var updatedPetNamePrice = new PetNamePrice("Tony", 13);
            string requestForPatch = JsonConvert.SerializeObject(updatedPetNamePrice);
            var requestBodyForPatch = new StringContent(requestForPatch, Encoding.UTF8, "application/json");

            var urI = "petStore/pets";
            var patchResponse = await client.PatchAsync(urI, requestBodyForPatch);

            // then
            Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
        }

        [Fact]
        public async Task Should_Return_UpdatedPet_Given_Pet_Existed_When_Successfully_Updated_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");
            var pet = new Pet("SHISHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/pet", requestBody);

            // when
            var updatedPetNamePrice = new PetNamePrice("SHISHI", 13);
            string requestForPatch = JsonConvert.SerializeObject(updatedPetNamePrice);
            var requestBodyForPatch = new StringContent(requestForPatch, Encoding.UTF8, "application/json");

            var urI = "petStore/pets";
            var patchResponse = await client.PatchAsync(urI, requestBodyForPatch);

            // then
            patchResponse.EnsureSuccessStatusCode();

            var getResponse = await client.GetAsync($"petStore/petName/{updatedPetNamePrice.Name}");
            var responseString = await getResponse.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(updatedPetNamePrice.Price, actualPet.Price);
        }
    }
}
