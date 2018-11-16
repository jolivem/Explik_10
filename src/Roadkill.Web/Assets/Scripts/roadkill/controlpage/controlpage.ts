/// <reference path="../typescript-ref/references.ts" />
module Roadkill.Web {
    export class ControlPage {
        private _timeout: any = null;
        private _tagBlackList: string[] =
            [
                "#", ",", ";", "/", "?", ":", "@", "&", "=", "{", "}", "|", "\\", "^", "[", "]", "`"
            ];

        constructor(tags: string[]) {
            // Setup tagmanager
            this.initializeTagManager(tags);
        }

		/**
		Sets up the Bootstrap tag manager
		*/
        private initializeTagManager(tags: string[]) {
            // Use jQuery UI autocomplete, as typeahead is currently broken for BS3
            $("#TagsEntry").autocomplete({
                source: tags
            });

            $("#TagsEntry").tagsManager({
                tagClass: "tm-tag-success",
                blinkBGColor_1: "#FFFF9C",
                blinkBGColor_2: "#CDE69C",
                delimeters: [44, 186, 32, 9], // comma, ";", space, tab
                output: "#RawTags",
                preventSubmitOnEnter: false,
                validator: (input: string) => {
                    var isValid: Boolean = this.isValidTag(input);
                    if (isValid === false) {
                        toastr.error("The following characters are not valid for tags: <br/>" + this._tagBlackList.join(" "));
                    }

                    return isValid;
                }
            });

            $("#TagsEntry").keydown((e) => {
                // Tab adds the tag, but then focuses the toolbar (the next tab index)
                var code = e.keyCode || e.which;
                if (code == "9") {
                    var tag: string = $("#TagsEntry").val();
                    if (this.isValidTag(tag)) {
                        if ($("#IsLocked").length == 0)
                            $(".wysiwyg-bold").focus();
                        else
                            $("#IsLocked").focus();
                    }
                    return false;
                }

                return true;
            });

            $("#TagsEntry").blur(function (e) {
                // Push the tag when focus is lost, e.g. Save is pressed
                $("#TagsEntry").tagsManager("pushTag", $("#TagsEntry").val());

                // Fix the tag's styles from being blank
                $(".tm-tag-remove").each(function () {
                    $(this).html("&times;");
                });
                $(".tm-tag").each(function () {
                    $(this).addClass("tm-tag-success");
                    $(this).addClass("tm-success");
                });
            });
        }

		/**
		 Returns false if the tag contains any characters that are blacklisted.
		*/
        private isValidTag(tag: string): Boolean {
            for (var i: number = 0; i < tag.length; i++) {
                if ($.inArray(tag[i], this._tagBlackList) > -1) {
                    return false;
                }
            }

            return true;
        }
    }
}