$(document).ready(function () {
    $("form").validate({
        errorPlacement: function (error, element) {
            //var t = $(element).closest("table").siblings("span");
            var t = $(element).closest("li").find(".question>label");
            error.insertAfter(t);
        }
    });
    $.validator.messages.required = "* Indicare una risposta";

    $('form').submit(function () {
        if ($(this).valid()) {
            $('input[type="submit"]').attr('disabled', 'disabled');
        }
    });
});