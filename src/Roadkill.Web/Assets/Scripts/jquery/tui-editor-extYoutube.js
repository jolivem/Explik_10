
var Editor = tui.Editor;
Editor.defineExtension('youtube', function () {
    Editor.codeBlockManager.setReplacer('youtube', function (youtubeId) {
        var wrapperId = 'yt' + Math.random().toString(36).substr(2, 10);
        setTimeout(renderYoutube.bind(null, wrapperId, youtubeId), 0);

        return '<div id="' + wrapperId + '"></div>';
    });
});

function renderYoutube(wrapperId, youtubeId) {
    var el = document.querySelector('#' + wrapperId);
    //el.innerHTML = '<iframe width="420" height="315" src="https://www.youtube.com/embed/' + youtubeId + '"></iframe>';
    el.innerHTML = '<div> TOTO TATA TITI ' + youtubeId + '</div > ';
}

