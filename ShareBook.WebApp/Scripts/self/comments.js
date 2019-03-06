var note_id = -1;
    $(function () {
        toastr.options = {
            //"debug": false,
            "positionClass": "toast-top-center",
            //"onclick": null,
            //"fadeIn": 300,
            //"fadeOut": 1000,
            "timeOut": 2000,
            //"extendedTimeOut": 1000
        }
        $('#modal_comment').on('show.bs.modal', function (e) {
            var btn = $(e.relatedTarget);
            note_id = btn.data("note-id");
            $("#modal_comment_body").load("/Comment/ShowCommentsandLikes/" + note_id);
            
        });

        
            $('#modal_comment').on('hide.bs.modal', function (e) {
                location.reload();
            });

    });


    function doComment(btn, mode, commentid, pid) {
        var button = $(btn);
        var state = button.data("edit-mode");
        if (mode === "edit") {
            if (!state) {
                button.data("edit-mode", true);
                button.removeClass("btn-primary");
                button.addClass("btn-success");
                var btni = button.find("i");
                btni.removeClass("fa-pencil");
                btni.addClass("fa-check-square");
                $(pid).attr("contenteditable", true);
                pid.addClass("editText");
                $(pid).focus();  
            }
            else {
                button.data("edit-mode", false);
                button.addClass("btn-primary");
                button.removeClass("btn-success");
                var btni = button.find("i");
                btni.addClass("fa-pencil");
                btni.removeClass("fa-check-square");
                $(pid).attr("contenteditable", false);
                var commentText = $(pid).text();
                $.ajax({
                    method: "POST",
                    url: "/Comment/Edit/" + commentid,
                    data: { text: commentText },
                 

                }).done(function (data)
                    {
                    if (data.Result) {


                        $("#modal_comment_body").load("/Comment/ShowCommentsandLikes/" + note_id);
                        toastr.success('Yorumunuz Güncellendi');

                    }
                    else {
                        toastr.info("Yorumunuzda değişiklik yapmadınız.");
                    }
                    
                    }).fail(function ()
                    {

                        alert("Sunucu ile bağlantı kurulamadı.")
                    });


            }

        }
        else if (mode === 'delete') {

            swal({
                title: 'Emin misiniz?',
                text: "Yorumunuzu kalıcı olarak silmek istediğinizden!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#C9302C',
                cancelButtonColor: '#EC971F',
                confirmButtonText: 'Evet silmek istiyorum!'
            }).then((result) => {
                if (result.value) {
                    swal(
                        'Slindi!',
                        'Yorumunuz silindi.',
                        'success'
                    )

                    $.ajax({
                        method: "GET",
                        url: "/Comment/Delete/" + commentid,

                    }).done(function (data) {
                        if (data.Result) {
                            $("#modal_comment_body").load("/Comment/ShowCommentsandLikes/" + note_id);
                            toastr.info('Yorumunuz Silindi');
                        }
                        else {
                            swal({
                                type: 'error',
                                title: 'Oops...',
                                text: 'Birşeyler ters gitti!',
                                footer: '<a href>Why do I have this issue?</a>',
                            })
                        }
                    }).fail(function () {
                        swal({
                            type: 'error',
                            title: 'Oops...',
                            text: 'Sunucu ile bağlantı kurulamadı.!',
                            footer: '<a href>Why do I have this issue?</a>',
                        })
                 
                    });
                }
            })         
        } else if (mode === 'insercomment') {

            var text = $('#insert_comment_text').val();

            $.ajax({
                method: "POST",
                url: "/Comment/Insert/" + commentid,
                data: { commentText: text, "note_id": note_id }
            }).done(function (data) {
                if (data.Result) {
                    $("#modal_comment_body").load("/Comment/ShowCommentsandLikes/" + note_id);
                    toastr.success('Yorumunuz Eklendi');
                }
                else {
                    alert("Yorum eklenemedi...")
                }
            }).fail(function () {

                alert("Sunucu ile bağlantı kurulamadı.")
            });
        }

    }

 
   
    var btn = $(this);
    var noteid = btn.data("note-id");
    var spanCommentCount = btn.find("span.comment-count");

    $.ajax({
        method: "POST",
        url: "/Sharing/GetCommentCount",
        data: { "noteid": noteid }
    }).done(function (data) {

        spanCommentCount.text(data.result);

    }).fail(function () {
        //alert("Sunucu ile bağlantı kurulamadı...")
    });

  