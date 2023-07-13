$(document).ready(function () {
    // Show Data in Table
    let num = 0;
    let selectedEmployeeId = null; // Variable to store the selected employee ID for update

    const employeeTable = $('#employees-table').DataTable({
        ajax: {
            url: "https://localhost:7114/api/Employee",
            dataType: "JSON",
            dataSrc: "data",
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

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
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
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
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
                //console.log('Employee ' + (selectedEmployeeId ? 'updated' : 'created') + ' successfully:', response);
                Swal.fire({
                    icon: 'success',
                    title: 'Employee Data has been ' + (selectedEmployeeId ? 'updated' : 'created'),
                    showConfirmButton: false,
                    timer: 1500
                })

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
                console.log('Error ' + (selectedEmployeeId ? 'updating' : 'creating') + ' employee:', error)
                Swal.fire({
                    icon: 'error',
                    title: 'Employee Data has not been ' + (selectedEmployeeId ? 'updated' : 'created'),
                    showConfirmButton: false,
                    timer: 1500
                })
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

// Get all data from the DataTable
let num = 0;
let selectedEmployeeId = null; // Variable to store the selected employee ID for update

let tableData = []; // Variable to store all data from the API

fetch("https://localhost:7114/api/Employee")
    .then(response => response.json())
    .then(data => {
        tableData = data.data;
        console.log(tableData); // Output the data to the console

        // Perform operations on the data as needed

        // Call a function to generate the chart using the data
        generateGenderChart(tableData);
    })
    .catch(error => {
        console.error("Error fetching data:", error);
    });

let genderChart;

function generateGenderChart(data) {
    if (Array.isArray(data)) {
        const maleCount = data.filter(item => item.gender === 1).length;
        const femaleCount = data.filter(item => item.gender === 0).length;

        console.log(maleCount);
        console.log(femaleCount);

        const ctx = document.getElementById('genderChart').getContext('2d');

        if (genderChart) {
            genderChart.data.datasets[0].data = [maleCount, femaleCount];
            // Re-render the chart
            genderChart.update();
        } else {
            genderChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ['Male', 'Female'],
                    datasets: [{
                        data: [maleCount, femaleCount],
                        backgroundColor: ['blue', 'pink'],
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false, // Set maintainAspectRatio to false
                    width: 400, // Set the width of the chart
                    height: 400, // Set the height of the chart
                },
            });
        }
    } else {
        console.error('Invalid data format:', data);
    }
}

// After a successful creation of a new employee or update
generateGenderChart(tableData);



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
