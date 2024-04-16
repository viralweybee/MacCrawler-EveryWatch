// Code : Viral Devmurari
// Date : 29/02/2024
import { fuzzySearch } from "./fuzzySearch.js";
var tableEl = document.getElementById('table');
var manufacutreIdEl = document.getElementById('manufactureId');
var manufactureNameEl = document.getElementById('manufactureName');
var manufactureCountEl = document.getElementById('manufactureCount');
const addManuallyEl = document.getElementById('addManually');
const saveBtnEl = document.getElementById('save');
const fileinputEl = document.getElementById('file');
const addEl = document.getElementById('add');
const googlesheetEl = document.getElementById('googleSheet');
const columnEl = document.getElementById('column')
const rowEl = document.getElementById('row');
const deletedataEl = document.getElementById('delete_data');
const confirmDeleteEl = document.getElementById('confirm_delete');


//declaring global data
var data;
var editFlag;

//Fetch Initally when page load
function KnownManufacturers() {
    var apiUrl = "https://localhost:44349/api/Sheet1";
    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(apiData => {

            console.log("API Response:", apiData);
            data = apiData;
            fillDataTable(data)
        })
        .catch(error => {
            console.error("Error:", error);
        });
}
KnownManufacturers()

//fill the data when api is fully loaded
function fillDataTable(data) {
    var columns = [];
    if (data.length > 0) {
        Object.keys(data[0]).forEach(function (key) {
            columns.push({ data: key });
        });
    }
    var actionColumnExists = columns.some(column => column.data === 'Action');
    if (!actionColumnExists) {
        columns.push({
            data: 'Action',
            render: function (data, type, row, meta) {
                return '<div class=btn1>' +
                    `<button class="edit-btn btn btn-warning" data-row-id="${row.pid}">Edit</button>` +
                    `<button class="delete-btn btn btn-danger" data-row-id="${row.pid}">Delete</button>` +
                    '</div>';
            }
        });
    }
    var table = $('#example').DataTable({
        data: data,
        columns: columns,
        "bDestroy": true
    });
    var thead = $('#example thead');
    thead.empty();
    var headerRow = $('<tr>');
    columns.forEach(function (column) {
        // Convert the first letter of each word to uppercase
        var columnHeader = column.title || column.data;
        columnHeader = columnHeader.replace(/\b\w/g, l => l.toUpperCase());
        headerRow.append('<th>' + columnHeader + '</th>');
    });
    thead.append(headerRow);
    table.destroy();
    $('#example').DataTable({
        data: data,
        columns: columns,
        "bDestroy": true
    });
}
//Making working button of edit and delete
document.addEventListener('click', function (event) {
    if (event.target.classList.contains('edit-btn')) {
        var rowId = event.target.getAttribute('data-row-id');
        console.log(rowId)
        editRow(parseInt(rowId));
    }

    else if (event.target.classList.contains('delete-btn')) {
        var rowId = event.target.getAttribute('data-row-id');
        deleteRow(parseInt(rowId));
    }
    else if (event.target.classList.contains('btn2')) {
        var row = event.target.closest('tr');
        const addingdata = {};
        if (row.cells[2].textContent == 'NO') {
            var newId = data.reduce((maxId, item) => Math.max(maxId, item.id), 0) + 1;
            addingdata.id = newId
            addingdata.name = row.cells[1].textContent
            addingdata.count = null
        }
        else {
            addingdata.id = parseInt(row.cells[0].textContent);
            addingdata.name = row.cells[1].textContent
            addingdata.count = null
        }
        addingDataManually(addingdata);

    }
});

var editId;
//Function of edit open and close
function editRow(id) {
    editFlag = true;
    console.log(id);
    var rowData = data.find(row => row.pid === id);

    // Fill the modal input fields with row data
    manufacutreIdEl.value = rowData.id;
    manufactureNameEl.value = rowData.name;
    manufactureCountEl.value = rowData.count;
    document.querySelector('.conflict-msg').classList.remove('show');
    document.querySelector('.success-msg').classList.remove('show');
    manufactureNameEl.classList.remove('is-invalid')
    saveBtnEl.disabled=false;
    $('#exampleModal').modal('show');
    editId = id;
}

