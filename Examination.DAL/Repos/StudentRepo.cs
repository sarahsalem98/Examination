using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Repos
{
    public class StudentRepo : Repo<Student>, IStudentRepo
    {
        private readonly AppDbContext _db;
        public StudentRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Student student)
        {
            var objFromDb = dbSet.FirstOrDefault(s => s.Id == student.Id);
            if (objFromDb != null)
            {
                objFromDb.User.FirstName = student.User.FirstName;
                objFromDb.User.Email = student.User.Email;
                objFromDb.User.Phone = student.User.Phone;
                objFromDb.DepartmentBranchId = student.DepartmentBranchId;
                objFromDb.EnrollmentDate = student.EnrollmentDate;
                objFromDb.User.LastName = student.User.LastName;
                objFromDb.DateOfBirth = student.DateOfBirth;
                objFromDb.User.Status = student.User.Status;
                objFromDb.User.Password = student.User.Password ?? objFromDb.User.Password;
                _db.Students.Update(objFromDb);
            }
        }

    }

}
