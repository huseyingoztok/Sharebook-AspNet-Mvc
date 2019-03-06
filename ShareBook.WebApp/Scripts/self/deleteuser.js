
    $(function () {

        $('#modal_delete').on('show.bs.modal', function (e) {
            $("#modal_delete_body").load("/Home/DeleteUser/");

        });


        $('#modal_delete').on('hide.bs.modal', function (e) {
            location.reload();
        });

    });
