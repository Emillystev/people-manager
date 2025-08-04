using AutoFixture;
using CsvHelper;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;
        public CountriesServiceTest()
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };

            DbContextMock<PersonsDbContext> dbContextMock = new DbContextMock<PersonsDbContext>(
              new DbContextOptionsBuilder<PersonsDbContext>().Options
             );

            PersonsDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            _countriesService = new CountriesService(null);
        }

        #region AddCountry
        [Fact] 
        public async Task AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;
            Func<Task> action = (async() =>
            {
                await _countriesService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact] 
        public async Task AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>().With(i => i.CountryName, null as string).Create();
            Func<Task> action = (async() =>
            {
                await _countriesService.AddCountry(request);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public async Task GetCountryByCountryID__NullCountryID()
        {
            Guid? countryID = null;
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);
            country_response_from_get_method.Should().BeNull();
        }        
        #endregion
    }
}
