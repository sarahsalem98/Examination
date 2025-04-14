var Courses = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 1,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/course/list",
            data: { studentSearch: Courses.currentSearchData, PageSize: Courses.pageSize, Page: Page },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#studentList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    Courses.totalPages = Math.ceil(totalCounts / Courses.pageSize);
                    Courses.InitializePagination()
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
            totalPages: Courses.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != Courses.currentPage) {
                    Courses.currentPage = page;
                    Courses.Fetch(page);
                }
            }
        });
    }

}