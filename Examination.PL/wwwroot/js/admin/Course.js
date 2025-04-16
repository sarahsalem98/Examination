var Courses = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/course/list",
            data: { courseSearch: Courses.currentSearchData, PageSize: Courses.pageSize, Page: Page },
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
                console.error("Error fetching Course data:", error);
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
    },
    ShowAddUpdateModal: function (id) {
        if (id > 0) {
            $('#addUpdateModal .modal-title').text('Edit Course');
        } else {
            $('#addUpdateModal .modal-title').text('Create New Course');
        }

        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Course/AddUpdate",
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
            url: "/Admin/course/AddUpdate",
            data: $("#admin-Course-form").serialize(),
            success: function (response) {
                // $("#loader").removeClass("show");
                if (response.success) {
                    Courses.Fetch(Courses.currentPage);
                    //toastr.success(response.message);
                    $('#addUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error adding/updating Course:", error);
            }
        });

    },
    SetAdvancedSearchData: function () {
        Courses.currentSearchData = {
            Name: $("#Name").val(),
            Status: $("#Status").val(),
            DepartmentId: $("#DepartmentId").val(),
            BranchId: $("#BranchId").val(),
            TrackType: $("#TrackType").val(),

        };
        console.log("Values Are " + Courses.currentSearchData)

    },
    Search: function () {
        Courses.SetAdvancedSearchData();
        Courses.Fetch(1);
    },
    Reset: function () {
        $("#Name").val("");
        $("#Status").val("");
        $("#DepartmentId").val("");
        $("#BranchId").val("");
        $("#TrackType").val("");
        Courses.currentSearchData = {};
        Courses.Fetch(1);
    },


}


