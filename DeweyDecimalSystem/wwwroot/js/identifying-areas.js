var correctDictionary = {};
var lastName;
var enteredName;
var timeSinceStart;
var timerInterval;
var roundNumber = 1;

$(window).on('pageshow', function () {
    initialDataCall();
});

function initialDataCall() {
    $('.timer-wrapper').show();
    $('.endgame-controls-wrapper').hide();
    $.ajax({
        method: "GET",
        url: "/IdentifyingAreas/Initialise?roundNumber=" + roundNumber,
        success: (jsonString) => {
            var parsedData = JSON.parse(jsonString);
            lastName = parsedData.lastName;
            var areaDictionary = parsedData.areaDictionary;
            this.correctDictionary = areaDictionary;
            var shuffledKeys = parsedData.keys;
            var shuffledValues = parsedData.values;

            buildQuestionsAndAnswers(shuffledKeys, shuffledValues);
            if (roundNumber == 1) showForm();

            var instructions;
            if (roundNumber % 2 == 1) {
                instructions = "Match the call number on the left to the category on the right. Complete the round faster to rank higher"
            } else {
                instructions = "Match the category on the left to the call number on the right. Complete the round faster to rank higher"
            }
            $('.instructions').html(instructions);
        }
    });
}

$('.play-button').on('click', () => {
    setupDragAndDrop();
    setupTimer();

    $('.timer-text').html("0s");
    $('.play-button').fadeOut(200, () => {
        $('.timer-text').fadeIn(200, () => { });
    });
});

function setupTimer() {
    var startTime = new Date().getTime();
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
    roundNumber++;
    $('.timer-text').fadeOut(200, () => {
        $('.play-button').fadeIn(200, () => { });
    });
    $('.custom-progress-bar').css('width', "0%");
    initialDataCall();
});

$('.endgame-control.leaderboard').on('click', () => {
    window.location.href = '/Leaderboards/IdentifyingAreas';
});

function showForm() {
    $('#name-text').val(lastName);
    $('.popup-container').fadeIn();
}

$('.save-score-button').on('click', () => {
    enteredName = $('#name-text').val();
    $('.popup-container').fadeOut();
});

function saveScore() {
    var name = enteredName;
    var time = timeSinceStart;
    $.ajax({
        method: "POST",
        url: "/IdentifyingAreas/SaveTime",
        data: {
            name,
            time
        }
    });
}


function buildQuestionsAndAnswers(keys, values) {
    var questionsWrapper = $('.left-column');
    var answersWrapper = $('.right-column');

    $(questionsWrapper).html('');
    $(answersWrapper).html('');

    for (i = 0; i < 4; i++) {
        var question = keys[i];
        var questionHtml = $('<div class="question-wrapper"><span class="question-text">' + question + '</span><div class="host-wrapper empty drop" data-key="' + question + '"><span class="host-placeholder">Drag answer here...</span></div></div>');
        $(questionsWrapper).append($(questionHtml));
    }

    for (i = 0; i < 7; i++) {
        var answer = values[i];
        var answerHtml = $('<div class="answer-wrapper draggable" data-value="' + answer + '"><span class= "answer-text">' + answer + '</span></div>');
        $(answersWrapper).append($(answerHtml));
    }

}

function checkAnswers() {
    var drops = $('.drop');
    console.log('Drops');
    console.log(drops);
    var numCorrect = 0;
    for (var i = 0; i < drops.length; i++) {
        var draggable = $(drops[i]).children(".draggable");
        if ($(draggable).length > 0) {
            var question = $(drops[i]).attr('data-key');
            var answer = $(draggable).attr('data-value');
            if (correctDictionary[question.toString()] == answer) {
                numCorrect++;
                console.log('Correct', numCorrect);
            }
        }

    }
    var percCorrect = numCorrect / 4 * 100;
    $('.custom-progress-bar').css('width', percCorrect + "%");

    if (percCorrect == 100) {
        endGame();
    }
}


function setupDragAndDrop() {
    $(".draggable").draggable({ revert: "invalid" });
    $(".drop").droppable({
        accept: ".draggable",
        drop: function (event, ui) {
            console.log("drop");
            $(this).removeClass("empty").removeClass("over").addClass("dropped");
            var dropped = ui.draggable;
            var droppedOn = $(this);
            $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
            //Make sure empty questions have area to drop
            enableDrops();
            checkAnswers();
        },
        over: function (event, elem) {
            if ($(this).has(".draggable").length) {
                $(this).droppable("disable");
            }
            else {
                $(this).addClass("over");
                console.log("over");
            }
        },
        out: function (event, elem) {
            console.log($(event));
            $(this).removeClass("over");
        },
    });
    $("#origin").droppable({
        accept: ".draggable",
        drop: function (event, ui) {
            console.log("drop");
            var dropped = ui.draggable;
            var droppedOn = $(this);
            $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
            //Make sure empty questions have area to drop
            enableDrops();
            checkAnswers();
        }
    });
}

function enableDrops() {
    var drops = $('.drop');
    console.log('Drops');
    console.log(drops);
    for (var i = 0; i < drops.length; i++) {
        console.log("Loop");
        if ($(drops[i]).has(".draggable").length == 0) {
            $(drops[i]).addClass('empty');
            $(drops[i]).droppable("enable");
        }
    }
}
