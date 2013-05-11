$(function () {
    $('#btn').click(function (e) {
        e.preventDefault();
        $(this).next().click();
    });

    $('#avatar').change(function () {
        var file = $(this)[0].files[0];
        if (file)
            $('#avatar-name').html(file.name);
        else
            $('#avatar-name').html('');
    });
});