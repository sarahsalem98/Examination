function openPopup() {
    document.getElementById("popupOverlay").style.display = "block";
    document.getElementById("popupForm").style.display = "block";
}

function closePopup() {
    document.getElementById("popupOverlay").style.display = "none";
    document.getElementById("popupForm").style.display = "none";
}


$(document).ready(function () {
    $('#update-profile-form').submit(function (e) {
        e.preventDefault();

        var studentData = {
            Id: $('#studentId').val(),
            UserId: $('#userId').val(),
            DepartmentId: $('#departmentId').val(),
            BranchId: $('#branchId').val(),
            TrackType: $('#trackType').val(),
            EnrollmentDate: $('#enrollmentDate').val(),
            User: {
                FirstName: $('#firstName').val(),
                LastName: $('#lastName').val(),
                Email: $('#email').val(),
                Phone: $('#phone').val()
            }
        };

        console.log("Data being sent:", studentData);
        console.log("First Name:", $('#firstName').val());
        console.log("Last Name:", $('#lastName').val());
        console.log("Email:", $('#email').val());
        console.log("Phone:", $('#phone').val());
        console.log("Student ID:", $('#studentId').val());
        console.log("User ID:", $('#userId').val());

        $.ajax({
            url: '/Student/Profile/UpdateProfile',
            type: 'POST',
            contentType: 'application/json', // Tell ASP.NET this is JSON
            dataType: 'json',                // Expect JSON response
            data: JSON.stringify(studentData),
            success: function (res) {
                if (res.success) {
                    $('#studentName').text($('#firstName').val() + ' ' + $('#lastName').val());
                    $('#emailDisplay').text($('#email').val());
                    $('#phoneDisplay').text($('#phone').val());

                    toastr.success("Profile updated successfully!");
                    closePopup();
                    setTimeout(() => location.reload(), 1000);
                } else {
                    toastr.error("Update failed. Please try again.");
                }
            },
            error: function (xhr, status, error) {
                console.error("XHR:", xhr.responseText);
                toastr.error("An error occurred while updating the profile.");
            }

        });
    });
});


