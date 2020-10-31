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
        url: "/FindingCallNumber/Initialise",
        success: (jsonString) => {
            var parsedData = JSON.parse(jsonString);
            lastName = parsedData.lastName;
            

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
    var time = timeSinceStart;
    $.ajax({
        method: "POST",
        url: "/FindingCallNumbers/SaveTime",
        data: {
            name,
            time
        }
    });
}

