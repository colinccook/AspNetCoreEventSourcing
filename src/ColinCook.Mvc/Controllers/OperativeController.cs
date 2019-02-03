using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.Mvc.Controllers
{
    [Route("/operatives")]
    public class OperativeController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public OperativeController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        // GET: Operative
        [HttpGet("index")]
        public IActionResult Index()
        {
            var allOperatives = _queryProcessor.Process(
                    new AllOperativesQuery(), CancellationToken.None);

            return new JsonResult(allOperatives);
        }

                // GET: Operative
        [HttpGet("colin2")]
        public IActionResult colin2()
        {
            //var operatives = _operativeRepository.GetAll();

            //// return View(operatives);
            //return new JsonResult(operatives);
            return Ok();
        }

        // GET: Operative/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Operative/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Operative/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Operative/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Operative/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Operative/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Operative/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}