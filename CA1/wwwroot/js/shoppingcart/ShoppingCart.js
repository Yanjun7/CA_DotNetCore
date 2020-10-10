//Have been merged with AddToCart.js

/*window.onload = function () {
    let elem = document.getElementsByClassName("button_minus");
    for (let i = 0; i < elem.length; i++) {
        elem[i].addEventListener("click", remove);
    }
}

function remove(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId(productId);
}

function sendProctId(productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/ShoppingCart/Minus");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200 || this.status == 302) {

                let data = JSON.parse(this.responseText);

                if (data.status == "success") {
                    let p = document.getElementById("quantity_" + productId);
                    p.innerHTML = data.quantity;
                }
            }
        }
    };
    // send productId to Add controller
    xhr.send(JSON.stringify({
        ProductId: productId
    }));
}
*/