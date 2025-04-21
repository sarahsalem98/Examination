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

    ShowAddUpdateModel: function (id) {
        console.log("id:"+id);
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Branch/AddUpdate",
            data: { id: id },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $("#AddUpdateModal").modal("show");
            },
            error: function (xhr, status, error) {
                console.error("Error fetching Branches data:", error);
            }
        });
    },//end of ShowAddUpdate

    AddUpdate: function (e) {
        e.preventDefault();

        //update/add branch data
        var branchData = {
            Id: $("#BranchId").val() ? parseInt($("#BranchId").val()) : 0,
            Name: $("#BranchName").val(),
            Location: $("#BranchLocation").val(),
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Branch/AddUpdate",
            contentType: "application/json",
            data: JSON.stringify(branchData),
            success: function (response) {
                if (response.success) {
                    AdminBranch.Fetch(AdminBranch.currentPage);
                    $('#AddUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error adding/updating branch:", error);
            }
        });
    },//end of AddUpdate

    ChangeStatus: function (id, status) {
        $.ajax({
            type: "POST",
            url: "/Admin/Branch/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {
                if (response.success) {
                    AdminBranch.Fetch(AdminBranch.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error changing branch status:", error);
            }
        })
    },//end of ChangeStatus

    

}
