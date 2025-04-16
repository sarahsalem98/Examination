var AdminExamQs = {
    ShowAddUpdate: function () {
        $.ajax({
            type: "POST",
            url: "/Admin/ExamQuestion/AddUpdate", 
            success: function (response) {
                $("#Loading").html(response);
                console.log(response);
                $('#detailsButton').addClass('active');
                $('#settingsButton').removeClass('active');
            },
            error: function (xhr, status, error) {
                console.error("Error loading Questions:", error);
            }
        });
       
    }
    
}