$(function () {
        var shareids = [];

        $("div[data-note-id]").each(function (i, e) {

            shareids.push($(e).data("note-id"));
        });


        $.ajax({
            method: "POST",
            url: "/Sharing/GetLiked",
            data: { ids: shareids }
        }).done(function (data) {

            if (data.Result != null && data.Result.length>0) {
                for (var i = 0; i < data.Result.length; i++) {
                    var id = data.Result[i];
                   
                    var likedShare = $("div[data-note-id=" + id + "]");
                    var btn = likedShare.find("button[data-liked]")

                    btn.data("liked", true);

                    btn.removeClass("btn-outline-danger");
                    btn.addClass("btn-danger")
                }
            }


            }).fail(function () {
                //alert("Sunucu ile bağlantı kurulamadı...")
        });

        $("button[data-liked]").click(function () {
            var btn = $(this);
            var liked = btn.data("liked");
            var noteid = btn.data("note-id");
            var spanLikeCount = btn.find("span.like-count");


            $.ajax({

                method: "POST",
                url: "/Sharing/SetLikeState",
                data: { "noteid": noteid, "liked": !liked }



            }).done(function (data) {

                if (data.hasError) {
                    alert(data.errorMessage);
                } else {
                    liked = !liked;
                    btn.data("liked", liked);
                    spanLikeCount.text(data.result);
                    btn.removeClass("btn-outline-danger");
                    btn.removeClass("btn-danger");


                    if (liked) {
                        btn.addClass("btn-danger");
                    }
                    else {
                        btn.addClass("btn-outline-danger");
                        
                        location.reload();
                    }
                }



                }).fail(function () {
                   
            });


        });

    });