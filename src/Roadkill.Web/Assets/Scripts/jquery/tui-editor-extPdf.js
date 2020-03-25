
var Editor = tui.Editor;
Editor.defineExtension('pdf', function () {
    Editor.codeBlockManager.setReplacer('pdf', function (pdfFile) {
        var wrapperId = 'pdf' + Math.random().toString(36).substr(2, 10);
        setTimeout(renderPdf.bind(null, wrapperId, pdfFile), 0);

        return '<div id="' + wrapperId + '"></div>';
    });
});

function renderPdf(wrapperId, pdfFile) {
    var el = document.querySelector('#' + wrapperId);
    // pdfFile has " at he begining and at the end
    // insert parameters
    pdfParameters = pdfFile.substr(0, pdfFile.length - 1);
    pdfParameters += '?#statusbar=1&toolbar=0&navbar=1'
    el.innerHTML = '<div class="pdf-responsive"><iframe src=' + pdfParameters + '> </iframe></div>';
}

