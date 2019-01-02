using System.ComponentModel.DataAnnotations;
using OCER.Enums;

namespace OCER.Models
{
    /// <summary>
    /// Implementation of Equipment class
    /// </summary>
    public class Equipment
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Equipment()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="equipmentType"></param>
        internal Equipment(int id, string name, EquipmentType equipmentType)
        {
            Id = id;
            Name = name;
            EquipmentType = equipmentType;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Id of the equipment
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Name of the equipment
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Type of the equipment
        /// </summary>
        [Required]
        public EquipmentType EquipmentType { get; set; }
        /// <summary>
        /// Days of rent of the equipment
        /// </summary>
        [Range(1, 100, ErrorMessage = "Please enter valid integer number")]
        public int RentDays { get; set; }
        /// <summary>
        /// Price of the equipment
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Loyalty point of the equipment
        /// </summary>
        public int LoyaltyPoint { get; set; }

        #endregion

        #region Overrides

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var toCompareWith = obj as Equipment;

            if (toCompareWith == null)
            {
                return false;
            }

            return Id == toCompareWith.Id
                   && Name == toCompareWith.Name
                   && EquipmentType == toCompareWith.EquipmentType
                   && RentDays == toCompareWith.RentDays
                   && Price == toCompareWith.Price
                   && LoyaltyPoint == toCompareWith.LoyaltyPoint;
        }

        #endregion
    }
}
