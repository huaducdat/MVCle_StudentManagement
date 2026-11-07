using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using StudentManagement.Models;
using StudentManagement.Repository;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;
        public StudentController(StudentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var students = _repository.GetAll();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            try
            {
                _repository.Add(student);
                TempData["Success"] = "Thêm sinh viên thành công!";
                return RedirectToAction("Index");
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                ModelState.AddModelError(
                    "StudentCode", "Mã sinh viên đã tồn tại");

                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _repository.GetById(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(student);

            try
            {
                bool result = _repository.Update(student);

                if (!result)
                    return NotFound();

                TempData["Success"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                ModelState.AddModelError(
                    "StudentCode", "Mã sinh viên đã tồn tại");

                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _repository.GetById(id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = _repository.Delete(id);

            if (!result)
                return NotFound();

            TempData["Success"] = "Xóa sinh viên thành công!";
            return RedirectToAction("Index");
        }
    }
}