//Function of delete button written over itself--------------------------pending
function deleteRow(id) {
    fetch(`https:localhost:44349/api/Sheet1/${id}`, {
        method: 'GET',
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error(`Http error! Status: ${response.status}`);
        })
        .then(data => {
            deletedataEl.innerHTML = `Id : <b>${data.id}</b> and Manufacturer Name : <b>${data.name}</b>`
            $('#DeleteModal').modal('show');
        })
        .catch(error => {
            console.error(`Error : ${error}`);
        })

    confirmDeleteEl.addEventListener('click', () => {
        fetch(`https://localhost:44349/api/Sheet1/${id}`, {
            method: 'DELETE',
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                removeRowFromFrontend(id);
                $('#DeleteModal').modal('hide');
                console.log(`Row with ID ${id} deleted successfully.`);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    })
    //There is a bug in this-----------------------------
    // bug is if i click on frontend delete then close modal again open the modal again close then click on delete then there is 3 time total this function call happens
}

function removeRowFromFrontend(id) {
    var rowIndex = data.findIndex(row => row.pid === id);

    if (rowIndex !== -1) {
        $('#example').DataTable().row(rowIndex).remove().draw();
        data.splice(rowIndex, 1);
    } else {
        console.warn(`Row with ID ${id} not found in the frontend.`);
    }
}

addManuallyEl.addEventListener('click', () => {
    //when ever add button click first of all set to empty
    manufacutreIdEl.value = '';
    manufactureNameEl.value = '';
    manufactureCountEl.value = '';
    document.querySelector('.conflict-msg').classList.remove('show');
    document.querySelector('.success-msg').classList.remove('show');
    manufactureNameEl.classList.remove('is-invalid')
    saveBtnEl.disabled=false;
    editFlag = false;
})

//Adding data manually
function addingDataManually(addingdata) {
    if (addingdata.name === '' && addingdata.id == '') {
        return 'not valid';
    }
    const apiUrl = 'https://localhost:44349/api/Sheet1';


    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(addingdata)
    })
        .then(response => {
            if (response.status === 409) {
                document.querySelector('.conflict-msg').classList.add('show');
                manufactureNameEl.classList.add('is-invalid')
            }
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            if (response.ok) {
                document.querySelector('.success-msg').classList.add('show');
                saveBtnEl.disabled=true;
            }
            return response.json();
        })
        .then(apidata => {

            var tempdata = {}
            tempdata = apidata;
            addRowInFrontend(tempdata);
            // Find and remove the corresponding row from the table
            var table = document.getElementById('popup-table');
            var rows = table.getElementsByTagName('tr');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].cells[0].textContent == tempdata.id.toString()) {
                    table.deleteRow(i);
                    break;
                }
            }

            data.push(tempdata);
            console.log(apidata)
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
//save button 
saveBtnEl.addEventListener('click', () => {
    if (!editFlag) {
        const addingdata = {};
        if (manufactureNameEl.value.trim() == '') {
            manufactureNameEl.classList.add('is-invalid')
            return;
        }
        addingdata.id = fuzzy(manufactureNameEl.value)
        addingdata.name = manufactureNameEl.value.trim()
        addingdata.count = manufactureCountEl.value.trim()
        addingDataManually(addingdata);
        
    }
    else {
        editingDataManually(editId);
       
    }
})

function editingDataManually(editId) {
    const modifiedData = {};
    modifiedData.pid = editId;
    modifiedData.id = parseInt(manufacutreIdEl.value);
    modifiedData.name = manufactureNameEl.value;
    modifiedData.count = manufactureCountEl.value;

    fetch(`https://localhost:44349/api/Sheet1/${editId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(modifiedData),
    })
        .then(response => {
            if (response.status === 409) {
                document.querySelector('.conflict-msg').classList.add('show');
                manufactureNameEl.classList.add('is-invalid')
            }
            if (response.ok) {
                document.querySelector('.success-msg').classList.add('show');
                saveBtnEl.disabled=true;
            }
        })
        .then(() => {
            // Find the corresponding data item and update it
            for (var i = 0; i < data.length; i++) {
                if (data[i].pid === editId) {
                    data[i].id = modifiedData.id;
                    data[i].name = modifiedData.name;
                    data[i].count = modifiedData.count;
                    updateRowInFrontend(data[i], i);
                    break;
                }
            }
            
        })
        .catch(error => {
            console.error('Error:', error);
        });
}


//Add Data by Path
addEl.addEventListener('click', add);
function add() {
    var arrayOfArrays = [];
    var fileInput = fileinputEl.files[0];
    var googleSheetUrl = googlesheetEl.value.trim();
    console.log(googleSheetUrl)
    if (fileInput && googleSheetUrl !== '') {
        console.log(2);
    }
    else if (fileInput && googleSheetUrl === '') {
        var reader = new FileReader();

        reader.onload = function (e) {
            var fileContent = e.target.result;
            // Use Papaparse to parse CSV content
            Papa.parse(fileContent, {
                header: false,
                skipEmptyLines: true,
                complete: function (results) {
                    arrayOfArrays = results.data;
                    addAll(arrayOfArrays);
                },
                error: function (error) {
                    console.error("Error parsing CSV:", error);
                }
            });
        };
        reader.readAsText(fileInput);
    } else if (googleSheetUrl !== '') {
        googleSheetData();
    } else {
        console.error('No file or Google Sheet URL is selected');
    }
}
function addAll(arr) {

    var row = rowEl.value === '' ? 1 : parseInt(rowEl.value);
    row--;
    var column = columnEl.value === '' ? 'a'.charCodeAt(0) - 97 : columnEl.value.toLowerCase().charCodeAt(0) - 97
    if (column > 25 || column < 0 || row < 0) {
        return;
    }

    console.log(row, column);
    document.getElementById('loader').classList.remove('d-none');
    var blurElements = document.querySelectorAll('.blur1');

    blurElements.forEach(blurElement => {
        blurElement.classList.add('blur');
    });
    var postdata = []
    for (let i = row; i < arr.length; i++) {
        if (arr[i][column]) {
            postdata.push(arr[i][column]);
        }
    }
    const apiUrl = 'https://localhost:44349/api/Sheet1/Incomingdata';

    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(postdata),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log('Data successfully sent to the backend:', data)

            document.getElementById('loader').classList.add('d-none');
            blurElements.forEach(blurElement => {
                blurElement.classList.remove('blur');
            });
            $('#successmodal').modal('show');
            fillPopUp(data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}


function googleSheetData() {
    var url = googlesheetEl.value;
    const regex = /\/spreadsheets\/d\/([^\/]+)\/edit/;
    const match = url.match(regex);
    if (match) {
        const spreadsheetId = match[1];
        const apiKey = 'AIzaSyB5sY9iE6Em0qsZn5dmYL36yiUCLtmg5lo';

        // Replace with your range
        const sheetname = 'Sheet1';

        const apiUrl = `https://sheets.googleapis.com/v4/spreadsheets/${spreadsheetId}/values/${sheetname}?key=${apiKey}`;

        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                const values = data.values || [];
                const arrayOfArrays = [];
                // Loop through rows and push each row as an array to arrayOfArrays

                for (const row of values) {
                    arrayOfArrays.push(row);
                }
                // fuzzy(arrayOfArrays);
                addAll(arrayOfArrays);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    } else {
        console.error("No match found");
    }
}


