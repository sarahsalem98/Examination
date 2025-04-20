var AdminBranch = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Branch/list",
            data: { BranchSearch: AdminBranch.currentSearchData, PageSize: AdminBranch.pageSize, Page: Page },

            success: function (response) {
                $("#loader").removeClass("show");
                $("#BranchList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter("#TotalCount");
                    const totalcount = parseInt($totalInput.val());

                    if (totalcount === 0) {
                        $("#pagination").hide();
                        return;
                    }
                    AdminBranch.totalPages = Math.ceil(totalcount / AdminBranch.pageSize);
                    AdminBranch.InitializePagination();

                };

            },
            error: function (xhr, status, error) {
                console.error("Error fetching Branches data:", error);
            }
        });
    },//end of fetch

    InitializePagination: function () {
        console.log(AdminBranch.totalPages);
        $('#pagination').twbsPagination('destroy');
        $('#pagination').twbsPagination({
            totalPages: AdminBranch.totalPages,
            visiblePages: 5,
            startPage: 1,
            onPageClick: function (event, page) {
                if (page != AdminBranch.currentPage) {
                    AdminBranch.currentPage = page;
                    AdminBranch.Fetch(page);
                }
            }
        });

    },//end of InitializePagination

    SetAdvancedSearchData: function () {
        AdminBranch.currentSearchData = {
            Name: $("#Name").val(),
            Location: $("#Location").val(),
            Status: $("#Status").val(),
        };
    },//end of AdvancedSearchData

    Search: function () {
        console.log("Search clicked");
        AdminBranch.SetAdvancedSearchData();
        AdminBranch.Fetch(1);
        console.log(AdminBranch.currentSearchData);
    },//end of search

    Reset: function () {
        $("#Name").val("");
        $("#Location").val("");
        $("#Status").val("");
        AdminBranch.currentSearchData = {};
        AdminBranch.Fetch(1);
    },//end of Reset
}
