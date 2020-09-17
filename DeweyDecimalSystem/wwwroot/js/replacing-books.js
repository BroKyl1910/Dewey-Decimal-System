﻿var userOrder = [];
var correctOrder = [];
var timeSinceStart;
var timerInterval;


$(document).ready(function () {
    $.ajax({
        method: "GET",
        url: "ReplacingBooks/Initialise",
        success: (data) => {
            data = JSON.parse(data);
            var bookViewModels = data.bookViewModels;
            this.correctOrder = data.sortedBooks;
            console.log("Correct Order");
            console.log(this.correctOrder);
            buildBooks(bookViewModels);
        }
    });
});

$('.play-button').on('click', () => {
    setupSortFunctionality();
    setupTimer();

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

function buildBooks(bookViewModels) {
    var bookWrapper = $('.book-wrapper');
    for (var bookViewModel of bookViewModels) {
        var number = bookViewModel.Book.CallNumber.Number;
        var name = bookViewModel.Book.CallNumber.Name;
        var bookHtml = $('<li class="book" data-id="' + number + '_' + name + '"><div class="cover left" ></div><div class="spine"><div class="sticker"><span class="number">' + number + '</span><span class="initials">' + name + '</span></div></div><div class="cover right"></div></li >');
        $(bookWrapper).append($(bookHtml));
    }
}

function setupSortFunctionality() {
    //https://stackoverflow.com/questions/5320194/get-order-of-list-items-in-a-jquery-sortable-list-after-resort
    var $sortableList = $(".book-wrapper");
    var sortEventHandler = function (event, ui) {
        userOrder = [];
        var listElements = $sortableList.children();
        for (element of listElements) {
            var callNumber;
            if ($(element).attr('data-id') != undefined) {
                callNumber = $(element).attr('data-id').split('_')[0] + " " + $(element).attr('data-id').split('_')[1];
            } else {
                callNumber = undefined;
            }
            userOrder.push(callNumber);
        }
        checkOrder();
    };

    $sortableList.sortable({
        stop: sortEventHandler
    });
    $sortableList.on("sortchange", sortEventHandler);
}

function checkOrder() {
    var correct = true;
    numCorrect = 0;

    console.log("Length of correctOrder: " + correctOrder.length);
    console.log("Length of userOrder: " + userOrder.length);
    for (var i = 0; i < correctOrder.length; i++) {
        if (userOrder[i] != undefined && userOrder[i] == (correctOrder[i].CallNumber.Number + " " + correctOrder[i].CallNumber.Name)) {
            numCorrect++;
        }
    }
    var correctPerc = numCorrect / 10 * 100;
    console.log(correctPerc + "%");
    $('.custom-progress-bar').css('width', correctPerc + "%");

    if (correctPerc == 100) {
        endGame();
    }
}

function endGame() {
    setTimeout(() => {
        alert('YOU WIN TWAT');
        clearInterval(timerInterval);
    }, 1000);

}
