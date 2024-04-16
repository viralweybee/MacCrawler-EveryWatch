import { fuzzySearch } from "./fuzzySearch.js";
const saveBtnEl = document.getElementById('save');
const addManuallyEl = document.getElementById('addManually');
var manufactureTextEl = document.getElementById('knownManufactureText');
var externalIdEl = document.getElementById('externalId');
var isFromExternalEl = document.getElementById('isFromExternal');
var fileinputEl = document.getElementById('file');
var addEl = document.getElementById('add');
var googlesheetEl = document.getElementById('googleSheet');

var data;
var editFlag;
//Api call to fetch the data
function KnownManufacturers() {
    var apiUrl = "https://localhost:44349/api/KnownManufacturers";
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
            fillDataTable(data);
        })
        .catch(error => {
            console.error("Error:", error);
        });
}
KnownManufacturers()

document.addEventListener('click', function (event) {
  
    if (event.target.classList.contains('edit-btn')) {
        var rowId = event.target.getAttribute('data-row-id');
        editRow(parseInt(rowId));
    }
    
    else if (event.target.classList.contains('delete-btn')) {
        var rowId = event.target.getAttribute('data-row-id');
        deleteRow(parseInt(rowId));
    }
});

//Fill the data in table 
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
                    `<button class="edit-btn" data-row-id="${row.id}">Edit</button>` +
                    `<button class="delete-btn" data-row-id="${row.id}">Delete</button>` +
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
        headerRow.append('<th>' + (column.title || column.data) + '</th>');
    });
    thead.append(headerRow);
    table.destroy();
    $('#example').DataTable({
        data: data,
        columns: columns,
        "bDestroy": true
    });
}

//Edit each row
var editId;
function editRow(id) {
    editFlag = true;
    console.log(id);
    var rowData = data.find(row => row.id === id);

    // Fill the modal input fields with row data
    manufactureTextEl.value = rowData.knownManfacturesText;
    externalIdEl.value = rowData.externalId;
    isFromExternalEl.value = rowData.isFromExternal;
    $('#exampleModal').modal('show');
    editId = id;
}

//Set flag when add manually
addManuallyEl.addEventListener('click', () => {
    //when ever add button click first of all set to empty
    manufactureTextEl.value = '';
    externalIdEl.value = '';
    isFromExternalEl.value = '';
    editFlag = false;
})

//save button 
saveBtnEl.addEventListener('click', () => {
    if (!editFlag) {
        addingDataManually();
    }
    else {
        editingDataManually(editId);
    }
})

//Adding Data into database
function addingDataManually() {
    if (manufactureTextEl.value.trim() == '') {
        return 'not valid';
    }

    const currentDateTime = new Date().toISOString();  // Get current date and time in ISO format

    const apiUrl = 'https://localhost:44349/api/KnownManufacturers';
    const requestBody = {
        "manufacturerText": manufactureTextEl.value,
        "externalId": parseInt(externalIdEl.value),
        "isFromExternal": null,
        "createdDate": currentDateTime,
        "knownModel": [],
        "knownReferenceNumber": []
    };

    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(apidata => {
            var tempdata = {};
            tempdata.id = apidata.id;
            tempdata.knownManfacturesText = apidata.manufacturerText
            tempdata.externalId = apidata.externalId
            tempdata.isFromExternal = apidata.isFromExternal
            tempdata.createdDate = apidata.createdDate;
            addRowInFrontend(tempdata);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// edit data manually
function editingDataManually(id) {
    fetch(`https://localhost:44349/api/KnownManufacturers/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            const modifiedData = {
                ...data,
                manufacturerText: manufactureTextEl.value
            };
            console.log(modifiedData);

            return putapicall(modifiedData)
        })
}
function putapicall(modifiedData) {
    fetch(`https://localhost:44349/api/KnownManufacturers/${editId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(modifiedData),
    });
    for (var i = 0; i < data.length; i++) {
        if (data[i].id === editId) {
            data[i].knownManfacturesText = modifiedData.manufacturerText
            data[i].externalId = modifiedData.externalId;
            data[i].isFromExternal = modifiedData.isFromExternal;
            break;
        }
    }
    updateRowInFrontend(data[i], i);
    console.log(data);
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
    for (let i = 0; i < arr.length; i++) {
        addingDataeach(arr[i]);
    }
}
function addingDataeach(arr) {
    const currentDateTime = new Date().toISOString();
    const apiUrl = 'https://localhost:44349/api/KnownManufacturers';
    const requestBody = {
        "manufacturerText": arr[1],
        "externalId": null,
        "isFromExternal": null,
        "createdDate": currentDateTime,
        "knownModel": [],
        "knownReferenceNumber": []
    };
    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(apidata => {
            var tempdata = {};
            tempdata.id = apidata.id;
            tempdata.knownManfacturesText = apidata.manufacturerText
            tempdata.externalId = apidata.externalId
            tempdata.isFromExternal = apidata.isFromExternal
            tempdata.createdDate = apidata.createdDate;
            addRowInFrontend(tempdata);
            data.push(tempdata);
        })
        .catch(error => {
            console.log('Skipping insertion for:', arr);
            console.log(error);
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
                fuzzy(arrayOfArrays);
                // addAll(arrayOfArrays);  
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    } else {
        console.error("No match found");
    }
}

//update Ui function
function updateRowInFrontend(updatedRow, rowIndex) {
    $('#example').DataTable().row(rowIndex).data(updatedRow).draw();
}
function addRowInFrontend(updatedRow) {
    $('#example').DataTable().row.add(updatedRow).draw();
}
// function addRowInFrontends(updatedRow){
//     $('#example').DataTable().rows.add(updatedRow).draw();
// }

function fuzzy(incomingData) {
    let tempData = [];
    let extractData = [];

    for (let j = 0; j < data.length; j++) {
        extractData.push(data[j].knownManfacturesText.trim());
    }
    for (let i = 0; i < incomingData.length; i++) {
        for (let j = 0; j < extractData.length; j++) {
            if (fuzzySearch(extractData[j], incomingData[i][1].trim(), 0.7)) {
                tempData.push(data[j]);
            }
        }
    }
    console.log(tempData)
}
function deleteRow(id){
    console.log(id);
}
