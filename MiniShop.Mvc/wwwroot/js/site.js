
function nullableEmailCheck(emailStr) {
    var pattern = /^([\.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
    if (emailStr != "" && !pattern.test(emailStr)) {
        return false;
    } else {
        return true;
    }
}

function nullablePhoneCheck(phoneStr) {
    var pattern = /^1\d{10}$/;
    if (phoneStr != "" && !pattern.test(phoneStr)) {
        return false;
    } else {
        return true;
    }
}


