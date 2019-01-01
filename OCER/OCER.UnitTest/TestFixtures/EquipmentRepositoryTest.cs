using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using OCER.Enums;
using OCER.Interfaces;
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

        private readonly Mock<IFileIoOperations> _mockFileIoOperations;
        private readonly EquipmentRepository _equipmentRepository;

        private readonly Equipment _testEquipment = new Equipment()
        {
            Id = 1,
            Name = "Caterpillar bulldozer",
            EquipmentType = EquipmentType.Heavy,
            RentDays = 0,
            Price = 0,
            LoyaltyPoint = 0,
        };

        private readonly string[] _testFile  = { "1;Caterpillar bulldozer;Heavy" };

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EquipmentRepositoryTest()
        {
            _mockFileIoOperations = new Mock<IFileIoOperations>();
            _equipmentRepository = new EquipmentRepository(_mockFileIoOperations.Object);
        }

        #endregion

        #region Tests

        #region GetAllEquipment

        /// <summary>
        /// Tests the GetAllEquipment method with valid input values
        /// </summary>
        [Test]
        public void GetAllEquipment_ValidValues_ReturnWithList()
        {
            _mockFileIoOperations.Setup(repo => repo.ReadFile()).Returns(_testFile);

            var equipmentList = _equipmentRepository.GetAllEquipment();
            Assert.IsTrue(equipmentList.Any());
        }

        /// <summary>
        /// Tests the GetAllEquipment method with invalid id
        /// </summary>
        [Test]
        [Explicit]
        public void GetAllEquipment_InvalidId_ThrowException()
        {
            string[] testFile = { "Caterpillar bulldozer;Heavy" };
            _mockFileIoOperations.Setup(repo => repo.ReadFile()).Returns(testFile);

            Assert.Throws<Exception>(() => _equipmentRepository.GetAllEquipment());
        }

        /// <summary>
        /// Tests the GetAllEquipment method with invalid type
        /// </summary>
        [Test]
        [Explicit]
        public void GetAllEquipment_InvalidType_ThrowException()
        {
            string[] testFile = { "1;Caterpillar bulldozer;Hea" };
            _mockFileIoOperations.Setup(repo => repo.ReadFile()).Returns(testFile);

            Assert.Throws<Exception>(() => _equipmentRepository.GetAllEquipment());
        }

        #endregion

        #region AddEquipment

        /// <summary>
        /// Tests the AddEquipment method with valid Equipment
        /// </summary>
        [Test]
        public void AddEquipment_ValidEquipment()
        {
            var beforeCount = _equipmentRepository.Equipments.Count;

            _equipmentRepository.AddEquipment(_testEquipment);

            Assert.AreEqual(beforeCount + 1, _equipmentRepository.Equipments.Count);
        }

        /// <summary>
        /// Test the AddEquipment method if the Equipment is null
        /// </summary>
        [Test]
        public void AddEquipment_NullEquipment()
        {
            Assert.Throws<NullReferenceException>(() => _equipmentRepository.AddEquipment(null));
        }

        #endregion

        #region GetEquipmentById

        /// <summary>
        /// Tests the GetEquipmentById method with valid Id
        /// </summary>
        [Test]
        public void GetEquipmentById_ValidId_ReturnsEquipment()
        {
            _mockFileIoOperations.Setup(repo => repo.ReadFile()).Returns(_testFile);

            var expectedResult = _testEquipment;
            var result = _equipmentRepository.GetEquipmentById(1);

            Assert.AreEqual(expectedResult.Id, result.Id);
            Assert.AreEqual(expectedResult.Name, result.Name);
            Assert.AreEqual(expectedResult.EquipmentType, result.EquipmentType);
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
            Assert.Throws<InvalidOperationException>(() => _equipmentRepository.GetEquipmentById(10));
        }

        #endregion

        #region UpdateEquipment

        /// <summary>
        /// Tests the UpdateEquipment method with valid input value
        /// </summary>
        [Test]
        public void UpdateEquipment_ValidEquipment_ReturnsEquipment()
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

            var result = _equipmentRepository.UpdateEquipment(inputEquipment);

            Assert.AreEqual(expectedResult.Price, result.Price);
            Assert.AreEqual(expectedResult.LoyaltyPoint, result.LoyaltyPoint);

            inputEquipment.EquipmentType = EquipmentType.Regular;
            inputEquipment.RentDays = 2;

            expectedResult.EquipmentType = EquipmentType.Regular;
            expectedResult.Price = 220;
            expectedResult.LoyaltyPoint = 1;

            result = _equipmentRepository.UpdateEquipment(inputEquipment);

            Assert.AreEqual(expectedResult.Price, result.Price);
            Assert.AreEqual(expectedResult.LoyaltyPoint, result.LoyaltyPoint);
        }

        /// <summary>
        /// Tests the UpdateEquipment method with valid input value
        /// </summary>
        [Test]
        public void UpdateEquipment_InValidRentDays_ReturnsEquipment()
        {
            var inputEquipment = _testEquipment;
            Assert.Throws<Exception>(() => _equipmentRepository.UpdateEquipment(inputEquipment));
        }

        /// <summary>
        /// Tests the UpdateEquipment with null input value
        /// </summary>
        [Test]
        public void UpdateEquipment_NullEquipment_ThrowException()
        {
            Assert.Throws<NullReferenceException>(() => _equipmentRepository.UpdateEquipment(null));
        }

        #endregion

        #endregion
    }
}
