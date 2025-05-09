﻿
var AdminDepartment = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Department/List",
            data: {
                departmentSearch: AdminDepartment.currentSearchData,
                PageSize: AdminDepartment.pageSize,
                Page: Page
            },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#DepartmentList").html(response);
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
                if (page !== AdminDepartment.currentPage) {
                    AdminDepartment.currentPage = page;
                    AdminDepartment.Fetch(page);
                }
            }
        });
    },

    SetAdvancedSearchData: function () {
        AdminDepartment.currentSearchData = {
            Name: $("#Name").val(),
            BranchId: $("#BranchId").val(),
            Status: $("#Status").val()
        };
    },

    Search: function () {
        AdminDepartment.SetAdvancedSearchData();
        AdminDepartment.Fetch(1);
    },

    Reset: function () {
        $("#Name").val("");
        $("#BranchId").val("");
        $("#Status").val("");
        AdminDepartment.currentSearchData = {};
        AdminDepartment.Fetch(1);
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
                console.error("Error loading department modal:", error);
            }
        });
    },

    AddUpdate: function (e) {
        e.preventDefault();
        debugger;
        // Manually collect department data
        var departmentData = {
            Id: $("#Id").val(),
            Name: $("#NameDp").val(),
            Capacity: $("#Capacity").val(),
            Description: $("#Description").val(),
            // Add other department properties here as needed
        };

        // Collect selected branches and create SelectedBranches objects
        var selectedBranches = [];
        $('input[type="checkbox"]:checked').each(function () {
            var branchId = $(this).val();
            var branchName = $('#name_' + branchId).val();
            var canBeRemoved = $('#canBeRemoved_' + branchId).val() === "true";
            var status = $('#status_' + branchId).val();

            // Create the SelectedBranches object for each selected branch
            var selectedBranch = {
                Id: branchId,
                Name: branchName,
                CanBeRemoved: canBeRemoved,
                status: status
            };

            selectedBranches.push(selectedBranch);
        });

        // Add the selectedBranches to the department data
        departmentData.selectedBranches = selectedBranches;
       // $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Department/AddUpdate",
            data: departmentData,
            success: function (response) {  
             //   $("#loader").removeClass("show");
                if (response.success) {
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
        $.ajax({
            type: "POST",
            url: "/Admin/Department/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {
                if (response.success) {
                    AdminDepartment.Fetch(AdminDepartment.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error changing department status:", error);
            }
        });
    }
};
