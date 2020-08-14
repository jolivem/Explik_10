

$(document).ready(function () {

    // see weblog project
    // changing rating is not actually working

    //************** begin star 32 rating system *****************
    function setRating32(span, rating) { // change star display
        span.find('.rating32.stars32').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating32.stars32.active").mouseover(function () {
        var srating = $("#srating").attr("value");
        if (srating == 0) {
            var span = $(this).parent("span");
            var rating = $(this).attr("value");
            setRating32(span, rating);
        }
    });

    $(".rating32.stars32.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        if (rating == 0) {
            setRating32(span, rating);
        }
    });

    $(".rating32.stars32.active").click(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        $("#newrating").attr("value", newRating);
        var title = $(this).attr("title");
        $("#rate-info").html(title);
        $("#srating").val(newRating);
        setRating32(span, newRating);

        var pID = $("#page-view").attr("pageid");
        //var pRating = $("#srating").val();

        $.ajax({
            type: "POST",
            url: "/Pages/PageRating",
            data: {
                id: pID,
                rating: newRating
            },
            success: function (response) {
                toastr.success("Rating taken into account");
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });

    //************** begin star 24 rating system *****************
    function setRating(span, rating) { // change star display
        span.find('.rating.stars').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating.stars.active").mouseover(function () {
        var srating = $("#srating").attr("value");
        if (srating == 0) {
            var span = $(this).parent("span");
            var rating = $(this).attr("value");
            setRating(span, rating);
        }
    });

    $(".rating.stars.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        if (rating == 0) {
            setRating(span, rating);
        }
    });

    $(".rating.stars.active").click(function () {
        var previousRating = $("#current-rating").val();
        if (previousRating == 0) {
            var span = $(this).parent("span");
            var newRating = $(this).attr("value");
            $("#newrating").attr("value", newRating);
            var title = $(this).attr("title");
            $("#rate-info").html(title);
            $("#srating").val(newRating);
            setRating(span, newRating);
            $("#submit-rating").css("display", "inline");
            $("#submit-unrating").css("display", "none");

        }
        //var pID = $("#page-view").attr("pageid");
        //var pRating = $("#srating").val();
    });

    $("#submit-rating").click(function () {

        // check login if text-login-rating exists (exists for competition rating, not for usual view)
        if ($('#text-login-rating').length)  
        {
            var currentUser = $("#current-user").attr("name");
            if (currentUser == "") {
                bootbox.setDefaults({ animate: false });
                bootbox.alert($("#text-login-rating").attr("value"))
                return;
            }
        }

        var pID = $("#page-view").attr("pageid");
        var previousRating = $("#current-rating").val();
        var newRating = $("#srating").val();
        if (previousRating != 0) {
            // this is "cancel rating"
            newRating = 0;
        }
        $.ajax({
            type: "POST",
            url: "/Pages/PageRating",
            data: {
                id: pID,
                rating: newRating
            },
            success: function (response) {

                    toastr.success($("#rating-added").attr("value"));
                    var span = $("#ratings");
                    $("#current-rating").val(newRating);
                    $(".rating.stars").css("background-image", 'url("/Assets/CSS/images/grey-yellow-24.png")');
                    $("#submit-rating").css("display", "none"); //prevents multiple clicks
                    setTimeout(function () {
                        $("#submit-unrating").css("display", "inline");
                    }, 1000);
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });

    $("#submit-unrating").click(function () {

        // check login if text-login-rating exists (exists for competition rating, not for usual view)
        if ($('#text-login-rating').length) {
            var currentUser = $("#current-user").attr("name");
            if (currentUser == "") {
                bootbox.setDefaults({ animate: false });
                bootbox.alert($("#text-login-rating").attr("value"))
                return;
            }
        }

        var pID = $("#page-view").attr("pageid");
        //var previousRating = $("#current-rating").val();
        //var newRating = $("#srating").val();
        //if (previousRating != 0) {
        //    // this is "cancel rating"
        //    newRating = 0;
        //}

        $.ajax({
            type: "POST",
            url: "/Pages/PageRating",
            data: {
                id: pID,
                rating: "0"
            },
            success: function (response) {

                    toastr.success($("#rating-removed").attr("value"));
                    var span = $("#ratings");
                    setRating(span, 0);
                    //setActive(span);
                    $("#current-rating").val(0);
                    $("#srating").val(0);
                    $("#rate-info").html("&nbsp;");
                    $(".rating.stars").css("background-image", 'url("/Assets/CSS/images/grey-green-24.png")');
                    $("#submit-rating").css("display", "none");
                    $("#submit-unrating").css("display", "none");
                    //document.getElementById("submit-rating").text = $("#text-rate").attr("value");
                    //document.getElementById("submit-rating").style.color = "red";
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });


    $("#buttoncommentalert").click(function () {
        var pID = $("#commentalert").attr("commentid");

        $.ajax({
            type: "POST",
            url: "/Pages/CommentAlert",
            data: { id: pID },
            success: function (response) {
                toastr.success("Alert taken into account");
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });

    $("#remove-alert").click(function () {

        var pID = $("#page-view").attr("pageid");

        $.ajax({
            type: "POST",
            url: "/Pages/PageRemoveAlert",
            data: { id: pID },
            success: function (response) {
                toastr.success($("#alert-removed").attr("value"));
                $("#remove-alert").hide();
                setTimeout(function () {
                    var elmt = $("#pagealert-link");
                    elmt.toggle();
                    elmt.css("visibility", "visible")
                }, 3000);
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }

        });
    });
});

function ConfirmGetbackPage(pId) {
    var confirmMessage = $("#confirm-unpublish-text").attr("value");
    Roadkill.Web.Dialogs.confirm(confirmMessage, function (result) {

        if (!result)
            return;
        // Ajax request
        $.ajax({
            type: "POST",
            url: "/Pages/SetDraft",
            data: { id: pId },
            success: function (response) {
                location.reload(); // refresh page
                //toastr.success('blup');
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
}

