var Account = {

    Login: function () {
        var Email = $("#Email").val();
        var password = $("#Password").val();
        var returnUrl = $("#returnUrl").val();
       
        $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: { Email: Email, Password: password, ReturnUrl: returnUrl },
            success: function (response) {
                console.log(response);
                if (response.success) {
                    toastr.success(response.message);
                    console.log(returnUrl);
                    setTimeout(function () {
                    window.location.href = response.redirectUrl;
                    },1500)
                    console.log("Login successful:", response.success);
                } else {
                    
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error during login:", error);
                toastr.error("An error occurred while processing your request.");
            }
        });
         
    }
}