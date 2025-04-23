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
            data: { GeneratedExamSearch: InstructorGeneratedExam.currentSearchData, PageSize: InstructorGeneratedExam.pageSize, Page: Page },

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
            Status: $("#Status").val(),
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
        $("#Status").val("");
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
    ShowAddUpdateModal: function () {
       
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Instructor/GeneratedExam/GenerateExam",
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

}