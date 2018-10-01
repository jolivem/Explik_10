/// <reference path="../typescript-ref/filemanager.references.ts" />
var Roadkill;
(function (Roadkill) {
    var Web;
    (function (Web) {
        var FileManager;
        (function (FileManager) {
            var AjaxRequest = /** @class */ (function () {
                function AjaxRequest() {
                }
                AjaxRequest.prototype.getFolderInfo = function (path, successFunction) {
                    var url = ROADKILL_FILEMANAGERURL + "/folderinfo";
                    var data = { dir: path };
                    var errorMessage = ROADKILL_FILEMANAGER_ERROR_DIRECTORYLISTING + " <br/>";
                    this.makeAjaxRequest(url, data, errorMessage, successFunction);
                };
                AjaxRequest.prototype.deleteFolder = function (folder, successFunction) {
                    var url = ROADKILL_FILEMANAGERURL + "/deletefolder";
                    var data = { folder: folder };
                    var errorMessage = ROADKILL_FILEMANAGER_ERROR_DELETEFOLDER + " <br/>";
                    this.makeAjaxRequest(url, data, errorMessage, successFunction);
                };
                AjaxRequest.prototype.deleteFile = function (fileName, filePath, successFunction) {
                    var url = ROADKILL_FILEMANAGERURL + "/deletefile";
                    var data = { filename: fileName, filepath: filePath };
                    var errorMessage = ROADKILL_FILEMANAGER_ERROR_DELETEFILE + " <br/>";
                    this.makeAjaxRequest(url, data, errorMessage, successFunction);
                };
                AjaxRequest.prototype.newFolder = function (currentPath, newFolder, successFunction) {
                    var url = ROADKILL_FILEMANAGERURL + "/newFolder";
                    var data = { currentFolderPath: currentPath, newFolderName: newFolder };
                    var errorMessage = ROADKILL_FILEMANAGER_ERROR_CREATEFOLDER + " <br/>";
                    this.makeAjaxRequest(url, data, errorMessage, successFunction);
                };
                AjaxRequest.prototype.makeAjaxRequest = function (url, data, errorMessage, successFunction) {
                    var request = $.ajax({
                        type: "POST",
                        url: url,
                        data: data,
                        dataType: "json"
                    });
                    request.done(successFunction);
                    request.fail(function (jqXHR, textStatus, errorThrown) {
                        // Logged out since the call was made
                        if (errorThrown.message.indexOf("unexpected character") !== -1) {
                            window.location.href = window.location.href;
                        }
                        else {
                            toastr.error(errorMessage + errorThrown);
                        }
                    });
                };
                return AjaxRequest;
            }());
            FileManager.AjaxRequest = AjaxRequest;
        })(FileManager = Web.FileManager || (Web.FileManager = {}));
    })(Web = Roadkill.Web || (Roadkill.Web = {}));
})(Roadkill || (Roadkill = {}));
//# sourceMappingURL=ajaxrequest.js.map