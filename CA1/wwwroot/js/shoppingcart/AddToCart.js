﻿window.onload = function () {
    /*let elemList = document.getElementsByClassName("button");*/
    let elemList = document.getElementsByClassName("img_container");
    console.log("enter addToCart.js, elem:" + elemList);
    

    for (let i = 0; i < elemList.length; i++) {
        console.log("enter addToCart.js, elem:" + elemList);
        elemList[i].addEventListener("click", onAdd);
    }
}


function onAdd(event) {
    console.log("enter addToCart.js");
    let elem = event.currentTarget;
    let productId = elem.getAttribute("producId");


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
    xhr.send(JSON.stringify({
        ProductId: productId
    }));
}