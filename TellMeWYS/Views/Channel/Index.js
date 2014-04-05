$(function () {
    var logTemplate = Handlebars.compile($('#log-template').html());
    var imageTemplate = Handlebars.compile($('#image-template').html());

    var $logHolder = $('#log-holder');

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("ChannelHub");
    hub.on('SendUrl', function (url, isSafe) {
        var $log = $('<div>')
            .html(logTemplate({ 'url': url, 'isSafe': isSafe, time: new Date().toLocaleTimeString() }))
            .hide()
            .prependTo($logHolder)
            .fadeIn();
        $logHolder.removeClass('no-msgs');
    });
    hub.on('SendImage', function (url) {
        var $log = $('<div>')
            .html(imageTemplate({ 'url': url, time: new Date().toLocaleTimeString() }))
            .hide()
            .prependTo($logHolder)
            .fadeIn();
        $logHolder.removeClass('no-msgs');
    });

    conn.start()
        .done(function () {
            hub.invoke("BeginListen", $.app.channelId);
        });

    $(document).on('click', '#log-holder .image img', function () {
        $(this).toggleClass('full-size')
    });
});