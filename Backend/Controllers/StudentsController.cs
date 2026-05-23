using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAPI.Data;
using WebLabAPI.Models;

namespace WebLabAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return await _context.Students.OrderBy(s => s.FirstName).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = "Student not found" });
        return student;
    }

    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.RollNumber) ||
            string.IsNullOrWhiteSpace(student.FirstName) ||
            string.IsNullOrWhiteSpace(student.Email))
            return BadRequest(new { message = "Required fields missing" });

        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.Email == student.Email);
        if (existingStudent != null)
            return BadRequest(new { message = "Email already exists" });

        student.CreatedAt = DateTime.UtcNow;
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingStudent = await _context.Students.FindAsync(id);
        if (existingStudent == null)
            return NotFound(new { message = "Student not found" });

        existingStudent.RollNumber = student.RollNumber;
        existingStudent.FirstName = student.FirstName;
        existingStudent.LastName = student.LastName;
        existingStudent.Email = student.Email;
        existingStudent.PhoneNumber = student.PhoneNumber;
        existingStudent.DateOfBirth = student.DateOfBirth;
        existingStudent.Department = student.Department;
        existingStudent.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Update failed" });
        }

        return Ok(existingStudent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = "Student not found" });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Student deleted successfully" });
    }
}
