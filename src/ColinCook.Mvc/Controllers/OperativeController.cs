﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Operatives.Entities;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.Mvc.Controllers
{
    [Route("/operatives")]
    public class OperativeController : Controller
    {
        private readonly LiteRepository _repository;

        public OperativeController(LiteRepository repository)
        {
            _repository = repository;
        }

        // GET: Operative
        [HttpGet("colin")]
        public IActionResult Index()
        {
            var operatives = _repository.Fetch<OperativeEntity>();

            // return View(operatives);
            return new JsonResult(operatives);
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