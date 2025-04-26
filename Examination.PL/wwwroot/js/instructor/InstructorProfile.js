function openPopup() {
    document.getElementById("popupOverlay").style.display = "block";
    document.getElementById("popupForm").style.display = "block";
}

function closePopup() {
    document.getElementById("popupOverlay").style.display = "none";
    document.getElementById("popupForm").style.display = "none";
}

function togglePassword(inputId, iconWrapper) {
    const input = document.getElementById(inputId);
    const icon = iconWrapper.querySelector("i");

    if (input.type === "password") {
        input.type = "text";
        icon.classList.remove("bi-eye-slash");
        icon.classList.add("bi-eye");
    } else {
        input.type = "password";
        icon.classList.remove("bi-eye");
        icon.classList.add("bi-eye-slash");
    }
}

$(document).ready(function () {
    $('#update-password-form').submit(function (e) {
        e.preventDefault();

        const passwordData = {
            UserId: $('#userId').val(),
            CurrentPassword: $('#currentPassword').val(),
            NewPassword: $('#newPassword').val(),
            ConfirmPassword: $('#confirmPassword').val()
        };

        $.ajax({
            url: '/Instructor/Profile/UpdatePassword',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(passwordData),
            success: function (res) {
                if (res.success) {
                    toastr.success("Password updated successfully!");
                    closePopup();
                    $('#currentPassword, #newPassword, #confirmPassword').val('');
                } else {
                    toastr.error(res.message || "Update failed.");
                }
            },
            error: function () {
                toastr.error("An error occurred.");
            }
        });
    });
});
