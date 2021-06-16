
(function ($) {
    "use strict";


    /*==================================================================
    [ Focus Contact2 ]*/
    $('.input100').each(function(){
        $(this).on('blur', function(){
            if($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })    
    })
  
  
    /*==================================================================
    [ Validate ]*/
    var baslik = $('.validate-input input[name="baslik"]');
    var dosya = $('.validate-input input[name="dosya"]');
    var tip = $('.validate-input input[name="tip"]');




    $('.validate-form').on('submit', function () {
        var selectedVal = $('#SunucuMu').find(":selected").text();
        if (selectedVal == "Sunucu") {
            var check = true;

            if ($(baslik).val().trim() == '') {
                showValidate(baslik);
                check = false;
            }


            if ($(dosya).val().trim() == '') {
                showValidate(dosya);
                check = false;
            }

            if ($(tip).val().trim() == '') {
                showValidate(tip);
                check = false;
            }

            return check;
        }
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
       });
    });

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    
    

})(jQuery);