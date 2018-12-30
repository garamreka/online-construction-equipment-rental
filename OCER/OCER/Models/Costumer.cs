using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OCER.Models
{
    /// <summary>
    /// Implementation of Costumer class.
    /// </summary>
    public class Costumer
    {
        /// <summary>
        /// Name of the costumer
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Loyalty point of the costumer
        /// </summary>
        public int LoyaltyPoint { get; }
    }
}
