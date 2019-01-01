using System.Collections.Generic;
using OCER.Models;

namespace OCER.Interfaces
{
    /// <summary>
    /// Implementation of IFileIoOperations interface
    /// </summary>
    public interface IFileIoOperations
    {
        string[] ReadFile();

        void CreateInvoice(IEnumerable<Equipment> equipments);

        void ClearInvoice();
    }
}
