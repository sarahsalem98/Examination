
var AdminExam = {

    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Exam/list",
            data: { examSearch: AdminExam.currentSearchData, PageSize: AdminExam.pageSize, Page: Page },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#List").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminExam.totalPages = Math.ceil(totalCounts / AdminExam.pageSize);
                    AdminExam.InitializePagination()
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
            totalPages: AdminExam.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminExam.currentPage) {
                    AdminExam.currentPage = page;
                    AdminExam.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminExam.currentSearchData = {
            Name: $("#Name").val(),
            Status: $("#Status").val(),
            Type: $("#Type").val(),
            CourseId: $("#CourseId").val()

        };

    },
    Search: function () {
        AdminExam.SetAdvancedSearchData();
        AdminExam.Fetch(1);
    },
    Reset: function () {
        $("#Name").val("");
        $("#Status").val("");
        $("#DepartmentId").val("");
        $("#Type").val("");
        $("#CourseId").val("");
        AdminExam.currentSearchData = {};
        AdminExam.Fetch(1);
    },

    ShowAddUpdateModal: function (id) {
        console.log(id);
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Exam/AddUpdate",
            data: { id: id },
            success: function (response) {

                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error fetching Exam:", error);
            }
        });
    },

    AddUpdate: function (e) {
        e.preventDefault();
        $.ajax({
            type:"POST",
            url: "/Admin/Exam/AddUpdate",
            data: $("#admin-exam-form").serialize(),
            success: function (response) {
                
                if (response.success) {
                    AdminExam.Fetch(AdminExam.currentPage);
               
                    $('#addUpdateModal').modal('hide');
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error adding/updating Exam:", error);
            }


        })
    },
    ChangeStatus: function (id, status) {
        $.ajax({
            type: "PUT",
            url: "/Admin/Exam/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {

                if (response.success) {
                    AdminExam.Fetch(AdminExam.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error Changing Exam Status:", error);
            }
        })
    }

}