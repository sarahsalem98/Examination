var InstructorGeneratedExam = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Instructor/GeneratedExam/List",
            data: { search: InstructorGeneratedExam.currentSearchData, PageSize: InstructorGeneratedExam.pageSize, Page: Page },

            success: function (response) {
                $("#loader").removeClass("show");
                $("#generatedexamList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    InstructorGeneratedExam.totalPages = Math.ceil(totalCounts / InstructorGeneratedExam.pageSize);
                    InstructorGeneratedExam.InitializePagination()
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching exams data:", error);
            }
        });

    },
    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');

        $('#pagination').twbsPagination({
            totalPages: InstructorGeneratedExam.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != InstructorGeneratedExam.currentPage) {
                    InstructorGeneratedExam.currentPage = page;
                    InstructorGeneratedExam.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        InstructorGeneratedExam.currentSearchData = {
            Name: $("#Name").val(),
            BranchId: $("#BranchId").val(),
            DepartmentId: $("#DepartmentId").val(),
            ExamType: $("#ExamType").val(),
            CourseId: $("#CourseId").val()
        }
    },
    Search: function () {

        InstructorGeneratedExam.SetAdvancedSearchData();
        InstructorGeneratedExam.Fetch(1);
        console.log(InstructorGeneratedExam.currentSearchData);

    },
    Reset: function () {
        $("#Name").val("");
        $("#DepartmentId").val("");
        $("#BranchId").val("");
        $("#ExamType").val("");
        $("#CourseId").val("");
        InstructorGeneratedExam.currentSearchData = {};
        InstructorGeneratedExam.Fetch(1);
    },
    GetDepartmentsByBranchId: function () {
        var branchId = $('#branchId').val();
        console.log(branchId);

        $.ajax({
            type: "GET",
            url: "/Instructor/GeneratedExam/GetDepartmentsByBranchId",
            data: { BranchId: branchId },
            success: function (response) {
                console.log(response);
                var departmentDropdown = $("#departmentDropdown");
                departmentDropdown.empty();

                departmentDropdown.append('<option selected >select department</option>');

                $.each(response.data, function (index, department) {

                    departmentDropdown.append(`<option value="${department.id}">${department.name}</option>`);
                });

               

            },
            error: function (xhr, status, error) {
                console.error("Error fetching department data:", error);
            }
        });

    }, 
    GetExamsByBranchInstructorDepartment: function () {
        var branchId = $('#branchId').val();
        var departmentId = $("#departmentDropdown").val()
        console.log(branchId);
        console.log(departmentId)

        $.ajax({
            type: "GET",
            url: "/Instructor/GeneratedExam/GetExamsByInstructorDepartmentBranch",
            data: { department_id: departmentId, branch_id: branchId },
            success: function (response) {
                console.log(response);
                var examsDropdown = $("#examsmentDropdown");
                examsDropdown.empty();

                examsDropdown.append('<option selected >Select Exam</option>');

                $.each(response.data, function (index, exam) {

                    examsDropdown.append(`<option value="${exam.id}">${exam.name}</option>`);
                });



            },
            error: function (xhr, status, error) {
                console.error("Error fetching exam data:", error);
            }
        });

    },
    ShowAddUpdateModal: function (id) {
       
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Instructor/GeneratedExam/GenerateExam",
            data: {id:id},
            success: function (response) {

                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    },
    GenerateExam: function (e) {
       
        e.preventDefault();
        var examId = $('#examsmentDropdown').val();
        var departmentId = $('#departmentDropdown').val();
        var branchId = $('#branchId').val();
        var numsTS = $('input[name="NumsTS"]').val();
        var numsMCQ = $('input[name="NumsMCQ"]').val();
        var takenDate = $('input[name="TakenDate"]').val();
        var takenTime = $('input[name="takenTime"]').val();

        if (!examId || !departmentId || !branchId || !numsTS || !numsMCQ || !takenDate || !takenTime) {
            toastr.error("Please fill in all required fields.");
            return;
        }

        var formData = {
            ExamId: examId,
            DepartmentId: departmentId,
            BranchId: branchId,
            NumsTS: numsTS,
            NumsMCQ: numsMCQ,
            TakenDate: takenDate,
            TakenTime: takenTime
        };
        $.ajax({
            type: "POST",
            url: "/Instructor/GeneratedExam/GenerateExam",
            data: formData, 
            success: function (response) {
               
                if (response.success) {
                    InstructorGeneratedExam.Fetch(InstructorGeneratedExam.currentPage);
                   
                    $('#addUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error generateexam:", error);
            }
        });
    },

}