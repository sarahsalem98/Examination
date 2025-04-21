var InstructorCourse = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Instructor/Course/List",
            data: { courseSearch: InstructorCourse.currentSearchData, PageSize: InstructorCourse.pageSize, Page: Page },

            success: function (response) {
                $("#loader").removeClass("show");
                $("#courseList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    InstructorCourse.totalPages = Math.ceil(totalCounts / InstructorCourse.pageSize);
                    InstructorCourse.InitializePagination()
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching course data:", error);
            }
        });

    },
    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');

        $('#pagination').twbsPagination({
            totalPages: InstructorCourse.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != InstructorCourse.currentPage) {
                    InstructorCourse.currentPage = page;
                    InstructorCourse.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        InstructorCourse.currentSearchData = {
            Name: $("#Name").val(),
            BranchId: $("#BranchId").val(),
            DepartmentId: $("#DepartmentId").val(),
            Status: $("#Status").val(),
            CourseId: $("#CourseId").val()
        }
    },
    Search: function () {

        InstructorCourse.SetAdvancedSearchData();
        InstructorCourse.Fetch(1);
        console.log(InstructorCourse.currentSearchData);

    },
    Reset: function () {
        $("#Name").val("");
        $("#DepartmentId").val("");
        $("#BranchId").val("");
        $("#Status").val("");
        $("#CourseId").val("");
        InstructorCourse.currentSearchData = {};
        InstructorCourse.Fetch(1);
    },
    CompleteCourse: function (DepartmentBranchId, instructor_id,course_id)
    {
        $("#loader").addClass("show");
        $.ajax({
            type: "Post",
            url: "/Instructor/Course/CompleteCourse",
            data: { DepartmentBranchId: DepartmentBranchId, instructor_id: instructor_id,course_id: course_id },
            success: function (response) {

                $("#loader").removeClass("show");
                if (response.success) {

                    InstructorCourse.Fetch(InstructorCourse.currentPage);
                    toastr.success(response.message);

                } else {
                   
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching Complete Course:", error);
            }
        })
    }

}