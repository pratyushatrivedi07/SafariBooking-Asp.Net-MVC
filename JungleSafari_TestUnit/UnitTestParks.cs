using Jungle.Entities;
using Jungle.Repos;
using JungleSafari.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace JungleSafari_TestUnit
{
    public class UnitTestParks
    {
        ParksController controller = null;

        public UnitTestParks()
        {
            //AAA
            var options = new DbContextOptionsBuilder<MydbContext>()
               .UseSqlServer("Server=DESKTOP-03SVV1S\\SQLEXPRESS;Database=Mydb;Trusted_Connection=True;").Options;
            MydbContext context = new MydbContext(options);
            IParkRepository repository = new ParkRepository(context);
            controller = new ParksController(repository);

        }
        [Fact]
        public void GetAllParks_ReturnsList()
        {
            //AAA
            //Arrange

            //Act:
            var result = controller.Get() as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);  //status code checked
            Assert.Equal(8, (result.Value as List<Parks>).Count); //count
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(103)]
        [InlineData(104)]
        [InlineData(105)]
        [InlineData(106)]
        [InlineData(107)]
        [InlineData(109)]
        public void Get_ParkId_ReturnParkIfFound(int parkId)
        {
            var result = controller.Get(parkId) as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            Assert.Equal(parkId, (result.Value as Parks).ParkId);
        }

        [Fact]
        public void Post_Park_Return201Status()
        {
            //Arrange
            Parks park = new Parks
            {
                Name = "Tadoba",
                Location = "Madhya Pradesh",
                Fee = 600,
                
            };

            var result = controller.Post(park) as CreatedResult;

            Assert.Equal(201, result.StatusCode.Value);
            Assert.Equal(park, result.Value as Parks);

        }
        
  

        [Fact]
        public void Post_Park_NegativeFee_ReturnBadRequestStatus()
        {
            //Arrange
            Parks park = new Parks
            {
                Name = "Pench National Park",
                Location = "Madhya Pradesh",
                Fee = -800,

            };

            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Post(park) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Theory]
        [InlineData(109)]
        public void Delete_inputPark_ReturnParkDeletedStatus(int parkid)
        {
            //Act
            var result = controller.Delete(parkid) as OkObjectResult;
            Assert.Equal(200, result.StatusCode.Value);
        }

        [Fact]
        public void Delete_IfParkNotFound_ReturnNotFound()
        {
            //Arrange
            int parkId = 2;

            //Act
            var result = controller.Delete(parkId) as NotFoundResult;

            //Assert
            Assert.Equal(404, result.StatusCode);
            //Assert.True();
        }

        [Fact]
        public void Update_Park_ReturnCreatedStatus()
        {
            //Arrange
            Parks park = new Parks
            {
                ParkId = 105,
                Name = "Pobitora",
                Location = "Assam",
                Fee = 800,

            };
            //Act
            var result = controller.Put(105, park) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);

        }

        [Fact]
        public void Update_ParkIdDoesNotMatch_ReturnBadRequest()
        {
            //Arrange
            Parks park = new Parks
            {
                ParkId = 2044,
                Name = "Pench National Park",
                Location = "Solapur",
                Fee = 800,

            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");
            var result = controller.Put(2044, park) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, result.StatusCode.Value);
        }

        [Fact]
        public void Update_ParkNegativeFee_ReturnBadRequest()
        {
            //Arrange
            Parks park = new Parks
            {
                ParkId = 111,
                Name = "Pench National Park",
                Location = "Solapur",
                Fee = -800,

            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");
            var result = controller.Put(111, park) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, result.StatusCode.Value);
        }

        [Fact]
        public void SearchParkId_OkIfFound()
        {
            //Arrange
            int parkId = 102;

            //Act
            var result = controller.GetSearch($"search/{parkId}") as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            //Assert.True();
        }

        [Fact]
        public void SearchParkName_OkIfFound()
        {
            //Arrange
            string name = "Bhimgad";

            //Act
            var result = controller.GetSearch($"search/{name}") as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            //Assert.True();
        }


        //[Fact]
        //public void Post_DuplicatePark_ReturnConflict409Status()
        //{
        //    //Arrange
        //    Parks park = new Parks
        //    {
        //        Name = "Tadoba",
        //        Location = "Chandrapur",
        //        Fee = 800

        //    };

        //    var result = controller.Post(park) as ConflictObjectResult;
        //    Assert.Equal(409, result.StatusCode.Value);
        //}

    }
}
