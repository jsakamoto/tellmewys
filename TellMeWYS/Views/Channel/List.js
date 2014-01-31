$(function () {
    var channelItemTemplate = Handlebars.compile($('#channel-item-template').html());
    var $channelListHolder = $('#channel-list');

    // initial display.
    $.each($.app.channels, function () {
        $channelListHolder.append(channelItemTemplate(this));
    });

    // create new channel.
    $('#btn-create-channel').click(function (e) {
        e.preventDefault();
        if (confirm($.Localize.Sure) === false) return;

        $.post('/Channel/Create')
            .done(function (channel) {
                var $item = $("<span/>").append(channelItemTemplate(channel)).hide();
                $channelListHolder.append($item);
                $item.fadeIn();
            })
            .fail(function () {
                alert('An error occurred while processing your request.');
            });
    });
});