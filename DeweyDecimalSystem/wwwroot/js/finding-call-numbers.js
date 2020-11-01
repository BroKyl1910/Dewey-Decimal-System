var playing = false;
var questionCallNumber;
var level1Answers = [];
var level2Answers = [];
var level3Answers = [];
var correctAnswers = [];
var lastName;
var enteredName = '';
var timeSinceStart;
var timerInterval;
var levelNumber = 1;
var numCorrect = 0;
var points = 0;

$(window).on('pageshow', function () {
    initialDataCall();
});

function initialDataCall() {
    $('.timer-wrapper').show();
    $('.endgame-controls-wrapper').hide();

    $.ajax({
        method: "GET",
        url: "/FindingCallNumbers/Initialise",
        success: (jsonString) => {
            var parsedData = JSON.parse(jsonString);
            lastName = parsedData.lastName;
            questionCallNumber = parsedData.randomCallNumber;
            level1Answers = parsedData.level1Answers;
            level2Answers = parsedData.level2Answers;
            level3Answers = parsedData.level3Answers;
            correctAnswers = parsedData.correctAnswers;

            if (enteredName == '') {
                showForm();
            }

            levelNumber = 1;
            points = 0;
            buildGameUI();
        }
    });
}

$('.play-button').on('click', () => {
    setupTimer();
    playing = true;


    $('.answer-list-item').addClass('active');
    $('.timer-text').html("0s");
    $('.play-button').fadeOut(200, () => {
        $('.timer-text').fadeIn(200, () => { });
    });
});

function buildGameUI() {
    var answerName = questionCallNumber.Name;
    $('.top-call-number').html(answerName);
    setQuestionText(answerName);
    buildAnswers();
}

function buildAnswers() {
    var answersList;
    if (levelNumber == 1) {
        answersList = level1Answers;
    } else if (levelNumber == 2) {
        answersList = level2Answers;
    } else {
        answersList = level3Answers;
    }

    var answerUl = $('.answer-list');
    $(answerUl).empty();
    for (answer of answersList) {
        var answerHtml = '<li class="answer-list-item" data-id="' + answer.Number + '"><span class="answer-text">' + answer.Number + ' ' + answer.Name + '</span ></li >';
        $(answerUl).append(answerHtml);
    }

    $('.answer-list-item').on('click', (e) => {
        if (!playing) return;

        $('.answer-list-item').removeClass('active');


        correctAnswers[levelNumber - 1]
        console.log('correctAnswers', correctAnswers);
        console.log('levelNumber', levelNumber);
        console.log('correctAnswers[levelNumber - 1]', correctAnswers[levelNumber - 1]);

        var clickedCallNumber = $(e.currentTarget).attr('data-id');
        var correct = false;
        if (correctAnswers[levelNumber - 1].Number == clickedCallNumber) {
            $(e.currentTarget).addClass('correct');
            correct = true;
            numCorrect++;
        } else {
            $(e.currentTarget).addClass('incorrect');
            $('.answer-list-item[data-id="' + correctAnswers[levelNumber - 1].Number + '"]').addClass('correct');
        }

        playing = false;
        calculatePoints(correct);
        setTimeout(resetLevel, 2000);
    });

}

function calculatePoints(correct) {
    clearInterval(timerInterval);
    var time = timeSinceStart;
    if (correct) {
        if (time <= 1) {
            points += 15;
        } else if (time <= 3) {
            points += 10;
        } else if (time <= 5) {
            points += 8;
        } else if (time <= 8) {
            points += 7;
        } else if (time <= 10) {
            points += 6;
        } else {
            points += 5;
        }
    }
    $('.timer-text').html(points + "pts");
}

function setProgressBar() {
    var progress = (levelNumber - 1) / 3 * 100;
    $('.custom-progress-bar').css('width', progress + "%");
}

function resetLevel() {
    levelNumber++;
    setProgressBar();
    if (levelNumber > 3) {
        endGame();
        return;
    }

    buildGameUI();
    setupTimer();

    $('.answer-list-item').removeClass('correct');
    $('.answer-list-item').removeClass('incorrect');
    $('.answer-list-item').addClass('active');
    playing = true;
}


function setQuestionText(answerName) {
    var instructions;
    if (levelNumber === 1) {
        instructions = "Which top level category does <em>" + answerName + "</em> belong to?";
    }
    else if (levelNumber === 2) {
        instructions = "Which second level category does <em>" + answerName + "</em> belong to?";
    }
    else {
        instructions = "Which call number matches <em>" + answerName + "</em>?";
    }
    $('.question-text').html(instructions);
}

function setupTimer() {
    var startTime = new Date().getTime();
    timeSinceStart = 0;
    $('.timer-text').html(timeSinceStart + "s");
    timerInterval = setInterval(function () {

        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = now - startTime;

        timeSinceStart = Math.floor(distance / 1000);

        // Display the result in the element with id="demo"
        $('.timer-text').html(timeSinceStart + "s");
    }, 1000);
}

function endGame() {
    setTimeout(() => {
        clearInterval(timerInterval);
        saveScore();
        $('.timer-wrapper').hide();
        $('.endgame-controls-wrapper').css('display', 'flex');
    }, 1000);

}

$('.endgame-control.next').on('click', () => {
    levelNumber++;
    $('.timer-text').fadeOut(200, () => {
        $('.play-button').fadeIn(200, () => { });
    });
    $('.custom-progress-bar').css('width', "0%");
    initialDataCall();
});

$('.endgame-control.leaderboard').on('click', () => {
    window.location.href = '/Leaderboards/FindingCallNumbers';
});

function showForm() {
    $('#name-text').val(lastName);
    $('.popup-container').fadeIn();
}

$('.save-score-button').on('click', () => {
    enteredName = $('#name-text').val();
    if (enteredName == '') {
        $('.error-text').html('Please enter a name');
        return;
    }
    $('.error-text').html('');

    $('.popup-container').fadeOut();
});

function saveScore() {
    var name = enteredName;
    $.ajax({
        method: "POST",
        url: "/FindingCallNumbers/SaveScore",
        data: {
            name,
            points
        }
    });
}

