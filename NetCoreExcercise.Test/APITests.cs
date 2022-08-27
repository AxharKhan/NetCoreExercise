using Microsoft.AspNetCore.Mvc.Testing;
using NetCoreExercise.Models;
using System.Text.Json;

namespace NetCoreExercise.Test
{
    [TestClass]
    public class APITests
    {
        private HttpClient _httpClient;
        private int testId = 0;

        public APITests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestInitialize]
        public async Task Initialize()
        {
            var customer = new Dictionary<string, string>()
            {
                {"Name" , "Test Name" },
                {"Address" , "Test Address " },
                {"State" , "TS" },
                {"City" , "Test" },
                {"Zip" , "0000" },
                {"CustomerTypeId","1" }
                };

            var response = await _httpClient.PostAsync("/api/Customers/AddCustomer", new FormUrlEncodedContent(customer));

            var responseString = await response.Content.ReadAsStringAsync();

            CreatedResponse customerObject = JsonSerializer.Deserialize<CreatedResponse>(responseString);

            testId = customerObject.id;
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            var deleteResponse = await _httpClient.DeleteAsync("/api/Customers/DeleteCustomer/" + testId);
        }

        [TestMethod]
        public async Task Index_Correct()
        {
            var response = await _httpClient.GetAsync("/api/Customers/Index");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task AddCustomer_Incorrect()
        {
            var customer = new Dictionary<string, string>()
            {
                {"Name" , "Test Name" },
                {"Address" , "Test Address that is more than 50 words and should throw an error." },
                {"State" , "More than 2 characters" },
                {"City" , "Test City that is also more than 50 words long and should throw an error." },
                {"Zip" , "adas300000000000000000" }
                };

            var response = await _httpClient.PostAsync("/api/Customers/AddCustomer", new FormUrlEncodedContent(customer));

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task AddCustomer_Correct()
        {
            var customer = new Dictionary<string, string>()
            {
                {"Name" , "Test Name" },
                {"Address" , "Test Address " },
                {"State" , "TS" },
                {"City" , "Test" },
                {"Zip" , "0000" },
                {"CustomerTypeId","1" }
                };

            var response = await _httpClient.PostAsync("/api/Customers/AddCustomer", new FormUrlEncodedContent(customer));

            var responseString = await response.Content.ReadAsStringAsync();

            CreatedResponse customerObject = JsonSerializer.Deserialize<CreatedResponse>(responseString);

            var newTestId = customerObject.id;

            var deleteResponse = await _httpClient.DeleteAsync("/api/Customers/DeleteCustomer/" + newTestId);

            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_Correct()
        {
            var customer = new Dictionary<string, string>()
            {
                {"Id" , testId.ToString() },
                {"Name" , "Test Name2" },
                {"Address" , "Test Address " },
                {"State" , "TS" },
                {"City" , "Test" },
                {"Zip" , "0000" },
                {"CustomerTypeId","1" }
                };

            var response = await _httpClient.PutAsync("/api/Customers/UpdateCustomer/" + testId, new FormUrlEncodedContent(customer));

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomer_Correct()
        {
            var response = await _httpClient.GetAsync("/api/Customers/GetCustomer/" + testId);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCustomer_Correct()
        {
            var response = await _httpClient.DeleteAsync("/api/Customers/DeleteCustomer/" + testId);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }

    public class CreatedResponse
    {
        public int id { get; set; }
        public Customer Customer { get; set; }
    }
}