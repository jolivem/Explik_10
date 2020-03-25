
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
    el.innerHTML = '<div class="video-responsive"><iframe width="100%" height="100%" src="https://www.youtube.com/embed/' + youtubeId + '"></iframe></div>';
    //el.innerHTML = '<div> TOTO TATA TITI ' + youtubeId + '</div > ';
}