function addRowInFrontend(updatedRow) {
    $('#example').DataTable().row.add(updatedRow).draw();
}
//update Ui function
function updateRowInFrontend(updatedRow, rowIndex) {
    $('#example').DataTable().row(rowIndex).data(updatedRow).draw();
}

// This is a fuzzy algorithm that takes an input of a string and return a number (id)
function fuzzy(incomingstring) {
    let tempData = [];
    let extractData = [];
    var minId = 0;
    for (let j = 0; j < data.length; j++) {
        extractData.push(data[j].name.trim());
        minId = Math.max(data[j].id, minId);
    }
    minId++;
    for (let j = 0; j < extractData.length; j++) {
        if (fuzzySearch(extractData[j], incomingstring, 0.7)) {
            tempData.push(data[j]);
            minId = Math.min(data[j].id, minId);
        }
    }
    console.log(minId)
    return minId;
}

//external add fill the data
function fillPopUp(apiData) {

    var tableBody = document.getElementById('popup-table-body');
    var tableHeader = document.getElementById('popup-table').getElementsByTagName('thead')[0];


    tableBody.innerHTML = '';
    tableHeader.innerHTML = '';


    if (apiData.length > 0) {

        var headerRow = document.createElement('tr');


        var columnNames = ["Id", "Name", "VaraintFound", "AlreadyExists", "Action"];
        columnNames.forEach(function (columnName) {
            var headerCell = document.createElement('th');
            headerCell.textContent = columnName;
            headerRow.appendChild(headerCell);
        });

        tableHeader.appendChild(headerRow);


        apiData.forEach(function (item) {

            var row = document.createElement('tr');
            var shouldChangeColor = item.isVaraintFound === 'YES' && item.alreadyexists === 'NO';
            var shouldChangeColor1 = item.isVaraintFound && !item.alreadyexists;
            for (var key in item) {
                var cell = document.createElement('td');
                item[key] = (item[key] === true) ? 'YES' : (item[key] === false) ? 'NO' : item[key];
                cell.textContent = item[key];
                if (shouldChangeColor || shouldChangeColor1) {
                    cell.style.backgroundColor = '#e6e8ea';
                }
                row.appendChild(cell);
            }

            var actionCell = document.createElement('td');
            if (item.alreadyexists === 'YES') {
                actionCell.textContent = 'Alreadyexists skipping......'
            }
            else {
                if (item.isVaraintFound === 'YES') {
                    actionCell.style.backgroundColor = '#e6e8ea'
                }
                var actionButton = document.createElement('button');
                actionButton.textContent = 'Add';
                actionButton.className = 'btn2';
                actionCell.appendChild(actionButton);
            }
            row.appendChild(actionCell)

            tableBody.appendChild(row);
        });
    } else {
        var emptyRow = document.createElement('tr');
        var emptyCell = document.createElement('td');
        emptyCell.colSpan = columnNames.length;
        emptyCell.textContent = 'No data available.';
        emptyRow.appendChild(emptyCell);
        tableBody.appendChild(emptyRow);
    }


    document.getElementById('SaveAll').addEventListener('click', () => {
        var maxId = data.reduce((max, item) => Math.max(max, item.id), 0) + 1;
        for (let i = 0; i < apiData.length; i++) {
            const addingdata = {};
            if (apiData[i].alreadyexists === 'NO') {
                if (apiData[i].isVaraintFound == 'NO') {
                    const newId = maxId;
                    addingdata.id = newId
                    addingdata.name = apiData[i].name
                    addingdata.count = null
                    maxId++;
                }
                else {
                    addingdata.id = parseInt(apiData[i].id);
                    addingdata.name = apiData[i].name
                    addingdata.count = null
                }
                addingDataManually(addingdata);
            }
        }
    })
}