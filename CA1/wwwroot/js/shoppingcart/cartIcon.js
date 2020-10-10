//Have been merged with AddToCart.js

/*window.onload = function () {
    console.log("cartIcon");
    for (let i = 1; i > 0; i--) {
        toStart();
    }
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
}*/