var AdminReport = {
    RenderReport: function (ReportName) {
        $.ajax({
            type: "GET",
            url: "/Admin/Report/GetReport",
            data: { ReportName: ReportName },
            success: function (response) {
                $("#Report").html(response);
                console.log("Report changed to: " + ReportName);
            },
            error: function (xhr, status, error) {
                console.error("Error Fetching Report:", error);
            }
        });
    }
};
