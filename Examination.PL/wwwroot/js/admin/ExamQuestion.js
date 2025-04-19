var AdminExamQs = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 7,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/ExamQuestion/list",
            data: { searchMV: AdminExamQs.currentSearchData, PageSize: AdminExamQs.pageSize, Page: Page },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#ExamQList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminExamQs.totalPages = Math.ceil(totalCounts / AdminExamQs.pageSize);
                    AdminExamQs.InitializePagination()
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
            totalPages: AdminExamQs.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminExamQs.currentPage) {
                    AdminExamQs.currentPage = page;
                    AdminExamQs.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminExamQs.currentSearchData = {
            CourseId: $("#CourseId").val(),
            Status: $("#Status").val(),
            Type: $("#ExamType").val(),
            QuestionType: $("#QuestionType").val()

        };

    },
    Search: function () {
        AdminExamQs.SetAdvancedSearchData();
        AdminExamQs.Fetch(1);
    },
    Reset: function () {
        $("#CourseId").val("");
        $("#Status").val("");
        $("#ExamType").val("");
        $("#QuestionType").val("");
        AdminExamQs.currentSearchData = {};
        AdminExamQs.Fetch(1);
    },
    
}