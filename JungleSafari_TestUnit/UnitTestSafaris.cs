using Jungle.Entities;
using Jungle.Repos;
using JungleSafari.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JungleSafari_TestUnit
{
   public class UnitTestSafaris
    {
        SafariController controller = null;

        public UnitTestSafaris()
        {
            //AAA
            var options = new DbContextOptionsBuilder<MydbContext>()
               .UseSqlServer("Server=DESKTOP-03SVV1S\\SQLEXPRESS;Database=Mydb;Trusted_Connection=True;").Options;
            MydbContext context = new MydbContext(options);
            ISafariDetailRepos repository = new SafariRepository(context);
            controller = new SafariController(repository);

        }

        [Fact]
        public void Post_Safari_Return201Status()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariName = "Tiger Safari",
                SafariDate =new DateTime(2021-08-12),
                SafariTime= "Morning",
                ParkId=102,
                SafariCost = 200,


            };

            var result = controller.Post(safari) as CreatedResult;

            Assert.Equal(201, result.StatusCode.Value);
            Assert.Equal(safari, result.Value as SafariDetail);

        }

        [Fact]
        public void GetAllSafari_ReturnsList()
        {
            //AAA
            //Arrange

            //Act:
            var result = controller.Get() as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);  //status code checked
            Assert.Equal(2, (result.Value as List<SafariDetail>).Count); //count
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        
        public void Get_SafariId_ReturnSafariIfFound(int SafariId)
        {
            var result = controller.Get(SafariId) as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            Assert.Equal(SafariId, (result.Value as SafariDetail).SafariId);
        }

        [Fact]
        public void Post_Safari_NegativeCost_ReturnBadRequestStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariName = "deer Safari",
                SafariDate = new DateTime(2021 - 08 - 12),
                SafariTime = "Morning",
                ParkId = 105,
                SafariCost = -200,


            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Post(safari) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }
        
        [Fact]
        public void Post_Safari_ParkIdNotFound_ReturnBadRequestStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariName = "Monkey Safari",
                SafariDate = new DateTime(2021 - 08 - 12),
                SafariTime = "Morning",
                ParkId = 8005,
                SafariCost = 200,
            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Post(safari) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Post_Safari_DateInPast_ReturnBadRequestStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariName = "Monkey Safari",
                SafariDate = new DateTime(1999 - 08 - 12),
                SafariTime = "Morning",
                ParkId = 8005,
                SafariCost = 200,
            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Post(safari) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Theory]
        [InlineData(2002)]
        public void Delete_inputSafariId_ReturnSafariDeletedStatus(int safariid)
        {
            //Act
            var result = controller.Delete(safariid) as OkObjectResult;
            Assert.Equal(200, result.StatusCode.Value);
        }

        [Fact]
        public void Update_Safari_ReturnCreatedStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariId=2000,
                SafariName = "Tiger Safari",
                SafariDate = new DateTime(2021 - 08 - 12),
                SafariTime = "Evening",
                ParkId = 105,
                SafariCost = 200,


            };
            //Act
            var result = controller.Put(2000, safari) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);

        }

        [Fact]
        public void Update_Safari_NegativeCost_ReturnBadRequestStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariId = 2000,
                SafariName = "deer Safari",
                SafariDate = new DateTime(2021 - 08 - 12),
                SafariTime = "Morning",
                ParkId = 105,
                SafariCost = -200,
            };


            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Put(2000,safari) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_Safari_DateinPast_ReturnBadRequestStatus()
        {
            //Arrange
            SafariDetail safari = new SafariDetail
            {
                SafariId = 2000,
                SafariName = "deer Safari",
                SafariDate = new DateTime(1999 - 08 - 12),
                SafariTime = "Morning",
                ParkId = 105,
                SafariCost = 200,
            };


            //Act

            controller.ModelState.AddModelError("Name", "Required");
            var result = controller.Put(2000,safari) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode.Value);
        }



        [Fact]
        public void SearchSafaribyParkId_OkIfFound()
        {
            //Arrange
            int parkiId = 101;

            //Act
            var result = controller.GetByPark(parkiId) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            //Assert.True();
        }


    }
}
