InitializeTable();

function InitializeTable() {
    var tableName = "#CustomersTable";

    if ($.fn.DataTable.isDataTable(tableName)) {
        let table = $(tableName).DataTable();
        table.clear().draw();
        table.destroy();
    }

    $(tableName + '>tbody').empty();
    $(tableName + '>thead').empty();
    $(tableName + '>tfoot').empty();


    var newTable = $(tableName).DataTable({
        "ordering": true,
        dom: 'lBfrtip',
        language: {
            "processing": "Please wait..."
        },
        processing: true,
        "deferRender": true,
        ajax: {
            url: "/api/Customers/Index",
            type: "GET",
            dataSrc: ''
        }, "displayLength": 25,

        columns: [
            {
                "data": "id",
                "title": "Id"
            },
            {
                "data": "name",
                title: "Name"
            },
            {
                "data": null, render: function (data, type, row) {
                    return data.customerType.name
                },
                title: "Customer Type"
            },
            {
                "data": "address",
                title: "Address",
                className: "none"
            },
            {
                "data": "description",
                title: "Description",
                className: "none",
                defaultContent: ""
            },
            {
                "data": "city",
                title: "City"
            },
            {
                "data": "state",
                title: "State"
            },
            {
                "data": "zip",
                title: "Zip"
            },
            {
                "data": "lastUpdated",
                title: "Last Updated"
            },
            {
                "data": null, render: function (data, type, row) {
                    return '<button class="btn" onclick="openEditModal(' + data.id + ')" type="button"><i class="text-primary bi bi-pencil-square"></i></button>'
                        + '<button class="btn" onclick="openDeleteModal(' + data.id + ',\'' + data.name + '\')" type="button"><i class="text-danger bi bi-trash-fill"></i></button>'
                },
            }
        ],
        "columnDefs":
            [
                {
                    render: function (data, type, row) {
                        // If display or filter data is requested, format the date
                        if (type === 'display') {
                            return moment.utc(data).format("LLL");
                        } else if (type == 'filter') {
                            return moment.utc(data).format("LLL");
                        }
                        return moment.utc(data).valueOf();
                    },
                    targets: 8
                },
            ],

    });
}

function openEditModal(id) {
    $.ajax({
        url: '/Home/GetEditModal',
        data: { Id: id },
        dataType: "text",
        success: function (data) {
            if (data != null) {
                $("body").append(data);
                $(".modal:last").modal('show');
            }
        }
    });
}

$("#CreateForm").submit(function (e) {
    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            if (data != null && data.responseJSON != null && data.responseJSON.errors != null) {
                $(".alert").addClass("alert-danger");
                $(".alert").removeClass("alert-success");

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-x-circle-fill");
                $("#AlertText").text("Error in creating Customer");

                $("#CreateAlertText").text(JSON.stringify(data.responseJSON.errors));
            } else {
                $(".alert").addClass("alert-success");
                $(".alert").removeClass("alert-danger");
                InitializeTable();

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-check-circle-fill");
                $("#AlertText").text("Customer created successfully.");

                var createModalEL = document.getElementById('CreateModal')
                var createModal = bootstrap.Modal.getInstance(createModalEL) // Returns a Bootstrap modal instance
                createModal.hide();
            }
            $(".alert").addClass('show');


            setTimeout(function () {
                $(".alert").hide();
            }
                , 5000);
        },
        error: function (data) {
            if (data.responseJSON != null && data.responseJSON.errors != null) {
                $(".alert").addClass("alert-danger");
                $(".alert").removeClass("alert-success");

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-x-circle-fill");
                $("#AlertText").text("Error in creating Customer");
                $(".alert").addClass('show');

                $("#CreateAlertText").text(JSON.stringify(data.responseJSON.errors));
            }
        }
    });
});
$('body').on('submit', '#EditForm', function (e) {
    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "PUT",
        url: actionUrl,
        data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            if (data != null && data.responseJSON != null && data.responseJSON.errors != null) {
                $(".alert").addClass("alert-danger");
                $(".alert").removeClass("alert-success");

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-x-circle-fill");
                $("#AlertText").text("Error in updating Customer");

                $("#CreateAlertText").text(JSON.stringify(data.responseJSON.errors));
            } else {
                $(".alert").addClass("alert-success");
                $(".alert").removeClass("alert-danger");
                InitializeTable();

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-check-circle-fill");
                $("#AlertText").text("Customer updated successfully.");
            }

            $(".alert").addClass('show');
            $("#EditModal" + data.id).hide();
            $("#EditModal" + data.id).remove();

            setTimeout(function () {
                $(".alert").hide();
            }
                , 5000);

        }
    });
});

function openDeleteModal(id, name) {
    $("#DeleteForm").attr("action", "/api/Customers/DeleteCustomer/" + id)
    $("#DeleteName").val(name);
    $("#DeleteId").val(id);
    $("#DeleteModal").modal('show');
};

$("#DeleteForm").submit(function (e) {
    e.preventDefault(); // avoid to execute the actual submit of the form.

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "DELETE",
        url: actionUrl,
        //data: form.serialize(), // serializes the form's elements.
        success: function (data) {
            if (data != null && data.responseJSON != null && data.responseJSON.errors != null) {
                $(".alert").addClass("alert-danger");
                $(".alert").removeClass("alert-success");

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-x-circle-fill");
                $("#AlertText").text("Error in deleting Customer");

                $("#CreateAlertText").text(JSON.stringify(data.responseJSON.errors));
            } else {
                $(".alert").addClass("alert-success");
                $(".alert").removeClass("alert-danger");
                InitializeTable();

                $("#AlertIcon").removeClass();
                $("#AlertIcon").addClass("bi bi-check-circle-fill");
                $("#AlertText").text("Customer deleted successfully.");
            }
            $(".alert").addClass('show');

            $("#DeleteModal").hide();

            setTimeout(function () {
                $(".alert").hide();
            }
                , 5000);
        }
    });
});

// Readjust column widths when changing tabs
$(document).on('shown.bs.tab', 'button[data-bs-toggle="tab"]', function (e) {
    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
});