using DataAccessLayer;
using FrontEnd_WIth_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd_WIth_Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Employee emp = await GetEmpById(id);
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee emp)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/employees/{emp.Id}",emp);

            string msg = "";
            if (response.IsSuccessStatusCode)
            {
                msg = "Record Updated !";
                return RedirectToAction("Index", new { m = msg });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Employee emp = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            HttpResponseMessage response = await client.GetAsync($"api/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                emp = JsonConvert.DeserializeObject<Employee>(result);
            }

            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Employee model)
        {
            Employee emp = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            HttpResponseMessage response = await client.DeleteAsync($"api/employees/{model.Id}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                emp = JsonConvert.DeserializeObject<Employee>(result);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? m)
        {
            if (!string.IsNullOrEmpty(m))
            {
                ViewBag.Result = m;
            }

            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            HttpResponseMessage res = await client.GetAsync("api/employees");

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(result);
            }
            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Employee emp = await GetEmpById(id);
            return View(emp);
        }

        private static async Task<Employee> GetEmpById(int id)
        {
            Employee emp = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            HttpResponseMessage response = await client.GetAsync($"api/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                emp = JsonConvert.DeserializeObject<Employee>(result);
            }

            return emp;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };

           // HttpResponseMessage response = await client.PostAsync("api/employees/", new StringContent(JsonConvert.SerializeObject(emp), Encoding.UTF8, "application/json"));
           HttpResponseMessage response = await client.PostAsJsonAsync("api/employees/",emp);

            string msg = "";
            if (response.IsSuccessStatusCode)
            {
                msg = "Record Inserted !";
            }
            return RedirectToAction("Index", new { m = msg });

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
