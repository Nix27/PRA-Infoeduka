var dataTable;

$(() => {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblCourses').DataTable({
        "ajax": {
            "url": "/Admin/Course/GetAllCourses"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "ects", "width": "15%" },
            { "data": "lectures", "width": "15%" },
            { "data": "exercises", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Course/EditCourse?id=${data}" class="btn btn-primary mx-2"> Edit</a>
                        <a onclick=Delete('/Admin/Course/DeleteCourse/${data}') class="btn btn-danger mx-2"> Delete</a>
                    </div> `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure you want delete this course?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Deleted successfuly!',
                            'success'
                        )
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Unable to delete!'
                        })
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Unable to delete!'
                    })
                }
            })
        }
    });
}