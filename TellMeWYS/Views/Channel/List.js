$(function () {
    var updateList = function () {
        $('#channel-list')
        .load('/Channel/List_ItemsPartial');
    };
    updateList();

    $('#btn-create-channel').click(function (e) {
        e.preventDefault();
        if (confirm('Sure?') === false) return;
        $.post('/Channel/Create')
        .done(function () {
            updateList();
        });
    });
});