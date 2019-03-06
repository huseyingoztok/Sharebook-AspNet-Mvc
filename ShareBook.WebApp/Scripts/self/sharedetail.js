$(function () {

        $('#modal_notdetail').on('show.bs.modal', function (e) {
            var btn = $(e.relatedTarget);
            note_id = btn.data("note-id");
            $("#modal_notdetail_body").load("/Sharing/ShowSharingDetail/" + note_id);
        });


    });