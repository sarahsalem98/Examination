using Examination.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.DAL.Seeding
{
    public class DataSeeder
    {

        private readonly AppDbContext _db;
        public DataSeeder(AppDbContext db)
        {
            _db = db;
        }
        public void StudentSeeder()
        {
            var userPassword = "123456";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPassword);

            if (!_db.Branches.Any(b => b.Name == "Alex-Branch") && !_db.Departments.Any(d => d.Name == "Professional Web Development"))
            {
                var branch = new Branch
                {
                    Name = "Alex-Branch",
                    Location = "Alexandria",
                    Status = 1,
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now
                };
                _db.Branches.Add(branch);
                var department = new Department
                {
                    Name = "Professional Web Development",
                    Status = 1,
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now,
                    Capacity = 30,
                    Description = "Covers on .NET mainly"
                };
                _db.Departments.Add(department);
                _db.SaveChanges();
                var depatmentBranch = new DepartmentBranch
                {
                    BranchId = branch.Id,
                    DepartmentId = department.Id,
                };
                _db.DepartmentBranches.Add(depatmentBranch);
                _db.SaveChanges();


                if (!_db.UserTypes.Any(u => u.TypeName == "Student"))
                {
                    var userType = new UserType
                    {
                        TypeName = "Student"
                    };
                    _db.UserTypes.Add(userType);
                    _db.SaveChanges();
                }

                var StudentUserType = _db.UserTypes.FirstOrDefault(u => u.TypeName == "Student");
                if (!_db.Users.Any(u => u.Email == "mrgnedy@gmail.com"))
                {
                    var user = new User
                    {
                        FirstName = "Ahmed",
                        LastName = "Gnedy",
                        Email = "mrgnedy@gmail.com",
                        Phone = "0101000000",
                        Password = hashedPassword,
                        Age = 25,
                        Status = 1,
                        CreatedBy = 1,
                        CreatedAt = DateTime.Now,

                    };
                    user.UserTypes.Add(StudentUserType);    
                    _db.Users.Add(user);
                    _db.SaveChanges();

                    var student = new Student
                    {
                        UserId = user.Id,
                        EnrollmentDate = new DateTime(2024, 10, 9),
                        DateOfBirth = new DateOnly(1998, 10, 5),
                        TrackType = 1,//9 months,
                        DepartmentBranchId = depatmentBranch.Id,
                    };
                    _db.Students.Add(student);
                    _db.SaveChanges();

                }


            }

            



        }

        public void SuperAdminSeeder()
        {
            var userPassword = "123456";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPassword);
            List<UserType> userTypes = new List<UserType>();
            if (!_db.UserTypes.Any(u => u.TypeName == "SuperAdmin"))
            {
                var userType = new UserType
                {
                    TypeName = "SuperAdmin"
                };
             userTypes.Add(userType);
            }
            if (!_db.UserTypes.Any(u => u.TypeName == "Instructor"))
            {
                var userType = new UserType
                {
                    TypeName = "Instructor"
                };
               userTypes.Add(userType);
            }
            if (!_db.UserTypes.Any(u => u.TypeName == "Student"))
            {
                var userType = new UserType
                {
                    TypeName = "Student"
                };
                userTypes.Add(userType);
            }
            _db.UserTypes.AddRange(userTypes);
            _db.SaveChanges();
            var AdminUserType = _db.UserTypes.FirstOrDefault(u => u.TypeName == "SuperAdmin");
            if (!_db.Users.Any(u => u.Email == "superAdmin@gmail.com"))
            {
                var user = new User
                {

                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "superAdmin@gmail.com",
                    Phone = "0101000000",
                    Password = hashedPassword,
                    Age = 25,
                    Status = 1,
                    CreatedBy = 1,
                    CreatedAt = DateTime.Now,

                };
                user.UserTypes.Add(AdminUserType);
                _db.Users.Add(user);
                _db.SaveChanges();



            }
        }
    }
}
