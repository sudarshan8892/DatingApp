$(document).ready(function () {
    // Initialize variables
    var queueLength = 0;
    var queueProgress = 0;

    // Function to update queue length
    function updateQueueLength() {
        $("#queueLength").text(queueLength);
        if (queueLength > 0) {
            $(".Upload-queue").show();
        } else {
            $(".Upload-queue").hide();
        }
    }


    // Function to update queue progress
    function updateQueueProgress() {
        $("#queueProgressBar").css("width", queueProgress + "%");
    }

    // Function to add file to upload table
    function addFileToUploadTable(file) {
        $("#uploadTableBody").append('<tr><td><strong>' + file.name + '</strong></td><td>' + (file.size / 1024 / 1024).toFixed(2) + ' MB</td><td nowrap><button type="button" class="btn btn-danger btn-xs"><span class="fa fa-trash"></span></button></td></tr>');
    }


    $("#baseDropZone, #anotherDropZone").on('dragover', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).addClass('nv-file-over');
    });

    $("#baseDropZone, #anotherDropZone").on('dragleave', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).removeClass('nv-file-over');
    });

    $("#baseDropZone, #anotherDropZone").on('drop', function (e) {
        if (e.originalEvent.dataTransfer) {
            if (e.originalEvent.dataTransfer.files.length) {
                e.preventDefault();
                e.stopPropagation();
                $(this).removeClass('nv-file-over');
                $.each(e.originalEvent.dataTransfer.files, function (index, file) {
                    addFileToUploadTable(file);
                    queueLength++;
                    updateQueueLength();
                });
            }
        }
    });


    // Function to handle file selection
    $("#multipleFileSelect").change(function () {
        var files = $(this)[0].files;
        $.each(files, function (index, file) {
            addFileToUploadTable(file);
            queueLength++;
            updateQueueLength();
        });
    });
    $("#singleFileSelect").change(function () {
        var files = $(this)[0].files;
        $.each(files, function (index, file) {
            addFileToUploadTable(file);
            queueLength++;
            updateQueueLength();
        });
    });


    // Function to handle remove file
    $(document).on("click", ".btn-danger", function () {
        $(this).closest('tr').remove();
        queueLength--;
        updateQueueLength();

    });


    // Function to handle clear queue
    $("#clearQueueBtn").click(function () {
        $("#uploadTableBody").empty();
        queueLength = 0;
        queueProgress = 0;
        updateQueueLength();
        updateQueueProgress();
    });

    // Function to handle file upload
    $(document).on("click", ".btn-success", function () {
        // Implement file upload logic
    });

    // Function to handle cancel upload
    $(document).on("click", ".btn-warning", function () {
        // Implement cancel upload logic
    });





    // Function to handle cancel all
    $("#cancelAllBtn").click(function () {
        // Implement cancel all logic
    });

    // Function to handle upload all
    $("#uploadAllBtn").click(function () {
        var formData = new FormData();

        var multipleFiles = $('#multipleFileSelect')[0].files;
        for (var i = 0; i < multipleFiles.length; i++) {
            formData.append('files', multipleFiles[i]); // Append each file to FormData
        }

        var singleFile = $('#singleFileSelect')[0].files[0];
        if (singleFile) {
            formData.append('files', singleFile); // Append the single file to FormData
        }

        $.ajax({
            url: '/Home/UploadPhotos',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            dataType:'html',

            success: function (result) {
                /*
                var newPhotoHtml = `
                        <div class="col-md-3 mb-1 mt-3">
                            <img src="${result.url}" alt="photo of Users" class="img-thumbnail mb-1">
                            <div class="text-center justify-content-between align-items-center">
                                <button class="btn ${result.isMain ? 'btn-success' : 'btn-outline-success'}" ${result.isMain ? 'disabled' : ''}
                                  onclick="SetMainPhoto('@photo.Url', '@photo.Id', '@photo.IsMain')">
                                    Main
                                </button>
                                <button class="btn btn-sm btn-danger"> <i class="fas fa-trash"></i></button>
                            </div>
                        </div>
                    `;
                $('#photos .row.mb-3').append(newPhotoHtml);*/
                $("#upload").html(result);
                toastr.success('File uploaded successfully');
                queueLength = 0;
                queueProgress = 0;
                updateQueueLength();

            },
            error: function (xhr, status, error) {
                console.error('Error uploading file:', error);

            }
        });

    });
    updateQueueLength();

 
});


function SetMainPhoto(photoUrl, photoId, isMain, button) {
    var data = {
        Id: parseInt(photoId),
        Url: photoUrl,
        IsMain: Boolean(isMain)
    };
    button.disabled = true;
    $.ajax({
        url: '/Home/SetMainPhoto',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        data: JSON.stringify((data)),
      
        success: function (response) {
            
            $('#photoContainer').html(response)
            toastr.success('Photo set as main successfully');
            button.disabled = false;
        },
           
        
        error: function (xhr, status, error) {
            debugger
            toastr.error(xhr.responseText);
        }
    });
}