var AdminExamQs = {
    currentSearchData: {},
    totalPages: 1,
    currentPage: 1,
    pageSize: 7,
    optionCount: 0,

    Fetch: function (Page = 1) {
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/ExamQuestion/list",
            data: { searchMV: AdminExamQs.currentSearchData, PageSize: AdminExamQs.pageSize, Page: Page },
            success: function (response) {
                $("#loader").removeClass("show");
                $("#ExamQList").html(response);
                if (Page == 1) {
                    const $html = $($.parseHTML(response));
                    const $totalInput = $html.filter('#totalCount');
                    const totalCounts = parseInt($totalInput.val());
                    if (totalCounts === 0) {
                        $('#pagination').hide();
                        return;
                    }
                    AdminExamQs.totalPages = Math.ceil(totalCounts / AdminExamQs.pageSize);
                    AdminExamQs.InitializePagination()
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
            totalPages: AdminExamQs.totalPages,
            visiblePages: 5,
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                if (page != AdminExamQs.currentPage) {
                    AdminExamQs.currentPage = page;
                    AdminExamQs.Fetch(page);
                }
            }
        });
    },
    SetAdvancedSearchData: function () {
        AdminExamQs.currentSearchData = {
            CourseId: $("#CourseId").val(),
            Status: $("#Status").val(),
            Type: $("#ExamType").val(),
            QuestionType: $("#QuestionType").val()

        };

    },
    Search: function () {
        AdminExamQs.SetAdvancedSearchData();
        AdminExamQs.Fetch(1);
    },
    ChangeStatus: function (id, status) {
        $.ajax({
            type: "POST",
            url: "/Admin/ExamQuestion/ChangeStatus",
            data: { id: id, status: status },
            success: function (response) {

                if (response.success) {
                    AdminExamQs.Fetch(AdminExamQs.currentPage);
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error Changing ExamQustion Status:", error);
            }
        })
    },
    Reset: function () {
        $("#CourseId").val("");
        $("#Status").val("");
        $("#ExamType").val("");
        $("#QuestionType").val("");
        AdminExamQs.currentSearchData = {};
        AdminExamQs.Fetch(1);
    },
    toggleOptionInputs: function () {
        var option = $("#QuestionType").val();
        var optionContainerdiv = $("#option-container");
        var addOptionBtn = $(".add-option");

        optionContainerdiv.empty();
        AdminExamQs.optionCount = 0;

        if (option === "MCQ") {
            addOptionBtn.show();
            AdminExamQs.addOption();
            AdminExamQs.addOption();
        } else if (option === "TF") {
            addOptionBtn.hide();
            optionContainerdiv.append(`
            <div class="options">
                <div class="option-box">
                    <label>A</label>
                    <input type="text" name="option" value="True" readonly />
                    <label><input type="radio" name="correctAnswer" value="True" required /> Correct</label>
                </div>
                </div>
                <div class="options">
                <div class="option-box">
                    <label>B</label>
                    <input type="text" name="option" value="False" readonly />
                    <label><input type="radio" name="correctAnswer" value="False" required /> Correct</label>
                </div>
            </div>
        `);
        } else {
            console.log("666");
            addOptionBtn.hide();
        }
    },

    addOption: function (value = '', isCorrect = false) {
        const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const label = letters[AdminExamQs.optionCount];

        const html = `
    <div class="options option-item mb-2">
        <div class="option-box">
            <div class="d-flex justify-content-between align-items-center mb-1">
                <label class="option-label mx-3">${label}</label>
                <button type="button" class="btn btn-sm btn-danger remove-option rounded-0" onclick="AdminExamQs.removeOption(this)">×</button>
            </div>
            <input type="text" name="option" class="form-control mb-2" value="${value}" required />
            <label class="me-2">
                <input type="radio" name="correctAnswer" value="${AdminExamQs.optionCount}" ${isCorrect ? 'checked' : ''} required />
                Correct
            </label>
        </div>
    </div>
    `;

        $("#option-container").append(html);
        AdminExamQs.optionCount++;
    },

    removeOption: function (button) {
        const container = $("#option-container");
        const optionItems = container.find(".option-item");

        if (optionItems.length <= 2) {
            alert("At least two options are required.");
            return;
        }

        $(button).closest(".option-item").remove();
        AdminExamQs.optionCount--;

        // Re-label and re-index the options
        container.find(".option-item").each(function (index) {
            const letter = String.fromCharCode(65 + index);
            $(this).find(".option-label").text(letter);
            $(this).find("input[type='radio']").val(index);
        });
    },

    collectOptions: function () {
        var option = $("#QuestionType").val();
        const optionInputs = document.querySelectorAll('.option-item input[type="text"]');
        const radios = document.querySelectorAll('input[name="correctAnswer"]');
        let formattedOptions = '';
        let rightAnswerLabel = '';
        const letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

        if (option === "MCQ") {
            optionInputs.forEach((input, index) => {
                const letter = letters[index];
                const value = input.value.trim();
                if (value) {
                    formattedOptions += `${letter}) ${value}; `;
                    if (radios[index].checked) {
                        rightAnswerLabel = letter;
                    }
                }
            });
        } else if (option === "TF") {
            formattedOptions = `A) True; B) False`;
            const selected = document.querySelector('input[name="correctAnswer"]:checked');
            console.log(selected.value);
            if (selected) {
                rightAnswerLabel = selected.value === "True" ? "A" : "B";
            }
        }

        formattedOptions = formattedOptions.trim().replace(/;$/, '');

        console.log("Formatted Options:", formattedOptions);
        console.log("Right Answer Label:", rightAnswerLabel);

        return {
            Answers: formattedOptions,
            RightAnswer: rightAnswerLabel
        };
    },
    Save: function (e) {
        e.preventDefault();
        $.validator.unobtrusive.parse("#questionForm");

        if (!$("#questionForm").valid()) {
            // This triggers the error messages to appear
            return;
        }
       var options= AdminExamQs.collectOptions();
        var data = {
            Id:  $("#Id").val(),
            ExamId: $("#ExamId").val(),
            QuestionType: $("#QuestionType").val(),
            Question: $("#Question").val(),
            Degree: $("#Degree").val(),
            Answers: options.Answers,
            RightAnswer: options.RightAnswer
        };
        $("#loader").addClass("show");
        $.ajax({
            type: "POST",
            url: "/Admin/ExamQuestion/Details",
            data: { model:data},
            success: function (response) {
                 $("#loader").removeClass("show");
                if (response.success) {
                    console.log(response);
                    toastr.success(response.message);
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 1500)
                } else {
                    toastr.error(response.message);
                }
               
            },
            error: function (xhr, status, error) {
                console.error("Error fetching student data:", error);
            }
        });

    }






}

