var Topics = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 10,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/topic/list",
            data: { topicSearch: Topics.currentSearchData, PageSize: Topics.pageSize, Page: Page },
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
                    Topics.totalPages = Math.ceil(totalCounts / Topics.pageSize);
                    Topics.InitializePagination()
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
            totalPages: Topics.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != Topics.currentPage) {
                    Topics.currentPage = page;
                    Topics.Fetch(page);
                }
            }
        });
    },
    ShowAddUpdateModal: function (id) {
        if (id > 0) {
            $('#addUpdateModal .modal-title').text('Edit Topic');
        } else {
            $('#addUpdateModal .modal-title').text('Create New Topic');
        }

        $("#loader").addClass("show");
        $.ajax({
            type: "GET",
            url: "/Admin/topic/AddUpdate",
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
            url: "/Admin/topic/AddUpdate",
            data: $("#admin-Course-form").serialize(),
            success: function (response) {
                // $("#loader").removeClass("show");
                if (response.success) {
                    Topics.Fetch(Topics.currentPage);
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
        Topics.currentSearchData = {
            Name: $("#Name").val(),
        };
        console.log("Values Are " + Topics.currentSearchData.Name)

    },
    Search: function () {
        Topics.SetAdvancedSearchData();
        Topics.Fetch(1);
    },
    Reset: function () {
        $("#Name").val("");
        Topics.currentSearchData = {};
        Topics.Fetch(1);
    },
    DeleteTopic: function (id) {
        $.ajax({
            type: "POST",
            url: "/Admin/topic/DeleteTopic",
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    Topics.Fetch(Topics.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error While Deleting Topic:", error);
            }
        });
    }


}