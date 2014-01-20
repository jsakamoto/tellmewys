$(function () {

    var membersHolder = $('#members-holder')[0];

    $('.remove', membersHolder).click(function (e) {
        e.preventDefault();
        if (confirm('Sure?') === false) return;
        var $this = $(this);
        var $row = $this.closest('tr');
        var memberId = $row.data("member-id");
        $.post($this.attr('href'), { 'memberId': memberId });
        //TODO: Implement failed.

        $row.fadeOut(function () {
            membersHolder.deleteRow($row.prop('rowIndex'));
        });
    });


    $('#btn-delete-channel').click(function (e) {
        e.preventDefault();
        if (confirm('Sure?') === false) return;
        $.post($(this).attr('href'))
            .done(function () {
                location.href = $.app.channelListUrl;
            });
    });
});