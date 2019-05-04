
(function ($) {
    "use strict";
 
    /*==================================================================
    [ Validate ]*/

    $('.validate-form').on('invalid-form', function () {
        var self = this;
        debugger;
        var $validatr = $('form').data('validator');
        var errorsList = $validatr.errorList;
        for (var i = 0; i < errorsList.length; i++) {
            showValidate(errorsList[i].element, errorsList[i].message);
        }
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
        });
    });

    function showValidate(input, msg) {
        var thisAlert = $(input).parent();
        $(thisAlert).attr('data-validate', msg);
        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
})(jQuery);