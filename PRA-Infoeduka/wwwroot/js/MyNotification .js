var dataTable;

$(() => {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblMyNotifications').DataTable({
        "ajax": {
            "url": "/Professor/MyNotification/GetAllMyNotifications"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "course.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Professor/MyNotification/EditMyNotification?id=${data}" class="btn btn-primary mx-2"> Edit</a>
                        <a onclick=Delete('/Professor/MyNotification/DeleteMyNotification/${data}') class="btn btn-danger mx-2"> Delete</a>
                    </div> `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure you want delete this notification?',
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