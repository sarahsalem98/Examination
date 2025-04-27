var StudentExam = {
    questions: [],
    currentOrder: 1,
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
            var answerValue = answer.split(")")[0];
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
            <button class="btn butn rounded-0" onclick="StudentExam.nextQuestion()" ${isLast ? 'disabled' : ''}>Next Question</button>
        </div>
    `;

        questionContainer.innerHTML = questionHtml;
        this.updateProgressBar();
    },

    nextQuestion: function () {
        var res = this.submitQuestion(this.currentOrder);
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
    submitQuestion: function (order) {
        debugger;
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
        }
        return 1;


    },
    displayWarningMessage: function (order) {
        debugger;
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
        debugger;
        var questionContainer = document.getElementById(`question-${order}`);
        var warningMessage = questionContainer.querySelector('.warning-message');
        if (warningMessage) {
            questionContainer.removeChild(warningMessage);
        }
    },
    addRadioButtonEventListeners: function (order) {
        debugger;
        var question = this.findQuestionByOrder(order);
        if (!question) return;

        var radios = document.getElementsByName(`q${question.qId}`);

        radios.forEach(function (radio) {
            if (radio.checked) {
                StudentExam.removeWarningMessage(order);
            }
        });
    },


};
