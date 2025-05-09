﻿using Examination.DAL.Entities;
using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IInstructorService
    {
        public PaginatedData<InstructorMV> GetAllPaginated(InstructorSearchMV search,int page = 1, int pagesize = 10);
        public int Add(InstructorMV instructor);
        public int Update(InstructorMV instructor);
        public InstructorMV getById(int Id);
        public int ChangeStatus(int Id,int status);

        InstructorMV GetProfile(int userId);
        int UpdatePassword(InstructorPasswordUpdateMV model);
        public int GetInstructorIdbyUserID(int UserID);
    }
}
