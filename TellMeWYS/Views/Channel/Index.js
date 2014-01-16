$(function () {

    var $logHolder = $('#log-holder');

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("ChannelHub");
    hub.on('SendUrl', function (url, isSafe) {
        var $logElem = $('<span>');
        if (isSafe === true) {
            var $link = $('<a>').attr({
                target: '_blank',
                href: url
            })
            .text(url);
            $logElem.append($link);
        }
        else {
            $logElem.text(url);
        }
        var $logBoundary = $('<div>')
        $logBoundary
            .append($logElem)
            .hide()
            .appendTo($logHolder)
            .fadeIn();
    });

    conn.start()
        .done(function () {
            hub.invoke("BeginListen", $.app.channelId);
        });
});