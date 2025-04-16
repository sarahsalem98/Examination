



//Fetch: function (Page = 1) {
//    $("#loader").addClass("show");
//    $.ajax({
//        type: "POST",
//        url: "/Admin/Department/List",
//        data: {
//            search: AdminDepartment.currentSearchData,
//            PageSize: AdminDepartment.pageSize,
//            Page: Page
//        },
//        success: function (response) {
//            $("#loader").removeClass("show");
//            $("#departmentList").html(response);
//            if (Page === 1) {
//                const $html = $($.parseHTML(response));
//                const $totalInput = $html.filter('#totalCount');
//                const totalCounts = parseInt($totalInput.val());
//                if (totalCounts === 0) {
//                    $('#pagination').hide();
//                    return;
//                }
//                AdminDepartment.totalPages = Math.ceil(totalCounts / AdminDepartment.pageSize);
//                AdminDepartment.InitializePagination();
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error("Error fetching department data:", error);
//        }
//    });
//},

//InitializePagination: function () {
//    $('#pagination').twbsPagination('destroy');

//    $('#pagination').twbsPagination({
//        totalPages: AdminDepartment.totalPages,
//        visiblePages: 5,
//        startPage: 1,
//        initiateStartPageClick: false,
//        onPageClick: function (event, page) {
//            if (page !== AdminDepartment.currentPage) {
//                AdminDepartment.currentPage = page;
//                AdminDepartment.Fetch(page);
//            }
//        }
//    });
//},

//SetAdvancedSearchData: function () {
//    AdminDepartment.currentSearchData = {
//        Name: $("#Name").val(),
//        Status: $("#Status").val(),
//        BranchId: $("#BranchId").val()
//    };
//},

//Search: function () {
//    AdminDepartment.SetAdvancedSearchData();
//    AdminDepartment.Fetch(1);
//},

//Reset: function () {
//    $("#Name").val("");
//    $("#Status").val("");
//    $("#BranchId").val("");
//    AdminDepartment.currentSearchData = {};
//    AdminDepartment.Fetch(1);
//},



var AdminDepartment = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Department/List", // ✅ correct controller
            data: {
                departmentSearch: AdminDepartment.currentSearchData, // ✅ this matches your C# action
                PageSize: AdminDepartment.pageSize,
                Page: Page
            },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#DepartmentList").html(response); // ✅ correct div
                if (Page === 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminDepartment.totalPages = Math.ceil(totalCounts / AdminDepartment.pageSize);
                    AdminDepartment.InitializePagination();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching department data:", error);
                $("#loader").removeClass("show");
            }
        });
    },

    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');

        $('#pagination').twbsPagination({
            totalPages: AdminDepartment.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminDepartment.currentPage) {
                    AdminDepartment.currentPage = page;
                    AdminDepartment.Fetch(page);
                }
            }
        });
    },



















    ShowAddUpdateModal: function (id = 0) {
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Department/AddUpdate",
            data: { id: id },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error loading department form:", error);
            }
        });
    },

    AddUpdate: function (e) {
        e.preventDefault(); // Prevent default form submission
        $("#loader").addClass("show");

        $.ajax({
            type: "POST",
            url: "/Admin/Department/AddUpdate",
            data: $("#admin-department-form").serialize(),
            success: function (response) {
                $("#loader").removeClass("show");
                if (response.success) {
                    // Refresh data and close modal on success
                    AdminDepartment.Fetch(AdminDepartment.currentPage);
                    $('#addUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error saving department:", error);
                $("#loader").removeClass("show");
            }
        });
    },


    ChangeStatus: function (id, status) {
        // Confirm with user
        if (!confirm("Are you sure you want to change the status?")) {
            return;
        }

        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Department/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {
                $("#loader").removeClass("show");
                if (response.success) {
                    // Refresh current page after status change
                    AdminDepartment.Fetch(AdminDepartment.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error changing department status:", error);
                $("#loader").removeClass("show");
            }
        });
    }
};
