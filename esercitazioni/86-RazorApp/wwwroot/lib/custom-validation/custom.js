$(document).ready(function () {
    // Focus the first input field
    $("input").first().focus();

    // Validation logic
    $("input").on("blur keyup", function () {
        var $input = $(this);
        var $iconSuccess = $input.siblings(".icon").find(".icon-success");
        var $iconError = $input.siblings(".icon").find(".icon-error");

        if ($input.val().trim() === "") {
            // If the field is empty
            $input.removeClass("is-valid").addClass("is-invalid");
            $iconSuccess.hide();
            $iconError.show();
        } else {
            // If the field is filled
            $input.removeClass("is-invalid").addClass("is-valid");
            $iconError.hide();
            $iconSuccess.show();
        }
    });
});