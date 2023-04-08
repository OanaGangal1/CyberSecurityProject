"use strict"

const deleteFile = function (el) {
    $.ajax({
        type: 'Delete',
        url: 'Document/DeleteFile',
        data: { "id": el.value },
        beforeSend: function (xhr) {
            let token = localStorage.getItem("AccessToken");
            xhr.setRequestHeader('Authorization', token);
        },
        success: function (data) {
            toastr.success(data);
            let file = document.getElementById(el.value);
            file.parentNode.removeChild(file);
        },
        error: function (data) {
            toastr.error(data.responseText);
        }
    });
}

const init = function ()
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

    $("#search-file").on('submit', function (e) {
        e.preventDefault();
        let token = localStorage.getItem("AccessToken");
        $.ajax({
            type: 'Get',
            url: 'Document/GetFile',
            data: { "fileName": $("#search-file input").val()},
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', token);
            },
            success: function (data) {
                console.log(data)
                let table = document.getElementById("file-table");
                $("#file-table tr").remove(); 

                data.forEach(el =>
                {
                    let row = table.insertRow(0);
                    row.setAttribute("id", el["id"]);
                    let cell1 = row.insertCell(0);
                    let cell2 = row.insertCell(1);
                    let cell3 = row.insertCell(2);
                    cell3.classList.add("d-flex")
                    cell1.innerHTML = el["fileName"];
                    cell2.innerHTML = el["fileType"]
                    cell3.innerHTML = "<a href='/Document/Download?token=" + token + "&id=" + el["id"] + "' class='btn btn-success me-3' download='" + el["fileName"] + "'>Download</a>" +
                    "<button type='button' class='btn btn-danger' onclick='deleteFile(this)' value='" + el["id"] + "'>Delete</button>"
                });
            },
            error: function (data) {
                toastr.error(data.responseText);
            }
        });
    });
}();