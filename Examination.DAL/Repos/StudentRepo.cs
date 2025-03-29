using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    //public class StudentRepo : Repo<Student>, IStudentRepo
    //{
    //    private readonly AppDbContext _db;
    //    public StudentRepo(AppDbContext db) : base(db)
    //    {
    //        _db = db;
    //    }
    //    public void Update(Student student)
    //    {

    //        var objFromDb = dbSet.FirstOrDefault(s => s.Id == student.Id);
    //        if (objFromDb != null)
    //        {
    //            objFromDb.FirstName = student.FirstName;
    //            objFromDb.Email = student.Email;
    //            objFromDb.Phone = student.Phone;
    //            objFromDb.DepartmentId = student.DepartmentId;
    //            objFromDb.EnrollmentDate = student.EnrollmentDate;
    //            objFromDb.LastName = student.LastName;
    //            objFromDb.DateOfBirth = student.DateOfBirth;
    //            objFromDb.Status = student.Status;
    //            objFromDb.Password = student.Password ?? objFromDb.Password;
    //            _db.Students.Update(objFromDb);
         
    //        }
    //    }

    //}

}
