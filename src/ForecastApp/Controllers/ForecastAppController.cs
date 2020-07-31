using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastApp.Config;
using ForecastApp.ForecastAppModels;
using ForecastApp.MongoDB.Models;
using ForecastApp.MongoDB.MongoConnect;
using ForecastApp.OpenWeatherMapModels;
using ForecastApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ForecastApp.Controllers
{
    public class ForecastAppController : Controller
    {
        private readonly IForecastRepository _forecastRepository;
        private readonly IMongoDBClient _mongoDBClient;

        // Dependency Injection
        public ForecastAppController(IForecastRepository forecastAppRepo, IMongoDBClient mongoDBClient)
        {
            _forecastRepository = forecastAppRepo;
            _mongoDBClient = mongoDBClient;
        }

        // GET: ForecastApp/SearchCity
        public IActionResult SearchCity()
        {
            var viewModel = new SearchCity();
            viewModel.MongoDBClientName = _mongoDBClient.ConnectionString;
            return View(viewModel);
        }

        // POST: ForecastApp/SearchCity
        [HttpPost]
        public IActionResult SearchCity(SearchCity model)
        {
            // If the model is valid, consume the Weather API to bring the data of the city
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "ForecastApp", new { city = model.CityName });
            }
            return View(model);
        }

        // GET: ForecastApp/City
        public async Task<IActionResult> City(string city)
        {
            // Large amount of scope is there to split the code..

            City viewModel = new City();
            bool isCityExists = false;
            WeatherDocument existingDocument = null;
            var dbCollection = _mongoDBClient.DB.GetCollection<WeatherDocument>(Constants.COLLECTION_NAME);          
            var filterDefinition = Builders<WeatherDocument>.Filter.Where(x => x.SearchName == city.ToLower());

            if (dbCollection != null)
            {
                existingDocument = dbCollection.FindSync<WeatherDocument>(filterDefinition).FirstOrDefault();
                if (existingDocument != null)
                {                   
                    isCityExists = true;
                    viewModel = JsonConvert.DeserializeObject<City>(JsonConvert.SerializeObject(existingDocument));
                }
            }

            await Task.Run(async () =>
            {
                // Consume the OpenWeatherAPI in order to bring Forecast data in our page.
                WeatherResponse weatherResponse = _forecastRepository.GetForecast(city);


                if (weatherResponse != null)
                {
                    if (string.IsNullOrEmpty(weatherResponse.ErrorMessage))
                    {
                        viewModel.WeatherImage = string.Format(ForecastApp.Config.Constants.IMAGE_SOURCE_URL, weatherResponse.Weather[0].Icon);
                        viewModel.Name = weatherResponse.Name;
                        viewModel.Humidity = weatherResponse.Main.Humidity;
                        viewModel.Pressure = weatherResponse.Main.Pressure;
                        viewModel.Temp = weatherResponse.Main.Temp;
                        viewModel.Weather = weatherResponse.Weather[0].Main;
                        viewModel.Wind = weatherResponse.Wind.Speed;
                        viewModel.Country = weatherResponse.Sys.Country;
                        

                        var weatherDocument = JsonConvert.DeserializeObject<WeatherDocument>(JsonConvert.SerializeObject(viewModel));
                        // This is to avoid duplicate document saved because of case Letters
                        weatherDocument.SearchName = viewModel.Name.ToLower();
                        if (isCityExists)
                        {
                            weatherDocument.Id = existingDocument.Id;
                            await dbCollection.FindOneAndReplaceAsync(filterDefinition, weatherDocument);
                        }
                        else
                        {
                            await dbCollection.InsertOneAsync(weatherDocument);
                        }
                    }
                    else
                    {
                        viewModel.ErrorMessage = weatherResponse.ErrorMessage;
                    }
                }
            });

            return View(viewModel);
        }
    }
}