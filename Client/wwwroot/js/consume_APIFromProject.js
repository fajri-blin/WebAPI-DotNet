$(document).ready(function () {
    // Show Data in Table
    let num = 0;
    let selectedEmployeeId = null; // Variable to store the selected employee ID for update

    const employeeTable = $('#employees-table').DataTable({
        ajax: {
            url: "https://localhost:7114/api/Employee",
            dataType: "JSON",
            dataSrc: "data"
        },
        dom: "<'row'<'col-sm-6'lB><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                extend: 'colvis',
                text: 'Column Visibility',
                className: 'btn-secondary',
            },
            {
                extend: 'excel',
                text: 'Export Excel',
                title: 'EmployeeExcel',
                className: 'btn-secondary',
                exportOptions: {
                    columns: [...Array(8).keys()]
                },
            },
            {
                extend: 'pdf',
                text: 'Export PDF',
                title: 'EmployeePDF',
                className: 'btn-secondary',
                exportOptions: {
                    columns: [...Array(8).keys()]
                },
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn-secondary',

                exportOptions: {
                    columns: ':visible',
                },
            },
            {
                text: 'Create Employee',
                className: 'btn-success',
                action: () => {
                    $('#employeeModal').modal('show');
                }
            }
        ],
        columns: [
            {
                data: "",
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            { data: "nik" },
            {
                data: null,
                render: (data, type, row) => {
                    return row.firstName + " " + row.lastName
                }
            },
            {
                data: "birthDate",
                render: (data, type, row) => {
                    return moment(data).format('D MMMM YYYY');
                }
            },
            {
                data: "gender",
                render: (data, type, row) => {
                    return gender(data);
                }
            },
            {
                data: "hiringDate",
                render: (data, type, row) => {
                    return moment(data).format('D MMMM YYYY');
                }
            },
            { data: "email" },
            { data: "phoneNumber" },
            {
                data: null,
                render: (data, type, row) => {
                    return `<button data-guid="${row.guid}" class="btn btn-danger delete-button">Delete</button>` +
                        `<button data-guid="${row.guid}" class="btn btn-warning update-button">Update</button>`;
                }
            }
        ],
    });
    $('.dt-buttons .dt-button').removeClass('dt-button').addClass('btn');

    // Event delegation for delete buttons
    $('#employees-table').on('click', '.delete-button', function () {
        var employeeId = $(this).data('guid');

        // Perform the delete action
        $.ajax({
            url: `https://localhost:7114/api/Employee?guid=${employeeId}`,
            type: 'DELETE',
            success: function (response) {
                console.log('Employee deleted successfully');
                // Perform any additional actions after successful deletion

                // Refresh the DataTable
                employeeTable.ajax.reload();
            },
            error: function (error) {
                console.error('Error deleting employee:', error);
                // Handle error condition
            }
        });
    });

    // Event delegation for update buttons
    $('#employees-table').on('click', '.update-button', function () {
        selectedEmployeeId = $(this).data('guid');
        const employeeData = employeeTable.row($(this).parents('tr')).data();

        // Set the form values
        $('#inputNIK').val(employeeData.nik);
        $('#inputFirstname').val(employeeData.firstName);
        $('#inputLastname').val(employeeData.lastName);
        $('#inputBirthDate').val(moment(employeeData.birthDate).format('YYYY-MM-DD'));
        $('#inputGenderMale').prop('checked', employeeData.gender === 1);
        $('#inputGenderFemale').prop('checked', employeeData.gender === 0);
        $('#inputHiringDate').val(moment(employeeData.hiringDate).format('YYYY-MM-DD'));
        $('#inputEmail').val(employeeData.email);
        $('#inputPhone').val(employeeData.phoneNumber);

        // Show the modal
        $('#employeeModal').modal('show');
    });

    // Submit form event handler
    $('#employeeForm').submit(function (event) {
        event.preventDefault(); // Prevent the default form submission behavior

        // Get the form values
        var nik = $('#inputNIK').val();
        var firstName = $('#inputFirstname').val();
        var lastName = $('#inputLastname').val();
        var birthDate = $('#inputBirthDate').val();
        var gender = $('input[name="inputGender"]:checked').val();
        var hiringDate = $('#inputHiringDate').val();
        var email = $('#inputEmail').val();
        var phoneNumber = $('#inputPhone').val();

        // Create the employee object
        var employeeData = {
            guid: selectedEmployeeId ? selectedEmployeeId : undefined,
            nik: nik,
            firstName: firstName,
            lastName: lastName,
            birthDate: birthDate,
            gender: gender === 'male' ? 1 : 0,
            hiringDate: hiringDate,
            email: email,
            phoneNumber: phoneNumber
        };

        // Determine the HTTP method based on whether an employee ID is selected
        var httpMethod = selectedEmployeeId ? 'PUT' : 'POST';
        var apiUrl = selectedEmployeeId ? `https://localhost:7114/api/Employee` : 'https://localhost:7114/api/Employee';

        // Send the employee data to the API
        $.ajax({
            url: apiUrl,
            type: httpMethod,
            contentType: 'application/json',
            data: JSON.stringify(employeeData),
            success: function (response) {
                // Handle the success response
                console.log('Employee ' + (selectedEmployeeId ? 'updated' : 'created') + ' successfully:', response);

                // Reset the form and selectedEmployeeId
                $('#employeeForm')[0].reset();
                selectedEmployeeId = null;

                // Close the modal
                $('#employeeModal').modal('hide');

                // Refresh the DataTable
                employeeTable.ajax.reload();
            },
            error: function (xhr, status, error) {
                // Handle the error response
                console.log('Error ' + (selectedEmployeeId ? 'updating' : 'creating') + ' employee:', error);
                // Show an error message or perform any other error handling
            }
        });
    });

    // Hide the modal when it is closed
    $('#employeeModal').on('hidden.bs.modal', function () {
        // Reset the form and selectedEmployeeId
        $('#employeeForm')[0].reset();
        selectedEmployeeId = null;
    });

});

function gender(num) {
    let gender = ""
    switch (num) {
        case 0:
            gender = "Female"
            break;
        case 1:
            gender = "Male"
            break;
    }
    return gender;
}
