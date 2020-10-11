/*window.onload = function () {
    let checkoutButton = document.getElementById("button_checkout");
    checkoutButton.addEventListener("click", CheckOut);
}

function CheckOut(event) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/History/CheckOut");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                let data = JSON.parse(this.responseText);

                if (data.status == "success")
                    window.location = data.url;
                else
                    window.location = "/History/Unsuccessful";
            }
        }
    };

    xhr.send();
}*/