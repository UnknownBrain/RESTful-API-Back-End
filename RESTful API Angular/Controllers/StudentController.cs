using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RESTful_API_Angular.Models;
using RESTful_API_Angular.Context;

namespace RESTful_API_Angular.Controllers
{
    public class StudentController : ApiController
    {
        [HttpPost]
        public IHttpActionResult RegisterStudent(StudentRegistration student)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StudentModel studentContext = new StudentModel()
            {
                FirstName   = student.FirstName,
                LastName    = student.LastName,
                Email       = student.Email,
                Summary     = student.Summary,
                Description = student.Description
            };

            DatabaseContext context = new DatabaseContext();
            context.Students.Add(studentContext);
            context.SaveChanges();

            return Created("/api/Student/RegisterStudent", new { Message = "student_registered"});
        }

        [HttpGet]
        public IHttpActionResult GetStudent(int studentId)
        {
            DatabaseContext context = new DatabaseContext();
            
            if(!context.Students.Any(s => s.StudentId == studentId))
            {
                return BadRequest("No user matched this id");
            }

            StudentModel student = context.Students.Where(s => s.StudentId == studentId).First();

            return Ok<StudentModel>(student);
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(StudentModel student)
        {
            DatabaseContext context = new DatabaseContext();

            context.Students.Attach(student);

            context.Entry<StudentModel>(student).Property(s => s.FirstName).IsModified = true;
            context.Entry<StudentModel>(student).Property(s => s.LastName).IsModified = true;
            context.Entry<StudentModel>(student).Property(s => s.Email).IsModified = true;
            context.Entry<StudentModel>(student).Property(s => s.Summary).IsModified = true;
            context.Entry<StudentModel>(student).Property(s => s.Description).IsModified = true;

            context.SaveChanges();

            return Ok(new { Message = "student_updated"});
        }

        [HttpGet]
        public IHttpActionResult ListStudents()
        {
            return Ok(new { Users = (new DatabaseContext()).Students.ToList() });
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(int studentId)
        {
            DatabaseContext context = new DatabaseContext();
            StudentModel student = context.Students.Where(s => s.StudentId == studentId).First();

            context.Students.Remove(student);
            context.SaveChanges();

            return Ok(new { Message = "student_deleted"});
        }

        public class StudentRegistration
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string Summary { get; set; }
            [Required]
            public string Description { get; set; }
        }
    }
}
