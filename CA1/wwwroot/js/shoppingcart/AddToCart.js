window.onload = function () {
    console.log("enter js");
    let elemList = document.getElementsByClassName("add_button");
    /* let elemList = document.getElementsByClassName("product_img");*/
    console.log("enter addToCart.js, elem:" + elemList);


    for (let i = 0; i < elemList.length; i++) {
        console.log("enter addToCart.js, elem:" + elemList);
        elemList[i].addEventListener("click", onAdd);
    }

    console.log("begin toStart");
    //for (let i = 1; i > 0; i--) {
        toStart();
    //}
}

function onAdd(event) {
    console.log("enter addToCart.js");
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId(productId);
}

function sendProctId(productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/ShoppingCart/Add");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            // receive response from server

            if (this.status == 200)
            {
                data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    console.log("Successful operation: " + data.status);
                    window.location = data.url;
                }

                if (data.status == "redirect") {
                    window.location = data.url;
                }
            }
        }
    };
    // send productId to Add controller
    xhr.send(JSON.stringify({
        "ProductId": productId
    }));
}

function toStart() {
    console.log("entered toStart");
    let xhr2 = new XMLHttpRequest();

    xhr2.open("POST", "/ShoppingCart/CartIcon");
    xhr2.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr2.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {

            if (this.status == 200) {
                console.log(this.responseText);

                data = JSON.parse(this.responseText);

                let cartIcon = document.getElementById("cartIcon");
                cartIcon.innerHTML = data.count;
            }
        }
    };

    xhr2.send();
}

