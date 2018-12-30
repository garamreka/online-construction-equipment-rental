using System;
using System.Linq;
using NUnit.Framework;
using OCER.Enums;
using OCER.Models;
using OCER.Repositories;

namespace OCER.UnitTest.TestFixtures
{
    /// <summary>
    /// Test class for EquipmentRepository
    /// </summary>
    [TestFixture]
    public class EquipmentRepositoryTest
    {
        #region Fields

        private EquipmentRepository _equipmentRepository;

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
        public EquipmentRepositoryTest()
        {
            _equipmentRepository = new EquipmentRepository();
        }

        #endregion

        #region Tests

        #region GetAllEquipmentFromFile

        /// <summary>
        /// Tests the GetAllEquipmentFromFile method with valid input values
        /// </summary>
        [Test]
        [Explicit ("Access to Inventory file is not handled")]
        public void GetAllEquipmentFromFile_ValidValues_ReturnWithList()
        {
            var equipmentList = _equipmentRepository.GetAllEquipmentFromFile();
            Assert.IsTrue(equipmentList.Any());
        }

        /// <summary>
        /// Tests the GetAllEquipmentFromFile method with invalid id
        /// </summary>
        [Test]
        [Explicit ("TestInventory file is not created yet")]
        public void GetAllEquipmentFromFile_InvalidId_ThrowException()
        {
            Assert.Throws<Exception>(() => _equipmentRepository.GetAllEquipmentFromFile());
        }

        /// <summary>
        /// Tests the GetAllEquipmentFromFile method with invalid type
        /// </summary>
        [Test]
        [Explicit("TestInventory file is not created yet")]
        public void GetAllEquipmentFromFile_InvalidType_ThrowException()
        {
            Assert.Throws<Exception>(() => _equipmentRepository.GetAllEquipmentFromFile());
        }

        #endregion

        #region AddNewRentedEquipment

        /// <summary>
        /// Tests the AddNewRentedEquipment method with valid Equipment
        /// </summary>
        [Test]
        public void AddNewRentedEquipment_ValidEquipment()
        {
            var beforeCount = _equipmentRepository.Equipments.Count;

            _equipmentRepository.AddNewRentedEquipment(_testEquipment);

            Assert.AreEqual(beforeCount + 1, _equipmentRepository.Equipments.Count);
        }

        /// <summary>
        /// Test the AddNewRentedEquipment method if the Equipment is null
        /// </summary>
        [Test]
        public void AddNewRentedEquipment_NullEquipment()
        {
            Assert.Throws<NullReferenceException>(() => _equipmentRepository.AddNewRentedEquipment(null));
        }

        #endregion

        #region GetEquipmentById

        /// <summary>
        /// Tests the GetEquipmentById method with valid Id
        /// </summary>
        [Test]
        [Explicit("Access to Inventory file is not handled")]
        public void GetEquipmentById_ValidId_ReturnsEquipment()
        {
            var expectedResult = _testEquipment;
            var result = _equipmentRepository.GetEquipmentById(1);

            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// Tests the GetEquipmentById method with invalid Id
        /// </summary>
        [Test]
        public void GetEquipmentById_InvalidId_ThrowException()
        {
            Assert.Throws<Exception>(() => _equipmentRepository.GetEquipmentById(0));
        }

        /// <summary>
        /// Tests the GetEquipmentById method if the Id is not found in the inventory
        /// </summary>
        [Test]
        public void GetEquipmentById_NotFoundId_ThrowException()
        {
            Assert.Throws<Exception>(() => _equipmentRepository.GetEquipmentById(10));
        }

        #endregion

        #region ChangeEquipmentDefaultValues

        /// <summary>
        /// Tests the ChangeEquipmentDefaultValues method with valid input value
        /// </summary>
        [Test]
        public void ChangeEquipmentDefaultValues_ValidEquipment_ReturnsEquipment() //todo
        {
            var inputEquipment = _testEquipment;
            inputEquipment.RentDays = 4;

            var expectedResult = new Equipment()
            {
                Id = 1,
                Name = "Caterpillar bulldozer",
                EquipmentType = EquipmentType.Heavy,
                RentDays = 4,
                Price = 340,
                LoyaltyPoint = 2
            };

            var result = _equipmentRepository.ChangeEquipmentDefaultValues(inputEquipment);

            Assert.AreEqual(expectedResult.Price, result.Price);
            Assert.AreEqual(expectedResult.LoyaltyPoint, result.LoyaltyPoint);
        }

        /// <summary>
        /// Tests the ChangeEquipmentDefaultValues with null input value
        /// </summary>
        [Test]
        public void ChangeEquipmentDefaultValues_NullEquipment_ThrowException()
        {
            Assert.Throws<NullReferenceException>(() => _equipmentRepository.ChangeEquipmentDefaultValues(null));
        }

        #endregion

        #region CalculatePrice

        /// <summary>
        /// Tests the CalculatePrice method with valid input values
        /// </summary>
        [Test]
        public void CalculatePrice_ValidInputs_ReturnsPrice()
        {
            var result = _equipmentRepository.CalculatePrice(EquipmentType.Heavy, 4);
            Assert.AreEqual(340, result);
        }

        /// <summary>
        /// Tests the CalculatePrice method with invalid RentDays input
        /// </summary>
        [Test]
        public void CalculatePrice_ZeroRentDays_ThrowException()
        {
            Assert.Throws<Exception>(() => _equipmentRepository.CalculatePrice(EquipmentType.Heavy, 0));
        }

        #endregion

        #region GetLoyaltyPoint

        /// <summary>
        /// Tests the GetLoyaltyPoint method with valid type
        /// </summary>
        [Test]
        public void GetLoyaltyPoint_ValidType_ReturnPoint()
        {
            var result = _equipmentRepository.GetLoyaltyPoint(EquipmentType.Heavy);
            Assert.AreEqual(2, result);

            result = _equipmentRepository.GetLoyaltyPoint(EquipmentType.Regular);
            Assert.AreEqual(1, result);
        }

        #endregion

        #endregion
    }
}
