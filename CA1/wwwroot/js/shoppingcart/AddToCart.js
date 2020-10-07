window.onload = function () {
    let elemList = document.getElementsByClassName("button");

    for (let i = 0; i < elemList.length; i++) {
        elemList[i].addEventListener("click", onAdd);
    }
}


function onAdd(event) {




    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/Add");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status === 200 || this.status === 302) {
                let data = JSON.parse(this.responseText);

                if (this.status === 200) {
                    console.log("Successful operation: " + data.success);
                }
                else if (this.status === 302) {
                    window.location = data.redirect_url;
                }
            }
        }
    };

    // send productId
    xhr.send(productId);
}
