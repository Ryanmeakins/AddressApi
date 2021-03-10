var request = null;

var url = "http://localhost:8080/";
var endpointSpecifiedCountry = "Address";
var endpointCrossCountry = "Address";
var requestType = 'GET'; //"GET" or POST

var currentCountryModel = null;


function sendSpecifiedCountryRequest() {
    if(currentCountryModel != null) {
    request = new XMLHttpRequest();
    request.open(requestType, url + endpointSpecifiedCountry, true);

    let body = "{";

    for (let index = 0; index < currentCountryModel.length; index++) {
        if(currentCountryModel[index] == "COUNTRY") {
            body = body + "\"" + currentCountryModel[index] + "\""+ ":" + "\"" + document.getElementById("sel").value + "\"";
        } else {
            body = body + "\"" + currentCountryModel[index] + "\""+ ":" + "\"" + document.getElementById(currentCountryModel[index]).value + "\"";
        }
        
        if(index < currentCountryModel.length - 1) {
            body = body + ", ";
        }
    }

    body = body + "}";

    request.onload = function () {

        // Begin accessing JSON data here
        var data = JSON.parse(this.response);
        if (request.status >= 200 && request.status < 400) {
          //Note: work on this part once we can send and receive data via APIs
          data.forEach(address => {
            console.log(data);
            document.getElementById("addressArea").innerHTML = data;
        });
        } else {
          console.log("Error sending requeststatus: " + request.status, request);
        }
      }

    request.send(body);
    } else {
        alert("Country not selected!");
    }
}


function sendCrossCountryRequest() {
    request = new XMLHttpRequest();
    request.open('GET', url + endpointCrossCountry, true);

    let body = {};

    body.NUMBER = document.getElementById("crossCountryNumber").value;
    body.STREET = document.getElementById("crossCountryStreet").value;

    request.onload = function () {

        // Begin accessing JSON data here
        var data = JSON.parse(this.response);
        if (request.status >= 200 && request.status < 400) {
            //Note: work on this part once we can send and receive data via APIs
            data.forEach(address => {
                console.log(data);
                document.getElementById("addressArea").innerHTML = data;
            });
        } else {
            console.log("Error sending requeststatus: " + request.status, request);
        }
    }

    request.send(JSON.stringify(body));
}

// Page formatting scripts below

function change_myselect(sel) {
    fetch("schema_json/"+sel+".json").then(response => response.json()).then(data =>
    {
        var obj, dbParam, xmlhttp, myObj, x, txt = "";
        for (fields in data['FormOrder']) {
            if(data['FormOrder'][fields] != 'COUNTRY')
            {
                txt += "<label for=\"fname\">"+data['FormOrder'][fields]+":</label>";
                txt += "<input class=\"addressTable\" type=\"text\" id=\""+data['FormOrder'][fields]+"\" name=\""+data['FormOrder'][fields]+"\"><br><br>";
          
            }
        }
        txt += "<input type=\"button\" value=\"Search\" onclick=\"getDetails(sel.value)\"></input>";
        //console.log(data['FormOrder']);
        currentCountryModel = data['FormOrder'];
        document.getElementById("get_detailsSearch").innerHTML = txt;
    });
    }
    
    
    function getDetails(sel) {
    
        console.log("sel: "+sel);
            // Get data from backend to populate the table
        directoryPath = "schema_json/"+sel+".json";
    
        fetch(directoryPath).then(response => response.json()).then(data =>
        {		
            // testing by printing heading for the results table, 
            getMatchingAddresses = "<table><tr>";
            for (fields in data['DisplayOrder']) {
                
                // Get data from api and populate here using data['DisplayOrder'][fields]
                getMatchingAddresses += data['DisplayOrder'][fields] +",";
            }
            getMatchingAddresses = getMatchingAddresses.substring(0, getMatchingAddresses.length-1);
            getMatchingAddresses += "</tr></table>";
            getMatchingAddresses = getMatchingAddresses.replaceAll("{", "");
            getMatchingAddresses = getMatchingAddresses.replaceAll("}", "");
            
            document.getElementById("get_addresses").innerHTML = getMatchingAddresses;
        });	
    }