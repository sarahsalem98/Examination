﻿using Examination.PL.ModelViews;

namespace Examination.PL.IBL;

public interface ICourseService
{
    public int Add(CourseMV course);
    public PaginatedData<CourseMV> GetAllPaginated(string searchName, int PageSize = 10, int Page = 1);
}
