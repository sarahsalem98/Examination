var AdminStudent = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Student/list",
            data: { studentSearch: AdminStudent.currentSearchData, PageSize: AdminStudent.pageSize, Page: Page },
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
                    AdminStudent.totalPages = Math.ceil(totalCounts / AdminStudent.pageSize);
                    AdminStudent.InitializePagination()
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
            totalPages: AdminStudent.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminStudent.currentPage) {
                AdminStudent.currentPage = page;
                AdminStudent.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminStudent.currentSearchData = {
            Name: $("#Name").val(),
            Status: $("#Status").val(),
            DepartmentId: $("#DepartmentId").val(),
            BranchId: $("#BranchId").val(),
            TrackType: $("#TrackType").val(),
          
        };
       
    },
    Search: function () {
        AdminStudent.SetAdvancedSearchData();
        AdminStudent.Fetch(1);
    },
    Reset: function () {
        $("#Name").val("");
        $("#Status").val("");
        $("#DepartmentId").val("");
        $("#BranchId").val("");
        $("#TrackType").val("");
        AdminStudent.currentSearchData = {};
        AdminStudent.Fetch(1);
    },
    GetDepartmentsByBranchId: function(departmentId) {
        var branchId = $('#branchId').val();
        console.log(branchId);

        $.ajax({
            type: "GET",
            url: "/Admin/Student/GetDepartmentsByBranchId",
            data: { BranchId: branchId },
            success: function (response) {
                console.log(response);
                var departmentDropdown = $("#departmentDropdown");
                departmentDropdown.empty(); 

                departmentDropdown.append('<option selected >select department</option>');

                $.each(response.data, function (index, department) {
                    
                    departmentDropdown.append(`<option value="${department.id}">${department.name}</option>`);
                });

                if (departmentId) {
                    departmentDropdown.val(departmentId); 
                   
                }

            },
            error: function (xhr, status, error) {
                console.error("Error fetching student data:", error);
            }
        });

    },
    ShowAddUpdateModal: function (id) {
        console.log(id);
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Student/AddUpdate",
            data: { id: id },
            success: function (response) {
                console.log(response);
                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
               // AdminStudent.SetUpValidation();
            },
            error: function (xhr, status, error) {
                console.error("Error fetching student data:", error);
            }
        });
    },
    AddUpdata: function (e) {
        // $("#loader").addClass("show");
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Admin/Student/AddUpdate",
            data: $("#admin-student-form").serialize(),
            success: function (response) {
                // $("#loader").removeClass("show");
                if (response.success) {
                    AdminStudent.Fetch(AdminStudent.currentPage);
                    //toastr.success(response.message);
                    $('#addUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error adding/updating student:", error);
            }
        });
    },
    ChangeStatus: function (id, status) {
        $.ajax({
            type: "POST",
            url: "/Admin/Student/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {
                if (response.success) {
                    AdminStudent.Fetch(AdminStudent.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error changing student status:", error);
            }
        });
    } 

 }