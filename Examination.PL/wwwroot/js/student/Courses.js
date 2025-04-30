var StudentCourses = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 8,
    Fetch: function (Page = 1) {
        //debugger;
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Student/Courses/list",
            data: { name: StudentCourses.currentSearchData.Name, PageSize: StudentCourses.pageSize, Page: Page },

            success: function (response) {
                $("#loader").removeClass("show");
                $("#CoursesCards").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter("#TotalCount");
                    const totalcount = parseInt($totalInput.val());

                    if (totalcount === 0) {
                        $("#pagination").hide();
                        return;
                    }
                    StudentCourses.totalPages = Math.ceil(totalcount / StudentCourses.pageSize);
                    StudentCourses.InitializePagination();

                };

            },
            error: function (xhr, status, error) {
                console.error("Error fetching Branches data:", error);
            }
        });
    },//end of fetch

    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');
        $('#pagination').twbsPagination({
            totalPages: StudentCourses.totalPages,
            visiblePages: 5,
            startPage: 1,
            onPageClick: function (event, page) {
                if (page != StudentCourses.currentPage) {
                    StudentCourses.currentPage = page;
                    StudentCourses.Fetch(page);
                }
            }
        });

    },//end of InitializePagination

    SetAdvancedSearchData: function () {
        StudentCourses.currentSearchData = {
            Name: $("#Name").val(),
        };
    },//end of AdvancedSearchData

    Search: function () {
        console.log("Search clicked");
        StudentCourses.SetAdvancedSearchData();
        StudentCourses.Fetch(1);
        console.log(StudentCourses.currentSearchData);
    },//end of search

    Reset: function () {
        $("#Name").val("");
        StudentCourses.currentSearchData = {};
        StudentCourses.Fetch(1);
    },//end of Reset

}
