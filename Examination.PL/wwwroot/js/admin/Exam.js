var AdminExam = {

    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Exam/list",
            data: { studentSearch: AdminExam.currentSearchData, PageSize: AdminExam.pageSize, Page: Page },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#List").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminExam.totalPages = Math.ceil(totalCounts / AdminExam.pageSize);
                    AdminExam.InitializePagination()
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching student data:", error);
            }
        });

    },
    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');

        $('#pagination').twbsPagination({
            totalPages: AdminExam.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminExam.currentPage) {
                    AdminExam.currentPage = page;
                    AdminExam.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminExam.currentSearchData = {
            Name: $("#Name").val(),
            Status: $("#Status").val(),
            DepartmentId: $("#Type").val(),
            BranchId: $("#CourseId").val()

        };

    },
    Search: function () {
        AdminExam.SetAdvancedSearchData();
        AdminExam.Fetch(1);
    },
    Reset: function () {
        $("#Name").val("");
        $("#Status").val("");
        $("#DepartmentId").val("");
        $("#Type").val("");
        $("#CourseId").val("");
        AdminExam.currentSearchData = {};
        AdminExam.Fetch(1);
    },





}