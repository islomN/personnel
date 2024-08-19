$(document).ready(function() {
    $('#import').on('change', function(event) {
        event.preventDefault();

        let formData = new FormData(this);

        $("#loading").show();
        $.ajax({
            url: '/Personnel/Import/',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function(response) {
                alert("Success count:" + response.count);
                location.reload();
            },
            error: function(error) {
                alert("Error message:" + error.responseJSON.message);
            },
            complete: function(){
                $("#loading").hide();
            }
        });
    });
});