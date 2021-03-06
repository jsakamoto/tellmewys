$(function () {
    var queryStr = location.search.split('?').pop();
    if (queryStr === "") return;

    var query = {};
    $.each(queryStr.split('&'), function () {
        var pair = this.split('=');
        query[pair[0]] = pair[1];
    });
    if (query.hasOwnProperty('msg') === false) return;

    var msg = query.msg;
    delete query.msg;

    var $msgBar = $('#msg-bar');
    $('.content-wrapper', $msgBar).text(decodeURI(msg));
    $msgBar.slideDown();

    queryStrs = [];
    for (var propName in query) {
        if (query.hasOwnProperty(propName) === true) {
            queryStrs.push([propName, query[propName]].join('='));
        }
    }

    var urlparts = [location.protocol, '//', location.host, location.pathname];
    if (queryStrs.length !== 0) {
        urlparts.push('?');
        urlparts.push(queryStrs.join('&'));
    }
    var url = urlparts.join('');
    history.replaceState(null, null, url);
});