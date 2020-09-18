$(window).on('pageshow', function () {
    initialDataCall();
});

function initialDataCall() {
    $.ajax({
        method: "GET",
        url: "/Leaderboards/ReplacingBooksLeaderboardData",
        success: (jsonString) => {
            var scoreViewModels = JSON.parse(jsonString);
            buildLeaderboard(scoreViewModels);
        }
    });
}

function buildLeaderboard(scoreViewModels) {
    if (scoreViewModels.length == 0) {
        var noRecordsHtml = '<tr><td>No recorded times yet</td></tr >';
        $('#tbody').append(noRecordsHtml);

    }
    for (var i = 0; i < scoreViewModels.length; i++) {
        var rowHtml = '<tr><th scope = "row" >' + (i + 1) + '</th><td>' + scoreViewModels[i].Name + '</td><td>' + scoreViewModels[i].Score + 's</td></tr >';
        $('#tbody').append(rowHtml);
    }
}
