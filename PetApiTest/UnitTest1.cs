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
        public async Task Should_Return_Conflict_Given_Name_Already_Existed_When_Add_Pet()
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
            var response = await client.PostAsync("petStore/pet", requestBody);

            // then
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
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

        [Fact]
        public async Task Should_Return_All_Pets_With_Matching_Type_When_Get_Pet_By_Type()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");

            var pet1 = new Pet("SHISHI", Animal.Dog, "Blue", 12);
            var pet2 = new Pet("Tony", Animal.Cat, "RED", 12);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);

            var requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            var requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/pet", requestBody1);
            await client.PostAsync("petStore/pet", requestBody2);

            // when
            var response = await client.GetAsync("petStore/pets?type=Dog");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet1 }, actualPets);
        }

        [Fact]
        public async Task Should_Return_All_Pets_With_Matching_PriceRange_When_Get_Pet_By_PriceRange()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");

            var pet1 = new Pet("SHISHI", Animal.Dog, "Blue", 5);
            var pet2 = new Pet("Tony", Animal.Cat, "RED", 12);
            var pet3 = new Pet("Tom", Animal.Cat, "RED", 13);
            var pet4 = new Pet("Jerry", Animal.Cat, "RED", 14);

            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            string request3 = JsonConvert.SerializeObject(pet3);
            string request4 = JsonConvert.SerializeObject(pet4);

            var requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            var requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            var requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            var requestBody4 = new StringContent(request4, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/pet", requestBody1);
            await client.PostAsync("petStore/pet", requestBody2);
            await client.PostAsync("petStore/pet", requestBody3);
            await client.PostAsync("petStore/pet", requestBody4);

            // when
            var response = await client.GetAsync("petStore/pets?maxPrice=13&minPrice=12");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet2, pet3 }, actualPets);
        }

        [Fact]
        public async Task Should_Return_All_Pets_With_Matching_Color_When_Get_Pet_By_Color()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/pets");

            var pet1 = new Pet("SHISHI", Animal.Dog, "Blue", 12);
            var pet2 = new Pet("Tony", Animal.Cat, "RED", 12);
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);

            var requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            var requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/pet", requestBody1);
            await client.PostAsync("petStore/pet", requestBody2);

            // when
            var response = await client.GetAsync("petStore/pets?color=Blue");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet1 }, actualPets);
        }
    }
}
