using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KuetOverflow.Data;
using KuetOverflow.Models;
using KuetOverflow.Models.SchoolViewModels;

namespace KuetOverflow.Controllers
{
    public class LecturesController : Controller
    {
        private readonly SchoolContext _context;

        public LecturesController(SchoolContext context)
        {
            _context = context;    
        }

        // GET: Lectures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecture.ToListAsync());
        }

        // GET: Lectures/Details/5
        public async Task<IActionResult> Details(int? id, int course_id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["lecture"] = id;

            var model = new LectureCommentViewModel();

            model.Lecture = await _context.Lecture
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Comments = await _context.Comment
                .Where(c => c.LectureID == id)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = new Lecture_LectureListViewModel();
            viewModel.LectureCommentViewModel = model;
            viewModel.Course_ID = course_id;

            viewModel.Lectures = await _context.Lecture
                .Where(l => l.CourseId == course_id)
                .AsNoTracking()
                .ToListAsync();

            if (model.Lecture == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Lectures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CourseId,Title,Body")] Lecture lecture)
        {
            lecture.UpdateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lecture);
        }

        // GET: Lectures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lecture.SingleOrDefaultAsync(m => m.ID == id);
            if (lecture == null)
            {
                return NotFound();
            }
            return View(lecture);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CourseId,Title,Body")] Lecture lecture)
        {
            lecture.UpdateTime = DateTime.Now;

            if (id != lecture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new {id=id});
            }
            return View(lecture);
        }

        // GET: Lectures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lecture
                .SingleOrDefaultAsync(m => m.ID == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecture = await _context.Lecture.SingleOrDefaultAsync(m => m.ID == id);
            _context.Lecture.Remove(lecture);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LectureExists(int id)
        {
            return _context.Lecture.Any(e => e.ID == id);
        }
    }
}
