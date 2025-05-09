﻿var StudentExam = {
    questions: [],
    GeneratedExamId: 0,
    currentOrder: 1,
    totalTime: 0,
    timeLeft: 0,
    ExamType: 0,
    InstructorCourseId: 0,
    loadQuestion: function (order) {
        //debugger;
        //console.log(order);
        var questionContainer = document.getElementById("question-container");
        var question = this.findQuestionByOrder(order);


        if (!question) {
            questionContainer.innerHTML = "<p>No more questions.</p>";
            return;
        }

        var questionHtml = `
            <div class="question mb-3 p-3 rounded-0" id="question-${question.qorder}">
                <h4>Question ${question.qorder}</h4>
                <p>${question.question}</p>
        `;

        var answerOptions = question.answers.split(';');
        answerOptions.forEach(function (answer, ansIndex) {
            var answerValue = answer.split(")")[0].trim();
            questionHtml += `
                <div class="form-check">
                    <input class="form-check-input" type="radio" onchange="StudentExam.addRadioButtonEventListeners(${order});"" name="q${question.qId}" id="q${question.qId}" value="${answerValue}">
                    <label class="form-check-label" for="q${question.qId}">${answer}</label>
                </div>
            `;
        });

        var isFirst = this.currentOrder === 1;
        var isLast = this.currentOrder === this.questions.length;

        questionHtml += `
        </div>
        <div class="d-flex justify-content-between mt-3">
            <button class="btn butn rounded-0" onclick="StudentExam.previousQuestion()" ${isFirst ? 'disabled' : ''}>Previous Question</button>
           <button class="btn butn rounded-0" onclick="${isLast ? 'StudentExam.submitAndClose()' : 'StudentExam.nextQuestion()'}">
        ${isLast ? 'Finish Exam' : 'Next Question'}
          </button>
        </div>
    `;

        questionContainer.innerHTML = questionHtml;
        this.updateProgressBar();
    },

    nextQuestion: async function () {
        var res = await this.submitQuestion(this.currentOrder);
        if (res == 1) {
            this.currentOrder++;
            this.loadQuestion(this.currentOrder);

        }
    },

    previousQuestion: function () {
        this.currentOrder--;
        this.loadQuestion(this.currentOrder);
    },
    findQuestionByOrder: function (order) {
        return this.questions.reduce((found, q) => {
            if (q.qorder == order) {
                found = q;
            }
            return found;
        }, null);
    },
    updateProgressBar: function () {
        var totalQuestions = this.questions.length;
        var progress = (this.currentOrder) / totalQuestions * 100;
        var progressBar = document.getElementById("progress-bar");
        progressBar.style.width = progress + "%";
        progressBar.setAttribute("aria-valuenow", progress);

    },
    submitQuestion: async function (order) {
        // debugger;
        var question = this.findQuestionByOrder(order);
        var radios = document.getElementsByName(`q${question.qId}`);

        var selectedAnswer = null;
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                selectedAnswer = radios[i].value;
                break;
            }
        }

        if (!selectedAnswer) {
            this.displayWarningMessage(order);
            return 0;
        } else {
            var data = {
                GenratedExamId: StudentExam.GeneratedExamId,
                QId: question.qId,
                StdAnswer: selectedAnswer
            };

            try {
                const response = await $.ajax({
                    type: "POST",
                    url: "/Student/Exam/SubmitAnswer",
                    data: data
                });
                console.log("Question submitted successfully:", response);
                if (response.success) {
                    return 1;
                } else {
                    return 0;
                }
                return 1;
            } catch (error) {
                console.error("Error submitting question:", error);
                return 0;
            }
        }
    },

    displayWarningMessage: function (order) {
        //debugger;
        var questionContainer = document.getElementById(`question-${order}`);
        var existingWarning = questionContainer.querySelector('.warning-message');
        if (!existingWarning) {
            var warningMessage = document.createElement('p');
            warningMessage.classList.add('warning-message', 'text-danger');
            warningMessage.classList.add('warning-message', 'mt-3');
            warningMessage.innerText = "Please select an answer before submitting the question.";
            questionContainer.appendChild(warningMessage);
        }
    },
    removeWarningMessage: function (order) {
        // debugger;
        var questionContainer = document.getElementById(`question-${order}`);
        var warningMessage = questionContainer.querySelector('.warning-message');
        if (warningMessage) {
            questionContainer.removeChild(warningMessage);
        }
    },
    addRadioButtonEventListeners: function (order) {
        //debugger;
        var question = this.findQuestionByOrder(order);
        if (!question) return;

        var radios = document.getElementsByName(`q${question.qId}`);

        radios.forEach(function (radio) {
            if (radio.checked) {
                StudentExam.removeWarningMessage(order);
            }
        });
    },
    updateTimer: async function () {
        //debugger;
        var minutes = Math.floor(StudentExam.timeLeft / 60);
        var seconds = Math.floor(StudentExam.timeLeft % 60);
        document.getElementById('time').textContent = `${minutes < 10 ? '0' + minutes : minutes}:${seconds < 10 ? '0' + seconds : seconds}`;

        if (StudentExam.timeLeft <= 0) {
            clearInterval(timer);
            await StudentExam.submitAndClose();
        } else {
            StudentExam.timeLeft--;
        }
    },
    submitAndClose: async function () {
        //debugger;
        var res = await this.submitQuestion(this.currentOrder);
        if (res == 1) {
            var response = await $.ajax({
                type: "POST",
                url: "/Student/Exam/CorrectExam",
                data: { GeneratedExamId: StudentExam.GeneratedExamId, InstructorCourseId: StudentExam.InstructorCourseId, ExamType: StudentExam.ExamType },

            });
            if (response.success) {
                toastr.success("Exam submitted successfully!");
                if (window.opener) {
                    window.opener.location.reload();
                }

                setTimeout(function () {
                    window.close(); 
                }, 2000);
            } else {
                toastr.error("An error occurred while submitting the exam.");
            }
        } 
    }



};
