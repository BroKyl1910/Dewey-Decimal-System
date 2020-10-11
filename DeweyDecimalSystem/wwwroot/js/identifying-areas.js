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
    },
    over: function (event, elem) {
        if ($(this).has(".draggable").length) {
            $(this).droppable("disable");
        } else {
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
    }
});

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
