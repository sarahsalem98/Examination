var InstructorStudent= {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,
    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Instructor/Student/List",
            data: { studentSearch: InstructorStudent.currentSearchData, PageSize: InstructorStudent.pageSize, Page: Page },

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
                    InstructorStudent.totalPages = Math.ceil(totalCounts / InstructorStudent.pageSize);
                    InstructorStudent.InitializePagination()
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
    totalPages: InstructorStudent.totalPages,
    visiblePages: 5,
    startPage: 1,
    initiateStartPageClick: false,
    onPageClick: function (event, page) {
        if (page != InstructorStudent.currentPage) {
            InstructorStudent.currentPage = page;
            InstructorStudent.Fetch(page);
        }
    }
});
    },
SetAdvancedSearchData: function () {
    InstructorStudent.currentSearchData = {
        Name: $("#Name").val(),
        BranchId: $("#BranchId").val(),
        DepartmentId: $("#DepartmentId").val(),
        TrackType: $("#TrackType").val(),
        courseId: $("#Course").val()
    }
},
Search: function () {
 
    InstructorStudent.SetAdvancedSearchData();
    InstructorStudent.Fetch(1);
    console.log(InstructorStudent.currentSearchData);

},
Reset: function () {
    $("#Name").val("");
    $("#DepartmentId").val("");
    $("#BranchId").val("");
    $("#TrackType").val("");
    $("#Course").val("");
    InstructorStudent.currentSearchData = {};
    InstructorStudent.Fetch(1);
},
ShowStudentDetailsModal: function (id) {
    console.log(id);
    $("#loader").addClass("show");
    $.ajax({
        type: "GET",
        url: "/Instructor/Student/ShowStudentDetails",
        data: { id: id },
        success: function (response) {

            $("#loader").removeClass("show");
            $("#StudentDetailsModalView").html(response);
            $('#StudentDetailsModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error("Error fetching Instructor data:", error);
        }
    });
}

}