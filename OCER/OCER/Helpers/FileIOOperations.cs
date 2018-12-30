using System;
using System.Collections.Generic;
using System.IO;
using OCER.Models;

namespace OCER.Helpers
{
    /// <summary>
    /// Implementation of FileIoOperations class
    /// </summary>
    internal static class FileIoOperations
    {
        #region Fields

        private const string InvoicePath = @"./TextFiles/Invoice.txt";
        private const string InventoryPath = @"./TextFiles/Inventory.txt";
        private const string Currency = "€";

        private static readonly log4net.ILog Log = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Internal methods

        /// <summary>
        /// Reads all line of a text file based on path.
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The lines of the file</returns>
        internal static string[] ReadFile(string path = InventoryPath)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw new Exception();
            }
        }

        /// <summary>
        /// Adds content to the text file called Invoice.
        /// </summary>
        /// <param name="equipments">The equipment which are rented and should be invoiced</param>
        internal static void CreateInvoice(IEnumerable<Equipment> equipments)
        {
            ClearInvoice();

            var totalPrice = 0;
            var totalLoyaltyPoints = 0;

            using (StreamWriter writer = File.AppendText(InvoicePath))
            {
                try
                {
                    writer.WriteLine("=== INVOICE ===");
                    writer.WriteLine("Rent details");
                    foreach (var equipment in equipments)
                    {
                        writer.WriteLine($"{equipment.Name}: {equipment.Price}{Currency}");
                        totalPrice += equipment.Price;
                        totalLoyaltyPoints += equipment.LoyaltyPoint;
                    }
                    writer.WriteLine($"Total price: {totalPrice}{Currency}");
                    writer.WriteLine($"Number of bonus points earned: {totalLoyaltyPoints}");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        /// <summary>
        /// Clears the content of text file called Invoice.
        /// </summary>
        internal static void ClearInvoice()
        {
            try
            {
                FileStream fileStream = File.Open(InvoicePath, FileMode.Open);

                fileStream.SetLength(0);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        #endregion
    }
}
