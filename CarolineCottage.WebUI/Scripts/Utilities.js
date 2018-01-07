/// <reference path="Utilities.js" />

function docReadyUtilityFunctions() {

    jQuery.fn.ForceNumericOnly = function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                var ctrlMode = e.ctrlKey;
                var vKey = 86;
                var cKey = 67;
                // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                // RETURN (13) added
                // ctrl v added
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == vKey && ctrlMode ||
                    key == cKey && ctrlMode ||
                    (key >= 37 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            });
        });
    };

}