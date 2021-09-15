function checkPasswordStrength() {
    var pwd = $("#password").val();
    if (pwd.length < 8) {
        alert("Password length must be at least 8 characters.");
        return false;
    }

    if (pwd != $("#confirmPassword").val()) {
        alert("Password and Confirm password does not match.");
        return false;
    }

    if (hasLowerCase(pwd) == false) {
        alert("Password must contain at least one lower case character.");
        return false;
    }

    if (hasUpperCase(pwd) == false) {
        alert("Password must contain at least one upper case character.");
        return false;
    }

    if (hasNumbers(pwd) == false) {
        alert("Password must contain at least one numeric.");
        return false;
    }

    if (hasSpecialCharacters(pwd) == false) {
        alert("Password must contain at least one special characters.");
        return false;
    }

    return true;
}

function hasLowerCase(str) {
    return str.match(/[a-z]/) != null;
}

function hasUpperCase(str) {
    return str.match(/[A-Z]/) != null;
}

function hasNumbers(t) {
    var regex = /\d/g;
    return regex.test(t);
}

function hasSpecialCharacters(str) {
    var regex = /^[A-Za-z0-9 ]+$/
    return !regex.test(str);
}
