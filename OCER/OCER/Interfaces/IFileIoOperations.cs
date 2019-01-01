using System.Collections.Generic;
using OCER.Models;

namespace OCER.Interfaces
{
    /// <summary>
    /// Implementation of IFileIoOperations interface
    /// </summary>
    public interface IFileIoOperations
    {
        /// <summary>
        /// Reads all line of a text file based on path.
        /// </summary>
        /// <returns>The lines of the file</returns>
        string[] ReadFile();

        /// <summary>
        /// Adds content to the text file called Invoice.
        /// </summary>
        /// <param name="equipments">The equipment which are rented and should be invoiced</param>
        void CreateInvoice(IEnumerable<Equipment> equipments);

        /// <summary>
        /// Clears the content of text file called Invoice.
        /// </summary>
        void ClearInvoice();
    }
}
