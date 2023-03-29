"use strict"

const onUpload = function ()
{
    $("#document-form").on('submit', function (e) {
        let formData = new FormData();
        let fileInput = $("#document-form")[0];
        formData.append('file', fileInput[0].files[0]);
        
        e.preventDefault();
        $.ajax({
            type: 'Post',
            url: 'Document/Index',
            encType: 'multipart/form-data',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            beforeSend: function (xhr) {
                let token = localStorage.getItem("AccessToken");
                xhr.setRequestHeader('Authorization', token);
            },
            success: function (data) {
                toastr.success(data);
            },
            error: function (data) {
                toastr.error(data.responseText);
            }
        });
    });
}();