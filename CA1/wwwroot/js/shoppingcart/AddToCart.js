window.onload = function () {
    console.log("enter js");
    let elemList = document.getElementsByClassName("add_button");
   /* let elemList = document.getElementsByClassName("product_img");*/
    console.log("enter addToCart.js, elem:" + elemList[1]);
    

    for (let i = 0; i < elemList.length; i++) {
        console.log("enter addToCart.js loop" + elemList[i]);
        elemList[i].addEventListener("click", onAdd);
    }
}

function onAdd(event) {
    console.log("enter onAdd");
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId(productId);
}

function sendProctId(productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/Add");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status === 200 || this.status === 302) {
                let data = JSON.parse(this.responseText);
                //console.log(data);
                //window.location = "/Login/index";

                if (this.status === 200) {
                    console.log("Successful operation: " + data.success);
                }
                else if (this.status === 302) {
                    window.location = data.redirect_url;
                }
            }
        }
    };
    // send productId to Add controller
    xhr.send(JSON.stringify({
        ProductId: productId
    }));
}

