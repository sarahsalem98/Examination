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
            data: {
                InstructorSearch: AdminInstructor.currentSearchData,
                PageSize: AdminInstructor.pageSize, Page: Page
            },

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
        console.log("Search clicked");
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
            url: "/Admin/instructor/AddUpdate",
            data: { id: id },
            success: function (response) {

                $("#loader").removeClass("show");
                $("#addUpdateModalView").html(response);
                $('#addUpdateModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error("Error fetching Instructor data:", error);
            }
        });
    },

    AddUpdate: function (e) {
        e.preventDefault();
        var DepartmentBranchIdSelectedId = 0;
        var instructorData = {
            User: {
                Id: $("#UserId").val() ? parseInt($("#UserId").val()) : 0,
                FirstName: $("#firstName").val(),
                LastName: $("#lastName").val(),
                Age: parseInt($("#age").val()),
                Email: $("#email").val(),
                Phone: $("#phone").val()
            },
            IsExternal: $("#IsExternal").val() === "true" ? true : false,
            UserId: $("#UserId").val() ? parseInt($("#UserId").val()) : 0,
            Id: $("#Id").val() ? parseInt($("#Id").val()) : 0,
            InstructorCourses: []
        };
        $("#InstructorAssignmentTable tbody tr").each(function () {
            var instructorCourseBranchId = $(this).find(".instructorCourseBranchId").val();
            var branchId = $(this).find(".branch").val();
            var departmentId = $(this).find(".department").val();
            var courseId = $(this).find(".course").val();
            if (branchId && departmentId && courseId) {
                instructorData.InstructorCourses.push({
                    CourseId: parseInt(courseId),
                    DepartmentBranch: {
                        BranchId: parseInt(branchId),
                        DepartmentId: parseInt(departmentId)
                    },
                    InstructorId: instructorData.Id,
                    Id: parseInt(instructorCourseBranchId)

                });
            }
        });
        debugger;
        $.ajax({
            type: "POST",
            url: "/Admin/instructor/AddUpdate",
            contentType: "application/json",
            data: JSON.stringify(instructorData),
            success: function (response) {

                if (response.success) {

                    AdminInstructor.Fetch(AdminInstructor.currentPage);
                    $('#addUpdateModal').modal('hide');

                    toastr.success(response.message);

                } else {
                    console.log("Repsonse" + JSON.stringify(response.data));
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {

                console.error("Error adding/updating instructor:", error);
            }
        })
    },
    ChangeStatus: function (id, status) {
        $.ajax({
            type: "PUT",
            url: "/Admin/instructor/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {
                if (response.success) {
                    AdminInstructor.Fetch(AdminInstructor.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            }
            , error: function (xhr, status, error) {
                console.error("Error changing student status:", error);
            }
        })
    },
    HandleDropDownDepartments: function (e) {
      
            var branchId = $(e).val();
            var row = $(e).closest("tr");
            var dept = row.find(".department");
            var course = row.find(".course");

            dept.empty().append('<option value="">Choose...</option>');
            course.empty().append('<option value="">Choose...</option>');

            if (branchId && branchId !== "null") {
                AdminInstructor.GetDepartmentsByBranchId(branchId, dept);
            }
       

    },
    HandelDropDownCourses: function (e) {
     
            var deptId = $(e).val();
            var row = $(e).closest("tr");
            var course = row.find(".course");
            course.empty().append('<option value="">Choose...</option>');

            if (deptId && deptId !== "null") {
                console.log("Fetching courses...");
                AdminInstructor.GetCoursesByDepartmentID(deptId, course);
            }
    
    },



    GetDepartmentsByBranchId: function (branchId, dropdown) {
        $.ajax({
            type: "GET",
            url: "/Admin/instructor/GetDepartmentsByBranchId",
            data: { BranchId: branchId },
            success: function (response) {
                if (response.success) {
                    $.each(response.data, function (index, department) {
                        dropdown.append(`<option value="${department.id}">${department.name}</option>`);
                    });
                }
            },
            error: function () {
                console.error("Error loading departments");
            }
        });
    },
    GetCoursesByDepartmentID: function (deptId, dropdown) {
        $.ajax({
            type: "GET",
            url: "/Admin/Instructor/GetCoursesByDepartmenID",
            data: { DepartmentId: deptId },
            success: function (response) {

                if (response.success) {
                    $.each(response.data, function (index, course) {
                        console.log("Course:", course);
                        dropdown.append(`<option value="${course.id}">${course.name}</option>`);
                    });
                } else {
                    console.warn("No success in response");
                }
            },
            error: function () {
                console.error("Error loading courses");
            }
        });
    },

    HandleAssignRow: function () {
        $("#assignBtn").click(function () {
            debugger;
            var rowCount = $("#InstructorAssignmentTable tbody tr").length;
            var selectedBranchIds = [];
            $("#InstructorAssignmentTable tbody tr .branch").each(function () {
                var val = $(this).val();
                if (val) selectedBranchIds.push(val);
            });
            var availableBranches = allBranches.filter(b => !selectedBranchIds.includes(b.id.toString()));
            var branchOptions = '<option value="">Choose...</option>';
            availableBranches.forEach(b => {
                branchOptions += `<option value="${b.id}">${b.name}</option>`;
            });
        
            var newRow = `

    <tr>
        <td>
            <select class="form-select branch" name="instructorCourses[${rowCount}].branchId" onchange="AdminInstructor.HandleDropDownDepartments(this)">
                ${branchOptions}
            </select>
        </td>
        <td>
            <select class="form-select department" name="instructorCourses[${rowCount}].departmentId" onchange="AdminInstructor.HandelDropDownCourses(this)">
                  <option value="">Choose...</option>
            </select>
        </td>
        <td>
            <select class="form-select course" name="instructorCourses[${rowCount}].courseId">
                 <option value="">Choose...</option>
            </select>
        </td>
        <td>
            <a href="javascriot:void(0);" class="removeRow" onclick="AdminInstructor.HandleDeleteRow(this)">
                <i class="bi bi-x-circle text-danger"></i>
            </a>
        </td>
    </tr>
`;
            $("#InstructorAssignmentTable tbody").append(newRow);
        });
    },

    HandleDeleteRow: function (e) {
        debugger;
        console.log("777");
        var data = $(e).closest("tr");
        data.remove();

    }
};

