

$(document).ready(function () {

    // see weblog project
    // changing rating is not actually working



    //************** begin star 48 rating system *****************
    function setRating48(span, rating){ // change star display
        span.find('.rating48.stars48').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating48.stars48.active").mouseover(function () {
        var srating = $("#srating").attr("value");
        if (srating == 0) {
            var span = $(this).parent("span");
            var rating = $(this).attr("value");
            setRating48(span, rating);
        }
    });

    $(".rating48.stars48.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        if (rating == 0) {
            setRating48(span, rating);
        }
    });

    $(".rating48.stars48.active").click(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        $("#newrating").attr("value", newRating);
        var title = $(this).attr("title");
        $("#rate-info").html(title); 
        $("#srating").val(newRating);
        setRating48(span, newRating);
    });



    //************** begin star 16 rating system *****************
    function setRating16(span, rating){ // change star display
        span.find('.rating16.stars16').each(function () {
            var value = parseFloat($(this).attr("value"));
            var imgSrc = $(this).attr("class");
            if (value <= rating) $(this).attr("class", imgSrc.replace("_off", "_on"));
            else $(this).attr("class", imgSrc.replace("_on", "_off"));
        });
    }

    $(".rating16.stars16.active").mouseover(function () {
        var srating = $("#srating").attr("value");
        if (srating == 0) {
            var span = $(this).parent("span");
            var rating = $(this).attr("value");
            setRating16(span, rating);
        }
    });

    $(".rating16.stars16.active").mouseout(function () {

        var span = $(this).parent("span");
        var rating = $(this).attr("value");
        if (rating == 0) {
            setRating16(span, rating);
        }
    });

    $(".rating16.stars16.active").click(function () {
        var span = $(this).parent("span");
        var newRating = $(this).attr("value");
        $("#newrating").attr("value", newRating);
        var title = $(this).attr("title");
        $("#rate-info").html(title); 
        $("#srating").val(newRating);
        setRating16(span, newRating);
    });



    //************** begin star 32 rating system *****************
    function setRating32(span, rating){ // change star display
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
        html2canvas($('#controlpage')[0], { scale : 0.5 }).
            //document.querySelector("#pagecomments")).
            then(canvas =>{
                document.body.appendChild(canvas)

                var pID = $("#controlpage").attr("pageid");
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
