

$(document).ready(function () {

    // see weblog project
    // changing rating is not actually working

    //************** begin star rating system *****************
    function setRating48(span, rating){ // change star display
        span.find('.rating48.stars48').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating48.stars48.active").mouseover(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        setRating48(span, newRating);
    });

    $(".rating48.stars48.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        setRating48(span, rating);
    });

    $(".rating48.stars48.active").click(function () {
        var newRating = $(this).attr("value");
        $("#newrating").attr("value", newRating);
        $("#rate-info").html("new rating = " + newRating); 
    });

    function setRating(span, rating){ // change star display
        span.find('.rating.stars').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating.stars.active").mouseover(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        setRating(span, newRating);
    });

    $(".rating.stars.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        setRating(span, rating);
    });

    $(".rating.stars.active").click(function () {
        var newRating = $(this).attr("value");
        $("#newrating").attr("value", newRating);
    });

    $("#buttonrating").click(function () {
        var newRating = $("#newrating").attr("value");
        var pID = $("#newrating").attr("pageid");
        var text = $("#commentarea").val();

        $.ajax({
            type : "POST",
            url : "/Pages/AddComment",
            data : { id : pID, rating : newRating, comment : text },
            success : function (response){
                toastr.success("Comment taken into account");
            },
            failure : function (response){
                alert("failure");
            },
            error : function (response){
                alert(response.responseText);
            }

        });
    });

    $("#pagealert-button").click(function () {
        var pID = $("#pagealert").attr("pageid");

        $.ajax({
            type : "POST",
            url : "/Pages/PageAlert",
            data : { id : pID },
            success : function (response){
                toastr.success("Alert taken into account");
            },
            failure : function (response){
                alert("failure");
            },
            error : function (response){
                alert(response.responseText);
            }

        });
    });

    $("#buttoncommentalert").click(function () {
        var pID = $("#commentalert").attr("commentid");

        $.ajax({
            type : "POST",
            url : "/Pages/CommentAlert",
            data : { id : pID },
            success : function (response){
                toastr.success("Alert taken into account");
            },
            failure : function (response){
                alert("failure");
            },
            error : function (response){
                alert(response.responseText);
            }

        });
    });

    $("#buttoncanvas").click(function () {
        html2canvas($('#pagecomments')[0], { scale : 0.5 }).
            //document.querySelector("#pagecomments")).
            then(canvas =>{
                document.body.appendChild(canvas)

                var pID = $("#pageinfo").attr("pageid");
                var dataURL = canvas.toDataURL("image/png");
                var dataURL = dataURL.replace('data:image/png;base64,', '');

                $.ajax({
                    type : "POST",
                    url : "/Pages/UploadCanvas",
                    data : '{ "id" : "' + pID + '", "image" : "' + dataURL + '" }',
                    contentType : 'application/json; charset=utf-8',
                    dataType : 'json',
                    success : function (response){
                        toastr.success("Canvas taken into account");
                    },
                    failure : function (response){
                        alert("failure");
                    },
                    error : function (response){
                        alert(response.responseText);
                    }

                });

            });
    });
});
