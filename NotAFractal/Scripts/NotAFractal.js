$(document).ready(function () {
    $(".toggleLink").click(function() {
        {
            $(this).parent().find(".nodeContainer").toggle();
        };
    });
})