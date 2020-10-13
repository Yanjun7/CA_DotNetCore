window.onload = function () {
    let errDiv = document.getElementById("err_msg");
    let form = document.getElementById("form");

    form.onsubmit = function () {
        let elemNUname = document.getElementById("newUsername");
        let elemPwd1 = document.getElementById("newPassword1");
        let elemPwd2 = document.getElementById("newPassword2");

        let nUname = elemNUname.value().trim();
        let nPwd1 = elemPwd1.value().trim();
        let nPwd2 = elemPwd2.value().trim();

        if (nUname.length === 0 || nPwd1.length === 0 || nPwd2.length === 0) {
            errDiv.innerHTML = "Please fill up all fields.";
            return false;	// cancel form submission
        }
        else if (!(nPwd1 === nPwd2)) {
            errDiv.innerHTML = "Passwords do not match."
        }

        return true;
    }
    let elems = document.getElementsByClassName("form-control");
    for (let i = 0; i < elems.length; i++) {
        // remove our error message as long as any 
        // of the input boxes have focus
        elems[i].onfocus = function () {
            errDiv.innerHTML = "";
        }
    }
}