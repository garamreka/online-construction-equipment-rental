using System.Collections.Generic;
using OCER.Models;

namespace OCER.Interfaces
{
    /// <summary>
    /// Implementation of IEquipmentRepository interface
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// Gets all equipment
        /// </summary>
        /// <returns>The equipment list</returns>
        IEnumerable<Equipment> GetAllEquipment();

        /// <summary>
        /// Gets the equipment based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> The equipment</returns>
        Equipment GetEquipmentById(int id);

        /// <summary>
        /// Adds the equipment
        /// </summary>
        /// <param name="equipment">The equipment</param>
        void AddEquipment(Equipment equipment);

        /// <summary>
        /// Updates the equipment
        /// </summary>
        /// <param name="equipment">The updated equipment</param>
        /// <returns></returns>
        Equipment UpdateEquipment(Equipment equipment);

        /// <summary>
        /// Finalizes the invoice for the rented equipments
        /// </summary>
        void FinalizeInvoice();

        /// <summary>
        /// Sets the default values
        /// </summary>
        void SetDefaultValues();
    }
}
