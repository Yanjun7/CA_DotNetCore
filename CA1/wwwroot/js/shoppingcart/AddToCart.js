window.onload = function () {
    //cartIcon
    cartIcon();

    //add to cart button on homepage
    console.log("enter js");
    let elem_addcart = document.getElementsByClassName("add_button");
    for (let i = 0; i < elem_addcart.length; i++) {
        console.log("enter addToCart.js, elem:" + elem_addcart);
        elem_addcart[i].addEventListener("click", onAdd);
    }

    //minus button on cart page
    let elem_minus = document.getElementsByClassName("button_minus");
    for (let i = 0; i < elem_minus.length; i++) {
        elem_minus[i].addEventListener("click", minus);
    }

    //plus button on cart page
    let elem_plus = document.getElementsByClassName("button_plus");
    for (let i = 0; i < elem_plus.length; i++) {
        elem_plus[i].addEventListener("click", plus);
    }

    //checkout button
    let checkoutButton = document.getElementById("button_checkout");
    checkoutButton.addEventListener("click", CheckOut);
}

function cartIcon() {
    console.log("entered cartIcon number change");
    let xhr_cartIcon = new XMLHttpRequest();

    xhr_cartIcon.open("POST", "/ShoppingCart/CartIcon");
    xhr_cartIcon.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr_cartIcon.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {

            if (this.status == 200) {
                //console.log(this.responseText);

                data = JSON.parse(this.responseText);

                let number = document.getElementById("cartIcon");
                number.innerHTML = data.count;
            }
        }
    };

    xhr_cartIcon.send();
}

function onAdd(event) {
    console.log("enter addToCart.js");
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId_Add(productId);
}

function sendProctId_Add(productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/ShoppingCart/Add");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            // receive response from server

            if (this.status == 200) {
                data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    console.log("Successful operation: " + data.status);
                    cartIcon();
                    total();
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

function minus(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId_minus(productId);
}

function sendProctId_minus(productId) {

    let xhr_minus = new XMLHttpRequest();

    xhr_minus.open("POST", "/ShoppingCart/Minus");
    xhr_minus.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr_minus.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200 || this.status == 302) {

                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    let quantity = document.getElementById("quantity_" + productId);
                    quantity.innerHTML = data.quantity;

                    let error = document.getElementById("quantity_error");
                    error.innerHTML = "";

                    cartIcon();
                    total();
                }
            }
        }
    };
    // send productId to Add controller
    xhr_minus.send(JSON.stringify({
        ProductId: productId
    }));
}

function plus(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId_plus(productId);
}

function sendProctId_plus(productId) {

    let xhr_plus = new XMLHttpRequest();

    xhr_plus.open("POST", "/ShoppingCart/Plus");
    xhr_plus.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr_plus.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200 || this.status == 302) {

                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    let quantity = document.getElementById("quantity_" + productId);
                    quantity.innerHTML = data.quantity;

                    let error = document.getElementById("quantity_error");
                    error.innerHTML = "";

                    cartIcon();
                    total();
                }
            }
        }
    };
    // send productId to Add controller
    xhr_plus.send(JSON.stringify({
        ProductId: productId
    }));
}

function total() {
    let xhr_total = new XMLHttpRequest();

    xhr_total.open("POST", "/ShoppingCart/Total");
    xhr_total.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr_total.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    let total = document.getElementsByClassName("total");
                    for (let i = 0; i < total.length; i++) {
                        total[i].innerHTML = data.total;
                    }
                }
            }
        }
    };

    xhr_total.send();
}

function CheckOut(event) {
    let xhr_checkout = new XMLHttpRequest();

    xhr_checkout.open("POST", "/History/CheckOut");
    xhr_checkout.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr_checkout.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                let data = JSON.parse(this.responseText);

                if (data.status == "success")
                    window.location = data.url;
                if (data.status == "unsuccessful") {
                    let error = document.getElementById("quantity_error");
                    error.innerHTML = "Item quantity cannot be 0";
                }
            }
        }
    };

    xhr_checkout.send();
}

