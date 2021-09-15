String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

//function isHandphoneNumberRegister(s){   
//    var patrn=/^[8,9]{1}([0-9]){7}$/;   
//    if (!patrn.exec(s)) 
//        return false; 
//    return true;
//}

function isHandphoneNumberRegister(s) {
    var patrn = /^[\d ()+-]+$/;
    if (!patrn.exec(s)) return false; 
    return true;
}


function IsBannedChange() {
    $("#BanReason").toggle();
}
function DocumentTypeChange(obj) {
    var text = $(obj).find("option:selected").text()
    if (text == "FIN" || text == "Passport") {
        $("#divExpiryDate").show()
    } else {
        $("#divExpiryDate").hide()
    }

}

function validate_email(field) {
    with (field) {
        apos = value.indexOf("@")
        dotpos = value.lastIndexOf(".")
        if (apos < 1 || dotpos - apos < 2) { return false }
        else { return true }
    }
}

function formatDatetime(date) {

    if (date == "")
        return "";    
    
    if (date.length == 10) {
        return date.substr(6, 4) + date.substr(3, 2) + date.substr(0, 2);
    }
    else {
        return date.substr(6, 4) + date.substr(3, 2) + date.substr(0, 2) + date.substr(11, 8);
    }
}


function checkFinUin(idtype, fin) {
    if (idtype == "Email") {
        return EmailValidator(fin);
    }
    else if (idtype == "Phone") {
        return PhoneValidator(fin);
    }
    if (idtype == "Passport" || idtype == "Without ID" || idtype =="") {
        return true;
    }
    fin = fin.toUpperCase();
    if (fin.length != 9) {
        return false;
    }
    else if (!IsNumeric(fin.substring(1, 8))) {
        return false;
    }
    else if (idtype == "NRIC") {
        return checkUINAlgo(fin);
    }
    else if (idtype == "FIN") {
        return checkFINAlgo(fin);
    }
    else {
        return true;
    }
}


function IsNumeric(sText) {

    var ValidChars = "0123456789";
    var IsNumber = true;
    var Char;

    for (i = 0; i < sText.length && IsNumber == true; i++) {

        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}

function checkFINAlgo(uin) {

    uin = uin.toUpperCase();
    if (uin.charAt(0) != 'F' && uin.charAt(0) != 'G') {
        return false;
    }

    var keys = new Array();
    keys["1"] = 'K';
    keys["2"] = 'L';
    keys["3"] = 'M';
    keys["4"] = 'N';
    keys["5"] = 'P';
    keys["6"] = 'Q';
    keys["7"] = 'R';
    keys["8"] = 'T';
    keys["9"] = 'U';
    keys["10"] = 'W';
    keys["11"] = 'X';

    var char1 = uin.charAt(1) * 2;
    var char2 = uin.charAt(2) * 7;
    var char3 = uin.charAt(3) * 6;
    var char4 = uin.charAt(4) * 5;
    var char5 = uin.charAt(5) * 4;
    var char6 = uin.charAt(6) * 3;
    var char7 = uin.charAt(7) * 2;
    var total = char1 + char2 + char3 + char4 + char5 + char6 + char7;
    if (uin.charAt(0) == 'G') {
        total = total + 4;
    }
    var remainder = total % 11;
    var p = 11 - remainder;
    if (uin.charAt(8) != keys[p]) {
        return false;
    }
    else {
        return true;
    }
}


function checkUINAlgo(uin) {

    uin = uin.toUpperCase();
    if (uin.charAt(0) != 'S' && uin.charAt(0) != 'T') {
        return false;
    }

    var keys = new Array();
    keys["1"] = 'A';
    keys["2"] = 'B';
    keys["3"] = 'C';
    keys["4"] = 'D';
    keys["5"] = 'E';
    keys["6"] = 'F';
    keys["7"] = 'G';
    keys["8"] = 'H';
    keys["9"] = 'I';
    keys["10"] = 'Z';
    keys["11"] = 'J';

    var char1 = uin.charAt(1) * 2;
    var char2 = uin.charAt(2) * 7;
    var char3 = uin.charAt(3) * 6;
    var char4 = uin.charAt(4) * 5;
    var char5 = uin.charAt(5) * 4;
    var char6 = uin.charAt(6) * 3;
    var char7 = uin.charAt(7) * 2;
    var total = char1 + char2 + char3 + char4 + char5 + char6 + char7;
    if (uin.charAt(0) == 'T') {
        total = total + 4;
    }
    var remainder = total % 11;
    var p = 11 - remainder;
    if (uin.charAt(8) != keys[p]) {
        return false;
    }
    else {
        return true;
    }
}
function IDNumberValidator(newVisitorIdNumberObj, valid, message) {
    if (!checkFinUin(newVisitorIdNumberObj.val())) {
        valid.html(message);
        return true;
    }
    else {
        valid.html("");
        return false;
    }
    
}
function EmailValidator(value) {
    var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (myreg.test(value)) {
        return true;
    }
    else {
        return false;
    }

}
function PhoneValidator(value) {
    var phoneReg = /^0?1[3|4|5|8][0-9]\d{8}$/;
    var landlineReg = /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/;
    if (phoneReg.test(value)) {
        return true;
    }
    else {
        if (landlineReg.test(value)) {
            return true;
        }
        else {
            return false;

        }
    }
}
