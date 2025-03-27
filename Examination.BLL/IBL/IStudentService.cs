using Examination.BLL.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.BLL.IBL
{
   public interface IStudentService
    {
        public int Add(StudentMV student);  

    }
}
