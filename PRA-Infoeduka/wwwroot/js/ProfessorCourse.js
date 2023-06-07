var dataTable;

$(() => {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblProfessorCourses').DataTable({
        "ajax": {
            "url": "/Professor/ProfessorCourse/GetAllProfessorCourses"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "ects", "width": "15%" },
            { "data": "lectures", "width": "15%" },
            { "data": "exercises", "width": "15%" }
        ]
    });
}