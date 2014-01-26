$(function () {
    var logTemplate = Handlebars.compile($('#log-template').html());

    var $logHolder = $('#log-holder');

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("ChannelHub");
    hub.on('SendUrl', function (url, isSafe) {
        var $log = $('<div>')
            .html(logTemplate({ 'url': url, time: new Date().toLocaleTimeString() }))
            .hide()
            .prependTo($logHolder)
            .fadeIn();
        $logHolder.removeClass('no-msgs');
    });

    conn.start()
        .done(function () {
            hub.invoke("BeginListen", $.app.channelId);
        });
});