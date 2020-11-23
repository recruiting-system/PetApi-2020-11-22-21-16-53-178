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
            var pet = new Pet("SIHSHI", Animal.Dog, "RED", 12);
            string request = JsonConvert.SerializeObject(pet);
            var requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var urI = "petStore/Pets";
            var response = await client.PostAsync("petStore/pets", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.True(pet.Equals(actualPet));
        }
    }
}
