var StdPrevExam = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $.ajax({
            type: "POST",
            url: "/Student/PreviousExam/List", 
            data: {
                search: StdPrevExam.currentSearchData,
                PageSize: StdPrevExam.pageSize,
                Page: Page
            },
            success: function (response) {
                $("#ExamList").html(response);

                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter("#TotalCount");
                    const totalcount = parseInt($totalInput.val());

                    if (totalcount === 0) {
                        $("#pagination").hide();
                        return;
                    }
                    StdPrevExam.totalPages = Math.ceil(totalcount / StdPrevExam.pageSize);
                    StdPrevExam.InitializePagination();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching previous exams data:", error);
            }
        });
    }, // end of Fetch

    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');
        $('#pagination').twbsPagination({
            totalPages: StdPrevExam.totalPages,
            visiblePages: 5,
            startPage: 1,
            onPageClick: function (event, page) {
                if (page != StdPrevExam.currentPage) {
                    StdPrevExam.currentPage = page;
                    StdPrevExam.Fetch(page);
                }
            }
        });
    }, // end of InitializePagination

    SetAdvancedSearchData: function () {
        StdPrevExam.currentSearchData = {
            ExamType: $("#ExamType").val(),       
            Name: $("#Name").val()                
        };
    }, // end of SetAdvancedSearchData

    Search: function () {
        console.log("Search clicked");
        StdPrevExam.SetAdvancedSearchData();
        StdPrevExam.Fetch(1);
        console.log(StdPrevExam.currentSearchData);
    }, // end of Search

    Reset: function () {
        $("#ExamType").val("");
        $("#Name").val("");
        StdPrevExam.currentSearchData = {};
        StdPrevExam.Fetch(1);
    } // end of Reset
};
