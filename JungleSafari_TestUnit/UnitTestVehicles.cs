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
    public class UnitTestVehicle
    {
        VehicleController controller = null;

        public UnitTestVehicle()
        {
            //AAA
            var options = new DbContextOptionsBuilder<MydbContext>()
               .UseSqlServer("Server=DESKTOP-03SVV1S\\SQLEXPRESS;Database=Mydb;Trusted_Connection=True;").Options;
            MydbContext context = new MydbContext(options);
            IVehicleRepository repository = new VehicleRepository(context);
            controller = new VehicleController(repository);

        }
        [Fact]
        public void GetAllVehicle_ReturnsList()
        {
            //AAA
            //Arrange

            //Act:
            var result = controller.Get() as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);  //status code checked
            Assert.Equal(2, (result.Value as List<Vehicle>).Count); //count

        }

        [Theory]
        [InlineData(300)]
        [InlineData(301)]
       

        public void Get_InputVehicleId_ReturnVehicleIfFound(int Vid)
        {
            var result = controller.Get(Vid) as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            // Assert.Single(result.Value as IEnumerable<Vehicle>);
            Assert.Equal(Vid, (result.Value as Vehicle).Vid);
        }

        [Fact]
        public void Post_Vehicle_Return201Status()
        {
            //Arrange
            Vehicle vehicle = new Vehicle
            {
                Name = "Thar",
                Vtype = "Park",
                EntryCost = 100,
                Capacity = "Seat6",
                ParkId = 102

            };

            var result = controller.Post(vehicle) as CreatedResult;

            Assert.Equal(201, result.StatusCode.Value);
            // Assert.Single(result.Value as IEnumerable<Vehicle>);
            Assert.Equal(vehicle, result.Value as Vehicle);

        }
       

        [Fact]
        public void Post_Vehicle_NegativeEntryCost_ReturnBadRequestStatus()
        {
            //Arrange
            Vehicle vehicle = new Vehicle
            {
                Name = "Mahindra",
                Vtype = "Park",
                EntryCost = -100,
                Capacity = "Seat6",
                ParkId = 103
            };
            //Act
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Post(vehicle) as BadRequestObjectResult;

            Assert.Equal(400, result.StatusCode);
            // Assert.Single(result.Value as IEnumerable<Employees>);


        }

        [Theory]
        [InlineData(301)]
        public void Delete_inputVehicleId_ReturnVehicleDeletedStatus(int Vid)
        {
            //Act
            var result = controller.Delete(Vid) as OkObjectResult;
            Assert.Equal(200, result.StatusCode.Value);
        }

        //[Fact]
        // public void Delete_IfVehicleNotFound_ReturnNotFound()
        // {
        //    //Arrange
        //   int Vid = 0;

        //    //Act
        //    var result = controller.Delete(Vid) as NotFoundObjectResult;

        //    //Assert
        //    Assert.Equal(404, result.StatusCode.Value);
        //    //Assert.True();
        //}

        [Fact]
        public void Update_Vehicle_ReturnCreatedStatus()
        {
            //Arrange
            Vehicle vehicle = new Vehicle
            {
                Vid = 300,
                Name = "Mahindra",
                Vtype = "Park",
                EntryCost = 300,
                Capacity = "Seat6",
                ParkId = 103
            };
            //Act
            var result = controller.Put(300, vehicle) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);

        }

        [Fact]
        public void Update_VehicleIdDoesNotMatch_ReturnBadRequest()
        {
            //Arrange
            Vehicle vehicle = new Vehicle
            {
                Vid = 801,
                Name = "Mahindra",
                Vtype = "Park",
                EntryCost = 300,
                Capacity = "Seat6",
                ParkId = 103

            };
            //Act
            var result = controller.Put(204, vehicle) as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, result.StatusCode.Value);
        }

        [Fact]
        public void GetByPark_OkIfFound()
        {
            //Arrange
            int parkId = 102;

            //Act
            var result = controller.GetByPark(parkId) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            //Assert.True();
        }


        //[Fact]
        //public void Post_DuplicateVehicle_ReturnConflict409Status()
        //{
        //    //Arrange
        //    Vehicle vehicle = new Vehicle
        //    {
        //        Name = "Mahindra",
        //        Vtype = "Park",
        //        EntryCost = 300,
        //        Capacity = "Seat6",
        //        ParkId = 103
        //    };

        //    var result = controller.Post(vehicle) as ConflictObjectResult;

        //    Assert.Equal(409, result.StatusCode.Value);
        //    // Assert.Single(result.Value as IEnumerable<Vehicle>);
        //}
    }
}
