using System;
using System.Collections.Generic;
using System.Linq;
using OCER.Enums;
using OCER.Helpers;
using OCER.Interfaces;
using OCER.Models;

namespace OCER.Repositories
{
    /// <summary>
    /// Implementation of EquipmentRepository class
    /// </summary>
    public class EquipmentRepository : IEquipmentRepository
    {
        #region Fields

        private readonly int _oneTimeRentalFee = 100;
        private readonly int _premiumDailyFee = 60;
        private readonly int _regularDailyFee = 40;
        private readonly int _heavyEquipmentLoyaltyPoint = 2;
        private readonly int _otherEquipmentLoyaltyPoint = 1;
        private readonly int _regularEquipmentSpecialPriceForDays = 2;
        private readonly int _specializedEquipmentSpecialPriceForDays = 3;
        private string[] _lines;

        /// <summary>
        /// List of rented equipments
        /// </summary>
        public List<Equipment> Equipments = new List<Equipment>();

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the lines from the file called Inventory.
        /// </summary>
        private string[] Lines => _lines ?? (_lines = FileIoOperations.ReadFile());

        #endregion

        #region Public methods

        /// <summary>
        /// Gets all equipment
        /// </summary>
        /// <returns>The equipment list</returns>
        public IEnumerable<Equipment> GetAllEquipment() => GetAllEquipmentFromFile();

        /// <summary>
        /// Gets a specific equipment based on id 
        /// </summary>
        /// <param name="id">The id of the equipment</param>
        /// <returns>The equipment</returns>
        public Equipment GetEquipmentById(int id)
        {
            if (id <= 0)
            {
                throw new Exception($"Invalid id: {id}. Id should be a positive integer.");
            }
            var equipment =  GetAllEquipmentFromFile().First(item => item.Id == id);

            if (equipment != null)
            {
                return equipment;
            }

            throw new Exception($"Could not find the equipment based on the given id: {id}");
        }

        /// <summary>
        /// Adds the equipment
        /// </summary>
        /// <param name="equipment">The equipment</param>
        public void AddEquipment(Equipment equipment)
        {
            AddNewRentedEquipment(equipment);
        }

        /// <summary>
        /// Updates the equipment
        /// </summary>
        /// <param name="equipment">The updated equipment</param>
        /// <returns></returns>
        public Equipment UpdateEquipment(Equipment equipment) => ChangeEquipmentDefaultValues(equipment);

        /// <summary>
        /// Finalizes the invoice for the rented equipments
        /// </summary>
        public void FinalizeInvoice()
        {
            if (Equipments.Any())
            {
                var equipments = Equipments.Select(UpdateEquipment);
                FileIoOperations.CreateInvoice(equipments);
            }
            else
            {
                throw new Exception("Could not find equipment to invoice");
            }
        }

        /// <summary>
        /// Sets the default values
        /// </summary>
        public void SetDefaultValues()
        {
            Equipments = new List<Equipment>();
            FileIoOperations.ClearInvoice();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets all Equipment from the file called Inventory.
        /// Id, Name and EquipmentType properties of the Equipment gets their values from the file,
        /// other properties are initialized with default values.
        /// </summary>
        /// <returns>The Equipments</returns>
        private IEnumerable<Equipment> GetAllEquipmentFromFile()
        {
            foreach (var line in Lines)
            {
                var equipmentElements = line.Split(';');
                var successfulIntParse = Int32.TryParse(equipmentElements[0], out int id);

                if (successfulIntParse)
                {
                    var successfulEnumParse = Enum.TryParse(equipmentElements[2], out EquipmentType type);

                    if (successfulEnumParse)
                        yield return new Equipment(id, equipmentElements[1], type);
                    else
                        throw new Exception("Unable to parse enum.");
                }
                else
                {
                    throw new Exception("Unable to parse int.");
                }
            }
        }

        /// <summary>
        /// Adds an equipment to _equipments
        /// </summary>
        /// <param name="equipment">The equipment</param>
        private void AddNewRentedEquipment(Equipment equipment)
        {
            if (equipment != null)
                Equipments.Add(equipment);
            else
                throw new NullReferenceException("Unable to add new rented equipment");
        }

        /// <summary>
        /// Changes the default property values for Equipment based on rent details.
        /// </summary>
        /// <param name="equipment">The rented equipment</param>
        /// <returns>With the Equipment</returns>
        private Equipment ChangeEquipmentDefaultValues(Equipment equipment)
        {
            if (equipment != null)
            {
                equipment.Price = CalculatePrice(equipment.EquipmentType, equipment.RentDays);
                equipment.LoyaltyPoint = GetLoyaltyPoint(equipment.EquipmentType);

                return equipment;
            }

            throw new NullReferenceException();
        }

        /// <summary>
        /// Calculates the equipment price based on its type and days of rent
        /// </summary>
        /// <param name="type">The equipment type</param>
        /// <param name="rentDays">The days of rent</param>
        /// <returns>The corresponding price</returns>
        private int CalculatePrice(EquipmentType type, int rentDays)
        {
            if (rentDays < 1) throw new Exception($"Invalid rent days: {rentDays}. Renting days should be at least 1.");

            switch (type)
            {
                case EquipmentType.Heavy:
                    return _oneTimeRentalFee + rentDays * _premiumDailyFee;
                case EquipmentType.Regular:
                    return _oneTimeRentalFee + _regularEquipmentSpecialPriceForDays * _premiumDailyFee +
                           (rentDays - _regularEquipmentSpecialPriceForDays) * _regularDailyFee;
                case EquipmentType.Specialized:
                    return _premiumDailyFee * _specializedEquipmentSpecialPriceForDays +
                           (rentDays - _specializedEquipmentSpecialPriceForDays) * _regularDailyFee;
                default: throw new Exception("Unknown equipment type!");
            }
        }

        /// <summary>
        /// Gets the loyalty point(s) based on equipment type
        /// </summary>
        /// <param name="type">The equipment type</param>
        /// <returns>The corresponding loyalty points</returns>
        private int GetLoyaltyPoint(EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.Heavy:
                    return _heavyEquipmentLoyaltyPoint;
                case EquipmentType.Regular:
                    return _otherEquipmentLoyaltyPoint;
                case EquipmentType.Specialized:
                    return _otherEquipmentLoyaltyPoint;
                default: throw new Exception("Unknown equipment type!");
            }
        }

        #endregion
    }
}
