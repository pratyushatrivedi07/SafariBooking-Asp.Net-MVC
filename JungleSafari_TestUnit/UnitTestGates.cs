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
    public class UnitTestGate
    {
        GateController controller = null;

        public UnitTestGate()
        {
            //AAA
            var options = new DbContextOptionsBuilder<MydbContext>()
               .UseSqlServer("Server=DESKTOP-03SVV1S\\SQLEXPRESS;Database=Mydb;Trusted_Connection=True;").Options;
            MydbContext context = new MydbContext(options);
            IGateRepository repository = new GateRepository(context);
            controller = new GateController(repository);

        }
        [Fact]
        public void GetAllGate_ReturnsList()
        {
            //AAA
            //Arrange

            //Act:
            var result = controller.Get() as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);  //status code checked
            Assert.Equal(2, (result.Value as List<Gate>).Count); //count

        }
        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        
        public void Get_InputGateId_ReturnGateIfFound(int GateId)
        {
            var result = controller.Get(GateId) as OkObjectResult;
            //Assert
            Assert.Equal(200, result.StatusCode.Value);
            // Assert.Single(result.Value as IEnumerable<Gate>);
            Assert.Equal(GateId, (result.Value as Gate).GateId);
        }
        [Fact]
        public void Post_Gate__Return201Status()
        {
            //Arrange
            Gate gate = new Gate
            {
                Name = "North",

                ParkId = 102

            };

            var result = controller.Post(gate) as CreatedResult;

            Assert.Equal(201, result.StatusCode.Value);
            // Assert.Single(result.Value as IEnumerable<Gate>);
            Assert.Equal(gate, result.Value as Gate);

        }
        //[Fact]
        //public void Post_DuplicateGate_ReturnConflict409Status()
        //{
        //    //Arrange
        //    Gate gate = new Gate
        //    {
        //        Name = "Bhopal",

        //        ParkId = 101
        //    };

        //    var result = controller.Post(gate) as ConflictObjectResult;

        //    Assert.Equal(409, result.StatusCode.Value);
        //    // Assert.Single(result.Value as IEnumerable<Gate>);
        //}
        [Theory]
        [InlineData(201)]
        public void Delete_inputGateId_ReturnGateDeletedStatus(int GateId)
        {
            //Act
            var result = controller.Delete(GateId) as OkObjectResult;
            Assert.Equal(200, result.StatusCode.Value);
        }
        [Fact]
        public void Update_Gate_ReturnCreatedStatus()
        {
            //Arrange
            Gate gate = new Gate
            {
                GateId = 200,
                Name = "Satana",

                ParkId = 101
            };
            //Act
            var result = controller.Put(200, gate) as OkObjectResult;

            //Assert
            Assert.Equal(200, result.StatusCode.Value);

        }
        [Fact]
        public void Update_GateIdDoesNotMatch_ReturnBadRequest()
        {
            //Arrange
            Gate gate = new Gate
            {
                GateId = 2,
                Name = "Satana",

                ParkId = 101

            };
            //Act
            var result = controller.Put(204, gate) as BadRequestObjectResult;

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


    }
}