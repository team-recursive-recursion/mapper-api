using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mapper_Api.Context;
using Mapper_Api.Models;

namespace Mapper_Api.Controllers
{
    public class PointsController : Controller
    {
        private readonly CourseDb _context;

        public PointsController(CourseDb context)
        {
            _context = context;
        }

        // GET: Points
        public async Task<IActionResult> Index()
        {
            return View(await _context.Point.ToListAsync());
        }

        // GET: Points/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point
                .SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // GET: Points/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Type,CourseElementID,GolfCourseID,HoleID")] Point point)
        {
            if (ModelState.IsValid)
            {
                point.CourseElementID = Guid.NewGuid();
                _context.Add(point);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(point);
        }

        // GET: Points/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point.SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (point == null)
            {
                return NotFound();
            }
            return View(point);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseID,Type,CourseElementID,GolfCourseID,HoleID")] Point point)
        {
            if (id != point.CourseElementID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(point);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PointExists(point.CourseElementID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(point);
        }

        // GET: Points/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point
                .SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var point = await _context.Point.SingleOrDefaultAsync(m => m.CourseElementID == id);
            _context.Point.Remove(point);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PointExists(Guid id)
        {
            return _context.Point.Any(e => e.CourseElementID == id);
        }
    }
}
