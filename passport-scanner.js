var passportListener = {
    listen: function (onMessage, //mandatory
        onOpen, // optional
        onClose, // optional
        onError // optional
    ) {
        if (!onMessage) {
            alert("you MUST specify a callback when web socket message detected");
            return;
        }
        var wsUrl = "ws://localhost:8122/PassportScannerResult";
        this.webSocket = new WebSocket(wsUrl);
        //Open connection  handler.
        this.webSocket.onopen = function () {
            if (onOpen) {
                console.log("ws opened.");
                onOpen();
            }
            //webSocket.send(nameTextBox.value);
        };

        //Message data handler.
        this.webSocket.onmessage = function (e) {
            console.log("ws message.");
            onMessage(e);
        };

        //Close event handler.
        this.webSocket.onclose = function () {
            if (onClose) {
                onClose();
            }
        };

        //Error event handler.
        this.webSocket.onerror = function (e) {
            if (onError) {
                onError();
            }
        }
    },

    closeConnection: function () {
        this.webSocket.close();
    }

}

function ddMMyyyy(dt) {
    if (dt == null || dt == undefined || dt == "") {
        return "";
    };
    var d = moment(dt);
    return `${d.date()}/${d.month() + 1}/${d.year()}`;
}
function SetValues(obj) {
    SetValueIfNotEmpty(obj.Name, "#txtName");
    //SetValueIfNotEmpty(obj.Race, "#Nationality");
    //SetValueIfNotEmpty(ddMMyyyy(obj.DateOfBirth), "#DateOfBirth");
    //SetValueIfNotEmpty(obj.Sector, "#Sector");
    SetValueIfNotEmpty(obj.Employer, "#Company");
    //SetValueIfNotEmpty(obj.Address, "#Address");
    //SetValueIfNotEmpty(ddMMyyyy(obj.DateOfIssue), "#DateOfIssue");
    SetValueIfNotEmpty(obj.IDNumber, "#FINNum");
    SetValueIfNotEmpty(obj.PermitNumber, "#IdNo");
    //THIS TWO NOT SURE >> NEED CONFIRM
    //SetValueIfNotEmpty(obj.CountryOfBirth, "#Nationality");

    if (obj.Sex == "F") {
        //$("input[name='Gender'][value='Female']").attr("checked", true);
    } else {
        //$("input[name='Gender'][value='Male']").attr("checked", true);
    }

    alert("Please flip the card.");
    IdTypeChange();
}

function SetValueIfNotEmpty(value, controlId) {
    if (value != null && value != undefined && value != "") {
        $(controlId).val(value);
    };
}
