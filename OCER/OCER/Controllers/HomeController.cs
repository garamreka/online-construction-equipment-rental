using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OCER.Helpers;
using OCER.Models;
using OCER.Repositories;

namespace OCER.Controllers
{
    /// <summary>
    /// Implementation of HomeController class
    /// </summary>
    [Route("")]
    public class HomeController : Controller
    {
        #region Fields

        private static readonly log4net.ILog log = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EquipmentRepository _equipmentRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="equipmentRepository"></param>
        public HomeController(EquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Displays the Index page
        /// </summary>
        /// <returns>The index page</returns>
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            try
            {
                return View("Index", _equipmentRepository.GetAllEquipmentFromFile().ToList());
            }
            catch (Exception e)
            {
                log.Error(e);
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays AddRentDetails page based on Equipment Id
        /// </summary>
        /// <param name="id">The Id</param>
        /// <returns>AddRentDetails page</returns>
        [HttpGet]
        [Route("/details/{id}")]
        public IActionResult AddRentDetails([FromQuery] int id)
        {
            if (id > 0)
            {
                try
                {
                    return View("AddRentDetails", _equipmentRepository.GetEquipmentById(id));
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return RedirectToAction("Error");
                }
            }

            log.Error("Invalid Id from query.");
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Gets an item from the user and redirects to Confirm page
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>Confirm page</returns>
        [HttpPost]
        [Route("/details/{id}")]
        public IActionResult AddToCart([FromForm]Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _equipmentRepository.AddNewRentedEquipment(equipment);
                    return RedirectToAction("Confirm");
                }
                catch (Exception e)
                {
                    log.Error(e);
                    return RedirectToAction("Error");
                }
            }

            log.Error("Invalid model state.");
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Displays the Confirm page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/confirm")]
        public IActionResult Confirm() 
        {
             return View("Confirm");
        }

        /// <summary>
        /// Finalize the invoice and displays the Final page
        /// </summary>
        /// <returns>Final page</returns>
        [HttpGet]
        [Route("/final")]
        public IActionResult Final() 
        {
            try
            {
                _equipmentRepository.FinalizeInvoice();
                _equipmentRepository.Equipments = new List<Equipment>();
                return View("Final");
            }
            catch (Exception e)
            {
                log.Error(e);
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays the Error page
        /// </summary>
        /// <returns>Error page</returns>
        [HttpGet]
        [Route("/error")]
        public IActionResult Error()
        {
            _equipmentRepository.Equipments = new List<Equipment>();
            FileIoOperations.ClearInvoice();
            return View("Error");
        }

        #endregion
    }
}
