var AdminInstructor = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/Instructor/list",
            data: { InstructorSearch: AdminInstructor.currentSearchData, PageSize: AdminInstructor.pageSize, Page: Page },
           
            success: function (response) {
                $("#loader").removeClass("show");
                $("#instructorList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminInstructor.totalPages = Math.ceil(totalCounts / AdminInstructor.pageSize);
                    AdminInstructor.InitializePagination()
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching instructor data:", error);
            }
        });

    },
    InitializePagination: function () {
        $('#pagination').twbsPagination('destroy');

        $('#pagination').twbsPagination({
            totalPages: AdminInstructor.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminInstructor.currentPage) {
                    AdminInstructor.currentPage = page;
                    AdminInstructor.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminInstructor.currentSearchData = {
            Name: $("#Name").val(),
            BranchId: $("#BranchId").val(),
            DepartmentId: $("#DepartmentId").val(),
            IsExternal: $("#IsExternal").val(),
            Status: $("#Status").val()
        }
    },
    Search: function () {
        console.log("Search clicked!");

        AdminInstructor.SetAdvancedSearchData();
        AdminInstructor.Fetch(1);
        console.log(AdminInstructor.currentSearchData);

    },
    Reset: function () {
        $("#Name").val("");
        $("#DepartmentId").val("");
        $("#BranchId").val("");
        $("#IsExternal").val("");
        $("#Status").val("");
        AdminInstructor.currentSearchData = {};
        AdminInstructor.Fetch(1);
    },
    ShowAddUpdateModal: function (id) {
        console.log(id);
        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/Instructor/AddUpdate",
            data: { id: id },
            success: function (response) {
                console.log(response);
                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
              
            },
            error: function (xhr, status, error) {
                console.error("Error fetching instructor data:", error);
            }
        });
    },
}