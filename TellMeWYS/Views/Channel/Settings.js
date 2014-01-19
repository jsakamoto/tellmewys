$(function () {
    $('#btn-delete-channel').click(function (e) {
        e.preventDefault();
        if (confirm('Sure?') === false) return;
        $.post($(this).attr('href'))
            .done(function () {
                location.href = $.app.channelListUrl;
            });
    });
});