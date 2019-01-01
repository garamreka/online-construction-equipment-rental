using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OCER.Controllers;
using NUnit.Framework;
using OCER.Enums;
using OCER.Interfaces;
using OCER.Models;

namespace OCER.UnitTest.TestFixtures
{
    /// <summary>
    /// Test class for HomeController
    /// </summary>
    [TestFixture]
    public class HomeControllerTest
    {
        #region Fileds

        private Mock<IEquipmentRepository> _mockEquipmentRepository;
        private HomeController _homeController;
        private Equipment _testEquipment = new Equipment()
        {
            Id = 1,
            Name = "Caterpillar bulldozer",
            EquipmentType = EquipmentType.Heavy,
            RentDays = 0,
            Price = 0,
            LoyaltyPoint = 0,
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public HomeControllerTest()
        {
            _mockEquipmentRepository = new Mock<IEquipmentRepository>();
            _homeController = new HomeController(_mockEquipmentRepository.Object);
        }

        #endregion

        #region Tests

        /// <summary>
        /// Tests the Index method of the HomeController
        /// </summary>
        [Test]
        public void Index_ReturnsView()
        {
            _mockEquipmentRepository
                .Setup(repo => repo.GetAllEquipment())
                .Returns(new List<Equipment>(){_testEquipment});

            var result = _homeController.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        /// <summary>
        /// Tests the AddRentalDetails method if the input is invalid
        /// </summary>
        [Test]
        public void AddRentDetails_ValidInput_ReturnsViewWithSelectedEquipment()
        {
            _mockEquipmentRepository
                .Setup(repo => repo.GetEquipmentById(It.IsAny<int>()))
                .Returns(_testEquipment);

            var result = _homeController.AddRentDetails(1) as ViewResult;
            Assert.IsNotNull(result);

            var equipment = result.Model as Equipment;
            Assert.AreEqual(_testEquipment, equipment);
        }

        /// <summary>
        /// Tests the AddRentalDetails method if the input is invalid
        /// </summary>
        [Test]
        public void AddRentDetails_InValidInput_RedirectToIndex()
        {
            var result = _homeController.AddRentDetails(0) as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ActionName);
        }

        /// <summary>
        /// Tests the AddToCart method if the model state is valid
        /// </summary>
        [Test]
        public void AddToCart_ModelStateIsValid_RedirectToConfirm()
        {
            _homeController.ModelState.Clear();
            var result = _homeController.AddToCart(_testEquipment) as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Confirm", result.ActionName);
        }

        /// <summary>
        /// Tests the AddToCart method if the model state is invalid
        /// </summary>
        [Test]
        public void AddToCart_ModelStateInValid_RedirectToIndex()
        {
            _homeController.ModelState.AddModelError("test", "test");
            var result = _homeController.AddToCart(new Equipment()) as RedirectToActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ActionName);
        }

        /// <summary>
        /// Tests the Confirm method of the HomeController
        /// </summary>
        [Test]
        public void Confirm_ReturnsConfirmView()
        {
            var result = _homeController.Confirm() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Confirm", result.ViewName);
        }

        /// <summary>
        /// Tests the Final method of the HomeController
        /// </summary>
        [Test]
        public void Final_ReturnsView()
        {
            _mockEquipmentRepository.Setup(repo => repo.FinalizeInvoice()).Verifiable();

            var result = _homeController.Final() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Final", result.ViewName);
        }

        /// <summary>
        /// Tests the Confirm method of the HomeController
        /// </summary>
        [Test]
        public void Error_ReturnsErrorView()
        {
            _mockEquipmentRepository.Setup(repo => repo.SetDefaultValues()).Verifiable();

            var result = _homeController.Error() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }

        #endregion

    }
}
