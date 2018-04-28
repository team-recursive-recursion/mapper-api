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
    public class GolfCoursesController : Controller
    {
        private readonly CourseDb _context;

        public GolfCoursesController(CourseDb context)
        {
            _context = context;
        }

        // GET: GolfCourses
        public async Task<IActionResult> Index()
        {
            return View(await _context.GolfCourses.ToListAsync());
        }

        // GET: GolfCourses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfCourse = await _context.GolfCourses
                .SingleOrDefaultAsync(m => m.CourseId == id);
            if (golfCourse == null)
            {
                return NotFound();
            }

            return View(golfCourse);
        }

        // GET: GolfCourses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GolfCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,CreatedAt,UpdatedAt")] GolfCourse golfCourse)
        {
            if (ModelState.IsValid)
            {
                golfCourse.CourseId = Guid.NewGuid();
                _context.Add(golfCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(golfCourse);
        }

        // GET: GolfCourses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfCourse = await _context.GolfCourses.SingleOrDefaultAsync(m => m.CourseId == id);
            if (golfCourse == null)
            {
                return NotFound();
            }
            return View(golfCourse);
        }

        // POST: GolfCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseId,CourseName,CreatedAt,UpdatedAt")] GolfCourse golfCourse)
        {
            if (id != golfCourse.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(golfCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GolfCourseExists(golfCourse.CourseId))
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
            return View(golfCourse);
        }

        // GET: GolfCourses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var golfCourse = await _context.GolfCourses
                .SingleOrDefaultAsync(m => m.CourseId == id);
            if (golfCourse == null)
            {
                return NotFound();
            }

            return View(golfCourse);
        }

        // POST: GolfCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var golfCourse = await _context.GolfCourses.SingleOrDefaultAsync(m => m.CourseId == id);
            _context.GolfCourses.Remove(golfCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GolfCourseExists(Guid id)
        {
            return _context.GolfCourses.Any(e => e.CourseId == id);
        }
    }
}
