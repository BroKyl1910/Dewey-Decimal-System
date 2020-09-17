var userOrder = [];
var correctOrder = [];
$(window).on("load", function () {
    $.ajax({
        method: "GET",
        url: "ReplacingBooks/Initialise",
        success: (data) => {
            data = JSON.parse(data);
            var bookViewModels = data.bookViewModels;
            var sortedBooks = data.sortedBooks;
            console.log(data);
            buildBooks(bookViewModels);
        }
    });
    setupSortFunctionality();
});

function buildBooks(bookViewModels) {
    var bookWrapper = $('.book-wrapper');
    for (var bookViewModel of bookViewModels) {
        var number = bookViewModel.Book.CallNumber.Number;
        var name = bookViewModel.Book.CallNumber.Name;
        var bookHtml = $('<li class="book" data-id="' + number + '' + name + '"><div class="cover left" ></div><div class="spine"><div class="sticker"><span class="number">' + number + '</span><span class="initials">' + name + '</span></div></div><div class="cover right"></div></li >');
        $(bookWrapper).append($(bookHtml));
    }
}

function setupSortFunctionality() {
    //https://stackoverflow.com/questions/5320194/get-order-of-list-items-in-a-jquery-sortable-list-after-resort
    var $sortableList = $(".book-wrapper");
    var sortEventHandler = function (event, ui) {
        var listElements = $sortableList.children();
        for (element of listElements) {
            userOrder.push($(element).attr('data-id'));
        }
        console.log(userOrder);
    };
    $sortableList.sortable({
        stop: sortEventHandler
    });
    $sortableList.on("sortchange", sortEventHandler);
}
