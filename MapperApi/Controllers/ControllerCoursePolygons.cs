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
    public class ControllerCoursePolygons : Controller
    {
        private readonly CourseDb _context;

        public ControllerCoursePolygons(CourseDb context)
        {
            _context = context;
        }

        // GET: ControllerCoursePolygons
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoursePolygons.ToListAsync());
        }

        // GET: ControllerCoursePolygons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursePolygon = await _context.CoursePolygons
                .SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (coursePolygon == null)
            {
                return NotFound();
            }

            return View(coursePolygon);
        }

        // GET: ControllerCoursePolygons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ControllerCoursePolygons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,PolygonRaw,CreatedAt,UpdatedAt,CourseElementID,GolfCourseID,HoleID")] CoursePolygon coursePolygon)
        {
            if (ModelState.IsValid)
            {
                coursePolygon.CourseElementID = Guid.NewGuid();
                _context.Add(coursePolygon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coursePolygon);
        }

        // GET: ControllerCoursePolygons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursePolygon = await _context.CoursePolygons.SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (coursePolygon == null)
            {
                return NotFound();
            }
            return View(coursePolygon);
        }

        // POST: ControllerCoursePolygons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Type,PolygonRaw,CreatedAt,UpdatedAt,CourseElementID,GolfCourseID,HoleID")] CoursePolygon coursePolygon)
        {
            if (id != coursePolygon.CourseElementID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coursePolygon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursePolygonExists(coursePolygon.CourseElementID))
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
            return View(coursePolygon);
        }

        // GET: ControllerCoursePolygons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursePolygon = await _context.CoursePolygons
                .SingleOrDefaultAsync(m => m.CourseElementID == id);
            if (coursePolygon == null)
            {
                return NotFound();
            }

            return View(coursePolygon);
        }

        // POST: ControllerCoursePolygons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var coursePolygon = await _context.CoursePolygons.SingleOrDefaultAsync(m => m.CourseElementID == id);
            _context.CoursePolygons.Remove(coursePolygon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursePolygonExists(Guid id)
        {
            return _context.CoursePolygons.Any(e => e.CourseElementID == id);
        }
    }
}
